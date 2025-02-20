using System;
using System.Text.Json.Serialization;
using Dievas.Models.WeatherForecast;

namespace Dievas.Models.WeatherForecast {

    /// <summary>
    /// Represents a single forecast period with weather details.
    /// </summary>
    public class NWSPeriodForecast {

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("startTime")]
        public DateTime startTime { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime endTime { get; set; }

        [JsonPropertyName("isDaytime")]
        public bool isDayTime { get; set; }

        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }

        [JsonPropertyName("temperatureUnit")]
        public string temperatureUnit { get; set; }

        [JsonPropertyName("windSpeed")]
        public string windSpeed { get; set; }

        [JsonPropertyName("windDirection")]
        public string windDirection { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("shortForecast")]
        public string shortForecast { get; set; }

        [JsonPropertyName("detailedForecast")]
        public string detailedForecast { get; set; }
    }
}
