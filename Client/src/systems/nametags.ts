import * as alt from "alt-client";
import {DrawText} from "../managers/DrawText.js";
import {ClientEvent, ServerEvent} from "../managers/EventManager.js";
import {Vector3} from "alt-shared";
import {player} from "../index.js";
import {Entity} from "alt-client";

let nametags = new Map<number, DrawText>();

const events = [
  new ClientEvent("entityStreamEnter", (entity: alt.Entity) => {
    if (entity instanceof alt.Player) {
      alt.emitServer("getPlayerNameTag", entity.character);
    }
  }),
  new ServerEvent("playerSetNametag", (player: alt.Player, name: string) => {
    nametags.set(player.character, new DrawText({
      text: name + `\n#${player.character}`,
      scale: 0.3,
      position: new Vector3(0, 0, 1.3)
    }));
  }),
  new ClientEvent("entityStreamLeave", (entity: alt.Entity) => {
    if(entity instanceof alt.Player) {
      if(!nametags.has(entity.character)) return;
      nametags.get(entity.character)!.remove();
    }
  }),
  new ServerEvent("UpdateFriendId", (player: alt.Player, name: string) => {
    if(!nametags.has(player.character)) return;
    nametags.get(player.character)!.Text = name + `\n#${player.character}`;
  }),
];

export function load() {
  //events.forEach(event => event.on());
}

export function unload() {
  events.forEach(event => event.off());
}