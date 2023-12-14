import * as alt from "alt-client"
import {BrowserEvent, ServerEvent} from "../managers/EventManager.js";

export class WebBrowser extends alt.WebView {
  ready: boolean = false;
  route?: { load: (...args: any[]) => void; unload: (...args: any[]) => void; };

  constructor(url: string) {
    super(url);
    this.focus();
  }

  private async WaitBrowserLoad() {
    while (!this.ready) await alt.Utils.wait(200);
  }

  public Call(name: string, ...args: any[]) {
    this.WaitBrowserLoad().then(() => this.emit(name, ...args))
  }

  public async ChangeRoute(url: string, args?: any[]) {
    this.ready = false;
    this.emit("Router:Change", "/" + url);
    try {
      if (this.route !== undefined) this.route.unload();
      this.route = await import(`../routes/${url}.js`);
      if(this.route == undefined) return;
      if (!args) this.route.load();
      else this.route.load(...args);
    } catch (e) {
      alt.log(e);
    }
  }
}

export const browser = new WebBrowser("http://localhost:3000/#/");
export default browser;

new BrowserEvent("ToServer", (name: string, ...args: any[]) => alt.emitServer(name, ...args));
new BrowserEvent("ReadyState", (state: boolean) => browser.ready = state);
new BrowserEvent("CursorState", (state: boolean) => {
  if (alt.isCursorVisible() === state) return;
  alt.showCursor(state);
  alt.toggleGameControls(!state);
});
new ServerEvent("ToCef", (name: string, args: any[]) => browser.Call(name, ...args));
new ServerEvent("Route:Change", browser.ChangeRoute.bind(browser));