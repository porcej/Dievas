using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using AFD.Dashboard.Models;
using Dievas.Models;
using Dievas.Models.Telestaff;
using Dievas.Models.Staffing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace Dievas.Controllers {

    /// <summary>
    ///     Controller Class <c>TelestaffController</c> Provided an API to access staffing information
    ///     
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TelestaffController : ControllerBase {

        /// <summary>
        ///     Application configuration for this Class
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<TelestaffController> _logger;

        /// <summary>
        ///     Web Client for this class
        /// </summary>
        private static HttpClient _http;

        /// <summary>
        ///     Holds roster data keyed by date
        /// </summary>
        private static readonly ConcurrentDictionary<DateTime, StaffingCache> _rosters = new ConcurrentDictionary<DateTime, StaffingCache>();

        /// <summary>
        ///     Holds a reference to the CAD state as maintained in a Singelton
        /// </summary>
        private static CAD _cad;
        
        /// <summary>
        ///     Default constructor for Class <c>TelestaffController</c>
        /// </summary>
        /// <param name="configuration">IConfiguration configuration informaiton</param>
        /// <param name="logger">ILogger: aggregate logger</param>
        /// <param name="cad">CAD: Singleton Representing state from a CAD</param>
        public TelestaffController(IConfiguration configuration,
                                   ILogger<TelestaffController> logger,
                                   CAD cad) {
            _config = configuration;
            _logger = logger;
            _cad = cad;

            HttpClientHandler httpClientHandler = new HttpClientHandler();

            bool allowInvalidCertificates = false;

            Boolean.TryParse(_config["Telestaff:AllowInvalidCertificates"], out allowInvalidCertificates);

            // If our settings allow us, skip ceritifcate validation
            if (allowInvalidCertificates) {
                httpClientHandler.ServerCertificateCustomValidationCallback = 
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }

            // Initilize our HttpClient object
            _http = new HttpClient(httpClientHandler);

            // Set base address to Telestaff URL
            _http.BaseAddress = new Uri(_config["Telestaff:Url"], UriKind.Absolute);
            
            // Set our user agent
            var productValue = new ProductInfoHeaderValue(_config["Telestaff:User-agent"]);
            _http.DefaultRequestHeaders.UserAgent.Add(productValue);

            // Handle user authentication for all requests
            var byteArray = Encoding.ASCII.GetBytes($"{_config["Telestaff:Username"]}:{_config["Telestaff:Password"]}");
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }


        




        /// <summary>
        ///     Filters roster records by station
        /// </summary>
        /// <param name="roster">StaffingRoster holding data to be filtered</param>
        /// <param name="station">String representing a station</param>
        /// <param name="offRoster">Boolean iff includes off roster records</param>
        /// <param name="telestaffOnly">Boolean iff excludes matching on CAD.</param>
        private StaffingRoster filterRosterByStation(StaffingRoster roster, string station = "", bool offRoster = false, bool telestaffOnly = false) {
            if (string.IsNullOrWhiteSpace(station)) return roster;

            _logger.LogInformation($"Filtering staffing records for station {station} where offRoster={offRoster.ToString()} and telestaffOnly={telestaffOnly.ToString()}.");

            // Get the units assigned to the station from CAD
            List<string> homedUnits = _cad.GetUnits()
                                          .Where(unit => unit.HomeStation == station)
                                          .Select(unit => unit.RadioName)
                                          .ToList();

            List<StaffingRecord> records = roster.Records.FindAll(record => 
                            (!telestaffOnly && homedUnits.Contains(record.UnitName))
                            || (!telestaffOnly && homedUnits.Contains(record.UnitAbbreviation))
                            || (record.StationName != null && record.StationName.Contains(station))
                            || (record.StationAbbreviation != null && record.StationAbbreviation.Contains(station)));

            if (!offRoster) {
                records = records.FindAll(record => record.IsWorking && !record.IsRequest && record.UnitName != "{off roster}");
            }

            return new StaffingRoster(roster.RosterDate, records);
        }

        /// <summary>
        ///     Returns staffing data and fetches new staffing data if needed
        /// </summary>
        /// <param name="date">string Representing the date to fetch a roster for</param>
        /// <param name="station">Station information to filter the roster records on</param>
        /// <param name="offRoster">Boolean iff includes off roster records</param>
        private StaffingRoster getStaffingForDate(DateTime date, string station = "", bool offRoster = false, bool telestaffOnly = false) {
            DateTime now = DateTime.Now;

            // Make sure we have a date and not a date & time to key on
            date = date.Date;

            // Check if we have staffing data for this date already
            if (_rosters.ContainsKey(date)) {
                
                // Check if our data is expired
                if (_rosters[date].IsValid()) {
                    return filterRosterByStation(_rosters[date].Roster, station, offRoster, telestaffOnly);
                }
            }

            // If we are here, we need to fetch new data:
            StaffingRoster roster = fetchTelestaffRoster(date.ToString(_config["Telestaff:TimeFormat"]));

            StaffingCache cachedRoster = new StaffingCache(roster, Convert.ToDouble(_config["Telestaff:ExpirationTimeInMinutes"]));

            _rosters.TryAdd(date, cachedRoster);

            // Start a thread to clean up old entries
            Task.Factory.StartNew(
                () => {
                    var expiredRosters = _rosters.Where(r => r.Value.IsExpired()).ToArray();

                    foreach (var expiredRoster in expiredRosters) {
                        string rosterDate = expiredRoster.Key.ToString("D");
                        string expirationDate = expiredRoster.Value.Expiration.ToString("F");
                        if (_rosters.TryRemove(expiredRoster)) {
                            _logger.LogInformation($"Removing expired roster for date {rosterDate} that expired on {expirationDate}.");
                        } else {
                             _logger.LogWarning($"Failed to remove expired roster for date {rosterDate} that expired on {expirationDate}.");    
                        }
                    }
                }
            );
            return filterRosterByStation(roster, station, offRoster, telestaffOnly);
        }


        /// <summary>
        ///     API Endpoint for fetching staffing information for the current day
        /// </summary>
        /// <returns> JSON formated string representation of the staffing information for the current calendar day</returns>
        [HttpGet("staffing")]
        public string GetStaffing([FromQuery] string station, [FromQuery] string offRoster, [FromQuery] string telestaffOnly) {
            bool isOffRoster;
            Boolean.TryParse(offRoster, out isOffRoster);
            bool isTelestaffOnly;
            Boolean.TryParse(telestaffOnly, out isTelestaffOnly);
            DateTime date = DateTime.Now;
            StaffingRoster roster = getStaffingForDate(date, station, isOffRoster, isTelestaffOnly);
            ApiWrapper response = new ApiWrapper{ Data = roster }; 
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        ///     API Endpoint for fetching staffing information for the provided date
        /// </summary>
        /// <param name="date">string Representing the date to fetch a roster for</param>
        /// <returns> JSON formated string representation of the staffing information for the <paramref name="date" /> day</returns>
        [HttpGet("staffing/{date}")]
        public string GetRosterByDate(string date, [FromQuery] string station, [FromQuery] string offRoster, [FromQuery] string telestaffOnly) {
            bool isOffRoster;
            Boolean.TryParse(offRoster, out isOffRoster);
            bool isTelestaffOnly;
            Boolean.TryParse(telestaffOnly, out isTelestaffOnly);
            ApiWrapper response = new ApiWrapper();

            try {
                DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", null);
                response.Data = getStaffingForDate(dt, station, isOffRoster, isTelestaffOnly);
                return JsonConvert.SerializeObject(response);
            } catch (FormatException) {
                response.Data = new {Error = $"The date provided, {date}, does not appear to be in the format \"yyyyMMdd\"."};
                response.StatusCode = 500;
            }
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        ///     API Endpoint for fetching active business units (nodes) from an oranization
        /// </summary>
        /// <param name="nodeType">Type of node to return data for</param>
        /// <param name="endDate">string Representing the date of the last roster when fetching multiple rosters</param>
        /// <returns> JSON formated string representation of the nodes of <paramref name="nodeType"/> within an organization.</returns>
        [HttpGet("activeNodes/{nodeType}")]
        public List<OrganizationNode> GetActiveNodes(string nodeType="UNIT"){
            return fetchActiveNodes(nodeType);
        }

        /// <summary>
        ///     API Endpoint for fetching Telestaff Schedule for the current day
        /// </summary>
        /// <returns> JSON formated string representation of the staffing information for the current staffing day.</returns>
        [HttpGet("telestaff/schedule")]
        public string GetRawScheduleByDate() {
            ApiWrapper response = new ApiWrapper();
            response.Data = fetchSchedule(); 
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        ///     API Endpoint for fetching Telestaff Schedule for a specific date
        /// </summary>
        /// <param name="date">string Representing the date to fetch schedule for.</param>
        /// <returns> JSON formated string representation of the staffing information for the <paramref name="date" /> day.</returns>
        [HttpGet("telestaff/schedule/{date}")]
        public string GetRawScheduleByDate(string date) {
            ApiWrapper response = new ApiWrapper();
            try {
                DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", null);
                date = dt.ToString(_config["Telestaff:TimeFormat"]);
                response.Data = fetchSchedule(date); 
                return JsonConvert.SerializeObject(response);
            } catch (FormatException) {
                response.Data = new {Error = $"The date provided, {date}, does not appear to be in the format \"yyyyMMdd\"."};
                response.StatusCode = 500;
            }
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        ///     API Endpoint for fetching a Raw Telestaff Roster
        /// </summary>
        /// <returns> JSON formated string representation of the staffing information for the current staffing day.</returns>
        [HttpGet("telestaff/roster")]
        public string GetRawRoster() {
            ApiWrapper response = new ApiWrapper();
            response.Data = fetchRoster();
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        ///     API Endpoint for fetching a Raw Telestaff Roster
        /// </summary>
        /// <param name="date">string Representing the date to fetch a roster for</param>
        /// <returns> JSON formated string representation of the staffing information for the <paramref name="date" /> day.</returns>
        [HttpGet("telestaff/roster/{date}")]
        public string GetRawRosterByDate(string date) {
            ApiWrapper response = new ApiWrapper();
            try {
                DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", null);
                date = dt.ToString(_config["Telestaff:TimeFormat"]);
                response.Data = fetchRoster(date); 
                return JsonConvert.SerializeObject(response);
            } catch (FormatException) {
                response.Data = new {Error = $"The date provided, {date}, does not appear to be in the format \"yyyyMMdd\"."};
                response.StatusCode = 500;
            }
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        ///     API Endpoint for fetching Raw Telestaff Position Data
        /// </summary>
        /// <returns> JSON formated string repesenting an API Wrapped Position Data, direct from Telestaff.</returns>
        [HttpGet("telestaff/position")]
        public string GetRawPostions() {
            ApiWrapper response = new ApiWrapper();
            response.Data = fetchPositions();
            return JsonConvert.SerializeObject(response);
        }
    }
}