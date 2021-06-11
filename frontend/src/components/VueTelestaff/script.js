import Utils from "./utils";
// import moment from "moment";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faStar,
  faCar,
  faFire,
  faUserMd,
  faSpinner,
  faExclamationTriangle,
  faSun,
  faMoon,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

library.add(
  faStar,
  faCar,
  faFire,
  faUserMd,
  faSpinner,
  faExclamationTriangle,
  faSun,
  faMoon
);

export default {
  name: "VueTelestaff",

  components: {
    FontAwesomeIcon,
  },

  props: {
    url: {
      type: String,
      default: "https://localhost:5001/api/TelestaffProxy/",
    },

    station: {
      type: String,
      required: true,
    },

    // Auto update interval in milliseconds, default is 15 minutes
    updateInterval: {
      type: Number,
      default: 15 * 60 * 1000,
    },

    // Roster Date
    date: {
      type: String,
      default: "",
    },
  },

  data() {
    return {
      loading: true,
      roster: null,
      error: null,
    };
  },

  mounted() {
    this.staff();
  },

  destroyed() {
    clearTimeout(this.timeout);
  },

  methods: {
    workingPositions(positions) {
      return positions.filter((position) => position.isWorking);
    },

    formatRank(position) {
      return Utils.getRank(position).icon;
    },

    formatName(name) {
      // return [name.replace(/\(.*$/g, "").trim(), name.replace(/^[^(]*|\(|\)/g, "").trim()];
      return name.replace(/\(.*$/g, "").trim();
    },

    formatTime(startTime, endTime) {
      const timeText = `${Utils.parseShiftTimes(
        startTime
      )} - ${Utils.parseShiftTimes(endTime)}`;
      if (timeText == "07:00 - 07:00") {
        return {
          isIcon: false,
          icon: null,
          text: "",
        };
      } else if (timeText == "07:00 - 19:00") {
        return {
          isIcon: true,
          icon: faSun,
          text: timeText,
        };
      } else if (timeText == "19:00 - 07:00") {
        return {
          isIcon: true,
          icon: faMoon,
          text: timeText,
        };
      }
      return {
        isIcon: false,
        icon: null,
        text: timeText,
      };
    },

    staff(setLoading = true) {
      this.$set(this, "loading", setLoading);
      return this.$nextTick()
        .then(this.loadRoster)
        .then(() => {
          this.$set(this, "error", null);
        })
        .catch((err) => {
          this.$set(this, "error", "" + err + err.stack);
        })
        .finally(() => {
          this.$set(this, "loading", false);
          this.autoupdate();
        });
    },

    autoupdate() {
      clearTimeout(this.timeout);
      const time = Number(this.updateInterval);
      if (!time || time < 10 || this.destroyed) {
        return;
      }
      this.timeout = setTimeout(() => this.staff(false), time);
    },

    loadRoster() {
      return Utils.fetchRoster({
        url: this.url,
        station: this.station,
        date: this.date,
      }).then((data) => {
        this.$set(this, "roster", data);
      });
    },
  },
};
