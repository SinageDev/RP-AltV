<template>
  <div :class="$style.container" v-if="inventory">
    <div :class="$style.useSlots">
      <div :class="$style.useItems">
        <div :class="[$style.slot, $style.defaultSlot]" v-for="(slot, i) in inventory.UsedSlots.Clothes" :key="i">
          <template v-if="slot.ItemId === undefined">
            <img :class="$style.defaultIcon" :src="useDefaultImage(i)">
            <div :class="$style.defaultText">{{ useDefaultInfo[i] }}</div>
          </template>
          <img v-else :class="$style.defaultIcon" :src="getItemImage(slot.ItemId)">
        </div>
      </div>
      <div :class="$style.indicators">
        <div :class="$style.indicatorCont">
          <water-icon :class="$style.waterIcon"/>
          <svg :class="$style.indicator" viewBox="0 0 166 14" fill="none" xmlns="http://www.w3.org/2000/svg">
            <g fill="#53ACFF">
              <rect x="4" y="0" :width="water < 20 ? water/20*30 : 30" height="10"/>
              <rect x="36" y="0" :width="water < 40 ? (water-20)/20*30 : 30" height="10" v-if="water > 20"/>
              <rect x="68" y="0" :width="water < 60 ? (water-40)/20*30 : 30" height="10" v-if="water > 40"/>
              <rect x="100" y="0" :width="water < 80 ? (water-60)/20*30 : 30" height="10" v-if="water > 60"/>
              <rect x="132" y="0" :width="water < 100 ? (water-80)/20*30 : 30" height="10" v-if="water > 80"/>
              <rect width="2" height="166" transform="matrix(4.37114e-08 1 1 -4.37114e-08 0 12)"/>
              <rect width="2" height="10" transform="matrix(-1 0 0 1 2 2)"/>
              <rect width="2" height="10" transform="matrix(-1 0 0 1 166 2)"/>
            </g>
          </svg>
        </div>
        <div :class="$style.backpackSlot">
          <div :class="$style.slot">
            <template v-if="inventory.UsedSlots.Backpack.ItemId === undefined">
              <img :class="$style.defaultIcon" :src="useDefaultImage(14)">
              <div :class="$style.defaultText">{{ useDefaultInfo[14] }}</div>
            </template>
            <img v-else :class="$style.defaultIcon" :src="getItemImage(inventory.UsedSlots.Backpack.ItemId)">
          </div>
        </div>
        <div :class="$style.indicatorCont">
          <svg :class="$style.indicator" viewBox="0 0 166 14" fill="none" xmlns="http://www.w3.org/2000/svg">
            <g fill="#FFD58C">
              <rect transform="matrix(-1 0 0 1 162 0)" :width="eat < 20 ? eat/20*30 : 30" height="10"/>
              <rect transform="matrix(-1 0 0 1 130 0)" :width="eat < 40 ? (eat-20)/20*30 : 30" height="10"
                    v-if="eat > 20"/>
              <rect transform="matrix(-1 0 0 1 98 0)" :width="eat < 60 ? (eat-40)/20*30 : 30" height="10"
                    v-if="eat > 40"/>
              <rect transform="matrix(-1 0 0 1 66 0)" :width="eat < 80 ? (eat-60)/20*30 : 30" height="10"
                    v-if="eat > 60"/>
              <rect transform="matrix(-1 0 0 1 34 0)" :width="eat < 100 ? (eat-80)/20*30 : 30" height="10"
                    v-if="eat > 80"/>
              <rect width="2" height="166" transform="matrix(4.37114e-08 1 1 -4.37114e-08 0 12)"/>
              <rect width="2" height="10" transform="matrix(-1 0 0 1 2 2)"/>
              <rect width="2" height="10" transform="matrix(-1 0 0 1 166 2)"/>
            </g>
          </svg>
          <eat-icon :class="$style.eatIcon"/>
        </div>
      </div>
    </div>
    <div>
      <div :class="$style.itemInfo" v-if="targetItem">
        <div :class="$style.itemHeader">
          <span :class="$style.itemName">Банка газировки</span>
          <span :class="$style.itemIcons">
            <div :class="$style.itemAction">
              <DropIcon :class="$style.actionIcon"/>
            </div>
            <div :class="$style.itemAction">
              <UseIcon :class="$style.actionIcon"/>
            </div>
          </span>
        </div>
        <div :class="$style.itemDescription">
          <div :class="$style.description">Можно приобрести в магазине 24/7.<br>Уменьшает голод.</div>
          <img src="@images/inventory/items/0.png" :class="$style.descriptionIcon"/>
        </div>
        <div :class="$style.itemStack">
          <div :class="$style.stackProgress">
            <input :class="$style.stackChange" type="range" v-model.number="stack" min="0" max="100">
            <div :class="$style.stackValue">{{ stack }}</div>
          </div>
          <div :class="[$style.slot, $style.stackResult]"></div>
        </div>
      </div>
      <div :class="$style.header">Инвентарь</div>
      <div :class="$style.mainSlots">
        <div :class="$style.slotContainer">
          <div :class="$style.slot" v-for="(slot, i) in inventory.MainSlots.Slots" :key="i">
            <img v-if="slot" :class="$style.itemImage" :src="getItemImage(slot.ItemId)">
          </div>
        </div>
        <div :class="$style.backpackInfo">{{ inventory.Backpack ? 'Содержимое рюкзака' : 'Рюкзак отсутствует' }}</div>
        <div v-if="inventory.Backpack" :class="[$style.slotContainer, $style.backpackSlots]">
          <div :class="$style.slot" v-for="(slot, i) in inventory.Backpack.Slots" :key="i"></div>
        </div>
        <div :class="$style.weightInfo">
          <div :class="$style.weightProgress">
            <div :class="$style.weightValue" :style="{height: `${weight}%`}"></div>
          </div>
          <div :class="$style.weightNumber">
            <span :class="$style.currentWeight">6.0</span>
            <span :class="$style.maxWeight">/15.0</span>
            <WeightIcon :class="$style.weightIcon"/>
          </div>
        </div>
      </div>
    </div>
    <div :class="$style.anotherContainer" v-if="inventory.AnotherSlots">
      <div :class="$style.header">Шкаф</div>
      <div :class="$style.mainSlots">
        <div :class="$style.slotContainer">
          <div :class="$style.slot" v-for="test in 24"></div>
        </div>
        <div :class="[$style.slotContainer, $style.blockContainer]">
          <BlockSlotIcon :class="$style.blockSlot" v-for="test in 12"/>
        </div>
        <div :class="$style.weightInfo">
          <div :class="$style.weightProgress">
            <div :class="$style.weightValue" :style="{height: `${weight}%`}"></div>
          </div>
          <div :class="$style.weightNumber">
            <span :class="$style.currentWeight">6.0</span>
            <span :class="$style.maxWeight">/15.0</span>
            <WeightIcon :class="$style.weightIcon"/>
          </div>
        </div>
      </div>
    </div>
    <input type="range" min="0" max="100" style="position: absolute;top: 10vh;left: 10vw;" v-model.number="weight">
    <div :class="$style.placeholder">
      <span :class="$style.placeholderText">Выбросить</span>
      <PlaceholderBackground :class="$style.placeholderTriangle"/>
    </div>
  </div>
</template>

<script lang="ts" setup>
import WaterIcon from "@images/inventory/indicator/water.svg"
import EatIcon from "@images/inventory/indicator/eat.svg"
import WeightIcon from "@images/inventory/weight.svg"
import BlockSlotIcon from "@images/inventory/block-slot.svg"
import DropIcon from "@images/inventory/actions/drop.svg"
import UseIcon from "@images/inventory/actions/use.svg"
import PlaceholderBackground from "@images/inventory/placeholder.svg";
import {computed, inject, provide, reactive, watch} from "vue";
import {CharacterInventory, IItems, InventoryItem, InventorySlot} from "arp-inventory";
import {Mp} from "../../../../services/Alt";
import {Key, KeyCode} from "../../../../services/KeyManager";

const useDefaultImage = (id: number) => new URL(`/src/assets/images/inventory/useSlots/${id}.svg`, import.meta.url);
const useDefaultInfo = ["Шапка", "Аксессуар", "Куртка", "Торс", "Перчатки", "Штаны", "Обувь", "Маска", "Очки", "Серьги", "Браслет", "Часы", "Броня", "SIM-карта", "Рюкзак"]

let targetItem = $ref<InventorySlot | null>(null);

let eat = $ref(50);
let water = $ref(50);
let weight = $ref(0);

let stack = $ref(0);

const getItemImage = (id: number) => new URL(`/src/assets/images/inventory/items/${id}.png`, import.meta.url);

const InventoryInstance = reactive<{ instance: null | CharacterInventory }>({instance: null});
const inventory = $computed(() => InventoryInstance.instance);
Mp.events.add("setInventory", (newInventory: string) => {
  console.log(newInventory);
  InventoryInstance.instance = JSON.parse(newInventory);
});

let inventories = $computed(() => inventory == null ? null : [
  inventory.AnotherSlots,
  inventory.MainSlots,
  inventory.Backpack
]);

Mp.events.add("inventorySlotUpdate", (inventoryId: number, slotId: number, slot: string) => {
  if (inventory == null || inventories == null) return;
  let inventorySearch = inventories.find(x => x && x.InventoryId == inventoryId);
  if (!inventorySearch) return;
  inventorySearch.Slots[slotId] = JSON.parse(slot);
});

const ItemsInstance = reactive<{instance: null | InventoryItem[]}>({instance: null});
const items = $computed(() => ItemsInstance.instance);
Mp.events.add("inventoryItems", (items: string) => ItemsInstance.instance = JSON.parse(items));
watch(() => inventory, () => console.log(inventory));
</script>

<style lang="stylus" module>
.container
  width 100vw
  height 100vh
  background linear-gradient(180deg, rgba(23, 23, 23, 0.4) -1.48%, rgba(23, 23, 23, 0.65) 28.97%, rgba(23, 23, 23, 0.9) 63.86%, rgba(23, 23, 23, 0.95) 100%)
  flex(cont:center, ialign:flex-end)
  padding-bottom 12.3vh
  font-family DinDisplay
  font-weight 400
  color #fff

.slot
  width 3.45vw
  height 3.45vw
  background rgba(255, 255, 255, 0.05)
  flex(column, center, ialign:center)
  border 0.1vh solid rgba(255, 255, 255, 0.05)
  border-radius 0.3vh

.useSlots
  position absolute
  left 3vw

.useItems
  display grid
  grid-template-columns repeat(7, 1fr)
  grid-column-gap: 0.6vh;
  grid-row-gap 0.6vh

.defaultIcon
  height 70%

.defaultText
  font-family DinDisplay
  color rgba(255, 255, 255, 0.35)
  font-size 0.6vw
  font-weight normal

.defaultSlot
  padding 0.5vh 0
  justify-content space-between

.indicators
  margin-top 0.9vh
  width 100%
  flex(cont:center, ialign:center)

.backpackSlot
  padding 0.6vh
  box-sizing content-box !important
  border 0.1vh solid rgba(255, 255, 255, 0.05)
  border-radius 0.3vh

.indicatorCont
  flex(cont:center, ialign:center)
  height 6.2vh
  width 100%
  border-top 0.1vh solid rgba(255, 255, 255, 0.05)
  border-bottom 0.1vh solid rgba(255, 255, 255, 0.05)

.eatIcon
  height 2.3vh
  margin-left 0.4vw

.waterIcon
  height 2.3vh
  margin-right 0.4vw

.indicator
  height 20%
  width 75%

.mainSlots
  position relative

.header
  font-size 2vh
  margin-bottom 0.6vh

.slotContainer
  display grid
  grid-template-columns repeat(6, 1fr)
  grid-column-gap 0.6vh
  grid-row-gap 0.6vh

.backpackInfo
  width 100%
  height 3vh
  background rgba(255, 255, 255, 0.03)
  margin-top 0.6vh
  border-radius 0.3vh
  color rgba(255, 255, 255, 0.2)
  font-size 0.75vw
  flex(cont:center, ialign:center)

.backpackSlots
  margin-top 0.6vh

.weightInfo
  height 100%
  position absolute
  top 0
  left 102%
  flex()
  font-family DinDisplay
  font-weight 400
  font-size 1.2vw

.weightProgress
  flex(cont:flex-end)
  height 100%
  width 0.45vw
  background radial-gradient(714.29% 357.14% at -614.29% 50%, rgba(255, 255, 255, 0.7) 0%, rgba(255, 255, 255, 0) 100%)
  border-radius 0.3vh
  overflow hidden
  transform rotateZ(180deg)

.weightValue
  height 50%
  width 50%
  background #FFDB59

.weightNumber
  position relative
  margin-left 0.4vw
  flex(ialign:flex-end)
  height 1.2vw

.maxWeight
  margin-left 0.2vw
  font-size 1vw
  color rgba(255, 255, 255, 0.25)

.weightIcon
  margin-bottom 0.25vw
  margin-left 0.2vw
  height 0.85vw

.anotherContainer
  position absolute
  right 7vw

.blockContainer
  margin-top 0.6vh



.itemHeader
  font-size 1.2vw

  flex(cont:space-between, ialign:center)


.itemIcons
  flex()

.itemAction
  margin-left 0.2vh
  width 1.5vw
  height 1.5vw
  background rgba(255, 255, 255, 0.05)
  border 0.1vh solid rgba(255, 255, 255, 0.05)
  border-radius 0.3vh
  flex(cont:center, ialign:center)

.actionIcon
  height 65%

.itemBlock
  margin-top 0.8vh

.itemDescription
  margin-top 0.3vh
  background rgba(255, 255, 255, 0.05)
  height 10vh
  flex(cont:space-between, ialign:center)
  border-radius 0.3vh
  padding 0 0.8vw

.descriptionIcon
  height 60%

.description
  margin-right 1.4vw
  font-size 0.85vw
  font-weight 400

.itemStack
  height 4.7vh
  width 100%
  flex()
  margin-top 0.3vh

.stackProgress
  height 100%
  width 100%
  flex(cont: center, ialign: center)

.stackChange
  margin-left 0

  height 100%
  width 100%
  appearance none
  overflow hidden
  &::-webkit-slider-runnable-track {
    appearance none
    width 100%
    height 100%
    background rgba(255, 255, 255, 0.05)
  }
  &::-webkit-slider-thumb {
    appearance none
    width 0
    height 100%
    box-shadow: -100vw 0 0 100vw rgba(255, 255, 255, 0.2);
  }

.stackValue
  position absolute

.stackResult
  height 4.7vh
  width 4.7vh

.placeholder
  position absolute
  left 20vw
  top 20vh
  flex(cont: center, ialign: center)
  border-radius 0.5vh
.placeholderText
  position absolute
  top 0.7vw
  color #000
  font-size 0.75vw
  font-weight 500
.placeholderTriangle
  width 6.5vw
.itemImage
  height 75%
</style>