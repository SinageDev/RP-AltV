import alt, {Entity} from "alt-client";

export class StreamMeta<T extends Entity> {
  static All = new Map<string, (entity: alt.BaseObject, value: any, oldValue: any) => void>();

  constructor(key: string, action: (entity: T, value: any, oldValue: any) => void) {
    StreamMeta.All.set(key, action as (entity: alt.BaseObject, value: any, oldValue: any) => void);
  }
}

alt.on("streamSyncedMetaChange", (entity, key, val, oldVal) => {
  if(!StreamMeta.All.has(key)) return;
  StreamMeta.All.get(key)!(entity, val, oldVal);
});