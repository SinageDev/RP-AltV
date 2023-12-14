export function ArrayRange(size:number, startAt:number = 0):ReadonlyArray<number> {
  return [...Array(size).keys()].map(i => i + startAt);
}

export function ArrayDefault(size: number, defaultValue: any): any {
  return [...Array(size).keys()].map(i => Array.isArray(defaultValue) ? defaultValue.slice() : defaultValue);
}