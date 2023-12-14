import * as alt from "alt-client";
import native from "natives";
import {BrowserEvent, EventManager} from "../managers/EventManager.js";
import {player} from "../index.js";
import {Ped} from "../managers/Ped.js";
import {Vector3} from "alt-shared";

const models = [
  alt.hash('MP_M_Freemode_01'),
  alt.hash('MP_F_Freemode_01')
]

const pedInfo = {
  position: new Vector3(-815.61755, -599.03735, 29.3),
  heading: -120.0
}

const cam = native.createCam("DEFAULT_SCRIPTED_CAMERA", true);
const ped = new Ped(models[0], pedInfo.position, pedInfo.heading);
let camY = 2;
let camZ = 0.3;
let camX = 0.0;
let rot = -120.0;
const rotMax = -50;
const rotMin = -190;

const events = EventManager.add([
  new BrowserEvent("Camera:MoveZ", (value: number) => {
    if (camZ - value >= 0.7 || camZ - value <= 0.25) return;
    camY += value * 4;
    camZ -= value;
    native.pointCamAtEntity(cam, alt.Player.local.scriptID, 0.0, 0.0, camZ, true);
    native.attachCamToEntity(cam, alt.Player.local.scriptID, camX, camY, camZ, true);
  }),
  new BrowserEvent("Camera:MoveX", (value: number) => {
    rot += value;
    if (rot < rotMin) rot = rotMin;
    else if (rot > rotMax) rot = rotMax;
    native.setEntityRotation(alt.Player.local.scriptID, 0, 0, rot, 2, true);
  }),
  new BrowserEvent("Creator:UpdateEye", ped.setEyeColor.bind(ped)),
  new BrowserEvent("Creator:UpdateHair", ped.setHair.bind(ped)),
  new BrowserEvent("Creator:UpdateOverlay", (id: number, value: number, color1: number, color2: number, opacity: number) => {
    ped.setHeadOverlay(id, {
      Index: value,
      FirstColor: color1,
      SecondColor: color2,
      Opacity: opacity
    });
  }),
  new BrowserEvent("Creator:UpdateFeatures", ped.setFaceFeature.bind(ped)),
  new BrowserEvent("Creator:UpdateParent", (mather: number, father: number, mix: number, color: number) => {
    ped.headBlendData = {
      Mather: mather,
      Father: father,
      Skin: color,
      Shape: mix,
    }
  }),
  new BrowserEvent("Creator:UpdateGender", updateGender)
])

function updateGender(gender: boolean) {
  ped.model = models[+gender];
  updateClothes(gender);
}

function updateClothes(gender: boolean) {
  ped.setClothes(11, gender ? 82 : 15, 0);
  ped.setClothes(6, gender ? 35 : 34, 0);
  ped.setClothes(4, gender ? 15 : 61, 0);
  ped.setClothes(3, 15, 0);
  ped.setClothes(8, 15, 0);
}

export const load = async () => {
  alt.log(ped.rotation);
  native.setEntityCollision(player, false, false);
  player.freeze(true);
  native.pointCamAtEntity(cam, player, 0.0, 0.0, camZ, true);
  ped.freeze = true;
  native.setCamActive(cam, true);
  native.renderScriptCams(true, false, 0, true, false, 0);
  native.attachCamToEntity(cam, player, camX, camY, camZ, true);
  alt.showCursor(true);
  if (alt.isCamFrozen()) alt.setCamFrozen(false);
  alt.toggleGameControls(false);
  native.displayRadar(false);
  updateClothes(false);
  native.setEntityAlpha(player, 0, false);
};

export const unload = () => {
  events.forEach((event) => event.off());
  ped.remove();
  alt.showCursor(false);
  alt.toggleGameControls(true);
  native.renderScriptCams(false, false, 0, true, false, 0);
  native.displayRadar(true);
  native.freezeEntityPosition(player, false);
  native.resetEntityAlpha(player);
}