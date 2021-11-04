using System;
using System.Collections.Generic;
using Backend.Models;

namespace Backend.Models {

    public class NWSForecast {
        public DateTime Updated{ get; set; }
        public string Units { get; set; }
        public string ForecastGenerator { get; set; }
        public DateTime GeneratedAt { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime ValidTimes { get; set; }
        public List<NWSPeriodForecast> Periods { get; set; }
    }
}
