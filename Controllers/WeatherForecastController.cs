using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
// using System.Net.Http.Json;
using System.Threading.Tasks;
using Dievas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Dievas.Controllers {


    /// <summary>
    ///     Controller Class <c>WeatherForecastController</c> Provides an API to access weather information
    ///     
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase {

        /// <summary>
        ///     Application configuration for this Class
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<WeatherForecastController> _logger;

        /// <summary>
        ///     Web Client for this class
        /// </summary>
        private static HttpClient _http;

        /// <sumary>
        ///     We only want to have the most recent forecast, so we will use 
        ///     a static property to hold it across instances
        /// </summary>
        private static NWSForecast _currentForecast = new NWSForecast();

        /// <sumary>
        ///     We will use this to check if we need to update the forecast
        ///     by default new DateTime() returns {01/01/0001 00:00:00}
        ///     so current time should be newer, forecing an update
        /// </summary>
        private static DateTime _forecastExpiration = new DateTime();


        /// <summary>
        ///     Default constructor for Class <c>TelestaffController</c>
        /// </summary>
        /// <param name="configuration">IConfiguration configuration informaiton</param>
        /// <param name="logger">ILogger: aggregate logger</param>
        public WeatherForecastController(
            IConfiguration configuration,
            ILogger<WeatherForecastController> logger) {

            _config = configuration;
            _logger = logger;

            HttpClientHandler httpClientHandler = new HttpClientHandler();

            bool allowInvalidCertificates = false;

            Boolean.TryParse(_config["NWS:AllowInvalidCertificates"], out allowInvalidCertificates);

            // If our settings allow us, skip ceritifcate validation
            if (allowInvalidCertificates) {
                httpClientHandler.ServerCertificateCustomValidationCallback = 
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }


            // Initilize our HttpClient object
            _http = new HttpClient(httpClientHandler);

            // Set base address to Telestaff URL
            _http.BaseAddress = new Uri(_config["NWS:Url"], UriKind.Absolute);
            
            // Set our user agent
            var productValue = new ProductInfoHeaderValue(_config["NWS:User-agent"]);
            _http.DefaultRequestHeaders.UserAgent.Add(productValue);

        }

        // We could place the logic in this method inside of the get method,
        // it is pulled out for clarity
        private static NWSForecast fetchNWSForecast(ILogger<WeatherForecastController> logger, string forecastEndpoint) {

            // Check if _forecastExpiration is in the future, if so just return
            // current forecast.  We wrap it in a try...catch to handle the 
            // case where _currentForecast does not yet exist.
            try  {
                int experationDelta = DateTime.Compare(_forecastExpiration,
                                                       DateTime.Now);
                if (experationDelta > 0) {
                    logger.LogInformation($"Retuning existing forecast that expires {_forecastExpiration.ToString()}.");
                    return _currentForecast;
                }

            } catch (Exception e) {
                logger.LogError(e, e.Message);
            }

            string nwsJsonRaw = "";

            try {
                logger.LogInformation($"Fetching Weather Forecast.");

                HttpResponseMessage result = _http.GetAsync(
                    forecastEndpoint).Result;

                nwsJsonRaw = result.Content.ReadAsStringAsync().Result;

                _forecastExpiration = DateTime.Parse(result.Content.Headers.GetValues("Expires").FirstOrDefault());

                logger.LogInformation($"Weather Forecast loaded, expiration set to {_forecastExpiration.ToString("F")}.");

            } catch (Exception e) {
                logger.LogError(e, e.Message);
            }

            try {
                // First we Parse the NWS Returne JSON
                JObject nwsJsonObj = JObject.Parse(nwsJsonRaw);

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
            return fetchNWSForecast(_logger, _config["NWS:ForecastEndpoint"]);
        }
    }
}