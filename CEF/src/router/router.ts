import { createRouter, createWebHashHistory } from "vue-router";
import routes from "./routes"
import {Mp} from "../services/Alt";

const router = createRouter({
  history: createWebHashHistory(),
  routes
});

router.beforeEach(() => {
  Mp.emit("ReadyState", false);
});

router.afterEach(() => {
  Mp.emit("ReadyState", true);
})

export default router;

Mp.events.add("Router:Change", (url: string) => router.push(url));