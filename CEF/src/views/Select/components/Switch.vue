<template>
  <div :class="[$style.container, $style.left, !characters[current-1] ? $style.hide : '']" @click="characters[current-1] ? current-- : ''">
    <ArrowIcon :class="$style.icon"/>
  </div>
  <div :class="[$style.container, $style.right, !characters[current+1] ? $style.hide : '']" @click="characters[current+1] ? current++ : ''">
    <ArrowIcon :class="$style.icon"/>
  </div>
</template>

<script lang="ts" setup>
import ArrowIcon from "@images/select/arrow.svg";
import {inject} from "vue";
import {IProvideSelect} from "../../../services/IProvide";

const {current, characters} = inject('select') as IProvideSelect;
</script>

<style lang="stylus" module>
.container
  position absolute
  width 6vw
  height 100%
  background linear-gradient(90deg, rgba(0, 0, 0, 0.6375) 0%, rgba(0, 0, 0, 0.425) 100%)
  flex(cont: center, ialign: center)

  &.right
    right 0
    transform rotate(180deg)

  &.left
    left 0

  &.hide
    background linear-gradient(90deg, rgba(0, 0, 0, 0.375) 0%, rgba(0, 0, 0, 0.25) 100%);

    & path
      fill-opacity 0.5

.icon
  width 5vw
</style>