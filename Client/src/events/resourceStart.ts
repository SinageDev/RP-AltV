import * as alt from "alt-client";
import {loadModel} from "../functions/loadModel.js";
import native from "natives";
import {player} from "../index.js";

alt.on("resourceStart", async () => {
  native.pauseClock(true);
  await loadModel(alt.hash('MP_F_Freemode_01'));
  await loadModel(alt.hash('MP_M_Freemode_01'));

  alt.everyTick(() => {
    native.disableControlAction(0, 23, true);
  });

  alt.requestIpl("hei_carrier");

  native.displayHud(false);
  alt.setWeatherSyncActive(false);
});