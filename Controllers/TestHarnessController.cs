using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFD.Dashboard.Models;
using Dievas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace Dievas.Controllers {

    /// <summary>
    ///     Controller Class <c>TestHarnessController</c> Provided an API to populate test data
    ///     
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TestHarnessController : ControllerBase {

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<TestHarnessController> _logger;

        /// <summary>
        ///     Web Client for this class
        /// </summary>
        private static HttpClient _http;

        /// <summary>
        ///     Flag for enabling this controller, endpoints are active iff this is true
        /// </summary>
        private static bool _enabled;

        /// <summary>
        ///     Holds a reference to the CAD state as maintained in a Singelton
        /// </summary>
        private static CAD _cad;
        
        /// <summary>
        ///     Default constructor for Class <c>TestHarnessController</c>
        /// </summary>
        /// <param name="configuration">IConfiguration configuration informaiton</param>
        /// <param name="logger">ILogger: aggregate logger</param>
        /// <param name="cad">CAD: Singleton Representing state from a CAD</param>
        public TestHarnessController(IConfiguration configuration,
                                   ILogger<TestHarnessController> logger,
                                   CAD cad) {

            Boolean.TryParse(configuration["TestHarness:Enabled"], out _enabled);

            // Only build out the API if its enabled
            if (_enabled) {
                _logger = logger;
                _cad = cad;

                bool allowInvalidCertificates = false;
                Boolean.TryParse(configuration["TestHarness:AllowInvalidCertificates"], out allowInvalidCertificates);


                HttpClientHandler httpClientHandler = new HttpClientHandler();

                // If our settings allow us, skip ceritifcate validation
                if (allowInvalidCertificates) {
                    httpClientHandler.ServerCertificateCustomValidationCallback = 
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                }

                // Initilize our HttpClient object
                _http = new HttpClient(httpClientHandler);

                // Set base address to Telestaff URL
                _http.BaseAddress = new Uri(configuration["TestHarness:Url"], UriKind.Absolute);
                
                // Set our user agent
                var productValue = new ProductInfoHeaderValue(configuration["TestHarness:User-agent"]);
                _http.DefaultRequestHeaders.UserAgent.Add(productValue);
            }
        }

        /// <summary>
        ///     Generates a 405 method not allowed API Response
        /// </summary>
        /// <param name="date">string Representing the date to fetch a roster for</param>
        /// <returns> JSON formated string representation of the staffing information for the <paramref name="date" /> day.</returns>
        private string disabledMessage() {
            ApiWrapper response = new ApiWrapper();
            response.Data = new {
                Error = "503 Service Unavalable",
                Message = "I'm a combined coffee/tea pot that is temporarily out of coffee."
            };
            response.StatusCode = 503;
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        ///     API Endpoint populates the incidnet list
        /// </summary>
        /// <returns> JSON formated string representation of the staffing information for the <paramref name="date" /> day.</returns>
        [HttpGet("incidents")]
        public string GetIncidents() {
            if (!_enabled) return disabledMessage();

            string endpoint  = "api/Dashboard/incidents";
            HttpResponseMessage result = _http.GetAsync(endpoint).Result;

            string apiResponse = result.Content.ReadAsStringAsync().Result;
            List<IncidentDto> testIncidents = JsonConvert.DeserializeObject<List<IncidentDto>>(apiResponse);
            foreach(IncidentDto incident in testIncidents){
                _cad.AddIncident(incident);
            }

            DateTime date = DateTime.Now;
            string message = $"{testIncidents.Count} units loaded at {date:HHL:mm}.";
            _logger.LogInformation(message);

            ApiWrapper response = new ApiWrapper {
                Data = message,
                StatusCode = 200,
            };
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        ///     API Endpoint for populating the units list
        /// </summary>
        /// <returns> JSON formated string repesenting an API Wrapped Position Data, direct from Telestaff.</returns>
        [HttpGet("units")]
        public string GetUnits() {
            if (!_enabled) return disabledMessage();
            
            string endpoint  = "api/Dashboard/units";
            HttpResponseMessage result = _http.GetAsync(endpoint).Result;

            string apiResponse = result.Content.ReadAsStringAsync().Result;
            List<UnitDto> testUnits = JsonConvert.DeserializeObject<List<UnitDto>>(apiResponse);
            _cad.PopulateUnitList(testUnits);

            DateTime date = DateTime.Now;
            string message = $"{testUnits.Count} units loaded at {date:HHL:mm}.";
            _logger.LogInformation(message);

            ApiWrapper response = new ApiWrapper {
                Data = message,
                StatusCode = 200,
            };
            return JsonConvert.SerializeObject(response);
        }
    }
}