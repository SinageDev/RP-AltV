import * as alt from "alt-client";
import * as native from "natives";
import {RGBA, Vector3} from "alt-shared";

interface IDrawText {
  message: string;
  position?: Vector3;
  scale?: number;
  font?: number;
  color?: RGBA;
  align?: number;
  useOutline?: boolean;
  useDropShadow?: boolean;
  attach?: number;
  offset?: Vector3;
}

export class DrawText {
  private static All: DrawText[] = [];

  private readonly Interval: number;

  constructor(info: IDrawText) {
    this.Interval = alt.setInterval(() => {
      if(info.position) native.setDrawOrigin(...info.position.toArray(), false);
      else if(info.attach && info.offset) {
        const entityPos = native.getEntityCoords(info.attach, false)
        native.setDrawOrigin(entityPos.x + info.offset.x, entityPos.y + info.offset.y, entityPos.z + info.offset.z, false);
      }
      else {
        native.setDrawOrigin(0, 0, 0, false);
      }
      native.beginTextCommandDisplayText('STRING');
      native.addTextComponentSubstringPlayerName(info.message);
      native.setTextFont(info.font || 0);
      native.setTextScale(1, info.scale || 0.5);
      native.setTextWrap(0.0, 1.0);
      native.setTextCentre(true);
      if(info.color) native.setTextColour(...info.color.toArray());
      else native.setTextColour(255, 255, 255, 255);
      native.setTextJustification(info.align || 0);
      if(info.useDropShadow) native.setTextDropShadow();
      if(info.useOutline) native.setTextOutline();
      native.endTextCommandDisplayText(0, 0, 0);
      native.clearDrawOrigin();
    }, 0);

    DrawText.All.push(this);
  }

  remove() {
    alt.clearInterval(this.Interval);
  }
}