using System;

namespace Backend.Models {

    public class NWSPeriodForecast {
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
        public string shortForecast { get; set; }
        public string detailedForecast { get; set; }
    }
}
