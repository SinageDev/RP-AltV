import {onMounted, onUnmounted} from "vue";

export abstract class Mp {
  static emit(event: string, ...args: any[]) {
    if("alt" in window) window.alt.emit(event, ...args);
  }

  static emitServer(event: string, ...args: any[]) {
    Mp.emit("ToServer", event, ...args);
  }

  static events = {
    temp: TempEvent,
    add: AddEvent,
    off: OffEvent
  }
}

function TempEvent(event: string, listener: (...args: any[]) => void) {
  onMounted(() => AddEvent(event, listener));
  onUnmounted(() => OffEvent(event, listener));
}
function AddEvent(event: string, listener: (...args: any[]) => void) {
  if("alt" in window) window.alt.on(event, listener);
}
function OffEvent(event: string, listener: (...args: any[]) => void) {
  if("alt" in window) window.alt.on(event, listener);
}