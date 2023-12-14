import * as alt from "alt-client"

export interface IMarker {
  type: number;
  pos: alt.Vector3;
  dir: alt.Vector3;
  rot: alt.Vector3;
  scale: alt.Vector3;
  color: alt.RGBA;
  face?: boolean; //true = автоматический поворот в сторону игрока
  ignoreInt?: boolean; //true = рисует сквозь стены
  textureDict?: string;
  textureName?: string;
  rotate?: boolean; //Поворачивать только по оси Y
  bobUpAndDown?: boolean;
}