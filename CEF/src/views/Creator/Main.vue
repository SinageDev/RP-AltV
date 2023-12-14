<template>
  <div :class="$style.background" @mousemove.alt="changeRotation" @wheel="changeZoom" tabindex="-1">
    <div :class="$style.switcher">
      <div :class="$style.opacityContainer" v-if="targetOverlay !== null">
        <div>{{ OverlayInfo[targetOverlay].header }}</div>
        <div :class="$style.input">
          <span :class="$style.inputValue">Прозрачность: {{ Math.floor(headOverlays[targetOverlay][3] * 100) }}%</span>
          <input :class="$style.inputOpacity" type="range" min="0.0" max="1.0" step="0.01"
                 :value="headOverlays[targetOverlay][3]" @input="updateHeadOverlayOpacity(targetOverlay, +$event.target.value)">
        </div>
      </div>
      <div :class="$style.buttons">
        <div :class="[$style.button, step ? $style.active : '']" @click="!step ? '' : step--">Назад</div>
        <div :class="[$style.button, $style.active]" @click="step !== 2 ? step++ : tryCreate()">
          {{ step !== 2 ? 'Далее' : 'Создать' }}
        </div>
      </div>
      <div :class="$style.random" @click="randomGenerate()">
        <div>Сгенерировать случайно</div>
        <RandomIcon :class="$style.randomIcon"/>
      </div>
    </div>
    <div :class="$style.editor">
      <div :class="$style.header">
        <span>{{ stepInfo[step] }}</span>
        <img :src="getStepIcon(step)" :class="$style.headerIcon"/>
      </div>
      <div :class="$style.step">ШАГ {{ step + 1 }} ИЗ 3</div>
      <template v-if="step === 0">
        <InfoBox/>
        <InfoParens/>
        <SingleValue :id="0" :value="params[0]" :min="0.0" header="Схожесть"
                     placeholder="Управляйте ползунком для изменения"
                     :icons="{left: WomanIcon, right: MaleIcon}" @update="(id, val) => params[id] = val"/>
        <SingleValue :id="1" :value="params[1]" :min="0.0" header="Оттенок кожи"
                     placeholder="Управляйте ползунком для измения цвета"
                     :icons="{left: WomanIcon, right: MaleIcon}"
                     @update="(id, val) => params[id] = val"/>
      </template>
      <template v-else-if="step === 1">
        <SelectCont :id="index" :cont-name="info.contName" :names="info.selects.map(i => i.name)"
                    :current="faceSwitch[index]" @updateValue="updateFaceShow"
                    v-for="(info, index) in faceInfo" :key="index" :click="faceEditActive" @show="showFaceFeature"/>
        <component v-if="faceEditActive !== -1" :is="faceInfo[faceEditActive].component.model"
                   :header="faceInfo[faceEditActive].contName"
                   @update="updateFaceFeature"
                   v-bind="faceInfo[faceEditActive].component.props"/>
      </template>
      <template v-else>
        <InfoOverlay></InfoOverlay>
      </template>
    </div>
    <HelpButtons :class="$style.helpButtons"/>
  </div>
</template>

<script lang="ts" setup>
import HelpButtons from "@images/creator/help-buttons.svg";
import InfoBox from "@views/Creator/components/InfoBox.vue";
import InfoParens from "@views/Creator/components/InfoParens.vue";
import {provide, reactive, ref, watch} from "vue";
import SingleValue from "@views/Creator/components/SingleValue.vue";
import WomanIcon from "@images/creator/woman.svg";
import MaleIcon from "@images/creator/male.svg";
import DoubleValue from "@views/Creator/components/DoubleValue.vue";
import SelectCont from "@views/Creator/components/SelectCont.vue";
import {ArrayDefault, ArrayRange} from "../../services/ArrayExtensions";
import InfoOverlay from "@views/Creator/components/InfoOverlay.vue";
import {IProvideCreator} from "../../services/IProvide";
import RandomIcon from "@images/random.svg"
import {Mp} from "../../services/Alt";
import {Key, KeyCode} from "../../services/KeyManager";
import {randomInArray, randomInRange, randomInt} from "../../services/RandomExtensions";

const step = ref(0);
const stepInfo = ["Создание персонажа", "Характеристики", "Внешний вид"]
const getStepIcon = (id: number) => new URL(`/src/assets/images/creator/step-${id}.svg`, import.meta.url);

const targetOverlay = ref<number | null>(null);

const parents = reactive([0, 0]);
const gender = ref(false);
const params = reactive([0.5, 0.5]);
const parentIds = [
  [...ArrayRange(21, 21), 45],
  [...Array(21).keys(), 42, 43, 44]
]

const faceValues = reactive<number[]>(ArrayDefault(20, 0.0));

//#region FaceValuesMap
const faceInfo = $computed(() => [
  {
    contName: "Лоб",
    ids: {
      x: 7,
      y: 6
    },
    component: {
      model: DoubleValue,
      props: {
        x: faceValues[7],
        y: faceValues[6],
        changeX: 7,
        changeY: 6,
        texts: {
          top: "Высокий",
          left: "Плоский",
          right: "Выпуклый",
          bottom: "Низкий"
        }
      }
    },
    selects: [
      {
        name: "Кастом",
        value: {
          x: 0.0,
          y: 0.0
        }
      },
      {
        name: "Высокий",
        value: {
          x: 0.0,
          y: -1.0
        }
      },
      {
        name: "Низкий",
        value: {
          x: 0.0,
          y: 1.0
        }
      },
      {
        name: "Плоский",
        value: {
          x: -1.0,
          y: 0.0
        }
      },
      {
        name: "Выпуклый",
        value: {
          x: 1.0,
          y: 0.0,
        }
      }
    ]
  },
  {
    contName: "Глаза",
    id: 11,
    component: {
      model: SingleValue,
      props: {
        id: 11,
        value: faceValues[11],
        placeholder: "Управляйте ползунком для настройки параметров",
        texts: {left: "Широкие", right: "Узкие"},
      }
    },
    selects: [
      {
        name: "Кастом",
        value: 0.0
      },
      {
        name: "Широкие",
        value: -1.0
      },
      {
        name: "Узкие",
        value: 1.0
      }
    ]
  },
  {
    contName: "Нос",
    ids: {
      x: 0,
      y: 1
    },
    component: {
      model: DoubleValue,
      props: {
        x: faceValues[0],
        y: faceValues[1],
        changeX: 0,
        changeY: 1,
        texts: {
          top: "Кверху",
          left: "Узкий",
          right: "Широкий",
          bottom: "Низкий"
        }
      }
    },
    selects: [
      {
        name: "Кастом",
        value: {
          x: 0.0,
          y: 0.0
        }
      },
      {
        name: "Высокий",
        value: {
          x: 0.0,
          y: -1.0
        }
      },
      {
        name: "Низкий",
        value: {
          x: 0.0,
          y: 1.0
        }
      },
      {
        name: "Узкий",
        value: {
          x: -1.0,
          y: 0.0
        }
      },
      {
        name: "Широкий",
        value: {
          x: 1.0,
          y: 0.0,
        }
      }
    ]
  },
  {
    contName: "Профиль носа",
    ids: {
      x: 2,
      y: 3
    },
    component: {
      model: DoubleValue,
      props: {
        x: faceValues[2],
        y: faceValues[3],
        changeX: 2,
        changeY: 3,
        texts: {
          top: "С горбинкой",
          left: "Длинный",
          right: "Короткий",
          bottom: "Выгнутый"
        }
      }
    },
    selects: [
      {
        name: "Кастом",
        value: {
          x: 0.0,
          y: 0.0
        }
      },
      {
        name: "Выгнутый",
        value: {
          x: 0.0,
          y: -1.0
        }
      },
      {
        name: "С горбинкой",
        value: {
          x: 0.0,
          y: 1.0
        }
      },
      {
        name: "Длинный",
        value: {
          x: -1.0,
          y: 0.0
        }
      },
      {
        name: "Короткий",
        value: {
          x: 1.0,
          y: 0.0,
        }
      }
    ]
  },
  {
    contName: "Кончик носа",
    ids: {
      x: 5,
      y: 4
    },
    component: {
      model: DoubleValue,
      props: {
        x: faceValues[5],
        y: faceValues[4],
        changeX: 5,
        changeY: 4,
        texts: {
          top: "Кончик вверх",
          left: "Скос влево",
          right: "Скос вправо",
          bottom: "Кончик вниз"
        }
      }
    },
    selects: [
      {
        name: "Кастом",
        value: {
          x: 0.0,
          y: 0.0
        }
      },
      {
        name: "Кончик вверх",
        value: {
          x: 0.0,
          y: -1.0
        }
      },
      {
        name: "Кончик вниз",
        value: {
          x: 0.0,
          y: 1.0
        }
      },
      {
        name: "Скос влево",
        value: {
          x: -1.0,
          y: 0.0
        }
      },
      {
        name: "Скос вправо",
        value: {
          x: 1.0,
          y: 0.0,
        }
      }
    ]
  },
  {
    contName: "Скулы",
    ids: {
      x: 9,
      y: 8
    },
    component: {
      model: DoubleValue,
      props: {
        x: faceValues[9],
        y: faceValues[8],
        changeX: 9,
        changeY: 8,
        texts: {
          top: "Высокие",
          left: "Узкие",
          right: "Широкие",
          bottom: "Низкие"
        }
      }
    },
    selects: [
      {
        name: "Кастом",
        value: {
          x: 0.0,
          y: 0.0
        }
      },
      {
        name: "Высокие",
        value: {
          x: 0.0,
          y: -1.0
        }
      },
      {
        name: "Низкие",
        value: {
          x: 0.0,
          y: 1.0
        }
      },
      {
        name: "Узкие",
        value: {
          x: -1.0,
          y: 0.0
        }
      },
      {
        name: "Широкие",
        value: {
          x: 1.0,
          y: 0.0,
        }
      }
    ]
  },
  {
    contName: "Щеки",
    id: 10,
    component: {
      model: SingleValue,
      props: {
        id: 10,
        value: faceValues[10],
        placeholder: "Управляйте ползунком для настройки параметров",
        texts: {left: "Надутые", right: "Худые"},
      }
    },
    selects: [
      {
        name: "Кастом",
        value: 0.0
      },
      {
        name: "Надутые",
        value: -1.0
      },
      {
        name: "Худые",
        value: 1.0
      }
    ]
  },
  {
    contName: "Губы",
    id: 12,
    component: {
      model: SingleValue,
      props: {
        id: 12,
        value: faceValues[12],
        placeholder: "Управляйте ползунком для настройки параметров",
        texts: {left: "Толстые", right: "Тонкие"},
      }
    },
    selects: [
      {
        name: "Кастом",
        value: 0.0
      },
      {
        name: "Толстые",
        value: -1.0
      },
      {
        name: "Тонкие",
        value: 1.0
      }
    ]
  },
  {
    contName: "Челюсть",
    ids: {
      x: 13,
      y: 14
    },
    component: {
      model: DoubleValue,
      props: {
        x: faceValues[13],
        y: faceValues[14],
        changeX: 13,
        changeY: 14,
        texts: {
          top: "Круглая",
          left: "Узкая",
          right: "Широкая",
          bottom: "Квадратная"
        }
      }
    },
    selects: [
      {
        name: "Кастом",
        value: {
          x: 0.0,
          y: 0.0
        }
      },
      {
        name: "Круглая",
        value: {
          x: 0.0,
          y: -1.0
        }
      },
      {
        name: "Квадратная",
        value: {
          x: 0.0,
          y: 1.0
        }
      },
      {
        name: "Узкая",
        value: {
          x: -1.0,
          y: 0.0
        }
      },
      {
        name: "Широкая",
        value: {
          x: 1.0,
          y: 0.0,
        }
      }
    ]
  },
  {
    contName: "Подбородок",
    ids: {
      x: 15,
      y: 16
    },
    component: {
      model: DoubleValue,
      props: {
        x: faceValues[15],
        y: faceValues[16],
        changeX: 15,
        changeY: 16,
        texts: {
          top: "Высокий",
          left: "Короткий",
          right: "Длинный",
          bottom: "Низкий"
        }
      }
    },
    selects: [
      {
        name: "Кастом",
        value: {
          x: 0.0,
          y: 0.0
        }
      },
      {
        name: "Высокий",
        value: {
          x: 0.0,
          y: -1.0
        }
      },
      {
        name: "Низкий",
        value: {
          x: 0.0,
          y: 1.0
        }
      },
      {
        name: "Короткий",
        value: {
          x: -1.0,
          y: 0.0
        }
      },
      {
        name: "Длинный",
        value: {
          x: 1.0,
          y: 0.0,
        }
      }
    ]
  },
  {
    contName: "Форма подбородка",
    ids: {
      x: 17,
      y: 18
    },
    component: {
      model: DoubleValue,
      props: {
        x: faceValues[17],
        y: faceValues[18],
        changeX: 17,
        changeY: 18,
        texts: {
          top: "Округлый",
          left: "Заостеренный",
          right: "Квадратный",
          bottom: "С ямочкой"
        }
      }
    },
    selects: [
      {
        name: "Кастом",
        value: {
          x: 0.0,
          y: 0.0
        }
      },
      {
        name: "Округлый",
        value: {
          x: 0.0,
          y: -1.0
        }
      },
      {
        name: "С ямочкой",
        value: {
          x: 0.0,
          y: 1.0
        }
      },
      {
        name: "Заостренный",
        value: {
          x: -1.0,
          y: 0.0
        }
      },
      {
        name: "Квадратный",
        value: {
          x: 1.0,
          y: 0.0,
        }
      }
    ]
  },
  {
    contName: "Ширина шеи",
    id: 19,
    component: {
      model: SingleValue,
      props: {
        id: 19,
        value: faceValues[19],
        placeholder: "Управляйте ползунком для настройки параметров",
        texts: {left: "Уже", right: "Шире"},
      }
    },
    selects: [
      {
        name: "Кастом",
        value: 0.0
      },
      {
        name: "Узкая",
        value: -1.0
      },
      {
        name: "Широкая",
        value: 1.0
      }
    ]
  },
]);
//#endregion

let faceEditActive = $ref(-1);
Key.temp(KeyCode.UpArrow, () => {
  if (step.value !== 1) return;
  if (faceEditActive == 0) return faceEditActive = faceInfo.length - 1;
  else faceEditActive--;
});
Key.temp(KeyCode.DownArrow, () => {
  if (step.value !== 1) return;
  if (faceEditActive == faceInfo.length - 1) faceEditActive = 0;
  else faceEditActive++;
});
watch(step, () => {
  targetOverlay.value = null;
  if (step.value === 1 && faceEditActive == -1) return faceEditActive = 0;
})
const faceSwitch = reactive(ArrayDefault(faceInfo.length, 0));

const showFaceFeature = (id: number) => faceEditActive = id;
const updateFaceFeature = (id: number, value: number, isAuto?: boolean) => {
  faceValues[id] = value;
  if (!isAuto && faceSwitch[faceEditActive] != 0) faceSwitch[faceEditActive] = 0;
  Mp.emit("Creator:UpdateFeatures", id, value);
}

function updateFaceShow(id: number, select: number) {
  const info = faceInfo[id];
  if (info.ids !== undefined) {
    updateFaceFeature(info.ids.x, info.selects[select].value.x, true);
    updateFaceFeature(info.ids.y, info.selects[select].value.y, true);
  } else updateFaceFeature(info.id, info.selects[select].value, true)
  faceSwitch[id] = select;
}

const hair = reactive([0, 0, 0]);
const maleHairIds = [0, 1, 3, 21, 37, 39, 40, 48, 57, 65, 67, 70, 73, 76];
const feMaleHairIds = [0, 1, 3, 5, 7, 9, 10, 13, 14, 16, 20, 21, 43, 79, 80];

const headOverlays = reactive<number[][]>([...ArrayDefault(13, [255, 0, 0, 1.0])]);
const OverlayInfo = [
  {
    header: "Пороки кожи",
    selects: [255, ...Array(24).keys()],
  },
  {
    header: "Растительность на лице",
    selects: [255, ...Array(13).keys()],
    color: "Цвет растительности"
  },
  {
    header: "Брови",
    selects: [255, ...Array(34).keys()],
    color: "Цвет бровей"
  },
  {
    header: "Старение",
    selects: [255, ...Array(15).keys()],
  },
  {
    header: "Макияж",
    selects: [255, ...Array(16).keys()],
  },
  {
    header: "Румянец",
    selects: [255, ...Array(7).keys()],
    color: "Цвет румянца"
  },
  {
    header: "Нездоровый цвет лица",
    selects: [255, ...Array(12).keys()],
  },
  {
    header: "Солнечные повреждения",
    selects: [255, ...Array(11).keys()],
  },
  {
    header: "Помада",
    selects: [255, ...Array(10).keys()],
    color: "Цвет помады"
  },
  {
    header: "Родинки/Веснушки",
    selects: [255, ...Array(18).keys()],
  },
  {
    header: "Волосы на теле",
    selects: [255, ...Array(7).keys()],
    color: "Цвет волос на теле"
  },
  {
    header: "Пятна на теле",
    selects: [255, ...Array(12).keys()],
  },
  {
    header: "Доп. пятна на теле",
    selects: [255, ...Array(2).keys()]
  }
];

const updateHeadOverlayOpacity = (id: number, value: number) => {
  headOverlays[id][3] = value;
  Mp.emit("Creator:UpdateOverlay", id, ...headOverlays[id])
}

const eyeColor = ref(0)
const eyeIds = [...Array(15).keys()];

const name = ref("");
const nameError = ref("");
const family = ref("");
const familyError = ref("");

provide<IProvideCreator>("creator", {
  parent: parents,
  gender: gender,
  hair: hair,
  overlay: headOverlays,
  eye: eyeColor,
  name,
  nameError,
  family,
  familyError,
  step,
  maleHairIds,
  feMaleHairIds,
  OverlayInfo,
  eyeIds,
  targetOverlay
});

const changeRotation = (event: MouseEvent) => {
  if (event.which != 3) return;
  Mp.emit("Camera:MoveX", event.movementX / 10);
}

function changeZoom(event: WheelEvent) {
  if (!event.altKey) return;
  Mp.emit("Camera:MoveZ", event.deltaY / 5000);
}

const updateParent = () => Mp.emit("Creator:UpdateParent", parentIds[0][parents[0]], parentIds[1][parents[1]], params[0], params[1]);
const updateFaceFeatures = () => {
  for (let i = 0; i < faceValues.length; i++) Mp.emit("Creator:UpdateFeatures", i, faceValues[i]);
}
const updateOverlay = () => {
  for (let i = 0; i < headOverlays.length; i++) Mp.emit("Creator:UpdateOverlay", i, ...headOverlays[i]);
}
const updateEyeColor = () => Mp.emit("Creator:UpdateEye", eyeColor.value);
watch(gender, () => {
  Mp.emit("Creator:UpdateGender", gender.value);
  updateParent();
  updateFaceFeatures();
  updateOverlay();
  updateEyeColor();
  hair[0] = 0;
});
watch([parents, params], updateParent, {immediate: true});
watch(eyeColor, () => updateEyeColor());
watch(hair, () => Mp.emit("Creator:UpdateHair", ...hair));

interface IHeadInfo {
  [key: number]: any;
}

const sendNameError = (err: string) => nameError.value = err;
const sendFamilyError = (err: string) => familyError.value = err;

function checkInputs() {
  let checkName = name.value;
  let checkFamily = family.value;

  if (checkName.length < 2) return sendNameError("Маленькое имя");
  if (checkName.search(/[^A-Za-z]/g) != -1) return sendNameError("запрещенные символы");
  sendNameError("Ожидается...");
  if (checkFamily.length < 2) return sendFamilyError("Маленькая фамилия");
  if (checkFamily.search(/[^A-Za-z]/g) != -1) return sendFamilyError("запрещенные символы");
  sendFamilyError("Ожидается...");
  if ((checkName + checkFamily).length > 32) return sendNameError("Имя И Фамилия слишком большие");
  return true;
}

watch([name, family], ([newName, newFam]) => {
  if (newName.length != 0) name.value = newName.replace(newName[0], newName[0].toUpperCase());
  if (newFam.length != 0) family.value = newFam.replace(newFam[0], newFam[0].toUpperCase());
  if (checkInputs() === true) Mp.emitServer("Creator:CheckName", name.value + "_" + family.value);
});

function tryCreate() {
  if (checkInputs() !== true) {
    if (step.value) step.value = 0;
    return;
  }
  const Customization = {
    HeadBlendData: {
      Mather: parentIds[0][parents[0]],
      Father: parentIds[1][parents[1]],
      Shape: params[0],
      Skin: params[1]
    },
    FaceFeatures: faceValues.reduce((curr, item, index) => {
      if (item < 0.05 && item > -0.05) return curr;
      curr[index] = item;
      return curr;
    }, {} as IHeadInfo),
    HeadOverlays: headOverlays.reduce((curr, item, index) => {
      if (item[0] === 255) return curr;
      curr[index] = {
        Index: item[0],
        FirstColor: item[1],
        SecondColor: item[2],
        Opacity: item[3]
      }
      return curr;
    }, {} as IHeadInfo),
    Hair: [...hair],
    EyeColor: eyeColor.value,
  };
  Mp.emitServer("Creator:CreateChar", `${name.value}_${family.value}`, gender.value, JSON.stringify(Customization));
}

function randomGenerate() {
  parents[0] = randomInt(parentIds[0].length);
  parents[1] = randomInt(parentIds[1].length);
  params[0] = Math.random();
  params[1] = Math.random();

  for (let i = 0; i < faceValues.length; i++) {
    faceValues[i] = randomInRange(-1.0, 1.0);
  }

  hair[0] = gender ? randomInArray(feMaleHairIds) : randomInArray(maleHairIds);
  hair[1] = randomInt(64);
  hair[2] = randomInt(64);

  for (let i = 0; i < headOverlays.length; i++) {
    const currentOverlay = headOverlays[i];
    currentOverlay[0] = randomInArray(OverlayInfo[i].selects);
    currentOverlay[1] = randomInt(64);
    currentOverlay[3] = Math.random();
  }
  updateOverlay();

  eyeColor.value = randomInArray(eyeIds);
}
</script>

<style lang="stylus" module>
.background
  flex(cont:center)
  background radial-gradient(50% 87.36% at 50% 50%, rgba(23, 23, 23, 0) 0%, rgba(23, 23, 23, 0.4) 51.04%, rgba(23, 23, 23, 0.6) 75%, rgba(23, 23, 23, 0.9) 100%)
  //url("@images/creator/background.png") center center no-repeat
  background-size cover
  width 100%
  height 100%
  color #fff
  font-family DinDisplay
  font-size 2.6vh

.editor
  position absolute
  width 30vh
  top 6vh
  left 4vw
  flex(column)

.header
  flex(cont:space-between, ialign:center)

.headerIcon
  width 3.2vh
  height 3.2vh

.step
  margin-top 1vh
  color rgba(255, 255, 255, 0.5)
  font-size 1.25vh
  text-align right

.switcher
  position absolute
  bottom 2vh
  flex(column, center, ialign:center)

.buttons
  flex(cont:center)

.button
  flex(cont:center, ialign:center)
  font-size 2vh
  background #fff
  width 14vh
  height 4vh
  font-weight 300
  color rgba(255, 255, 255, 0.2)
  background rgba(211, 211, 211, 0.2)
  transition background 0.1s ease

  &:first-child
    margin-right 0.8vh

  &.active
    background rgba(211, 211, 211, 0.4)
    color #fff

    &:hover
      background #fff
      color #242424


.random
  margin-top 0.5vh
  flex(ialign:flex-end)
  font-size 2vh
  line-height 1.8vh
  font-weight 300
  color rgba(255, 255, 255, 0.5)

  &:hover
    color rgba(255, 255, 255, 1)

    & svg path
      fill-opacity 1

.randomIcon
  margin-left 0.4vw
  height 1.75vh

.helpButtons
  position absolute
  right 4vw
  top 5vh
  height 13vh

.opacityContainer
  width 100%
  flex(column, center)
  margin-bottom 0.8vh
  text-shadow 0 0.1vh 0.1vh rgba(0, 0, 0, 0.15)
  font-size 1.65vh

.input
  position relative
  flex(cont:center, ialign:center)
  height 2.6vh
  width 100%

.inputValue
  position absolute
  font-size 1.5vh
  pointer-events none

.inputOpacity
  overflow hidden
  appearance none
  height 100%
  margin 0
  width 100%
  margin-top 0.3vh

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
</style>