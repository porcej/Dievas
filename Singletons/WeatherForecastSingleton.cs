using System;
using System.Collections.Generic;
using Dievas.Models.WeatherForecast;

namespace Dievas.Services
{
    /// <summary>
    /// Singleton class that stores the latest weather forecast data.
    /// </summary>
    public sealed class WeatherSingleton
    {
        private static readonly Lazy<WeatherSingleton> _instance = new(() => new WeatherSingleton());

        /// <summary>
        /// Gets the singleton instance of WeatherSingleton.
        /// </summary>
        public static WeatherSingleton Instance => _instance.Value;

        private WeatherSingleton() { }

        /// <summary>
        /// Gets the timestamp of the last forecast update.
        /// </summary>
        public DateTime LastUpdated { get; private set; }

        /// <summary>
        /// Gets the list of weather forecast periods.
        /// </summary>
        public NWSForecast Forecast { get; private set; } = new();

        /// <summary>
        /// Updates the weather forecast data.
        /// </summary>
        /// <param name="forecasts">The weather forecast.</param>
        public void UpdateWeather(NWSForecast forecast) {
            Forecast = forecast;
            // LastUpdated = lastUpdated;
        }
    }
}