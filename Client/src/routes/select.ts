import native from "natives";
import browser from "../api/cef.js";
import * as alt from "alt-client"
import {Ped} from "../managers/Ped.js";
import {Vector3} from "alt-shared";
import {radToDeg} from "../functions/rotateConvert.js";
import {BrowserEvent} from "../managers/EventManager.js";
import {AttachCamera} from "../managers/Camera.js";
import {DrawText} from "../managers/DrawText.js";
import {player} from "../index.js";

let characters: ICharacterInfo[];
const pedCoords = [
  {
    pos: new Vector3(-75.30989, -821.7758, 326.17358),
    rot: radToDeg(0.0494739)
  },
  {
    pos: new Vector3(-77.8022, -818.37366, 326.17358),
    rot: radToDeg(-1.5336909)
  },
  {
    pos: new Vector3(-73.3978, -816.83075, 326.17358),
    rot: radToDeg(2.473695)
  }
];

let peds: Ped[] = [];
let cameras: AttachCamera[] = [];
let draws: DrawText[] = [];

const models = [
  alt.hash('MP_M_Freemode_01'),
  alt.hash('MP_F_Freemode_01')
]
new BrowserEvent("Select:Change", (index: number) => {
  cameras[index].setActiveDuration(1000);
});

export const load = (max: number, chars: string) => {
  native.displayRadar(false);
  characters = JSON.parse(chars);
  browser.Call("Select:Load", max, characters);
  characters.forEach((character, i) => {
    const ped = new Ped(models[+character.Gender], pedCoords[i].pos, pedCoords[i].rot);
    ped.freeze = true;
    ped.setCustomization(character.Customization);
    const camera = new AttachCamera(ped.instance, new Vector3(-0.2, 1.0, 0.6), new Vector3(-0.2, 0, 0.6));
    peds.push(ped);
    cameras.push(camera);
    draws.push(new DrawText({
      text: character.Name.replace('_', ' '),
      position: ped.position.add(new Vector3(0, 0, 1.0))
    }));
  });
  cameras[0].setActive();
  native.renderScriptCams(true, false, 0, true, false, 0);
  native.setEntityAlpha(player, 0, false);
  native.freezeEntityPosition(player, true);
  alt.toggleGameControls(false);
  alt.showCursor(true);
}

export const unload = () => {
  native.renderScriptCams(false, false, 0, true, false, 0);
  native.setEntityAlpha(player, 255, false);
  native.resetEntityAlpha(player);
  alt.toggleGameControls(true);
  native.freezeEntityPosition(player, false);
  draws.forEach((draw) => draw.remove());
  peds.forEach((ped) => native.deletePed(ped.instance));
  native.displayRadar(true);
  alt.showCursor(false);
}