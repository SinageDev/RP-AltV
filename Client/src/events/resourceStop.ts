import * as alt from "alt-client";
import browser from "../api/cef.js";
import {Ped} from "../managers/Ped.js";

alt.on("resourceStop", () => {
  browser.route?.unload();
  Ped.All.forEach(ped => ped.remove());
});