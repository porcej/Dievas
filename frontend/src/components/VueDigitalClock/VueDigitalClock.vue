<template>
  <div class="vwdc_clock">
    <div class="vwdc_date">{{ date }}</div>
    <div class="vwdc_time">{{ time }}</div>
    <div v-if="showBackground" class="vwdc_background">
      {{ backgroundString }}
    </div>
  </div>
</template>

<script>
import moment from "moment";

export default {
  name: "VueDigitalClock",

  props: {
    //  Determines if this is a counter or clock
    counter: {
      type: Boolean,
      default: false,
    },

    countDown: {
      type: Boolean,
      default: false,
    },

    // For counter mode, determines counter start
    counterStart: {
      type: Number,
      default: 0,
    },

    // Clock format
    timeFormat: {
      type: String,
      default: "HH:mm:ss",
    },

    // Clock format
    dateFormat: {
      type: String,
      default: "dddd, MMMM Do YYYY",
    },

    // Update resolution for clock in ms
    updateInterval: {
      type: Number,
      default: 1000,
    },

    // Determined weather the background should be shown
    showBackground: {
      type: Boolean,
      default: true,
    },
  },

  data() {
    return {
      timeout: null,
      time: null,
      date: null,
      backgroundString: null,
    };
  },

  mounted() {
    this.windClock();
  },

  destroyed() {
    clearTimeout(this.timeout);
  },

  methods: {
    windClock() {
      const timeStr = moment().format(this.format);
      this.backgroundString = timeStr.replace(/[a-z0-9]/gim, "8");
      this.clockTick();
    },

    clockTick() {
      clearTimeout(this.timeout);
      const time = Number(this.updateInterval);
      if (!time || time < 10 || this.destroyed) {
        return;
      }
      this.time = moment().format(this.timeFormat);
      if (this.dateFormat && this.dateFormat !== "") {
        this.date = moment().format(this.dateFormat);
      }
      this.timeout = setTimeout(() => this.clockTick(), time);
    },
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

@font-face {
  font-family: 'Digital-7 Mono';
  src: url('./fonts/Digital-7Mono.eot');
  src: url('./fonts/Digital-7Mono.eot?#iefix') format('embedded-opentype'),
    url('./fonts/Digital-7Mono.woff2') format('woff2'),
    url('./fonts/Digital-7Mono.woff') format('woff'),
    url('./fonts/Digital-7Mono.ttf') format('truetype'),
    url('./fonts/Digital-7Mono.svg#Digital-7Mono') format('svg');
  font-weight: normal;
  font-style: normal;
}

.vwdc_time,
.vwdc_date,
.vwdc_background {
  text-align: center;
  /*font-size: 20vmin;*/
  font-size: 100px;
  /* 
    font-family: "Share Tech Mono", monospace;
    font-family: "Digital-7 Mono";
  */
  color: rgb(200, 20, 0);
  line-height: 1em;
  vertical-align: middle;
  height: 100%;
  width: 100%;
  top: 0;
  left: 0;
}

.vwdc_time {
  z-index: 2;
  text-shadow: 0 0 20px rgb(200, 20, 0);
}

.vwdc_date {
  font-size: 40px;
  text-shadow: 0 0 20px rgb(200, 20, 0);
}

.vwdc_background {
  position: absolute;
  z-index: 1;
  opacity: 0.2;
}

.vwdc_clock {
  position: relative;
  padding: 15px;
}

@media (max-height: 720px) {
  .vwdc_time,
  .vwdc_date,
  .vwdc_background {
    font-size: 24px;
  }
}
</style>
