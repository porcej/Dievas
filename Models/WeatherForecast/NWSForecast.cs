using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dievas.Models.WeatherForecast;

namespace Dievas.Models.WeatherForecast {


    /// <summary>
    /// Contains the weather forecast properties, including the update timestamp and forecast periods.
    /// </summary>
    public class NWSForecast {

        [JsonPropertyName("updated")]
        public DateTime updated{ get; set; }

        [JsonPropertyName("units")]
        public string Units { get; set; }

        [JsonPropertyName("forecastGenerator")]
        public string forecastGenerator { get; set; }

        [JsonPropertyName("generatedAt")]
        public DateTime generatedAt { get; set; }

        [JsonPropertyName("updateTime")]
        public DateTime updateTime { get; set; }

        [JsonPropertyName("validTimes")]
        public string validTimes { get; set; }

        [JsonPropertyName("periods")]
        public List<NWSPeriodForecast> Periods { get; set; }
    }
}
