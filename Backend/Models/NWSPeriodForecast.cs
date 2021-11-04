using System;

namespace Backend.Models {

    public class NWSPeriodForecast {
        public int Number { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDayTime { get; set; }
        public int Temperature { get; set; }
        public string TemperatureUnit { get; set; }
        public string WindSpeed { get; set; }
        public string WindDirection { get; set; }
        public string Icon { get; set; }
        public string ShortForecast { get; set; }
        public string DetailedForecast { get; set; }
    }
}
