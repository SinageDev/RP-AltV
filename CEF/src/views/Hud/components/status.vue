<template>
<div :class="$style.status">
  <span>{{ date }}</span>
  <span>
    <GroupIcon :class="$style.group"/>
    {{ players }}
  </span>
  <span>ID 3242</span>
</div>
</template>

<script lang="ts" setup>
import GroupIcon from "@images/status-bar/group.svg";
import {onMounted, onUnmounted} from "vue";
import {Mp} from "../../../services/Alt";

const dateOptions: Intl.DateTimeFormatOptions = {
  timeZone: "Europe/Moscow",
  year: "numeric",
  month: "2-digit",
  day: "2-digit",
  minute: "2-digit",
  hour: "2-digit"
}
let date = $ref(new Date().toLocaleString("ru-RU", dateOptions));
let players = $ref(0);
const interval = setInterval(() => date = new Date().toLocaleString("ru-RU", dateOptions), 1000);
const checkPlayers = setInterval(() => Mp.emit("Status:UpdatePlayers"));
onUnmounted(() => clearInterval(interval));

Mp.events.temp("Status:UpdatePlayers", (count: number) => players = count);
</script>

<style lang="stylus" module>
.status
  pos(left: 0, top: 0.75vh)
  flex(ialign: center)
  font-family DinDisplay
  letter-spacing 0.02vw
  font-weight 300
  font-size 1.3vh
  color #fff

  & span
    flex(cont: center, ialign: center)
    height 100%
    margin-left 1vw

.group
  margin-right 0.2vw
  height 1.75vh
</style>