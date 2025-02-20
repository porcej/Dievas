using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Dievas.Models;
using Dievas.Models.Telestaff;
using Dievas.Models.Staffing;

namespace Dievas.Services {


    /// <summary>
    ///     Background Service Class <c>TelestaffBackgroundtService</c> fetches
    ///     staffing data from Telestaff at regular intervals.   
    /// </summary>
    public class TelestaffBackgroundService : BackgroundService {


        /// <summary>
        ///     Application configuration for this Class
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<TelestaffBackgroundService> _logger;

        /// <summary>
        ///     Web Client for this class
        /// </summary>
        private readonly HttpClient _http;

        /// <summary>
        ///     Default update interval value if not specified in appsettings.json
        /// </summary>
        private const int DefaultUpdateIntervalMinutes = 15;

        /// <summary>
        ///     Update Interval
        /// </summary>
        private TimeSpan _updateInterval;


        /// <summary>
        ///     Initializes a new instance of the <see cref="TelestaffBackgroundService"/> class.
        /// </summary>
        /// <param name="configuration">Configuration instance</param>
        /// <param name="logger">Logger instance</param>
        public TelestaffBackgroundService(
            IConfiguration configuration,
            ILogger<TelestaffBackgroundService> logger) {
            
            _config = configuration;
            _logger = logger;

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


            // Set Cookies if provided
            string cookies = _config.GetValue<string>("Telestaff:Cookies", "");
            if (!string.IsNullOrEmpty(cookies)) {
                _logger.LogInformation("TelestaffBackgroundService: Using cookies for web requests.");
                _http.DefaultRequestHeaders.Add("Cookie", cookies);
            }
            
            // Set our user agent
            var productValue = new ProductInfoHeaderValue(_config["Telestaff:User-agent"]);
            _http.DefaultRequestHeaders.UserAgent.Add(productValue);

            // Handle user authentication for all requests
            var byteArray = Encoding.ASCII.GetBytes($"{_config["Telestaff:Username"]}:{_config["Telestaff:Password"]}");
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Read interval from appsettings.json, defaulting to default interval if not provided
            int intervalMinutes = _config.GetValue<int>("Telestaff:UpdateIntervalMinutes", DefaultUpdateIntervalMinutes);
            _logger.LogInformation($"TelestaffBackgroundService: Telestaff update interval set to {intervalMinutes} minutes.");
            _updateInterval = TimeSpan.FromMinutes(intervalMinutes);

        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await FetchTelestaffAsync();
                await Task.Delay(_updateInterval, stoppingToken);
            }
        }

        /// <summary>
        /// Fetches Telestaff Roster Data from the API and updates the singleton instance.
        /// </summary>
        private async Task FetchTelestaffAsync() {
            List<DateTime> dates = new List<DateTime> {
                DateTime.Today,                // Today's date
                DateTime.Today.AddDays(1),     // Tomorrow's date
                DateTime.Today.AddDays(2)      // Day after tomorrow
            };

            try {
                _logger.LogInformation("TelestaffBackgroundService: Fetching Telestaff Roster...");
                foreach (DateTime date in dates){
                    StaffingRoster roster = fetchTelestaffRoster(date.ToString(_config["Telestaff:TimeFormat"]));
                    StaffingCache cachedRoster = new StaffingCache(roster, Convert.ToDouble(_config["Telestaff:ExpirationTimeInMinutes"]));

                    StaffingSingleton.Instance.AddRoster(cachedRoster, date);
                }
                _logger.LogInformation("TelestaffBackgroundService: Roster updated from Telestaff successfully.");

            } catch (Exception ex) {
                _logger.LogError(ex, "Error fetching roster from Telestaff: {Message}", ex.Message);
            }

            try {
                    StaffingSingleton.Instance.CleanRosters();
            }  catch (Exception ex) {
                _logger.LogError(ex, "Error pruning old rosters from Telestaff: {Message}", ex.Message);
            }
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

                _logger.LogInformation($"TelestaffBackgroundService: Fetching Telestaff {endpoint} with options {opts}.");

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


            _logger.LogInformation($"TelestaffBackgroundService: Fetching Roster from {opts}.");
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
            _logger.LogInformation($"TelestaffBackgroundService: Fetching {organizationType} nodes from Telestaff");
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
    }
}