<template>
  <b-container fluid class="h-100 station-dashboard">
    <b-row class="h-100">
      <b-col cols="3" :style="wxBorderStyle"></b-col>
      <b-col :style="wxContentStyle">
        <!-- 900000ms = 15 minutes -->
        <vue-telestaff
          :station="$route.params.stationId"
          :updateInterval="15 * 60 * 1000"
          title="Today"
        />
      </b-col>
      <b-col :style="wxContentStyle">
        <!-- 900000ms = 15 minutes -->
        <vue-telestaff
          :station="$route.params.stationId"
          date="tomorrow"
          :updateInterval="15 * 60 * 1000"
        />
      </b-col>
    </b-row>
    <b-row class="footer fixed-bottom" :style="wxBorderStyle">
      <b-col cols="4" class="align-middle">
        <vue-clock :showBackground="false" />
      </b-col>
      <b-col cols="8">
        <vue-weather
          weatherUrl="https://localhost:5001/api/WeatherForecast/"
          :updateInterval="900000"
          :hideHeader="true"
        />
      </b-col>
    </b-row>
  </b-container>
</template>

<script>
import VueWeather from "@/components/VueWeatherWidget";
import VueClock from "@/components/VueDigitalClock";
import VueTelestaff from "@/components/VueTelestaff";

export default {
  components: {
    VueWeather,
    VueClock,
    VueTelestaff,
  },
  data() {
    return {
      wxBorderStyle: {
        backgroundColor: "#222222", // Vampire Christmas Theme
        // backgroundColor: "#050A30", // Blue Theme
        // backgroundColor: "#323031", // Coffee Theme
      },
      wxContentStyle: {
        backgroundColor: "inherit", // Vampire Christmas Theme
        // backgroundColor: "#000C66", // Blue Theme
        // backgroundColor: "#3d3b3c", // Coffee Theme
      },
    };
  },
};
</script>
<style>
.station-dashboard {
  font-family: "Frutiger CE 55 Roman";
  /*font-family: "Frutiger CE 65 Bold";*/
  color: #7ec8e3 !important;
}
</style>
