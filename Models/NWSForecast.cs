using System;
using System.Collections.Generic;
using Backend.Models;

namespace Dievas.Models {

    public class NWSForecast {
        public DateTime updated{ get; set; }
        public string Units { get; set; }
        public string forecastGenerator { get; set; }
        public DateTime generatedAt { get; set; }
        public DateTime updateTime { get; set; }
        public DateTime validTimes { get; set; }
        public List<NWSPeriodForecast> Periods { get; set; }
    }
}
