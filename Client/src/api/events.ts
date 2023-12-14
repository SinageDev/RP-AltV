import * as alt from "alt-client";
import browser from "./cef";

export abstract class EventManager {
  public Name: string;
  public Listener: (...args: any[]) => void;

  constructor(name: string, listener: (...args: any[]) => void, lazy?: boolean) {
    this.Name = name;
    this.Listener = listener;
    if(!lazy) this.on();
  }

  abstract on(): void;

  abstract off(): void;

  static add(events: EventManager[]) {
    return events;
  }
}

export class ClientEvent extends EventManager {
  on() {
    alt.on(this.Name, this.Listener);
  }

  off() {
    alt.off(this.Name, this.Listener);
  }
}

export class ServerEvent extends EventManager {
  on() {
    alt.onServer(this.Name, this.Listener);
  }

  off() {
    alt.offServer(this.Name, this.Listener);
  }
}

export class BrowserEvent extends EventManager {
  on() {
    browser.on(this.Name, this.Listener);
  }

  off() {
    browser.off(this.Name, this.Listener);
  }
}