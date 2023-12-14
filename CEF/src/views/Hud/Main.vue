<template>
  <div :class="$style.hud" v-show="!inventoryShow">
    <Chat/>
    <Speedometer v-if="speedShow"/>
  </div>
  <Inventory v-show="inventoryShow"/>
</template>

<script setup lang="ts">
import Chat from './components/chat.vue';
import Status from './components/status.vue';
import Speedometer from './components/speedometer.vue';
import {Mp} from "../../services/Alt";
import Inventory from "./components/Inventory/inventory.vue";
import {Key, KeyCode} from "../../services/KeyManager";
import {provide, reactive, watch, watchEffect} from "vue";
import {CharacterInventory} from "arp-inventory";

let speedShow = $ref(false);
Mp.events.temp("Speedo:ChangeState", (state: boolean) => speedShow = state);

let inventoryShow = $ref(false);
Key.temp(KeyCode.I, () => inventoryShow = !inventoryShow);

watch(() => inventoryShow, () => Mp.emit("CursorState", inventoryShow));
</script>

<style lang="stylus" module>
.hud
  width 100%
  height 100%
  //background url('@images/chat/background.jpg') center center no-repeat
  background-size cover
  color #fff
  font-family DinNext
  font-weight 300
  font-size 1.5vh
</style>