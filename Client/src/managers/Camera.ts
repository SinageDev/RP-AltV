import native from "natives";
import {Vector3} from "alt-shared";

export abstract class Camera {
  protected instance: number;
  static current?: Camera;

  protected constructor(cam: number) {
    this.instance = cam;
  }

  public setActive() {
    Camera.current = this;
    native.setCamActive(this.instance, true);
  }

  public setActiveDuration(duration: number) {
    if(Camera.current === undefined || Camera.current === this) return;
    native.setCamActiveWithInterp(this.instance, Camera.current.instance, duration, 4, 1);
    Camera.current = this;
  }

  public static EnableCamera() {

  }
}

export class DefaultCamera extends Camera {
  private _position: Vector3;
  get position() {
    return this._position;
  }

  set position(value: Vector3) {
    this._position = value;
    native.setCamCoord(this.instance, value.x, value.y, value.z);
  }

  private _rotation;
  get rotation() {
    return this._rotation;
  }

  set rotation(value: Vector3) {
    this._rotation = value;
    native.setCamRot(this.instance, value.x, value.y, value.z, 2);
  }

  private _fov;
  get fov() {
    return this._fov;
  }

  set fov(value: number) {
    this._fov = value;
    native.setCamFov(this.instance, value);
  }

  constructor(pos: Vector3, rot = new Vector3(0, 0, 0), fov = 50) {
    super(native.createCamWithParams("DEFAULT_SCRIPTED_CAMERA", ...pos.toArray(), ...rot.toArray(), fov, false, 2));
    this._position = pos;
    this._rotation = rot;
    this._fov = fov;
  }
}

export class AttachCamera extends Camera {
  public target: number;

  private _offset: Vector3 = new Vector3(0, 0, 0);
  get offset() {
    return this._offset;
  }
  set offset(value) {
    this._offset = value;
    native.attachCamToEntity(this.instance, this.target, ...value.toArray(), true);
  }
  private _point: Vector3 = new Vector3(0, 0, 0);
  get point() {
    return this._point;
  }
  set point(value) {
    this._point = value;
    native.pointCamAtEntity(this.instance, this.target, ...value.toArray(), true);
  }

  constructor(entity: number, camOffset: Vector3, pointOffset: Vector3) {
    super(native.createCam("DEFAULT_SCRIPTED_CAMERA", false))
    this.target = entity;
    this.offset = camOffset;
    this.point = pointOffset;
  }
}