const ICON_MAPPINGS = {
  skc: {
    description: "Fair/clear",
    day: "day-sunny",
    night: "night-clear",
  },
  few: {
    description: "A few clouds",
    day: "day-sunny-overcast",
    night: "night-partly-cloudy",
  },
  sct: {
    description: "Partly cloudy",
    day: "day-cloudy",
    night: "night-partly-cloudy",
  },
  bkn: {
    description: "Mostly cloudy",
    day: "day-cloudy",
    night: "night-cloudy",
  },
  ovc: {
    description: "Overcast",
    day: "day-sunny-overcast",
    night: "night-alt-cloudy",
  },
  wind_skc: {
    description: "Fair/clear and windy",
    day: "day-windy",
    night: "windy",
  },
  wind_few: {
    description: "A few clouds and windy",
    day: "day-cloudy-windy",
    night: "night-cloudy-windy",
  },
  wind_sct: {
    description: "Partly cloudy and windy",
    day: "day-cloudy-windy",
    night: "night-cloudy-windy",
  },
  wind_bkn: {
    description: "Mostly cloudy and windy",
    day: "cloudy-windy",
    night: "cloudy-windy",
  },
  wind_ovc: {
    description: "Overcast and windy",
    day: "cloudy-windy",
    night: "cloudy-windy",
  },
  snow: {
    description: "Snow",
    day: "day-snow",
    night: "night-snow",
  },
  rain_snow: {
    description: "Rain/snow",
    day: "day-rain-mix",
    night: "night-rain-mix",
  },
  rain_sleet: {
    description: "Rain/sleet",
    day: "day-rain-mix",
    night: "night-rain-mix",
  },
  snow_sleet: {
    description: "Rain/sleet",
    day: "day-rain-mix",
    night: "night-rain-mix",
  },
  fzra: {
    description: "Freezing rain",
    day: "day-sleet",
    night: "night-sleet",
  },
  rain_fzra: {
    description: "Rain/freezing rain",
    day: "day-sleet",
    night: "night-sleet",
  },
  snow_fzra: {
    description: "Freezing rain/snow",
    day: "day-snow",
    night: "night-snow",
  },
  sleet: {
    description: "Sleet",
    day: "day-sleet",
    night: "night-sleet",
  },
  rain: {
    description: "Rain",
    day: "day-rain",
    night: "night-alt-rain",
  },
  rain_showers: {
    description: "Rain showers (high cloud cover)",
    day: "day-showers",
    night: "night-alt-showers",
  },
  rain_showers_hi: {
    description: "Rain showers (low cloud cover)",
    day: "day-showers",
    night: "night-alt-showers",
  },
  tsra: {
    description: "Thunderstorm (high cloud cover)",
    day: "thunderstorm",
    night: "thunderstorm",
  },
  tsra_sct: {
    description: "Thunderstorm (medium cloud cover)",
    day: "day-thunderstorm",
    night: "night-thunderstorm",
  },
  tsra_hi: {
    description: "Thunderstorm (low cloud cover)",
    day: "day-thunderstorm",
    night: "night-thunderstorm",
  },
  tornado: {
    description: "Tornado",
    day: "tornado",
    night: "tornado",
  },
  hurricane: {
    description: "Hurricane conditions",
    day: "hurricane-warning",
    night: "hurricane-warning",
  },
  tropical_storm: {
    description: "Tropical storm conditions",
    day: "storm-warning",
    night: "storm-warning",
  },
  dust: {
    description: "Dust",
    day: "dust",
    night: "dust",
  },
  smoke: {
    description: "Smoke",
    day: "smoke",
    night: "smoke",
  },
  haze: {
    description: "Haze",
    day: "day-haze",
    night: "night-fog",
  },
  hot: {
    description: "Hot",
    day: "hot",
    night: "hot",
  },
  cold: {
    description: "Cold",
    day: "thermometer-exterior",
    night: "thermometer-exterior",
  },
  blizzard: {
    description: "Blizzard",
    day: "snow",
    night: "snow",
  },
  fog: {
    description: "Fog/mist",
    day: "day-fog",
    night: "night-fog",
  },
};

const utils = {
  fetchWeather(opts = {}) {
    return fetch(opts["url"])
      .then((resp) => resp.json())
      .then(utils.mapData);
  },

  mapData(data) {
    const { periods } = data;
    const current = periods.shift();
    return {
      currently: Object.assign({}, current, {
        icon: utils.mapIcon(current.icon),
        temperature: current.temperature,
        summary: current.shortForecast,
      }),
      daily: {
        data: periods.reduce((daily, period, pdx, periods) => {
          if (period.name.includes("Tonight")) {
            daily.push({
              name: period.name,
              icon: utils.mapIcon(period.icon),
              temperatureMin: period.temperature,
              temperatureMax: period.temperature,
            });
          } else if (!period.name.includes("Night")) {
            var temperatureMin = 0;
            if (typeof periods[pdx + 1] === "undefined") {
              temperatureMin = period.temperature;
            } else {
              temperatureMin = periods[pdx + 1].temperature;
            }
            daily.push({
              name: period.name,
              icon: utils.mapIcon(period.icon),
              temperatureMin: temperatureMin,
              temperatureMax: period.temperature,
            });
          }
          return daily;
        }, []),
      },
      hourly: {
        data: [{ temperature: 50 }],
      },
    };
  },

  mapIcon(NWSicon) {
    const timeOfDay = NWSicon.split("/")[5];
    const icon = NWSicon.split("/")[6].split("?")[0].split(",")[0];
    return ICON_MAPPINGS[icon][timeOfDay];
  },
};

export default utils;
