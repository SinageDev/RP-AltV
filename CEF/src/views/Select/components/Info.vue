<template>
<div :class="$style.container">
  <div :class="$style.info_block" v-if="character">
    <div :class="$style.header">Информация</div>
    <div :class="$style.line">
      <div :class="$style.info">
        <div :class="$style.name">На руках</div>
        <div :class="$style.value">{{ character.Money }}$</div>
      </div>
      <div :class="$style.info">
        <div :class="$style.name">Статус</div>
        <div :class="$style.value">Обычный</div>
      </div>
    </div>
    <div :class="$style.line">
      <div :class="$style.info">
        <div :class="$style.name">Последний вход</div>
        <div :class="$style.value">{{ character.LastDate }}</div>
      </div>
      <div :class="$style.info">
        <div :class="$style.name">Дата создания</div>
        <div :class="$style.value">{{ character.CreateDate }}</div>
      </div>
    </div>
    <div :class="$style.line">
      <div :class="$style.info">
        <div :class="$style.name">Уровень</div>
        <div :class="$style.value">{{ character.Lvl }}</div>
      </div>
      <div :class="$style.info">
        <div :class="$style.name">Опыта</div>
        <div :class="$style.value">{{ character.Exp }}</div>
      </div>
    </div>
    <div :class="$style.line">
      <div :class="$style.info">
        <div :class="$style.name">Фракция</div>
        <div :class="$style.value">{{ character.Fraction }}</div>
      </div>
      <div :class="$style.info">
        <div :class="$style.name">Дата вступления</div>
        <div :class="$style.value">-</div>
      </div>
    </div>
    <div :class="$style.line">
      <div :class="$style.info">
        <div :class="$style.name">Предупреждения</div>
        <div :class="$style.value">{{ character.Warn }} ИЗ 3</div>
      </div>
    </div>
  </div>
  <div :class="$style.button_cont">
    <div :class="$style.button" @click="tryJoin">
      <span :class="$style.button_text">Выбрать</span>
      <EnterIcon :class="$style.button_enter"/>
    </div>
    <RecycleIcon :class="$style.button_recycle"/>
  </div>
  <div :class="$style.donate">
    <span>На счету: </span>
    <span>200 <DonateIcon :class="$style.donateCoin"/></span>
  </div>
</div>
</template>

<script lang="ts" setup>
import EnterIcon from "@images/select/enter.svg";
import RecycleIcon from "@images/select/recycle.svg";
import DonateIcon from "@images/donate-coin.svg";

import {inject} from "vue";
import {IProvideSelect} from "../../../services/IProvide";
import {Mp} from "../../../services/Alt";

const {current, characters} = inject('select') as IProvideSelect;
function tryJoin() {
  Mp.emitServer("Select:Enter", current.value);
}
const character = $computed(() => {
  if(!characters.value) return false;

  const currChar = characters.value[current.value];
  const options: Intl.DateTimeFormatOptions = {
    year: "numeric",
    month: "2-digit",
    day: "2-digit"
  }
  return {
    LastDate: currChar.LastDate ? new Date(currChar.LastDate).toLocaleString('ru-RU', options) : "Пусто",
    CreateDate: new Date(currChar.RegDate).toLocaleString('ru-RU', options),
    Lvl: currChar.Level,
    Exp: currChar.Exp,
    Warn: currChar.Warn,
    Money: currChar.Money,
    Fraction: currChar.Fraction || "НЕ СОСТОИТЕ"
  }
});
</script>

<style lang="stylus" module>
.container
  pos(right: 10vw)
  height 100%
  background rgba(31, 30, 47, 0.8)
  border 0.1vh solid rgba(41, 40, 54, 0.6)
  padding 0 1.3vw
  font-weight 300
  letter-spacing 0.1vw
  flex(column, space-between, ialign: center)

.header
  font-size 3.35vh
  width 100%
  text-align center
  margin-top 3vh
  padding 3vh 0

.line
  display grid
  grid-template-columns 1.5fr 1fr
  width 100%

.info
  margin-top 1vh
  flex(column)


.name
  color rgba(255, 255, 255, 0.5)
  font-size 1.7vh

.value
  font-size 2.6vh

.button_cont
  position relative
  width 10vh
  height 10vh
  background radial-gradient(59.46% 59.46% at 50% 15.54%, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%) , linear-gradient(203.88deg, #2E2D4B 15.34%, #2C2B4A 36.08%, #383757 68.89%, #2E2D4B 84.96%);
  border-radius: 0.3vh;
  bottom 12.5vh
  &:hover
    background radial-gradient(59.46% 59.46% at 50% 15.54%, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%), linear-gradient(203.88deg, #494870 15.34%, #49486F 36.08%, #525175 68.89%, #595884 84.96%)

.button
  width 100%
  height 100%
  flex(column, flex-end, ialign: center)

.button_text
  font-size 2.6vh

.button_enter
  width 85%
  height 3vh
  margin-bottom 1vh

.button_recycle
  position absolute
  top: -1vh
  right: -1vh
  width 3vh

.donate
  padding 0 1vw
  pos(bottom: 2vh)
  width 100%
  font-size 2.5vh
  flex(cont: space-between, ialign: center)

.donateCoin
  height 2vh
</style>