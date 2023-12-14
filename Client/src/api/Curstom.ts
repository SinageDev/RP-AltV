import {IMarker} from "../interfaces/IMarker";
import * as native from "natives"
import * as alt from "alt-client"

export abstract class Custom {
  static createMarker(info: IMarker) {
    alt.setInterval(() => {
      // @ts-nocheck
      native.drawMarker(
        info.type,
        info.pos.x,
        info.pos.y,
        info.pos.z,
        info.dir.x,
        info.dir.y,
        info.dir.z,
        info.rot.x,
        info.rot.y,
        info.rot.z,
        info.scale.x,
        info.scale.y,
        info.scale.z,
        info.color.r,
        info.color.g,
        info.color.b,
        info.color.a,
        info.bobUpAndDown || false,
        info.face || true,
        2,
        info.rotate || false,
        //@ts-ignore
        info.textureDict,
        info.textureName,
        info.ignoreInt || false
      );
    }, 0);
  }
}