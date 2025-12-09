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
using Newtonsoft.Json;
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
            _logger.LogInformation($"TelestaffBackgroundService: Trace logging enabled: {_logger.IsEnabled(LogLevel.Trace)}");
            _updateInterval = TimeSpan.FromMinutes(intervalMinutes);

        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                try {
                    await FetchTelestaffAsync();
                } catch (Exception ex) {
                    _logger.LogError(ex, "Unexpected error in TelestaffBackgroundService main loop: {Message}", ex.Message);
                    // Continue running even if there's an error
                }
                
                try {
                    await Task.Delay(_updateInterval, stoppingToken);
                } catch (TaskCanceledException) {
                    // Expected when cancellation is requested
                    _logger.LogInformation("TelestaffBackgroundService is stopping.");
                    break;
                }
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
                var fetchTasks = dates.Select(date => FetchAndCacheRosterAsync(date));
                await Task.WhenAll(fetchTasks);
                _logger.LogInformation("TelestaffBackgroundService: Roster updated from Telestaff successfully.");

            } catch (Exception ex) {
                _logger.LogError(ex, "Error fetching roster from Telestaff: {Message}", ex.Message);
            }

            try {
                _logger.LogTrace("Cleaning old rosters fromt Telestaff Cache.");
                List<DateTime> removedDates = StaffingSingleton.Instance.CleanRosters();
                _logger.LogTrace("Removed {Count} old rosters from Telestaff Cache.", removedDates.Count);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error pruning old rosters from Telestaff: {Message}", ex.Message);
            }
        }

        /// <summary>
        ///     Fetches Telestaff Roster Data from the API and updates the singleton instance.
        /// </summary>
        /// <param name="date">DateTime Representing the date to fetch a roster for</param>
        private async Task FetchAndCacheRosterAsync(DateTime date) {
            try {
                StaffingRoster roster = await fetchTelestaffRosterAsync(date.ToString(_config["Telestaff:TimeFormat"]));
                StaffingCache cachedRoster = new StaffingCache(roster, Convert.ToDouble(_config["Telestaff:ExpirationTimeInMinutes"]));
                StaffingSingleton.Instance.AddRoster(cachedRoster, date);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error fetching and caching roster for date {Date}: {Message}", date.ToString("yyyy-MM-dd"), ex.Message);
                // Don't rethrow - allow other dates to be fetched
            }
        }

        /// <summary>
        ///     Fetches Information from Telestaff's API and returns it as a JSON string
        /// </summary>
        /// <param name="endpoint">Relative URL for TS API endpoint</param>
        /// <param name="opts">Options to pass to the TS API</param>
        /// <returns> JSON formated string representation of the staffing information or empty string on error</returns>
        private async Task<string> FetchStringAsync(string endpoint, string opts="{}") {
            try {
                var requestString = new StringContent(
                    opts,
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                _logger.LogInformation($"TelestaffBackgroundService: Fetching Telestaff {endpoint} with options {opts}.");
                
                if (_logger.IsEnabled(LogLevel.Trace)) {
                    _logger.LogTrace("Telestaff Request - Endpoint: {Endpoint}, Request Body: {RequestBody}", endpoint, opts);
                }

                var result = await _http.PostAsync(endpoint, requestString);
                string content = await result.Content.ReadAsStringAsync();
                
                if (_logger.IsEnabled(LogLevel.Trace)) {
                    _logger.LogTrace("Telestaff Response - Endpoint: {Endpoint}, Status: {StatusCode}, Response Body: {ResponseBody}", 
                        endpoint, result.StatusCode, content);
                }
                
                if (!result.IsSuccessStatusCode) {
                    _logger.LogWarning($"Telestaff API returned status code {result.StatusCode} for endpoint {endpoint}");
                    return "";
                }
                
                if (string.IsNullOrWhiteSpace(content)) {
                    _logger.LogWarning($"Telestaff API returned empty response for endpoint {endpoint}");
                    return "";
                }
                
                return content;

            } catch (HttpRequestException ex) {
                _logger.LogError(ex, "Network error fetching from Telestaff {endpoint}: {Message}", endpoint, ex.Message);
                return "";
            } catch (TaskCanceledException ex) {
                _logger.LogWarning(ex, "Request timeout fetching from Telestaff {endpoint}", endpoint);
                return "";
            } catch (Exception e) {
                _logger.LogError(e, "Unexpected error fetching from Telestaff {endpoint}: {Message}", endpoint, e.Message);
                return "";
            }
        }

        /// <summary>
        ///     Fetches Information from Telestaff's API and returns it as a JSON Object
        /// </summary>
        /// <param name="endpoint">Relative URL for TS API endpoint</param>
        /// <param name="opts">Options to pass to the TS API</param>
        /// <returns> JSON Object representation of the staffing information, or empty JObject on error</returns>
        private async Task<JObject> FetchJObjectAsync(string endpoint, string opts="{}") {
            string apiResponse = await FetchStringAsync(endpoint, opts);
            
            if (string.IsNullOrWhiteSpace(apiResponse)) {
                _logger.LogWarning("FetchJObjectAsync: Empty response for endpoint {Endpoint}, returning empty JObject", endpoint);
                return new JObject();
            }
            
            try {
                JObject jsonObj = JObject.Parse(apiResponse);
                
                // Check for Telestaff API failure response
                JToken successToken = jsonObj["success"];
                if (successToken != null && successToken.Type == JTokenType.Boolean && !successToken.Value<bool>()) {
                    string message = jsonObj["message"]?.Value<string>() ?? "Unknown error";
                    _logger.LogError("Telestaff API failure for endpoint {Endpoint}: {Message}", endpoint, message);
                    return new JObject();
                }
                
                return jsonObj;
            } catch (JsonReaderException ex) {
                _logger.LogError(ex, "Failed to parse JSON for endpoint {Endpoint}. Response length: {Length}", endpoint, apiResponse.Length);
                return new JObject();
            }
        }

        /// <summary>
        ///     Fetches Information from Telestaff's API and returns it as a JSON string
        /// </summary>
        /// <param name="endpoint">Relative URL for TS API endpoint</param>
        /// <param name="opts">Options to pass to the TS API</param>
        /// <returns> JSON formated string representation of the staffing information or </returns>
        // private string fetchString(string endpoint, string opts="{}") {

        //     string apiResponse = "";
        //     try {
        //         var requestString = new StringContent(
        //             opts,
        //             System.Text.Encoding.UTF8,
        //             "application/json"
        //         );

        //         _logger.LogInformation($"TelestaffBackgroundService: Fetching Telestaff {endpoint} with options {opts}.");

        //         HttpResponseMessage result = _http.PostAsync(
        //             endpoint,
        //             requestString).Result;

        //         apiResponse = result.Content.ReadAsStringAsync().Result;

        //     } catch (Exception e) {
        //         _logger.LogError(e, e.Message);
        //     }

        //     return apiResponse;
        // }

        /// <summary>
        ///     Fetches Information from Telestaff's API and returns it as a JSON string
        /// </summary>
        /// <param name="endpoint">Relative URL for TS API endpoint</param>
        /// <param name="opts">Options to pass to the TS API</param>
        /// <returns> JSON Object representation of the staffing information </returns>
        // private JObject fetchJObject(string endpoint, string opts="{}") {
        //     string apiResponse = fetchString(endpoint, opts);
        //     return JObject.Parse(apiResponse);
        // }

        /// <summary>
        ///     Fetches Schedule Information from Telestaff's API
        /// </summary>
        /// <param name="date">string Representing the date to fetch a roster for</param>
        /// <param name="startDate">string Representing the date of the first roster when fetching multiple rosters</param>
        /// <param name="endDate">string Representing the date of the last roster when fetching multiple rosters</param>
        private async Task<List<DaySchedule>> fetchScheduleAsync(string date = "",
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

            JObject scheduleJsonObj = await FetchJObjectAsync(
                TS.ScheduleEndpoint,
                $"{{\"fromDate\":\"{startDate}\",\"thruDate\":\"{endDate}\"}}"
            );

            List<DaySchedule> schedules = new List<DaySchedule>();

            if (scheduleJsonObj["schedules"] != null) {
                foreach (JToken scheduleJson in scheduleJsonObj["schedules"]) {
                    schedules.Add(scheduleJson.ToObject<DaySchedule>());
                }
            }
            return schedules;
        }

        /// <summary>
        ///     Fetches All Schedule Records for date and returns them as a single list
        /// </summary>
        /// <param name="date">string Representing the date to fetch staffing information for</param>
        /// <returns> List of <paramref name="PersonSchedule"/> objects representing staffing for <paramref name="date"/>.</returns>
        private async Task<List<PersonSchedule>> fetchAllPersonSchedulesAsync(string date = "") {

            // Ensure we have a date
            if (string.IsNullOrWhiteSpace(date)) {
                DateTime now = DateTime.Now;
                date = now.ToString(_config["Telestaff:TimeFormat"]);
            }

            // Create a request string for the staffing records
            string opts = $"{{\"fromDate\":\"{date}\",\"thruDate\":\"{date}\"}}";

            // Fetch data as JSON from Telestaff API - since this is only a single day, we only care about the first record
            var scheduleJsonObjs = await FetchJObjectAsync(TS.ScheduleEndpoint, opts);
            
            if (scheduleJsonObjs["schedules"] == null || !scheduleJsonObjs["schedules"].Any()) {
                _logger.LogWarning("No schedules found in response for date {Date}", date);
                return new List<PersonSchedule>();
            }
            
            JToken scheduleJsonObj = scheduleJsonObjs["schedules"][0];

            // This will form the basis of our response
            List<PersonSchedule> schedules = new List<PersonSchedule>();

            if (scheduleJsonObj["schedule"] != null) {
                foreach (JToken scheduleJson in scheduleJsonObj["schedule"]) {
                    if (scheduleJson["personSchedule"] != null) {
                        foreach (JToken personScheduleJson in scheduleJson["personSchedule"]){
                            schedules.Add(personScheduleJson.ToObject<PersonSchedule>());
                        }
                    }
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
        private async Task<List<Roster>> fetchRosterAsync(string date = "",
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
            string rosterData = await FetchStringAsync(TS.RosterEndpoint, opts);

            if (string.IsNullOrWhiteSpace(rosterData)) {
                _logger.LogWarning("TelestaffBackgroundService: Received empty roster data, returning empty list.");
                return new List<Roster>();
            }

            try {
                JObject rosterJsonObj = JObject.Parse(rosterData);

                // Check for Telestaff API failure response
                JToken successToken = rosterJsonObj["success"];
                if (successToken != null && successToken.Type == JTokenType.Boolean && !successToken.Value<bool>()) {
                    string message = rosterJsonObj["message"]?.Value<string>() ?? "Unknown error";
                    _logger.LogError("Telestaff API failure for roster endpoint: {Message}", message);
                    return new List<Roster>();
                }

                List<Roster> rosters = new List<Roster>();

                if (rosterJsonObj["rosters"] != null) {
                    foreach (JToken rosterJson in rosterJsonObj["rosters"]) {
                        rosters.Add(rosterJson.ToObject<Roster>());
                    }
                }

                return rosters;
            } catch (JsonReaderException ex) {
                _logger.LogError(ex, "Failed to parse roster JSON data. Data length: {Length}", rosterData?.Length ?? 0);
                return new List<Roster>();
            }
        }

        /// <summary>
        ///     Fetches Positions Information from Telestaff's API
        /// </summary>
        /// <param name="date">string Representing the date to fetch a roster for</param>
        /// <param name="startDate">string Representing the date of the first roster when fetching multiple rosters</param>
        /// <param name="endDate">string Representing the date of the last roster when fetching multiple rosters</param>
        private async Task<List<PositionNode>> fetchPositionsAsync() {
            JObject postionsJsonObj = await FetchJObjectAsync(TS.PositionsEndpoint);

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
        private async Task<List<OrganizationNode>> fetchOrganizationLevelAsync(string organizationType, bool enabled=true) {
            List<OrganizationNode> organizations = new List<OrganizationNode>();
            string requestJson = $"{{\"type\": \"{organizationType}\"}}";
            JObject organizationJObject = await FetchJObjectAsync(TS.OrginizationEndpoint, requestJson);
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
        private async Task<List<OrganizationNode>> fetchActiveNodesAsync(string nodeType="UNIT") {
            List<OrganizationNode> nodes = new List<OrganizationNode>();

            if (!TS.IsValidOrganizationNode(nodeType)){
                throw new ArgumentException($"{nodeType} is not a known organization level ['{String.Join("/", TS.OrganizationNodes)}']");
            }

            List<int> activeIds = new List<int>();

            for (int ndx = 0; ndx < TS.OrganizationNodes.Length; ndx++){
                nodes = (await fetchOrganizationLevelAsync(TS.OrganizationNodes[ndx]))
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
        private async Task<StaffingRoster> fetchTelestaffRosterAsync(string staffingDate="") {

            List<Roster> rosters = await fetchRosterAsync(staffingDate);
            if (rosters == null || rosters.Count == 0) {
                _logger.LogWarning("No rosters returned for date {Date}, returning empty StaffingRoster", staffingDate);
                return new StaffingRoster(DateTime.Today, new List<StaffingRecord>());
            }

            Roster roster = rosters[0];
            List<PersonSchedule> schedules = await fetchAllPersonSchedulesAsync(staffingDate);
            
            List<DaySchedule> daySchedules = await fetchScheduleAsync(staffingDate);
            if (daySchedules == null || daySchedules.Count == 0) {
                _logger.LogWarning("No schedules returned for date {Date}, returning empty StaffingRoster", staffingDate);
                return new StaffingRoster(DateTime.Today, new List<StaffingRecord>());
            }
            
            DaySchedule staffingSchedule = daySchedules[0];
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
                    Schedule personSchedule = staffingSchedule.Schedule?.Find(s => s.Person?.EmployeeId.ToString() == record.Badge);

                    if (personSchedule == null || personSchedule.PersonSchedule == null) {
                        record.Id = -2;
                        record.Title = "";
                        record.PositionDisplayName = "";
                        records.Add(record);
                        continue;
                    }

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