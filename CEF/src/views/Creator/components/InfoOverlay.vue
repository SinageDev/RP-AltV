<template>
  <div :class="$style.container">
    <template v-for="(value, index) in list[currShow]" :key="index">
      <template v-if="value.type === ComponentType.Hair">
        <div :class="$style.header">
          <span>Волосы</span>
          <span>Лысый</span>
        </div>
        <ImageSelect :images="gender ? feMaleHairImages : maleHairImages" :current="hair[0]" @update="(val) => hair[0] = val"
                     :select="gender ? feMaleHairIds : maleHairIds"/>
        <ColorSwitch header="Цвет волос" :current="hair[1]" @update="(val) => hair[1] = val"/>
        <ColorSwitch header="Оттенок волос" :current="hair[2]" @update="(val) => hair[2] = val"/>
      </template>
      <EyeSelect v-else-if="value.type === ComponentType.Eye"/>
      <template v-else>
        <OverlaySelect :header="OverlayInfo[value.id].header" :current="overlay[value.id][0]"
                       :select="OverlayInfo[value.id].selects"
                       @update="(val) => updateOverlay(value.id, 0, val)" @updateFocus="updateFocusOverlay(value.id)" :key="index"/>
        <ColorSwitch v-if="OverlayInfo[value.id].color !== undefined" :overlayId="value.id" :header="OverlayInfo[value.id].color"
                     :current="overlay[value.id][1]"
                     @update="(val) => updateOverlay(value.id, 1, val)"/>
      </template>
    </template>
  </div>
  <div :class="$style.switchers">
    <div :class="[$style.switcher, currShow === i ? $style.current : '']" v-for="(value, i) in list.length" :key="i"
         @click="currShow = i"/>
  </div>
</template>

<script lang="ts" setup>
import {inject, watch} from "vue";
import ImageSelect from "@views/Creator/components/ImageSelect.vue";
import ColorSwitch from "@views/Creator/components/ColorSwitch.vue";
import OverlaySelect from "@views/Creator/components/OverlaySelect.vue";
import {IProvideCreator} from "../../../services/IProvide";
import {Mp} from "../../../services/Alt";
import EyeSelect from "@views/Creator/components/EyeSelect.vue";

const currShow = $ref(0);

const {hair, overlay, gender, maleHairIds, feMaleHairIds, OverlayInfo, targetOverlay} = inject("creator") as IProvideCreator;

const updateFocusOverlay = (id: number) => {
  targetOverlay.value = id;
}

const maleHairImages = maleHairIds.reduce((arr, id) => {
  arr.push(new URL(`/src/assets/images/hairs/male-${('' + id + '').padStart(3, '0')}.jpg`, import.meta.url).href);
  return arr;
}, [] as string[]);

const feMaleHairImages = feMaleHairIds.reduce((arr, id) => {
  arr.push(new URL(`/src/assets/images/hairs/female-${('' + id + '').padStart(3, '0')}.jpg`, import.meta.url).href);
  return arr;
}, [] as string[]);

const updateOverlay = (id: number, index: number, val: number) => {
  overlay[id][index] = val;
  Mp.emit("Creator:UpdateOverlay", id, ...overlay[id])
}

const listOverlays = [
  [1, 2],
  [10, 0, 3, 6, 7, 11, 12],
  [4, 5, 8, 9]
]

enum ComponentType {
  Overlay,
  Hair,
  Eye
}

const list = [
  [
    {
      type: ComponentType.Hair
    },
    ...listOverlays[0].reduce((curr, id) => {
      curr.push({
        type: ComponentType.Overlay,
        id
      });
      return curr;
    }, [] as any)
  ],
  [
    ...listOverlays[1].reduce((curr, id) => {
      curr.push({
        type: ComponentType.Overlay,
        id
      });
      return curr;
    }, [] as any)
  ],
  [
    ...listOverlays[2].reduce((curr, id) => {
      curr.push({
        type: ComponentType.Overlay,
        id
      });
      return curr;
    }, [] as any),
    {type: ComponentType.Eye}
  ]
]
</script>

<style lang="stylus" module>
.container
  font-size 1.6vh
  font-weight 300
  flex(column)

.header
  margin-top 0.75vh
  flex(cont:space-between)

.switchers
  flex(cont:center)
  position fixed
  bottom 6vh
  width 30vh

.switcher
  width 3.5vh
  height 3.5vh
  border-radius 50%
  background: rgba(255, 255, 255, 0.25);
  border 0.4vw solid rgba(255, 255, 255, 0.25)
  background-clip: padding-box;
  transform scale(0.75)
  margin-left 0.4vw
  transition all 0.1s ease

  &.current
    background: #fff;
    background-clip: padding-box;
    transform scale(1)
</style>