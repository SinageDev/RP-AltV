<template>
  <div @click="currentFocus">
    <div :class="$style.header">
      <span>{{ header }}</span>
      <span>{{ current === 255 ? 'Пусто' : current+1 }}</span>
    </div>
    <div :class="$style.switchers" v-horizontal-scroll>
      <div :class="[$style.switch, current === overlay ? $style.current : '']" v-for="(overlay, i) in select" :key="i"
           @click="value = i">
        <span v-if="overlay !== 255">{{ overlayIds ? overlay : i }}</span>
        <svg v-else :class="$style.zeroImage" viewBox="0 0 21 22" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path
              d="M21 1.25713L19.8001 0L10.5 9.74286L1.19999 0L0 1.25713L9.30001 11L0 20.7429L1.19999 22L10.5 12.2571L19.8001 22L21 20.7429L11.7 11L21 1.25713Z"
              fill="#fff"/>
        </svg>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import {vHorizontalScroll} from "../../../services/ScrollExtensions";
import {Directive} from "vue";

function currentFocus() {
  emit("updateFocus")
}

const props = defineProps<{
  header: string;
  select: number[];
  current: number;
  overlayIds?: boolean;
}>();

const emit = defineEmits<{
  (e: "update", value: number): void;
  (e: "updateFocus"): void;
}>();

const value = $computed({
  get() {
    return props.current;
  },
  set(value: number) {
    emit("update", props.select[value]);
  }
})
</script>

<style lang="stylus" module>
.header
  margin-top 1vh
  flex(cont:space-between)

.switchers
  margin-top 0.3vh
  flex()
  overflow-x auto
  overflow-y hidden
  padding-bottom 0.6vh

  &::-webkit-scrollbar
    appearance none
    height 0.6vh
    background rgba(255, 255, 255, 0.15)

  &::-webkit-scrollbar-thumb
    height 100%
    background rgba(255, 255, 255, 0.5)

.switch
  font-size 1.77vh
  flex(cont: center, ialign: center)
  position relative
  min-width 4.28vh
  min-height 4.28vh
  box-shadow 0 1vh 1vh 0.1vh rgba(62, 62, 62, 0.07)
  border 0.1vh solid #fff

  &:hover
    background rgba(211, 211, 211, 0.4)

  &.current
    color: #000
    background #fff
    font-weight 500

    & svg path
      fill: #242424

.zeroImage
  height 45%
</style>