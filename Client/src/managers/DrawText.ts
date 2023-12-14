import * as alt from "alt-client";
import {RGBA, Vector3} from "alt-shared";
import {Entity} from "alt-client";
import native from "natives";
import {player} from "../index.js";
import {distance} from "../functions/distance.js";

interface IDrawText {
  text: string;
  position: Vector3;
  scale?: number;
  font?: number;
  color?: RGBA;
  align?: number;
  useOutline?: boolean;
  useDropShadow?: boolean;
  range?: number;
}

export class DrawText {
  public static All = new Set<DrawText>();

  public Text: string;
  public Color: RGBA;
  public Position: Vector3;
  public UseOutline: boolean;
  public UseDropShadow: boolean;
  public Font: number;
  public Align: number;
  public Scale: number;
  public Range = 10;
  public AttachTick?: number;

  constructor(info: IDrawText, entity?: Entity) {
    this.Text = info.text;
    this.Color = info.color || new RGBA(255,255,255, 255);
    this.Position = info.position || new Vector3(0,0,0);
    this.UseOutline = info.useOutline || true;
    this.Font = info.font || 0;
    this.UseDropShadow = info.useDropShadow || true;
    this.Align = info.align || 0;
    this.Scale = info.scale || 0.5;
    this.Range = info.range || 10;

    if(entity) this.attach(entity);

    DrawText.All.add(this);
  }

  remove() {
    DrawText.All.delete(this);
  }

  attach(entity: Entity) {
    if(this.AttachTick !== undefined) {
      alt.clearEveryTick(this.AttachTick);
      this.AttachTick = undefined;
    }
    const offsetPos = this.Position;
    this.AttachTick = alt.everyTick(() => {
      this.Position = entity.pos.add(offsetPos);
    });
  }
}

alt.everyTick(() => {
  for(let drawText of DrawText.All) {
    if(distance(player.pos, drawText.Position) > drawText.Range) continue;
    native.setDrawOrigin(drawText.Position.x, drawText.Position.y, drawText.Position.z, false);
    native.beginTextCommandDisplayText('STRING');
    native.addTextComponentSubstringPlayerName(drawText.Text);
    native.setTextFont(drawText.Font);
    native.setTextScale(1, drawText.Scale);
    native.setTextWrap(0.0, 1.0);
    native.setTextCentre(true);
    native.setTextColour(drawText.Color.r, drawText.Color.g, drawText.Color.b, drawText.Color.a);
    native.setTextJustification(drawText.Align);
    if (drawText.UseDropShadow) native.setTextDropShadow();
    if (drawText.UseOutline) native.setTextOutline();
    native.endTextCommandDisplayText(0, 0, 0);
    native.clearDrawOrigin();
  }
});