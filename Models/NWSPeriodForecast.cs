using System;

namespace Dievas.Models {


    /// <summary>
    ///     Class <c>NWSPeriodForecast</c> holds weather forecast data for one period
    ///     as defined by api.weather.gov
    /// </summary>
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
        public NWSProbabilityOfPrecipitation ProbabilityOfPrecipitation { get; set; }
        public NWSDewPoint DewPoint { get; set; }
        public NWSRelativeHumidity RelativeHumidity { get; set; }

    }

    public class NWSProbabilityOfPrecipitation {
        public string UnitCode { get; set; }
        public string Value { get; set; }
    }

    public class NWSDewPoint {
        public string UnitCode { get; set; }
        public string Value { get; set; }    
    }

    public class NWSRelativeHumidity{
        public string UnitCode { get; set; }
        public string Value { get; set; }  
    }
}
