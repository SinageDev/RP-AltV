import {ServerEvent} from "../managers/EventManager.js";
import browser from "../api/cef.js";
import {player} from "../index.js";
import {Vehicle} from "alt-client";
import alt from "alt-client";
import native from "natives";

let speedometerTick: number | undefined;

function clearSpeedTick() {
  if(speedometerTick === undefined) return;
  alt.clearEveryTick(speedometerTick);
  speedometerTick = undefined;
}

export function load(vehicle: Vehicle) {
  browser.Call("Speedo:ChangeState", true);
  browser.Call("Speedo:UpdateEngine", vehicle.engineState);
  browser.Call("Speedo:SetMaxSpeed", vehicle.maxSpeed);
  speedometerTick = alt.everyTick(() => {
    browser.emit("Speedo:UpdateSpeed", vehicle.newRpm() * 100, Math.round(vehicle.speedVector.length * 3.6), native.isControlPressed(0, 76));
  });
}

export function unload() {
  browser.Call("Speedo:ChangeState", false);
  clearSpeedTick();
}