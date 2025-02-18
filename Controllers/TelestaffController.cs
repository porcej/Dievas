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
using Dievas.Models;
using Dievas.Models.Telestaff;
using Dievas.Models.Staffing;
using Dievas.Services;

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
            
            var staffingData = StaffingSingleton.Instance;
            StaffingCache? staffingRoster = staffingData.getRoster(date);

            if (staffingRoster == null) {
                _logger.LogError($"Error loading staffing roster for {date}.");
            }
            StaffingRoster roster = staffingRoster.Roster;
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
        // [HttpGet("staffing/{date}")]
        // public string GetRosterByDate(string date, [FromQuery] string station, [FromQuery] string offRoster, [FromQuery] string telestaffOnly) {
        //     bool isOffRoster;
        //     Boolean.TryParse(offRoster, out isOffRoster);
        //     bool isTelestaffOnly;
        //     Boolean.TryParse(telestaffOnly, out isTelestaffOnly);
        //     ApiWrapper response = new ApiWrapper();

        //     try {
        //         DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", null);
        //         response.Data = getStaffingForDate(dt, station, isOffRoster, isTelestaffOnly);
        //         return JsonConvert.SerializeObject(response);
        //     } catch (FormatException) {
        //         response.Data = new {Error = $"The date provided, {date}, does not appear to be in the format \"yyyyMMdd\"."};
        //         response.StatusCode = 500;
        //     }
        //     return JsonConvert.SerializeObject(response);
        // }

        /// <summary>
        ///     API Endpoint for fetching active business units (nodes) from an oranization
        /// </summary>
        /// <param name="nodeType">Type of node to return data for</param>
        /// <param name="endDate">string Representing the date of the last roster when fetching multiple rosters</param>
        /// <returns> JSON formated string representation of the nodes of <paramref name="nodeType"/> within an organization.</returns>
        // [HttpGet("activeNodes/{nodeType}")]
        // public List<OrganizationNode> GetActiveNodes(string nodeType="UNIT"){
        //     return fetchActiveNodes(nodeType);
        // }

        /// <summary>
        ///     API Endpoint for fetching Telestaff Schedule for the current day
        /// </summary>
        /// <returns> JSON formated string representation of the staffing information for the current staffing day.</returns>
        // [HttpGet("telestaff/schedule")]
        // public string GetRawScheduleByDate() {
        //     ApiWrapper response = new ApiWrapper();
        //     response.Data = fetchSchedule(); 
        //     return JsonConvert.SerializeObject(response);
        // }

        /// <summary>
        ///     API Endpoint for fetching Telestaff Schedule for a specific date
        /// </summary>
        /// <param name="date">string Representing the date to fetch schedule for.</param>
        /// <returns> JSON formated string representation of the staffing information for the <paramref name="date" /> day.</returns>
        // [HttpGet("telestaff/schedule/{date}")]
        // public string GetRawScheduleByDate(string date) {
        //     ApiWrapper response = new ApiWrapper();
        //     try {
        //         DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", null);
        //         date = dt.ToString(_config["Telestaff:TimeFormat"]);
        //         response.Data = fetchSchedule(date); 
        //         return JsonConvert.SerializeObject(response);
        //     } catch (FormatException) {
        //         response.Data = new {Error = $"The date provided, {date}, does not appear to be in the format \"yyyyMMdd\"."};
        //         response.StatusCode = 500;
        //     }
        //     return JsonConvert.SerializeObject(response);
        // }

        /// <summary>
        ///     API Endpoint for fetching a Raw Telestaff Roster
        /// </summary>
        /// <returns> JSON formated string representation of the staffing information for the current staffing day.</returns>
        // [HttpGet("telestaff/roster")]
        // public string GetRawRoster() {
        //     ApiWrapper response = new ApiWrapper();
        //     response.Data = fetchRoster();
        //     return JsonConvert.SerializeObject(response);
        // }

        /// <summary>
        ///     API Endpoint for fetching a Raw Telestaff Roster
        /// </summary>
        /// <param name="date">string Representing the date to fetch a roster for</param>
        /// <returns> JSON formated string representation of the staffing information for the <paramref name="date" /> day.</returns>
        // [HttpGet("telestaff/roster/{date}")]
        // public string GetRawRosterByDate(string date) {
        //     ApiWrapper response = new ApiWrapper();
        //     try {
        //         DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", null);
        //         date = dt.ToString(_config["Telestaff:TimeFormat"]);
        //         response.Data = fetchRoster(date); 
        //         return JsonConvert.SerializeObject(response);
        //     } catch (FormatException) {
        //         response.Data = new {Error = $"The date provided, {date}, does not appear to be in the format \"yyyyMMdd\"."};
        //         response.StatusCode = 500;
        //     }
        //     return JsonConvert.SerializeObject(response);
        // }

        /// <summary>
        ///     API Endpoint for fetching Raw Telestaff Position Data
        /// </summary>
        /// <returns> JSON formated string repesenting an API Wrapped Position Data, direct from Telestaff.</returns>
        // [HttpGet("telestaff/position")]
        // public string GetRawPostions() {
        //     ApiWrapper response = new ApiWrapper();
        //     response.Data = fetchPositions();
        //     return JsonConvert.SerializeObject(response);
        // }
    }
}