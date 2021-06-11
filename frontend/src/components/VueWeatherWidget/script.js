import Utils from "./utils";
import WeatherIcons from "vue-weathericons";
import ShiftDate from "@/components/VueShiftDate";

export default {
  name: "VueWeatherWidget",

  components: {
    WeatherIcons,
    ShiftDate,
  },

  props: {
    // Controls whether to show or hide the title bar.
    hideHeader: {
      type: Boolean,
      default: false,
    },

    // Auto update interval in milliseconds
    updateInterval: {
      type: Number,
    },

    // Use static skycons
    disableAnimation: {
      type: Boolean,
      default: false,
    },

    // High Temperature Thereshold
    highTemperature: {
      type: Number,
      default: 90,
    },

    // Low Temperature Thereshold
    lowTemperature: {
      type: Number,
      default: 40,
    },

    // Color of the Temparature bar. Default: '#444'
    barColor: {
      type: String,
      // default: "#444",
      default: "#B3B3B3",
    },

    // Color of the text. Default: '#333'
    textColor: {
      type: String,
      // default: "#333",
      default: "#FFFFFF",
    },

    // Color for the temperature bar if above hot theshold
    hotColor: {
      type: String,
      default: "rgb(255, 1, 0 )",
    },

    // Color for the temperature bar if below cold theshold
    coldColor: {
      type: String,
      default: "rgb(121, 192, 255)",
    },

    // Number of daily forecasts to display
    numberForecasts: {
      type: Number,
      default: 5,
    },

    // Your positionstack api key for geocoding
    weatherUrl: {
      type: String,
      default: "https://localhost:5001/api/WeatherForecast/",
    },
  },

  data() {
    return {
      loading: true,
      weather: null,
      error: null,
      location: {},
      timeout: null,
    };
  },

  watch: {
    updateInterval: "hydrate",
  },

  mounted() {
    this.hydrate();
  },

  destroyed() {
    clearTimeout(this.timeout);
  },

  computed: {
    currently() {
      return this.weather.currently;
    },
    daily() {
      const forecasts = [];
      let globalMaxTemp = -Infinity;
      let globalMinTemp = Infinity;

      const tomorrow = new Date(new Date().toDateString());
      const today = tomorrow.getTime() / 1e3 + 24 * 3600 - 1;

      const daily = this.weather.daily.data;
      for (let i = 0; i < daily.length; i++) {
        const day = daily[i];
        if (day.temperatureMax > globalMaxTemp) {
          globalMaxTemp = day.temperatureMax;
        }
        if (day.temperatureMin < globalMinTemp) {
          globalMinTemp = day.temperatureMin;
        }
        forecasts.push(Object.assign({}, day));
      }

      const tempRange = globalMaxTemp - globalMinTemp;
      for (let i = 0; i < forecasts.length; ++i) {
        const day = forecasts[i];
        if (day.time <= today) {
          day.weekName = "Today";
        } else {
          day.weekName = new Date(day.time * 1000).toLocaleDateString(
            this.language,
            { weekday: "short" }
          );
        }
        const max = day.temperatureMax;
        const min = day.temperatureMin;
        day.height = Math.round((100 * (max - min)) / tempRange);
        day.top = Math.round((100 * (globalMaxTemp - max)) / tempRange);
        day.bottom = 100 - (day.top + day.height);
      }
      return forecasts;
    },
  },

  methods: {
    loadWeather() {
      return Utils.fetchWeather({
        url: this.weatherUrl,
      }).then((data) => {
        this.$set(this, "weather", data);
      });
    },

    autoupdate() {
      clearTimeout(this.timeout);
      const time = Number(this.updateInterval);
      if (!time || time < 10 || this.destroyed) {
        return;
      }
      this.timeout = setTimeout(() => this.hydrate(false), time);
    },

    hydrate(setLoading = true) {
      this.$set(this, "loading", setLoading);
      return this.$nextTick()
        .then(this.loadWeather)
        .then(() => {
          this.$set(this, "error", null);
        })
        .catch((err) => {
          this.$set(this, "error", "" + err);
        })
        .finally(() => {
          this.$set(this, "loading", false);
          this.autoupdate();
        });
    },
  },
};
