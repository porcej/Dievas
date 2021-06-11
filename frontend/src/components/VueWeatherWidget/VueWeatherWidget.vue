<template>
  <div class="vww__widget justify-content-center" :style="{ color: textColor }">
    <slot name="header">
      <div
        class="vww__header"
        :style="{ borderColor: barColor }"
        v-if="!hideHeader"
      >
        <slot name="title">
          <span class="vww__title">
            <shift-date format="dddd, MMMM Do YYYY" />
          </span>
        </slot>
      </div>
    </slot>

    <div class="vww__content">
      <div class="vww__loading" v-if="loading">
        <slot name="loading">
          <weather-icons :icon="'day-cloudy'" />
          <span class="vww__title">Loading...</span>
        </slot>
      </div>

      <div
        class="vww__error"
        v-else-if="error || !weather || !currently || !daily"
      >
        <slot name="error">
          <weather-icons :icon="'day-rain'" />
          <span class="vww__title">{{ error || "Something went wrong!" }}</span>
        </slot>
      </div>

      <template v-else>
        <div class="vww__currently">
          <div>
            <span width="80px" height="80px" style="display: block">
              <weather-icons :icon="currently.icon" :fixed="true" />
            </span>
            <div
              class="vww__temp"
              :style="[
                highTemperature <= currently.temperature
                  ? { color: hotColor }
                  : lowTemperature >= currently.temperature
                  ? { color: coldColor }
                  : {},
              ]"
            >
              {{ Math.round(currently.temperature) }}&deg;
            </div>
          </div>
          <div class="vww__summary">{{ currently.shortForecast }}</div>
          <div class="vww__wind">
            Wind: {{ currently.windSpeed }} ({{ currently.windDirection }})
          </div>
        </div>

        <div class="vww__daily">
          <div class="vww__day" :key="day.name" v-for="day in daily">
            <span>{{ day.name }}</span>
            <span>
              <weather-icons :icon="day.icon" :fixed="true" />
            </span>
            <div class="vww__day-bar">
              <div :style="{ height: `${day.top}%` }">
                <span>{{ Math.round(day.temperatureMax) }}&deg;</span>
              </div>
              <div
                :style="{
                  borderRadius: '10px',
                  background: barColor,
                  height: `${day.height}%`,
                }"
              >
                &nbsp;
              </div>
              <div
                v-if="day.temperatureMax !== day.temperatureMin"
                :style="{ height: `${day.bottom}%` }"
              >
                <span>{{ Math.round(day.temperatureMin) }}&deg;</span>
              </div>
            </div>
          </div>
        </div>
      </template>
    </div>
  </div>
</template>

<script src="./script.js"></script>

<style src="./style.css"></style>
