import alt, {Vehicle} from "alt-client";
import native from "natives";
import {SyncedMeta} from "../managers/SyncedMeta.js";
import {player} from "../index.js";
import browser from "../api/cef.js";

enum VehicleClass {
  Default = -1,
  Compacts,
  Sedans,
  SUVs,
  Coupes,
  Muscle,
  SportsClassics,
  Sports,
  Super,
  Motorcycles,
  OffRoad,
  Industrial,
  Utility,
  Vans,
  Cycles,
  Boats,
  Helicopters,
  Planes,
  Service,
  Emergency,
  Military,
  Commercial,
  Trains
}

export const SpeedometerBlackList: VehicleClass[] = [
  VehicleClass.Cycles,
];

declare module "alt-client" {
  export interface Vehicle {
    class: VehicleClass;
    engineState: boolean;
    setEngine: (state: boolean) => void;
    maxSpeed: number;
    newRpm: () => number;
    getSeatPed: (seat: number) => number | undefined;
  }
}

alt.Vehicle.prototype.class = VehicleClass.Default;
alt.Vehicle.prototype.newRpm = function () {
  return this.engineState ? this.rpm : 0;
}
alt.Vehicle.prototype.getSeatPed = function (seat: number) {
  if (native.isVehicleSeatFree(this, seat, false)) return null;
  return native.getPedInVehicleSeat(this, seat, true);
}
alt.Vehicle.prototype.engineState = false;
alt.Vehicle.prototype.setEngine = function (state: boolean) {
  this.engineState = state;
  native.setVehicleEngineOn(this, state, false, true);
}
alt.Vehicle.prototype.maxSpeed = 0;

new SyncedMeta("VehicleEngineState", (entity, value) => {
  if (entity instanceof alt.Vehicle) {
    entity.setEngine(value);
    if (player.vehicle != entity || player.seat != 0) return;
    browser.emit("Speedo:UpdateEngine", value);
  }
})

alt.on("gameEntityCreate", (entity) => {
  alt.log("created");
  if (entity instanceof Vehicle) {
    alt.log(`Test Max Speed: ${native.getVehicleEstimatedMaxSpeed(entity) * 3.6}`);
    alt.log(`Max Speed: ${native.getVehicleModelEstimatedMaxSpeed(entity.model) * 3.6}`);
    entity.maxSpeed = Math.round(native.getVehicleModelEstimatedMaxSpeed(entity.model) * 3.6);
    alt.log(`Max Speed: ${entity.maxSpeed}`);
    entity.class = native.getVehicleClass(entity);
  }
});