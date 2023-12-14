import {Vector3} from "alt-shared";

export function degToRad(degrees: number) {
  return (degrees * Math.PI) / 180;
}

// Convert Radians to Degrees
export function radToDeg(radians: number) {
  return (radians * 180) / Math.PI;
}