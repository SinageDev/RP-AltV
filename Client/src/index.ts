export const player = alt.Player.local;

import * as alt from "alt-client"
import "./extensions/vehicle.js"
import "./extensions/player.js"

import "./events/resourceStart.js";
import "./events/resourceStop.js";
import "./events/enteredVehicle.js";
import "./events/streamIn.js";

import "./admin/loader.js";

import "./systems/fps.js";

import "./managers/Inventory.js";