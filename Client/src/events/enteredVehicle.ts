import * as alt from "alt-client"
import native from "natives";
import {player} from "../index.js";
import {Key, KeyCode} from "../managers/KeyManager.js";
import {SpeedometerBlackList} from "../extensions/vehicle.js";

let speedometer: any | undefined;
alt.on("enteredVehicle", async (vehicle, seat) => {
  if (seat != 1) return;
  native.setPedConfigFlag(player, 184, true);
  native.setPedConfigFlag(player, 429, true);
  native.setPedConfigFlag(player, 241, true);
  native.setPedConfigFlag(player, 422, true);
  native.setPedConfigFlag(player, 359, true);
  native.setPedConfigFlag(player, 248, false);
  native.setPedConfigFlag(player, 220, false);
  native.setPedConfigFlag(player, 140, true);
  if(!SpeedometerBlackList.includes(vehicle.class)) {
    speedometer = await import("../systems/speedometer.js");
    speedometer.load(vehicle);
  }
  new Key(KeyCode.Two, () => alt.emitServer("Player:TryEngineChange", vehicle));
});

alt.on("leftVehicle",  (vehicle, seat) => {
  if (seat != 1) return;
  if(speedometer) {
    speedometer.unload();
    speedometer = undefined;
  }
  Key.remove(KeyCode.Two);
});