import native from "natives";
import {Vector3} from "alt-shared";
import {degToRad} from "../functions/rotateConvert.js";
import * as alt from "alt-client";
export class Ped {
  public readonly instance: number;
  public readonly model: number;

  private _position: Vector3;
  get position() {
    return this._position;
  }

  set position(value: Vector3) {
    this._position = value;
    native.setEntityCoordsNoOffset(this.instance, value.x, value.y, value.z, false, false, false);
  }

  private _rotation: Vector3;
  get rotation() {
    return this._rotation;
  }

  set rotation(value: Vector3) {
    this._rotation = value;
    native.setEntityRotation(this.instance, value.x, value.y, value.z, 2, true);
  }

  private _freeze = false;
  get freeze() {
    return this._freeze;
  }

  set freeze(value: boolean) {
    this._freeze = value;
    native.freezeEntityPosition(this.instance, value);
  }

  constructor(pedType: number, model: number, pos: Vector3, heading: number, isNetwork = false, bScript = false) {
    this.instance = native.createPed(pedType, model, ...pos.toArray(), heading, isNetwork, bScript);
    this._position = pos;
    this._rotation = new Vector3(0, 0, degToRad(heading));
    this.model = model;
  }

  public setCustomization(customization: IPedCustomization) {
    this.headBlendData = customization.HeadBlendData;
    this.eyeColor = customization.EyeColor;
    this.hair = customization.Hair;
    this.faceFeatures = customization.FaceFeatures;
    this.headOverlay = customization.HeadOverlays;
  }

  private _headBlendData: IHeadBlendData = {
    Mather: 0,
    Father: 0,
    Shape: 0.5,
    Skin: 0.5
  };
  set headBlendData(value: IHeadBlendData) {
    this._headBlendData = value;
    native.setPedHeadBlendData(this.instance, value.Mather, value.Father, 0, value.Mather, value.Father, 0, value.Shape, value.Skin, 0.0, false);
  }

  get headBlendData(): IHeadBlendData {
    return this._headBlendData;
  }

  private _faceFeatures: Map<number, number> = new Map();
  set faceFeatures(value: Map<number, number>) {
    this._faceFeatures = value;
    Object.entries(value).forEach(([key, size]) => native.setPedMicroMorph(this.instance, Number(key), size));
  }
  get faceFeatures(): Map<number, number> {
    return this._faceFeatures;
  }

  private _headOverlay: Map<number, IHeadOverlay> = new Map();
  set headOverlay(value: Map<number, IHeadOverlay>) {
    this._headOverlay = value;
    Object.entries(value).forEach(([key, params]: [string, IHeadOverlay]) => {
      native.setPedHeadOverlay(this.instance, Number(key), params.Index, params.Opacity);
      native.setPedHeadOverlayTint(this.instance, Number(key), 1, params.FirstColor, params.SecondColor);
    });
  }
  get headOverlay(): Map<number, IHeadOverlay> {
    return this._headOverlay;
  }

  private _hair: [number, number, number] = [0, 0, 0];
  set hair(value: [number, number, number]) {
    this._hair = value;
    native.setPedComponentVariation(this.instance, 2, value[0], 0, 0);
    native.setPedHairTint(this.instance, value[1], value[2]);
  }
  get hair() {
    return this._hair;
  }

  private _eyeColor: number = 0;
  set eyeColor(value: number) {
    this._eyeColor = value;
    native.setHeadBlendEyeColor(this.instance, value);
  }
  get eyeColor() {
    return this._eyeColor;
  }
}
