<template>
<div :class="$style.container">
  <img :class="[$style.image, id === select[i] ? $style.curr : '']" :src="image" v-for="(image, i) in images" :key="i" @click="id = i">
</div>
</template>

<script lang="ts" setup>
const props = defineProps<{
  images: string[];
  current: number;
  select: number[];
}>()

const id = $computed({
  get() {
    return props.current;
  },
  set(value: number) {
    emit("update", props.select[value]);
  }
})

const emit = defineEmits<{
  (e: "update", val: number): void;
}>();
</script>

<style lang="stylus" module>
.container
  margin-top 0.25vh
  padding 0.2vh 0.2vh
  width 100%
  display grid
  grid-template-columns repeat(5, 1fr)
  grid-row-gap 0.6vh
  grid-column-gap 0.6vh
  overflow-y auto
  overflow-x hidden
  &::-webkit-scrollbar
    appearance none
    width 0.25vw
    background rgba(255, 255, 255, 0.15)

  &::-webkit-scrollbar-thumb
    width 100%
    background rgba(255, 255, 255, 0.5)

.image
  width 100%
  opacity 0.25
  box-shadow 0 1.4vh 1.4vh 0.3vh rgba(62, 62, 62, 0.07)
  &.curr
    box-shadow 0 0 0 0.1vh rgba(255, 255, 255, 1)
    opacity 1
  &:hover
    opacity 1
</style>