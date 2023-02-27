using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Dievas.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class TelestaffAPIProxyController : ControllerBase {

        private readonly IConfiguration _config;

        private readonly ILogger<TelestaffAPIProxyController> _logger;

        private static HttpClient _http = new HttpClient();

        // We only want to have the most recent forecast, so we will use 
        // in memory (concurrentbag) storage for the latest forecast
        private static NWSForecast _currentRoster = new NWSForecast();

        // We will use this to check if we need to update the forecast
        //  by default new DateTime() returns {01/01/0001 00:00:00}
        //  so current time should be newer, forecing an update
        private static DateTime _forecastExpiration = new DateTime();

        public TelestaffAPIProxyController(IConfiguration configuration,
                                        ILogger<TelestaffAPIProxyController> logger) {
            _config = configuration;
            _logger = logger;
            
            // Set out user agent
            var productValue = new ProductInfoHeaderValue(_config["Telestaff:User-agent"]);
            _http.DefaultRequestHeaders.UserAgent.Add(productValue);

            // Handle user authentication for all requests
            var byteArray = Encoding.ASCII.GetBytes($"{_config["Telestaff:Username"]}:{_config["Telestaff:Password"]}");
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        private static string parseTelestaffSchedule(string json) {
            try {
                // First we Parse the NWS Returne JSON
                JObject tsSchedJsonObj = JObject.Parse(json);

                // We will make a list of all the period JSON objects to make them easier to deal with
                IList<JToken> schedulesObj = tsSchedJsonObj["schedules"].Children().ToList();


                foreach (JToken periodForecast in nwsJsonObj["properties"]["periods"]) {
                    NWSPeriodForecast parsedPeriodForecast = periodForecast.ToObject<NWSPeriodForecast>();
                    nwsForecast.Periods.Add(parsedPeriodForecast);
                }


                                // Create an instance of NWSForecast to hold the NWS Forecast Data
                NWSForecast nwsForecast = new NWSForecast {
                    updated = (DateTime)nwsJsonObj["properties"]["updated"],
                    Units = nwsJsonObj["properties"]["units"].ToString(),
                    forecastGenerator = nwsJsonObj["properties"]["forecastGenerator"].ToString(),
                    generatedAt = (DateTime)nwsJsonObj["properties"]["generatedAt"],
                    updateTime = (DateTime)nwsJsonObj["properties"]["updateTime"],
                    
                    // Newtonsoft.JSON does not do a good job at parsing 
                    // durations, so we will address this later
                    // validTimes = (DateTime)nwsJsonObj["properties"]["validTimes"],
                    Periods = new List<NWSPeriodForecast>()
                };

                // Finally we serialize JSON results into Objects
                // IList<NWSPeriodForecast> periodForecasts = new List<NWSPeriodForecast>();


                _currentForecast = nwsForecast;
                return _currentForecast;

            } catch (Exception e) {
                string errorMsg = "JSON Parsing Issue: " + e.Message;
                
                // Get stack trace for the exception 
                // with source file information
                var st = new StackTrace(e, true);
                
                // Get the top stack frame
                var frame = st.GetFrame(0);
                
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                
                Console.WriteLine(errorMsg);
                Console.WriteLine("On line " + line.ToString());
                return new NWSForecast{ Units = errorMsg};
            }
        }

        // This can be updated later to provide better logic or tap directly into telestaff
        private static string fetchRoster(string apiEndpoint, string date = "",
                                          string startDate = "", string endDate = "") {

            string rawTelestaffData= "{\"status_code\": 500, \"data\": \"Something went wrong.\"}";

            try {
                if (string.IsNullOrWhiteSpace(startDate)) {
                    startDate = "TODAY";
                }
                if (string.IsNullOrWhiteSpace(endDate)) {
                    endDate = "TODAY";
                }
                if (!string.IsNullOrWhiteSpace(date)) {
                    startDate = date;
                    endDate = date;
                }

                var requestString = new StringContent(
                    $"{{\"fromDate\":\"{startDate}\",\"thruDate\": \"{endDate}\"}}",
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage result = _http.PostAsync(apiEndpoint, requestString).Result;

                rawTelestaffData = result.Content.ReadAsStringAsync().Result;

                JObject


            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return rawTelestaffData;
        }

        [HttpGet]
        public string Get() {
            return fetchRoster(apiEndpoint: _config["Telestaff:Url"]);
        }

        [HttpGet("{date}")]
        public string GetRosterByDate(string date) {
            return fetchRoster(
                    apiEndpoint: _config["Telestaff:Url"],
                    date: date
                );
        }

        [HttpGet("{startDate}/{endDate}")]
        public string GetRosterByDate(string startDate, string endDate) {
            return fetchRoster(
                    apiEndpoint: _config["Telestaff:Url"],
                    startDate: startDate,
                    endDate: endDate
                );
        }
    }
}
