<template>
<div :class="$style.container">
  <div :class="$style.field" v-for="(field, index) in fields" :key="index">
    <div :class="$style.header">
      <span :class="$style.name">{{ field.name }}</span>
      <span :class="[$style.name, $style.error]">{{ field.error }}</span>
    </div>
    <input :class="$style.input" v-model.trim="field.value" :tabindex="index+1">
  </div>
  <div :class="[$style.name, $style.genderHeader]">Ваш пол</div>
  <div :class="$style.gender">
    <div :class="[$style.select, gender ? $style.noncurr : $style.current]" @click="gender = false">Мужской</div>
    <div :class="[$style.select, gender ? $style.current : $style.noncurr ]" @click="gender = true">Женский</div>
  </div>
</div>
</template>

<script lang="ts" setup>
import {inject, reactive, watchEffect} from "vue";
import {IProvideCreator} from "../../../services/IProvide";
import {Mp} from "../../../services/Alt";

const { gender, name, family, step, nameError, familyError } = inject("creator") as IProvideCreator;
const fields = reactive([
  {
    name: "Имя",
    value: name,
    error: nameError
  },
  {
    name: "Фамилия",
    value: family,
    error: familyError
  }
]);

Mp.events.temp("Creator:CheckResult", (result: boolean) => {
  if(step.value != 0) step.value = 0;
  nameError.value = result ? "Имя Фамилия занято" : "";
  familyError.value = "";
});
</script>

<style lang="stylus" module>
.container
  width 100%

.field
  margin-top 1vh
  flex-direction column

.header
  width 100%
  flex(cont: space-between)

.name
  font-weight 300
  font-size 1.5vh
  color rgba(255, 255, 255, 0.25)

.error
  color #FF6E6D

.input
  font-family DinDisplay
  color #fff
  font-weight 400
  padding 0 0.2vh
  font-size 1.95vh
  width 100%
  border-bottom 0.3vh solid rgba(255, 255, 255, 0.5)

.genderHeader
  margin-top 2vh

.gender
  margin-top 0.3vh
  width 100%
  flex()

.select
  padding 1vh 0
  color #fff
  font-size 1.775vh
  text-align center
  width 50%
  border 0.1vh solid #fff;

.noncurr
  &:hover
    background rgba(211, 211, 211, 0.4)

.current
  background #fff
  color #000
  pointer-events none
</style>