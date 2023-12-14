<template>
  <div :class="[$style.select, isFocus ? $style.focus : '']" v-select-cont :tabindex="id">
    <div :class="$style.switchCont">
      <LeftSwitcherIcon :class="[$style.switcher, $style.left, props.current ? $style.open : '']"
                        @click="currValue--" v-show="isFocus"/>
      <RightSwitcherIcon
          :class="[$style.switcher, $style.right, props.names.length-1 > props.current ? $style.open : '']"
          @click="currValue++" v-show="isFocus"/>
    </div>
    <span>{{ props.contName }}</span>
    <span>{{ props.names[props.current] }}</span>
  </div>
</template>

<script lang="ts" setup>
import LeftSwitcherIcon from "@images/creator/left-switcher.svg";
import RightSwitcherIcon from "@images/creator/right-switcher.svg";
import {computed, Directive, onUnmounted, watch} from "vue";
import {Key, KeyCode} from "../../../services/KeyManager";

const props = defineProps<{
  id: number,
  contName: string,
  names: string[],
  current: number,
  click?: number
}>();

const vSelectCont: Directive = {
  mounted: (el) => {
    if(props.click !== undefined) el.addEventListener("click", () => isFocus = true);
    else {
      el.addEventListener("focusin", () => isFocus = true);
      el.addEventListener("blur", () => isFocus = false);
    }
  }
}

let focus = $ref(false);
let isFocus = $computed({
  get() {
    if(props.click !== undefined) return props.click == props.id;
    else return focus;
  },
  set(value: boolean) {
    if(props.click !== undefined) return emit("show", props.id);
    else focus = value;
  }
});
const currValue = computed({
  get() {
    return props.current;
  },
  set(value: number) {
    if (value > props.names.length - 1 || value == -1) return;
    emit('updateValue', props.id, value)
  }
})

const emit = defineEmits<{
  (e: 'updateValue', id: number, value: number): void;
  (e: 'show', id: number): void;
}>();

let rightKey: Key | undefined;
let leftKey: Key | undefined;
watch(() => isFocus, () => {
  if(isFocus === true) {
    rightKey = new Key(KeyCode.RightArrow, () => {
      currValue.value++;
    });
    leftKey = new Key(KeyCode.LeftArrow, () => {
      currValue.value--;
    })
  } else if(rightKey !== undefined && leftKey !== undefined) {
    rightKey.remove();
    leftKey.remove();
  }
});

onUnmounted(() => {
  if(rightKey && leftKey) {
    rightKey.remove();
    leftKey.remove();
  }
})
</script>

<style lang="stylus" module>
.select
  position relative
  margin-top 1.2vh
  color #fff
  flex(cont: space-between, ialign: center)
  font-size 1.42vh
  height 2.3vh
  width 100%
  border-bottom 0.1vh solid rgba(255, 255, 255, 0.5)
  outline none
  font-weight 400
  transition padding 0.1s ease, transform 0.1s ease, background 0.1s ease

  &.focus
    color #000
    background #fff
    padding 0 0.2vw
    border-bottom 0.1vh solid transparent
    transform scale(1.05)

  &:hover:not(.focus)
    background rgba(211, 211, 211, 0.4)
    border-bottom 0.1vh solid transparent
    padding 0 0.2vw
    transform scale(1.05)

.switchCont
  padding-top 0.1vh
  position absolute
  left -10%;
  width 120%
  flex(cont:space-between)

.switcher
  height 2.3vh
  &.open path
    fill-opacity 1.0
</style>