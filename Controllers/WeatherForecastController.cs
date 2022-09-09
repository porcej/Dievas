using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Dievas.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase {

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IConfiguration _config;

        // We only want to have the most recent forecast, so we will use 
        // in memory (concurrentbag) storage for the latest forecast
        private static NWSForecast _currentForecast = new NWSForecast();

        // We will use this to check if we need to update the forecast
        //  by default new DateTime() returns {01/01/0001 00:00:00}
        //  so current time should be newer, forecing an update
        private static DateTime _forecastExpiration = new DateTime();

        public WeatherForecastController(
            IConfiguration configuration,
            ILogger<WeatherForecastController> logger) {

            _config = configuration;
            _logger = logger;
        }

        // We could place the logic in this method inside of the get method,
        // it is pulled out for clarity
        private static NWSForecast fetchNWSForecast(string baseUrl,
                                                    string userAgent) {

            // Check if _forecastExpiration is in the future, if so just return
            // current forecast.  We wrap it in a try...catch to handle the 
            // case where _currentForecast does not yet exist.
            try  {
                int experationDelta = DateTime.Compare(_forecastExpiration,
                                                       DateTime.Now);
                if (experationDelta > 0) {
                    return _currentForecast;
                }

            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            string _nwsJsonRaw = "";

            try {
                WebClient client = new WebClient();
                client.Headers.Add("User-agent", userAgent);
                Stream data = client.OpenRead(baseUrl);
                
                StreamReader reader = new StreamReader(data);
                _nwsJsonRaw = reader.ReadToEnd();
                data.Close();
                reader.Close();

                // Here we set the forecast expiration
                WebHeaderCollection myWebHeaderCollection = client.ResponseHeaders;  
                _forecastExpiration = DateTime.Parse(client.ResponseHeaders["Expires"]);

            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            try {
                // First we Parse the NWS Returne JSON
                JObject nwsJsonObj = JObject.Parse(_nwsJsonRaw);

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
                
                // We will make a list of all the period JSON objects to make them easier to deal with
                IList<JToken> periodsObj = nwsJsonObj["properties"]["periods"].Children().ToList();

                // Finally we serialize JSON results into Objects
                // IList<NWSPeriodForecast> periodForecasts = new List<NWSPeriodForecast>();
                foreach (JToken periodForecast in nwsJsonObj["properties"]["periods"]) {
                    NWSPeriodForecast parsedPeriodForecast = periodForecast.ToObject<NWSPeriodForecast>();
                    nwsForecast.Periods.Add(parsedPeriodForecast);
                }

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

        [HttpGet]
        public NWSForecast Get() {
            return fetchNWSForecast(_config["NWS:Url"],
                                    _config["NWS:User-agent"]);
        }
    }
}