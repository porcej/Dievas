using System;

namespace backend.Models
{
    public class NWSPeriodForecast
    {
        public int Number { get; set; }

        public string Name { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        public bool isDayTime { get; set; }

        public int Temperature { get; set; }

        public string temperatureUnit { get; set; }

        public string windSpeed { get; set; }

        public string windDirection { get; set; }

        public string Icon { get; set; }

        public string shortSummary { get; set; }

        public string Summary { get; set; }
    }
}
