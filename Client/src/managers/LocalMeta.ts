import * as alt from "alt-client";

type LocalMetaEvent = (value: any, oldValue: any) => void;

export class LocalMeta {
  static All = new Map<string, LocalMetaEvent[]>();

  constructor(key: string, action: LocalMetaEvent) {
    if(LocalMeta.All.has(key)) {
      LocalMeta.All.get(key)!.push(action);
    } else {
      LocalMeta.All.set(key, [action]);
    }
  }
}

alt.on("localMetaChange", (key, val, oldVal) => {
  if(!LocalMeta.All.has(key)) return;
  LocalMeta.All.get(key)!.forEach((value) => value(val, oldVal));
});