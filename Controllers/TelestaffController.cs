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
        ///     Fetches Information from Telestaff's API and returns it as a JSON string
        /// </summary>
        /// <param name="endpoint">Relative URL for TS API endpoint</param>
        /// <param name="opts">Options to pass to the TS API</param>
        /// <returns> JSON formated string representation of the staffing information or </returns>
        private string fetchString(string endpoint, string opts="{}") {

            string apiResponse = "";
            try {
                var requestString = new StringContent(
                    opts,
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                _logger.LogInformation($"Fetching Telestaff {endpoint} with options {opts}.");

                HttpResponseMessage result = _http.PostAsync(
                    endpoint,
                    requestString).Result;

                apiResponse = result.Content.ReadAsStringAsync().Result;

            } catch (Exception e) {
                _logger.LogError(e, e.Message);
            }

            return apiResponse;
        }

        /// <summary>
        ///     Fetches Information from Telestaff's API and returns it as a JSON string
        /// </summary>
        /// <param name="endpoint">Relative URL for TS API endpoint</param>
        /// <param name="opts">Options to pass to the TS API</param>
        /// <returns> JSON Object representation of the staffing information </returns>
        private JObject fetchJObject(string endpoint, string opts="{}") {
            string apiResponse = fetchString(endpoint, opts);
            return JObject.Parse(apiResponse);
        }

        /// <summary>
        ///     Fetches Schedule Information from Telestaff's API
        /// </summary>
        /// <param name="date">string Representing the date to fetch a roster for</param>
        /// <param name="startDate">string Representing the date of the first roster when fetching multiple rosters</param>
        /// <param name="endDate">string Representing the date of the last roster when fetching multiple rosters</param>
        private List<DaySchedule> fetchSchedule(string date = "",
                                                string startDate = "",
                                                string endDate = "") {

            DateTime now = DateTime.Now;
            string nowString = now.ToString(_config["Telestaff:TimeFormat"]);

            if (string.IsNullOrWhiteSpace(startDate)) {
                startDate = nowString;
            }
            if (string.IsNullOrWhiteSpace(endDate)) {
                endDate = nowString;
            }
            if (!string.IsNullOrWhiteSpace(date)) {
                startDate = date;
                endDate = date;
            }

            JObject scheduleJsonObj = fetchJObject(
                TS.ScheduleEndpoint,
                $"{{\"fromDate\":\"{startDate}\",\"thruDate\":\"{endDate}\"}}"
            );

            List<DaySchedule> schedules = new List<DaySchedule>();

            foreach (JToken scheduleJson in scheduleJsonObj["schedules"]) {
                schedules.Add(scheduleJson.ToObject<DaySchedule>());
            }
            return schedules;
        }

        /// <summary>
        ///     Fetches All Schedule Records for date and returns them as a single list
        /// </summary>
        /// <param name="date">string Representing the date to fetch staffing information for</param>
        /// <returns> List of <paramref name="PersonSchedule"/> objects representing staffing for <paramref name="date"/>.</returns>
        private List<PersonSchedule> fetchAllPersonSchedules(string date = "") {

            // Ensure we have a date
            if (string.IsNullOrWhiteSpace(date)) {
                DateTime now = DateTime.Now;
                date = now.ToString(_config["Telestaff:TimeFormat"]);
            }

            // Create a request string for the staffing records
            string opts = $"{{\"fromDate\":\"{date}\",\"thruDate\":\"{date}\"}}";

            // Fetch data as JSON from Telestaff API - since this is only a single day, we only care about the first record
            JToken scheduleJsonObj = fetchJObject(TS.ScheduleEndpoint, opts)["schedules"][0];

            // This will form the basis of our response
            List<PersonSchedule> schedules = new List<PersonSchedule>();

            foreach (JToken scheduleJson in scheduleJsonObj["schedule"]) {
                foreach (JToken personScheduleJson in scheduleJson["personSchedule"]){
                    schedules.Add(personScheduleJson.ToObject<PersonSchedule>());
                }
            }
            return schedules;
        }

        /// <summary>
        ///     Fetches Roster Information from Telestaff's API
        /// </summary>
        /// <param name="date">string Representing the date to fetch a roster for</param>
        /// <param name="startDate">string Representing the date of the first roster when fetching multiple rosters</param>
        /// <param name="endDate">string Representing the date of the last roster when fetching multiple rosters</param>
        private List<Roster> fetchRoster(string date = "",
                                          string startDate = "",
                                          string endDate = "") {

            DateTime now = DateTime.Now;
            string nowString = now.ToString(_config["Telestaff:TimeFormat"]);

            if (string.IsNullOrWhiteSpace(startDate)) {
                startDate = nowString;
            }
            if (string.IsNullOrWhiteSpace(endDate)) {
                endDate = nowString;
            }
            if (!string.IsNullOrWhiteSpace(date)) {
                startDate = date;
                endDate = date;
            }

            string opts =  $"{{\"fromDate\":\"{startDate}\",\"thruDate\":\"{endDate}\",\"includeRequestRecords\": true}}";

            _logger.LogInformation($"Fetching Roster from {opts}.");
            string rosterData = fetchString(TS.RosterEndpoint, opts);

            JObject rosterJsonObj = JObject.Parse(rosterData);

            List<Roster> rosters = new List<Roster>();

            foreach (JToken rosterJson in rosterJsonObj["rosters"]) {
                rosters.Add(rosterJson.ToObject<Roster>());
            }

            return rosters;
        }

        /// <summary>
        ///     Fetches Positions Information from Telestaff's API
        /// </summary>
        /// <param name="date">string Representing the date to fetch a roster for</param>
        /// <param name="startDate">string Representing the date of the first roster when fetching multiple rosters</param>
        /// <param name="endDate">string Representing the date of the last roster when fetching multiple rosters</param>
        private List<PositionNode> fetchPositions() {
            JObject postionsJsonObj = fetchJObject(TS.PositionsEndpoint);

            List<PositionNode> positions = new List<PositionNode>();

            foreach (JToken positionJson in postionsJsonObj["organizationNodes"]) {
                PositionNode position = positionJson.ToObject<PositionNode>();
                positions.Add(position);
            }

            return positions;
        }

        /// <summary>
        ///     Fetches all the nodes from a Telestaff organizatinoal level
        /// </summary>
        /// <param name="orgnizationType">string Representing TS Type of the Organization ['INSTITUTION/AGENCY/REGION/STATION/UNIT']</param>
        /// <param name="enabled">boolean if set to true only returns enabled nodes</param>
        /// <returns> Dictonary of <paramref name="OrganizationNodes"/> keyed by organizational ID</returns>
        private List<OrganizationNode> fetchOrganizationLevel(string organizationType, bool enabled=true) {
            List<OrganizationNode> organizations = new List<OrganizationNode>();
            string requestJson = $"{{\"type\": \"{organizationType}\"}}";
            JObject organizationJObject = fetchJObject(TS.OrginizationEndpoint, requestJson);
            _logger.LogInformation($"Fetching {organizationType} nodes from Telestaff");
            foreach (JToken organizationJson in organizationJObject["organizationNodes"]) {
                if (enabled){
                    if( (bool)organizationJson["enabled"]) {
                        organizations.Add(organizationJson.ToObject<OrganizationNode>());
                    }    
                } else {
                    organizations.Add(organizationJson.ToObject<OrganizationNode>());
                }
            }
            return organizations;
        }

        /// <summary>
        ///     Returns a list of active organizational nodes, transverse the organizational structure to remove
        ///     children of inactive parents
        /// </summary>
        /// <param name="nodeType">string Representing TS Type of the Organization ['INSTITUTION/AGENCY/REGION/STATION/UNIT']</param>
        /// <param name="enabled">boolean if set to true only returns enabled nodes</param>
        /// <returns> Dictonary of <paramref name="OrganizationNodes"/> keyed by organizational ID</returns>
        /// <exception cref="ArgumentException">If nodeType is not a known organization level ['INSTITUTION/AGENCY/REGION/STATION/UNIT'].</exception>
        private List<OrganizationNode> fetchActiveNodes(string nodeType="UNIT") {
            List<OrganizationNode> nodes = new List<OrganizationNode>();

            if (!TS.IsValidOrganizationNode(nodeType)){
                throw new ArgumentException($"{nodeType} is not a known organization level ['{String.Join("/", TS.OrganizationNodes)}']");
            }

            List<int> activeIds = new List<int>();

            for (int ndx = 0; ndx < TS.OrganizationNodes.Length; ndx++){
                nodes = fetchOrganizationLevel(TS.OrganizationNodes[ndx])
                            .FindAll(u => u.Enabled && (ndx == 0 || activeIds.Contains(u.Parent.ParentId)));

                // If this is the org level we care about return the results
                if (TS.IsValidOrganizationNode(nodeType)) return nodes;
                activeIds = nodes.Select(u => u.OrganizationId).ToList();
            }
            // there be dragons here
            return  nodes;
        }

        /// <summary>
        ///     Fetches Staffing records for use by Dievas Dashboards
        /// </summary>
        /// <param name="date">string Representing the date to fetch a roster for</param>
        /// <param name="startDate">string Representing the date of the first roster when fetching multiple rosters</param>
        /// <param name="endDate">string Representing the date of the last roster when fetching multiple rosters</param>
        private StaffingRoster fetchTelestaffRoster(string staffingDate="") {

            Roster roster = fetchRoster(staffingDate)[0];
            List<PersonSchedule> schedules = fetchAllPersonSchedules(staffingDate);
            DaySchedule staffingSchedule = fetchSchedule(staffingDate)[0];
            List<StaffingRecord> records = new List<StaffingRecord>();

            foreach (RosterRecord rosterRecord in roster.Records) {

                StaffingRecord record = new StaffingRecord(rosterRecord);

                // Request won't have a associated schedule records so we will skip for now
                if (record.IsRequest) {
                    record.Id = -1;
                    record.Title = "";
                    record.PositionDisplayName = "";
                } else {

                    // Find the person's schedule
                    Schedule personSchedule = staffingSchedule.Schedule.Find(s => s.Person.EmployeeId.ToString() == record.Badge);

                    // PersonSchedule thisSchedule = schedules.Find(t => t.StaffingNoIn == rosterRecord.StaffingNoIn);
                    PersonSchedule thisSchedule = personSchedule.PersonSchedule.Find(
                        t => $"{t.StartDate.ToString("yyyyMMdd", null)}{t.StartTime.ToString("HHmm", null)}" == record.StartTime.ToString("yyyyMMddHHmm"));

                    if (thisSchedule != null) {
                        if (thisSchedule.Shift != null) 
                            record.ShiftName = thisSchedule.Shift.Name ?? "";

                        if (thisSchedule.WorkCode != null) {
                            record.WorkCode = thisSchedule.WorkCode.Name ?? "";
                            record.IsWorking = TS.IsWorkingCodeType(thisSchedule.WorkCode.Type);
                        }
                        if (thisSchedule.Organization != null) {
                            record.InstitutionName = thisSchedule.Organization.Institution.Name ?? "";
                            if (thisSchedule.Organization.Agency != null) {
                                record.AgencyName = thisSchedule.Organization.Agency.Name ?? "";
                                record.RegionName = thisSchedule.Organization.Region.Name ?? "";
                            }
                            if (thisSchedule.Organization.Station != null) {
                                record.StationName = thisSchedule.Organization.Station.Name ?? "";
                            } else {
                                record.StationName = "";
                            }
                            if (thisSchedule.Organization.Position != null ) {
                                record.Id = thisSchedule.Organization.Position.Id;
                                record.Title = thisSchedule.Organization.Position.Name;
                                record.PositionDisplayName = thisSchedule.Organization.Position.DisplayName;
                            } else {
                                // This person is not assigned to a known position
                                record.Id = 0;
                                record.Title = "";
                                record.PositionDisplayName = "";
                            }
                        }
                    } else {
                        record.Id = -2;
                        record.Title = "";
                        record.PositionDisplayName = "";
                    }
                }
                records.Add(record);
            }
            return new StaffingRoster (roster.Date, records);
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