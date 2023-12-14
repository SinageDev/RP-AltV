import * as alt from "alt-client";
import {Entity} from "alt-client";

function EqualsArray(array1: readonly any[], array2: readonly any[]) {
  if(array1.length < array2.length || array1.length > array2.length) return false;
  for(let i = 0; i < array1.length; i++) if(array2[i] === undefined || array1[i] !== array2[i]) return false;
  return true;
}

let playersStreamedIn = alt.Player.streamedIn.filter(stream => stream.character !== -1);
let vehiclesStreamedIn = alt.Vehicle.streamedIn;

alt.setInterval(() => {
  const enterStreamed: Entity[] = [];
  const leaveStreamed: Entity[] = [];

  const playersIn = alt.Player.streamedIn.filter(stream => stream.character !== -1);
  if (!EqualsArray(playersIn, playersStreamedIn)) {
    enterStreamed.push(...playersIn.filter(stream => !playersStreamedIn.includes(stream)));
    leaveStreamed.push(...playersStreamedIn.filter(stream => !playersIn.includes(stream)));
    playersStreamedIn = playersIn;
  }

  const vehiclesIn = alt.Vehicle.streamedIn;
  if(!EqualsArray(vehiclesIn, vehiclesStreamedIn)) {
    enterStreamed.push(...vehiclesIn.filter(stream => !vehiclesStreamedIn.includes(stream)));
    leaveStreamed.push(...vehiclesStreamedIn.filter(stream => !vehiclesIn.includes(stream)));
    vehiclesStreamedIn = vehiclesIn;
  }

  for(let enter of enterStreamed) {
    alt.log("stream enter: ", enter.id);
    alt.emit("entityStreamEnter", enter);
  }
  for(let leave of leaveStreamed) {
    alt.log("stream leave: ", leave.id);
    alt.emit("entityStreamLeave", leave);
  }
}, 1000);