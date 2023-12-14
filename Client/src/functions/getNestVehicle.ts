import alt from "alt-client";
import {player} from "../index.js";
import {distance} from "./distance.js";

export function getNestVehicle() {
  if(alt.isCursorVisible()) return null;
  if(player.vehicle !== null) return null;
  let vehiclesInRange: Array<alt.Vehicle> =
    alt.Vehicle.streamedIn
      .filter((vehicle) => distance(player.pos, vehicle.pos) < 8);
  if(vehiclesInRange.length === 0) return null;
  let pressVehicle: [alt.Vehicle, number] | undefined;
  for(let vehicle of vehiclesInRange) {
    if(pressVehicle !== undefined && distance(player.pos, vehicle.pos) > pressVehicle[1]) continue;
    pressVehicle = [vehicle, distance(player.pos, vehicle.pos)];
  }
  if(pressVehicle == undefined) return null;
  return pressVehicle[0];
}