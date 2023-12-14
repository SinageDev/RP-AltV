export default [
  {
    path: '/creator',
    component: () => import("@views/Creator/Main.vue")
  },
  {
    path: '/hud',
    component: () => import("@views/Hud/Main.vue")
  },
  {
    path: '/select',
    component: () => import("@views/Select/Main.vue")
  },
  {
    path: '/dev',
    component: () => import("@views/Dev.vue")
  }
]