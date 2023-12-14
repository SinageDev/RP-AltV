<template>
  <div :class="$style.container">
    <CheckIcon :class="{[$style.check]: true, [$style.on]: breakPress}"/>
    <div :class="$style.speed">
      <div :class="$style.speedCount"
           :style="{ color: speed > vehicleMaxSpeed/100*90 ? 'red' : 'rgb(255, 255, 255)' }">
        {{ speed.toString().padStart(3, '0') }}
      </div>
      <div>КМ/Ч</div>
    </div>
    <svg :class="$style.speedLine" viewBox="0 0 185 204" fill="none" xmlns="http://www.w3.org/2000/svg">
      <circle cx="50%" cy="50%" r="78" ref="speedView" :stroke="rpm > 75 ? 'rgb(255, 0, 0)' : 'rgb(255, 255, 255)'"
              stroke-width="8" fill="none"/>
      <circle cx="50%" cy="50%" r="90" stroke-dasharray="362" stroke-width="1"
              :stroke="rpm > 75 ? 'rgb(255, 0, 0)' : 'rgb(255, 255, 255)'"/>
      <circle cx="50%" cy="50%" r="82" stroke-dasharray="330" stroke-width="16" stroke="url(#paint0_radial_606_620)"/>
      <defs>
        <radialGradient id="paint0_radial_606_620" cx="0" cy="0" r="1" gradientUnits="userSpaceOnUse"
                        gradientTransform="translate(102 102) rotate(90) scale(102)">
          <stop :stop-color="rpm > 75 ? 'rgb(255, 0, 0)' : 'rgb(255, 255, 255)'"/>
          <stop offset="1" :stop-color="rpm > 75 ? 'rgb(255, 0, 0)' : 'rgb(255, 255, 255)'" stop-opacity="0"/>
        </radialGradient>
      </defs>
    </svg>
    <svg :class="$style.fuelLine" viewBox="0 0 185 204" fill="none" xmlns="http://www.w3.org/2000/svg">
      <g :style="{ stroke: getFuelStrokeColor() }" stroke-width="8" fill="none">
        <circle cx="50%" cy="50%" r="78" stroke-width="8" :stroke-dasharray="`${fuel <= 25 ? fuel/5*4 : 20} 600`"
                stroke-dashoffset="0"/>
        <circle v-if="fuel>=25" cx="50%" cy="50%" r="78" :stroke-dasharray="`${fuel <= 50 ? (fuel-25)/5*4 : 20} 600`"
                stroke-dashoffset="-22"/>
        <circle v-if="fuel>=50" cx="50%" cy="50%" r="78" :stroke-dasharray="`${fuel <= 75 ? (fuel-50)/5*4 : 20} 600`"
                stroke-dashoffset="-44"/>
        <circle v-if="fuel>=75" cx="50%" cy="50%" r="78" :stroke-dasharray="`${fuel <= 100 ? (fuel-75)/5*4 : 20} 600`"
                stroke-dashoffset="-66"/>
      </g>
      <circle cx="50%" cy="50%" r="90" stroke-dasharray="98 600" stroke-width="1" stroke="#fff"/>
    </svg>
    <div :class="$style.iconsContainer">
      <IndicateEngineIcon :class="engineState ? $style.indicateOn : $style.indicateOff"/>
      <IndicateDoorIcon/>
      <IndicateRemIcon/>
      <IndicateLightIcon/>
    </div>
    <FuelIcon :class="$style.fuel"/>
  </div>
</template>

<script lang="ts" setup>
import CheckIcon from "@images/speedometer/check.svg";
import FuelIcon from "@images/speedometer/fuel.svg";
import IndicateDoorIcon from "@images/speedometer/indicate-door.svg";
import IndicateEngineIcon from "@images/speedometer/indicate-engine.svg";
import IndicateRemIcon from "@images/speedometer/indicate-rem.svg";
import IndicateLightIcon from "@images/speedometer/indicate-light.svg";
import {onMounted, ref, watch} from "vue";
import {Mp} from "../../../services/Alt";

let speed = $ref(100);
let zeroElement = $ref("");
const speedView = ref<SVGCircleElement | null>(null);
let rpm = $ref(100);
watch(() => rpm, () => {
  if (speedView === null) return;
  const strokeLength = speedView.value!.getTotalLength();
  speedView.value!.style.strokeDasharray = `${strokeLength * rpm / 156}, ${strokeLength}`;
});
let fuel = $ref(100);

const getFuelStrokeColor = () => {
  if (fuel <= 25) return `#D63636`;
  if (fuel <= 50) return `#D67036`;
  if (fuel <= 75) return `#D6B336`;
  return "#ffd20a";
}
let breakPress = $ref(false);

const updateSpeed = (Rpm: number, Speed: number, Break: boolean) => {
  speed = Speed;
  rpm = Rpm;
  breakPress = Break;
}
const updateFuel = (count: number) => fuel = count;

let engineState = $ref(false);

let vehicleMaxSpeed = $ref(0);

Mp.events.temp("Speedo:UpdateSpeed", updateSpeed);
Mp.events.temp("Speedo:UpdateFuel", updateFuel);
Mp.events.temp("Speedo:UpdateEngine", (state: boolean) => engineState = state);
Mp.events.temp("Speedo:SetMaxSpeed", (max: number) => vehicleMaxSpeed = max);

onMounted(() => rpm = 0);
</script>

<style lang="stylus" module>
.container
  pos(bottom:5vh, right:5vw)
  width 11vw
  height 19vh
  flex(column, space-evenly, ialign:center)
  text-align center
  font-size 1.7vh
  font-family DinDisplay
  padding 1vw

.speedCount
  font-family LcdMono2
  font-weight 700
  letter-spacing 0.2vw
  font-size 4.4vh

.speedLine
  height 25vh
  transform rotate(90deg)
  pos()

.iconsContainer
  flex(row, center, ialign:center);

  & svg
    width 0.8vw
    margin-right 0.3vw

.check
  height 2vh
  &.on g
    fill red
    fill-opacity 1
    filter url("#red_brake")

.fuelLine
  height 25vh
  transform scaleY(-1) rotate(-65deg)
  transform-origin center
  pos()

.fuel
  pos(bottom:0.2vh, right:-1vw)
  height 2vh

.indicateOn
  & g
    fill #6BFF5F

.indicateOff
  & g
    fill white
    fill-opacity 0.7
</style>