import * as alt from "alt-client";
import {ClientEvent} from "../managers/EventManager.js";
import {DrawText} from "../managers/DrawText.js";
import {Vector3} from "alt-shared";
import native from "natives";

const drawInfo = new Map<number, [DrawText, number]>();

function showVehicleInfo (vehicle: alt.Vehicle) {
  const drawText = `name: ${native.getDisplayNameFromVehicleModel(vehicle.model)}; ID = ${vehicle.id}`;
  let draw = new DrawText({
    text: drawText,
    scale: 0.3,
    position: new Vector3(0, 0, 0)
  }, vehicle);
  let tick = alt.everyTick(() => {
    draw.Text = drawText + `\nSpeed: ${Math.round(vehicle.speedVector.length * 3.6)}; Max: ${vehicle.maxSpeed}`
  });
  drawInfo.set(vehicle.id, [draw, tick]);
}


const dlEvents = [
  new ClientEvent("entityStreamEnter", (entity: alt.Entity) => {
    if (entity instanceof alt.Vehicle) {
      showVehicleInfo(entity);
    }
  }, true),
  new ClientEvent("entityStreamLeave", (entity: alt.Entity) => {
    if(entity instanceof alt.Vehicle) {
      if(drawInfo.has(entity.id)) {
        const draw = drawInfo.get(entity.id);
        draw![0].remove();
        alt.clearEveryTick(draw![1]);
      }
    }
  }, true)
];

alt.onServer("Dl:Enabled", (state: boolean) => {
  dlEvents.forEach(event => state ? event.on() : event.off());
  if(!state) {
    drawInfo.forEach((draw) => {
      draw[0].remove();
      alt.clearEveryTick(draw[1]);
    })
  } else {
    alt.Vehicle.streamedIn.forEach((veh) => {
      showVehicleInfo(veh);
    });
  }
});