import * as alt from "alt-client";

export class SyncedMeta {
  static All: SyncedMeta[] = [];

  name: string;
  task: (entity: alt.Entity, value: any, oldValue: any) => void;
  isOnce?: boolean;

  constructor(key: string, action: (entity: alt.Entity, value: any, oldValue: any) => void, once?: boolean) {
    this.name = key;
    this.task = action;
    this.isOnce = once;

    SyncedMeta.All.push(this);
  }

  remove() {
    const index = SyncedMeta.All.findIndex(x => x === this);
    if(index != -1) SyncedMeta.All.splice(index, 1);
  }
}

alt.on("syncedMetaChange", (entity, key, val, oldVal) => {
  for(let meta of SyncedMeta.All) {
    alt.log(meta.name, key);
    if(key !== meta.name) continue;
    meta.task(entity, val, oldVal);
    if(meta.isOnce) meta.remove();
    return;
  }
});