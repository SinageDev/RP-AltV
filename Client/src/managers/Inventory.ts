import * as alt from "alt-client";
import {CharacterInventory} from "arp-inventory";

alt.onServer("loadInventory", (inventory: string) => {
  let test = JSON.parse(inventory);
  alt.log(test);
});