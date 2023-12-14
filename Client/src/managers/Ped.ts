import native from "natives";
import {Vector3} from "alt-shared";
import {degToRad, radToDeg} from "../functions/rotateConvert.js";
import * as alt from "alt-client";

const headOverlayType = new Map([[2, 1], [1, 1], [10, 1], [5, 2], [8, 2]]);

function checkIfMap(check: any): Map<any, any> {
  if(check instanceof Map) return check;
  else if(Array.isArray(check)) {

  }
  else if(typeof check === "object") return new Map(Object.entries(check));
  return new Map();
}

export class Ped {
  public static All = new Set<Ped>();
  public instance: number;
  private _model: number;
  get model() {
    return this._model;
  }

  set model(value: number) {
    this._model = value;
    native.deletePed(this.instance);
    const {x, y, z} = this.position;
    this.instance = native.createPed(0, value, x, y, z, this.heading, false, false);
    this.freeze = this.freeze;
  }

  private _position: Vector3;
  get position() {
    return this._position;
  }

  set position(value: Vector3) {
    this._position = value;
    native.setEntityCoordsNoOffset(this.instance, value.x, value.y, value.z, false, false, false);
  }

  private _heading: number;
  get heading() {
    return radToDeg(this._rotation.z);
  }

  set heading(value) {
    this.rotation = new Vector3(0, 0, value);
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

  constructor(model: number, pos: Vector3, heading: number, isNetwork = false, bScript = false) {
    this.instance = native.createPed(0, model, ...pos.toArray(), heading, isNetwork, bScript);
    this._position = pos;
    this._heading = heading;
    this._rotation = new Vector3(0, 0, degToRad(heading));
    this._model = model;

    Ped.All.add(this);

    this.headBlendData = {
      Mather: 21,
      Father: 0,
      Shape: 0.5,
      Skin: 0.5
    }
  }

  public remove() {
    native.deletePed(this.instance);
    Ped.All.delete(this);
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

  private _faceFeatures = new Map<number, number>();
  set faceFeatures(value: Map<number, number>) {
    this.clearFaceFeatures();
    this._faceFeatures = checkIfMap(value);
    this._faceFeatures.forEach((value, key) => native.setPedMicroMorph(this.instance, key, +value));
  }

  get faceFeatures() {
    return this._faceFeatures;
  }

  public setFaceFeature(index: number, value: number) {
    this._faceFeatures.set(index, value);
    native.setPedMicroMorph(this.instance, index, value);
  }

  public clearFaceFeatures() {
    if (!this._faceFeatures.size) return;
    this._faceFeatures.forEach((value, key) => native.setPedMicroMorph(this.instance, key, 0.0));
    this._faceFeatures.clear();
  }

  private _headOverlay = new Map<number, IHeadOverlay>();
  set headOverlay(value: Map<number, IHeadOverlay>) {
    this._headOverlay = checkIfMap(value);
    this._headOverlay.forEach((overlay, key) => {
      native.setPedHeadOverlay(this.instance, key, overlay.Index, overlay.Opacity);
      native.setPedHeadOverlayTint(this.instance, key, 1, overlay.FirstColor, overlay.SecondColor);
    });
  }

  get headOverlay() {
    return this._headOverlay;
  }

  public setHeadOverlay(overlayId: number, overlayInfo: IHeadOverlay) {
    this._headOverlay.set(overlayId, overlayInfo);
    native.setPedHeadOverlay(this.instance, overlayId, overlayInfo.Index, overlayInfo.Opacity);
    native.setPedHeadOverlayTint(this.instance,
      overlayId, headOverlayType.has(overlayId) ? headOverlayType.get(overlayId)! : 0, overlayInfo.FirstColor, overlayInfo.SecondColor);
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

  public setHair(model: number, color1: number, color2: number) {
    this.hair = [model, color1, color2];
  }

  public setHairColor(color1: number, color2: number) {
    this._hair[1] = color1;
    this._hair[2] = color2;
    native.setPedHairTint(this.instance, color1, color2);
  }

  private _eyeColor: number = 0;
  set eyeColor(value: number) {
    this._eyeColor = value;
    native.setHeadBlendEyeColor(this.instance, value);
  }

  get eyeColor() {
    return this._eyeColor;
  }

  public setEyeColor(color: number) {
    this.eyeColor = color;
  }

  public setClothes(componentId: number, drawableId: number, textureId: number) {
    native.setPedComponentVariation(this.instance, componentId, drawableId, textureId, 0);
  }
}