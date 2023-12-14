<template>
  <div :class="$style.container">
    <div :class="$style.header">{{ header }}</div>
    <div :class="$style.field">
      <span :class="$style.placeholder">Управляйте ползунком для настройки параметров</span>
      <span :class="$style.placeholder" style="margin-top: 1vh">{{ texts.top }}</span>
      <div :class="$style.value">
        <span :class="[$style.placeholder, $style.left]">{{ texts.left }}</span>
        <div :class="$style.cursorContainer" v-cursor-container>
          <div :class="$style.cursor" :style="cursorPosition"></div>
          <table :class="$style.table" @mousemove="test">
            <tr>
              <th v-for="value in 4" v-once></th>
            </tr>
            <tr v-for="value in 4" v-once>
              <td v-for="value in 4" v-once></td>
            </tr>
          </table>
        </div>
        <span :class="[$style.placeholder, $style.right]">{{ texts.right }}</span>
      </div>
      <span :class="$style.placeholder">{{ texts.bottom }}</span>
    </div>
  </div>
</template>

<script lang="ts" setup>
import {Directive, reactive} from "vue";
import {$computed} from "vue/macros";

const vCursorContainer: Directive = {
  mounted: (el: HTMLElement) => {
    updateContainer.call(el);
    window.addEventListener("resize", updateContainer.bind(el))
  },
  beforeUnmount: (el: HTMLElement) => el.removeEventListener("resize", updateContainer)
}
const prop = defineProps<{
  header: string;
  x: number;
  changeX: number;
  y: number;
  changeY: number;
  texts: { top: string, left: string, bottom: string, right: string }
}>();

const emit = defineEmits<{
  (e: 'update', id: number, value: number): void;
}>()
const containerSize = reactive({ x: 0, y: 0 });

function updateContainer(this: HTMLElement) {
  containerSize.x = this.clientWidth;
  containerSize.y = this.clientHeight;
}

const cursorX = $computed(() => (prop.x + 1) * (containerSize.x / 2));
const cursorY = $computed(() => (prop.y + 1) * (containerSize.y / 2));
const cursorPosition = $computed(() => ({
  top: `calc(${cursorY}px - 0.5vh)`,
  left: `calc(${cursorX}px - 0.5vh)`
}));
const test = (event: MouseEvent) => {
  if (event.which != 1) return;
  const curr = event.currentTarget as HTMLElement;
  if (!curr) return;
  curr.style.cursor = "none";
  const currPos = curr.getBoundingClientRect();
  const pos = {
    x: event.clientX - currPos.x,
    y: event.clientY - currPos.y
  }
  emit("update", prop.changeX, Number(((pos.x - containerSize.x / 2) * (1 / (containerSize.x / 2))).toFixed(2)));
  emit("update", prop.changeY, Number(((pos.y - containerSize.y / 2) * (1 / (containerSize.y / 2))).toFixed(2)));
}
</script>

<style lang="stylus" module>
.container
  margin-top 2vh
  width 100%

.header
  padding 0.75vh 0.75vw
  font-size 1.77vh
  font-weight 500
  background rgba(211, 211, 211, 0.4)
  border 0.1vh solid rgba(217, 217, 217, 0.2)

.field
  padding 1.1vh 0 1.5vh 0
  flex(column)
  width 100%
  background rgba(211, 211, 211, 0.15)
  text-align center

.placeholder
  font-size 1.3vh
  font-weight 300

  &.left
    position absolute
    left 0
    height 10%
    width 17vh
    transform rotate(-90deg)
  &.right
    position absolute
    right 0
    height 10%
    width 17vh
    transform rotate(90deg)

.value
  flex(cont: center, ialign: center)

.table
  margin 0 auto
  border-collapse: collapse;
  border: 0.05vw solid #B7B7B7;
  width 100%
  height 100%

  & td, th
    border: 0.05vw solid #B7B7B7;
    pointer-events none

.cursorContainer
  flex(column, center, ialign:center)
  position relative
  width 16vh
  height 16vh
  margin 0.2vw
  z-index 1000

.cursor
  position absolute
  background #fff
  height 1vh
  width 1vh
  border-radius 50%
  box-shadow 0 0 0 0.3vw rgba(255, 255, 255, 0.1)
  pointer-events none
</style>