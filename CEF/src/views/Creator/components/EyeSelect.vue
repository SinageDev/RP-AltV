<template>
  <div>
    <div :class="$style.header">
      <span>Цвет глаз</span>
      <span>{{ eye+1 }}</span>
    </div>
    <div :class="$style.switchers" v-horizontal-scroll>
      <div :class="[$style.switch, eye === eyeId ? $style.current : '']" v-for="(eyeId, i) in eyeIds" :key="i"
           @click="eye = eyeId">
        {{ eyeId+1 }}
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import {vHorizontalScroll} from "../../../services/ScrollExtensions";
import {inject} from "vue";
import {IProvideCreator} from "../../../services/IProvide";

const { eye, eyeIds } = inject("creator") as IProvideCreator;
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
</style>