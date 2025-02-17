// using System;
// using System.Collections.Generic;
// using System.Collections.Concurrent;
// using System.Diagnostics;
// using System.IO;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Net.Http.Headers;
// // using System.Net.Http.Json;
// using System.Threading.Tasks;
// using Dievas.Models;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Dievas.Models;
using Dievas.Models.WeatherForecast;
using Dievas.Services;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json.Linq;

namespace Dievas.Controllers {


    /// <summary>
    ///     Controller Class <c>WeatherForecastController</c> Provides an API to access weather information
    ///     
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase {

        // /// <summary>
        // ///     Application configuration for this Class
        // /// </summary>
        // private readonly IConfiguration _config;

        // /// <summary>
        // ///     Logging Controller for this class
        // /// </summary>
        // private readonly ILogger<WeatherForecastController> _logger;



        [HttpGet]
        public NWSForecast Get() {
            var weatherData = WeatherSingleton.Instance;
            return weatherData.Forecast;
        }
    }
}