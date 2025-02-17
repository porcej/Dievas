using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dievas.Models.WeatherForecast;

namespace Dievas.Models.WeatherForecast
{
    /// <summary>
    /// Represents the top-level response from the National Weather Service API.
    /// </summary>
    public class NWSWeatherForecastResponse
    {
        /// <summary>
        /// Gets or sets the properties containing weather forecast details.
        /// </summary>
        [JsonPropertyName("properties")]
        // public ForecastProperties Properties { get; set; }
        public NWSForecast Properties { get; set; }
    }
}