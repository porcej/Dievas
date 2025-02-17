using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Dievas.Models.WeatherForecast;

namespace Dievas.Services {


    /// <summary>
    ///     Background Service Class <c>WeatherForecastBackgroundService</c> fetches the
    ///     weather forecast from the National Weather Service at regular
    ///     intervals.   
    /// </summary>
    public class WeatherForecastBackgroundService : BackgroundService {


        /// <summary>
        ///     Application configuration for this Class
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        ///     Logging Controller for this class
        /// </summary>
        private readonly ILogger<WeatherForecastBackgroundService> _logger;

        /// <summary>
        ///     Weather API URL
        /// </summary>
        private readonly string _weatherApiEndPoint;

        /// <summary>
        ///     Web Client for this class
        /// </summary>
        private static HttpClient _http;



        /// <summary>
        ///     Default update interval value if not specified in appsettings.json
        /// </summary>
        private const int DefaultUpdateIntervalMinutes = 60;

        /// <summary>
        ///     Update Interval
        /// </summary>
        private TimeSpan _updateInterval;


        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherForecastBackgroundService"/> class.
        /// </summary>
        /// <param name="configuration">Configuration instance</param>
        /// <param name="logger">Logger instance</param>
        public WeatherForecastBackgroundService(
            IConfiguration configuration,
            ILogger<WeatherForecastBackgroundService> logger) {
            
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

            // Set base address to NWS API
            _http.BaseAddress = new Uri(_config["NWS:Url"], UriKind.Absolute);
            
            // Set our user agent
            var productValue = new ProductInfoHeaderValue(_config["NWS:User-agent"]);
            _http.DefaultRequestHeaders.UserAgent.Add(productValue);

            // Read interval from appsettings.json, defaulting to default interval if not provided
            int intervalMinutes = _config.GetValue<int>("NWS:UpdateIntervalMinutes", DefaultUpdateIntervalMinutes);
            _logger.LogInformation($"Weather update interval set to {intervalMinutes} minutes.");
            _updateInterval = TimeSpan.FromMinutes(intervalMinutes);
            _weatherApiEndPoint = _config["NWS:ForecastEndpoint"];

        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await FetchWeatherAsync();
                await Task.Delay(_updateInterval, stoppingToken);
            }
        }

        /// <summary>
        /// Fetches the weather forecast from the API and updates the singleton instance.
        /// </summary>
        private async Task FetchWeatherAsync() {
            try {
                _logger.LogInformation("Fetching weather forecast...");
                var nwsResponse = await _http.GetStringAsync(_weatherApiEndPoint);
                var weatherData = JsonSerializer.Deserialize<NWSWeatherForecastResponse>(nwsResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (weatherData?.Properties != null) {
                    WeatherSingleton.Instance.UpdateWeather(weatherData.Properties);
                    _logger.LogInformation("Weather forecast updated successfully.");
                } else {
                    _logger.LogWarning("Received invalid weather data.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather forecast.");
            }
        }
    }
}