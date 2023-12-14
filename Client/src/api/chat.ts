import {BrowserEvent, ServerEvent} from "../managers/EventManager.js";
import {emitServer} from "alt-client";
import browser from "./cef.js";


function Send(...args: any[]) {
  browser.Call("Chat:Send", ...args);
}

new ServerEvent("Chat:Send", Send);
new BrowserEvent("Chat:Input", (text: string) => {
  if (text[0] == '/') {
    if (text.length == 1) return;
    const args = text.substring(1, text.length).split(" ");
    emitServer("Chat:Command", args.shift()?.toLowerCase(), ...args);
  } else emitServer("Chat:Message", text);
});