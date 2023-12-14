<template>
  <div>
    <div :class="$style.header">
      <span>{{ header }}</span>
      <span>{{ color+1}}</span>
    </div>
    <div :class="$style.colors" v-horizontal-scroll>
      <div :class="[$style.color, color===i ? $style.current : '']" v-for="(value, i) in overlayId === 5 || overlayId === 8 ? colorsLip : colors" :key="i" :style="{ background: value }" @click="color = i">
        <CurrIcon v-if="color === i" :class="$style.curr"/>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import CurrIcon from "@images/ok.svg";
import {vHorizontalScroll} from "../../../services/ScrollExtensions";
import {Directive} from "vue";

const props = defineProps<{
  header: string;
  overlayId?: number;
  current: number;
}>();

const color = $computed({
  get() {
    return props.current;
  },
  set(value: number) {
    emit("update", value);
  }
})

console.log(props.overlayId);

const emit = defineEmits<{
  (e: "update", value: number): void;
}>();
const colorsLip = [
  "#991C27",
  "#B33058",
  "#B04962",
  "#AA5F74",
  "#9E4B61",
  "#A63A46",
  "#7E2828",
  "#955D54",
  "#B08370",
  "#AF9287",
  "#B58984",
  "#A06E57",
  "#A8604B",
  "#9B492F",
  "#A36E71",
  "#B27C8C",
  "#C794AA",
  "#C0719A",
  "#C03379",
  "#A3456A",
  "#6A1A2D",
  "#4D171F",
  "#9F1B27",
  "#C51E35",
  "#B70B15",
  "#C34A68",
  "#C033A2",
  "#AB1FA0",
  "#951198",
  "#680E6F",
  "#67105F",
  "#550F56",
  "#65108E",
  "#143170",
  "#11449C",
  "#1579B2",
  "#1295AF",
  "#18B6BB",
  "#17B697",
  "#23B27A",
  "#139533",
  "#15720D",
  "#6FBE3A",
  "#B3C929",
  "#BABE27",
  "#CEC11F",
  "#C6AC1E",
  "#CA861D",
  "#CA5A0E",
  "#995520",
  "#CDB57E",
  "#C8BFAA",
  "#C8C5C3",
  "#A4A29D",
  "#8D8C85",
  "#534A4A",
  "#150F0F",
  "#508D92",
  "#4C6F86",
  "#162657",
  "#9C7B68",
  "#82604E",
  "#695141",
  "#372A25"
];
const colors = [
  '#1c1f21',
  '#272a2c',
  '#312e2c',
  '#35261c',
  '#4b321f',
  '#5c3b24',
  '#6d4c35',
  '#6b503b',
  '#765c45',
  '#7f684e',
  '#99815d',
  '#a79369',
  '#af9c70',
  '#bba063',
  '#d6b97b',
  '#dac38e',
  '#9f7f59',
  '#845039',
  '#682b1f',
  '#61120c',
  '#640f0a',
  '#7c140f',
  '#a02e19',
  '#b64b28',
  '#a2502f',
  '#aa4e2b',
  '#626262',
  '#808080',
  '#aaaaaa',
  '#c5c5c5',
  '#463955',
  '#5a3f6b',
  '#763c76',
  '#ed74e3',
  '#eb4b93',
  '#f299bc',
  '#04959e',
  '#025f86',
  '#023974',
  '#3fa16a',
  '#217c61',
  '#185c55',
  '#b6c034',
  '#70a90b',
  '#439d13',
  '#dcb857',
  '#e5b103',
  '#e69102',
  '#f28831',
  '#fb8057',
  '#e28b58',
  '#d1593c',
  '#ce3120',
  '#ad0903',
  '#880302',
  '#1f1814',
  '#291f19',
  '#2e221b',
  '#37291e',
  '#2e2218',
  '#231b15',
  '#020202',
  '#706c66',
  '#9d7a50'
]
</script>

<style lang="stylus" module>
.header
  margin-top 1vh
  flex(cont:space-between)

.colors
  flex()
  margin-top 0.3vh
  padding-bottom 0.6vh
  overflow-x auto
  overflow-y hidden
  &::-webkit-scrollbar
    appearance none
    height 0.6vh
    background rgba(255, 255, 255, 0.15)

  &::-webkit-scrollbar-thumb
    height 100%
    background rgba(255, 255, 255, 0.5)

.color
  flex(cont: center, ialign: center)
  min-width 4.28vh
  min-height 4.28vh
  box-shadow 0 1vh 1vh 0.1vh rgba(62, 62, 62, 0.07)

  &.current
    border: 0.1vw solid #fff

.curr
  width 40%
  & path
    stroke-width 0.1vw
</style>