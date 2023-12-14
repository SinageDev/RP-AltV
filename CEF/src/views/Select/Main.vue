<template>
  <div v-if="characters" :class="$style.background">
    <Switch/>
    <Info/>
    <Characters/>
  </div>
</template>

<script lang="ts" setup>
import Info from "./components/Info.vue";
import Switch from "@views/Select/components/Switch.vue";
import Characters from "@views/Select/components/Characters.vue";
import {Mp} from "../../services/Alt";
import {provide, ref, watch} from "vue";
import {IProvideSelect} from "../../services/IProvide";

const max = ref(2);
const current = ref(0);
const characters = ref<ICharacterInfo[] | undefined>(undefined);
provide("select", {
  max,
  characters,
  current
} as IProvideSelect);
Mp.events.temp("Select:Load", (maxchar: number, chars: ICharacterInfo[]) => {
  max.value = maxchar;
  characters.value = chars;
});
watch(current, () => Mp.emit("Select:Change", current.value))
</script>

<style lang="stylus" module>
.background
  pos(left:0)
  width 100vw
  height 100vh
  background-image radial-gradient(50% 87.36% at 50% 50%, rgba(86, 86, 86, 0) 0%, rgba(46, 45, 75, 0.2) 51.04%, rgba(33, 32, 56, 0.3) 75%, rgba(25, 24, 44, 0.45) 100%)
  background-size cover
  background-position center
  background-repeat no-repeat
  font-family BebasNeue
  color #fff
  font-weight 600
  letter-spacing 0.15vw
  font-size 6vh

</style>