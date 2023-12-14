import * as alt from "alt-client";
import {SyncedMeta} from "../managers/SyncedMeta";
import native from "natives";
import {player} from "../index.js";
import {StreamMeta} from "../managers/StreamMeta.js";

declare module "alt-client" {
  export interface Player {
    character: number;
    freeze: (state: boolean) => void;
  }
}

alt.Player.prototype.character = -1;

alt.Player.prototype.freeze = function (state: boolean) {
  native.freezeEntityPosition(this, state);
}

new StreamMeta<alt.Player>("Player:UpdateId", (player, value) => {
  player.character = value;
  alt.log(player.character);
})