import * as alt from "alt-client";
import "../api/chat.js";
import {player} from "../index.js";
import native from "natives";
import {BrowserEvent, ServerEvent} from "../managers/EventManager.js";
import browser from "../api/cef.js";
import {getNestVehicle} from "../functions/getNestVehicle.js";
import {Key, KeyCode} from "../managers/KeyManager.js";
import * as nametags from "../systems/nametags.js";
import {Ped} from "../managers/Ped.js";
import {IVector3, Vector3} from "alt-shared";
import {degToRad, radToDeg} from "../functions/rotateConvert.js";
import {loadModel} from "../functions/loadModel.js";

export const load = async () => {
  nametags.load();
  await loadModel(alt.hash("mp_f_bennymech_01"));
  const ped = new Ped(alt.hash("mp_f_bennymech_01"), new Vector3(316.57584, -223.14725, 54.049072), radToDeg(-1.8800083));
  ped.freeze = true;
  native.setPedConfigFlag(ped.instance, 18, false);
}

new Key(KeyCode.F, () => {
  const taskVehicle = getNestVehicle();
  if(taskVehicle == undefined) return;
  native.taskEnterVehicle(player, taskVehicle, 5000, -1, 2.0, 1, null);
});
new Key(KeyCode.G, () => {
  const taskVehicle = getNestVehicle();
  if(taskVehicle == undefined) return;
  if(!native.areAnyVehicleSeatsFree(taskVehicle)) return;
  let seat: number | undefined;
  for(let i = 0, seats = taskVehicle.seatCount; i < seats; i++) {
    if(!native.isVehicleSeatFree(taskVehicle, i, true)) continue;
    seat = i;
  }
  if(seat !== undefined) native.taskEnterVehicle(player, taskVehicle, 5000, seat, 2.0, 1, null);
});

new BrowserEvent("Status:UpdatePlayers", () => browser.Call("Status:UpdatePlayers", alt.Player.all.length));
new ServerEvent("Status:UpdateId", () => browser.Call("Status:UpdateId"))

export const unload = () => {

}

//you can replace the GetDirectionFromRotation function from Durty for the native https://natives.altv.mp/#/0x0A794A5A57F8DF91
