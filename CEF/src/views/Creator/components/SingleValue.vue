<template>
  <div :class="$style.container">
    <div :class="$style.header">{{ props.header }}</div>
    <div :class="$style.field">
      <span :class="$style.placeholder">{{ props.placeholder }}</span>
      <div :class="$style.inputCont">
        <component v-if="props.icons" :is="props.icons.left" :class="$style.iconHelper"></component>
        <span v-else :class="$style.text">{{ props.texts.left }}</span>
        <div :class="$style.rangeContainer">
          <div :class="$style.rangeBack">
            <div :class="$style.rangeLine" v-for="value in 4"></div>
          </div>
          <input tabindex="-1" :class="$style.inputRange" type="range" :min="props.min === undefined ? -1.0 : min" :max="props.max || 1.0" v-model.number="modelValue" step="0.01">
        </div>
        <component v-if="props.icons" :is="props.icons.right" :class="$style.iconHelper"></component>
        <span v-else :class="$style.text">{{ props.texts.right }}</span>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
const props = defineProps<{
  header: string;
  id: number;
  value: number;
  placeholder: string;
  icons?: { left: string, right: string };
  texts?: { left: string, right: string };
  min?: number;
  max?: number;
}>();

const modelValue = $computed({
  get() {
    return props.value;
  },
  set(value: number) {
    emit("update", props.id, value);
  }
})

const emit = defineEmits<{
  (e: "update", id: number, value: number): void;
}>();
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

.placeholder
  width 100%
  text-align center
  font-size 1.3vh
  font-weight 300

.inputCont
  margin-top 0.6vh
  flex(cont: center, ialign: center)
  height 3.5vh

.rangeContainer
  position relative
  flex(cont: center, ialign: center)
  text-align center
  width 62%
  height 100%

.rangeBack
  position absolute
  flex(cont: space-evenly, ialign: center)
  width 100%
  height 100%
  border 0.1vh solid #fff

.rangeLine
  border-left 0.1vh solid #fff
  height 3.3vh
  background #fff

.inputRange
  position relative
  width 85%
  height 100%
  z-index 1000
  appearance none !important
  &::-webkit-slider-thumb
    appearance none !important
    background #fff
    height 1vh
    width 1vh
    border-radius 50%
    box-shadow 0 0 0 0.3vw rgba(255, 255, 255, 0.1)

.iconHelper
  margin 0 0.4vw
  max-width 3vh
  height 100%
  &.black path
    fill #292522

.text
  font-size 0.8vw
  font-weight 200
  margin 0.3vw
  letter-spacing 0.03vw
</style>