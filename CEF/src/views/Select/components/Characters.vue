<template>
  <div :class="$style.container">
    <div v-for="(value, i) in 3" :key="i"
         :class="{[$style.char]: true, [$style.select]: buttons[i] === ButtonType.Current, [$style.block]: buttons[i] === ButtonType.Max}"
         @click="clickSwitch(i)">
      <CharSelectIcon v-if="buttons[i] === ButtonType.Current || buttons[i] === ButtonType.NotEmpty" :class="$style.icon"/>
      <CharAddIcon v-else-if="buttons[i] === ButtonType.Add" :class="$style.icon"/>
      <template v-else>
        <CharBlockIcon/>
        <div :class="$style.block_donate">
          200
          <DonateCoinIcon :class="$style.coin"/>
        </div>
      </template>
    </div>
  </div>
</template>

<script lang="ts" setup>
import CharAddIcon from "@images/select/char-add.svg";
import CharSelectIcon from "@images/select/char-select.svg";
import CharBlockIcon from "@images/select/block.svg";
import DonateCoinIcon from "@images/donate-coin.svg";
import {inject} from "vue";
import {IProvideSelect} from "../../../services/IProvide";
import {Mp} from "../../../services/Alt";

const {current, max, characters} = inject('select') as IProvideSelect;

const clickSwitch = (i: number) => {
  if(buttons[i] === ButtonType.Add) Mp.emitServer("Creator:Start");
  else if(buttons[i] === ButtonType.NotEmpty) current.value = i;
  else if(buttons[i] === ButtonType.Current) return;
  else return;
}

enum ButtonType {
  Current,
  NotEmpty,
  Add,
  Max
}

const buttons = $computed(() => {
  const result: ButtonType[] = [];
  for (let i = 0; i < 3; i++) {
    if (i == current.value) result.push(ButtonType.Current);
    else if (characters.value && characters.value[i] !== undefined) result.push(ButtonType.NotEmpty);
    else if (i >= max.value) result.push(ButtonType.Max);
    else result.push(ButtonType.Add);
  }
  return result;
});
</script>

<style lang="stylus" module>
.container
  pos(left:10vw, bottom:6vh)
  flex()

.char
  width 5.7vh
  height 5.7vh
  background radial-gradient(59.46% 59.46% at 50% 15.54%, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%), linear-gradient(203.88deg, #2E2D4B 15.34%, #2C2B4A 36.08%, #383757 68.89%, #2E2D4B 84.96%)
  border-radius 0.3vh
  flex(column, center, ialign:center)
  margin-right 0.5vw
  transition all 0.2s ease

  &:hover
    background radial-gradient(59.46% 59.46% at 50% 15.54%, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%), linear-gradient(203.88deg, #494870 15.34%, #49486F 36.08%, #525175 68.89%, #595884 84.96%)

.icon
  height 40%

.select
  background radial-gradient(59.46% 59.46% at 50% 15.54%, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%), linear-gradient(203.88deg, #494870 15.34%, #49486F 36.08%, #525175 68.89%, #595884 84.96%)
  box-shadow 0 0 0 0.3vh rgba(255, 255, 255, 0.2)

.block
  position relative
  background radial-gradient(59.46% 59.46% at 50% 15.54%, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%), linear-gradient(203.88deg, rgba(46, 45, 75, 0.6) 15.34%, rgba(44, 43, 74, 0.6) 36.08%, rgba(56, 55, 87, 0.6) 68.89%, rgba(46, 45, 75, 0.6) 84.96%);

.block_donate
  flex(cont:center, ialign:center)
  pos(bottom:-2.3vh)
  font-size 1.7vh

.coin
  margin-left 0.1vw
  height 1.7vh
</style>