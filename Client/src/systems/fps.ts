import * as alt from "alt-client";
import native from "natives";

let fps = alt.getFps();
let color = new alt.RGBA(255, 255, 255, 255);

alt.everyTick(() => {
  native.beginTextCommandDisplayText('STRING');
  native.addTextComponentSubstringPlayerName(`${alt.getFps()}`);
  native.setTextFont(0);
  native.setTextScale(1, 0.4);
  native.setTextWrap(0.0, 1.0);
  native.setTextCentre(true);
  native.setTextColour(...color.toArray());
  native.setTextJustification(0);
  native.setTextOutline();
  native.setTextDropShadow();
  native.endTextCommandDisplayText(0.99, 0, 0);
});

alt.setInterval(() => {
  fps = alt.getFps();
  if(fps < 30) color = new alt.RGBA(255, 0, 0, 255);
  else if(fps < 60) color = new alt.RGBA(255, 165, 0, 255);
  else color = new alt.RGBA(255, 255, 255, 255);
}, 1000)