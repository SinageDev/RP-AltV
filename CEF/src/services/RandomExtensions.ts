export function randomInt(max: number) {
  return Math.floor(Math.random() * max);
}

export function randomInRange(min: number, max: number) {
  return Math.random() < 0.5 ? ((1-Math.random()) * (max-min) + min) : (Math.random() * (max-min) + min);
}

export function randomInArray(array: number[]) {
  return array[randomInt(array.length)];
}