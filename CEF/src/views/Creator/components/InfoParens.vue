<template>
  <div :class="$style.container">
    <div :class="$style.header">Наследие</div>
    <div :class="$style.parents">
      <img :class="[$style.parent, $style.left]" :src="getParentImage(0)">
      <img :class="[$style.parent, $style.right]" :src="getParentImage(1)">
    </div>
    <SelectCont :id="index" :contName="parentFields[index]" :names="parentNames[index]" v-for="(value, index) in parent"
                :key="index" :current="value" @updateValue="updateValue" :tabindex="index+3"/>
  </div>
</template>

<script lang="ts" setup>
import {ArrayRange} from "../../../services/ArrayExtensions";
import {inject} from "vue";
import SelectCont from "@views/Creator/components/SelectCont.vue";
import {IProvideCreator} from "../../../services/IProvide";

const getParentImage = (id: number) => parentImages[id][parent[id]].default;
const updateValue = (id: number, value: number) => parent[id] = value;

const { parent } = inject("creator") as IProvideCreator;

const parentFields = ["Мать", "Отец"]

const parentImages = [
  Object.values(import.meta.globEager("@images/parents/mather-*.png")),
  Object.values(import.meta.globEager("@images/parents/father-*.png"))
]

const parentNames = [
  ["Ханна", "Одри", "Жасмин", "Жизель", "Амелия", "Изабелла", "Зои", "Ава", "Камила", "Виолетта", "София", "Эвелин", "Николь", "Эшли", "Грейс", "Брианна", "Натали", "Оливия", "Элизабет", "Шарлотта", "Эмма", "Мисти"],
  ["Бенджамин", "Даниэль", "Джошуа", "Ной", "Андрей", "Хуан", "Алекс", "Исаак", "Эван", "Итан", "Винсент", "Ангел", "Диего", "Адриан", "Габриэль", "Майкл", "Сантьяго", "Кевин", "Луи", "Самуэль", "Энтони", "Джон", "Нико", "Клод"]
];
</script>

<style lang="stylus" module>
.container
  margin-top 1.7vh
  width 100%

.header
  font-size 2.6vh

.parents
  position relative
  box-shadow inset 0 0 0 0.4vh rgba(100, 100, 100, 0.75)
  background-image url("@images/creator/parent-phon.png")
  width 100%
  height 16vh
  background-repeat no-repeat
  background-size cover

.parent
  position absolute
  width 14vh
  bottom 0.4vh

  &.left
    left 9%
    z-index 1000

  &.right
    right 9%
</style>