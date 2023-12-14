import * as alt from "alt-client";

alt.onceServer("AdminLoad", () => {
  import("./dl.js");
});
