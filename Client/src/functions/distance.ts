import {Vector3} from "alt-shared";

export function distance(vector1: Vector3, vector2: Vector3) {
  return Math.sqrt(
    Math.pow(vector1.x - vector2.x, 2) + Math.pow(vector1.y - vector2.y, 2) + Math.pow(vector1.z - vector2.z, 2)
  );
}