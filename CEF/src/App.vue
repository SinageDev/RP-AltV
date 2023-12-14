<script lang="ts" setup>
import Loading from "./components/Loading.vue";
</script>

<template>
  <!--<Loading></Loading>-->
  <router-view />
</template>

<style lang="stylus">
body
  margin 0
  overflow hidden
  user-select none


*, *:after, *:before
  box-sizing border-box

#app
  margin 0 auto
  width 100vw
  height 100vh
  font-size 1vh

input, button
  border none
  outline none
  padding 0
  background none

/*@font-face
  font-family LcdMono2
  font-weight 300
  src url('./assets/fonts/lcdmono2/lcdmono2light.ttf')

@font-face
  font-family LcdMono2
  font-weight 400
  src url('./assets/fonts/lcdmono2/lcdmono2normal.ttf')

@font-face
  font-family LcdMono2
  font-weight 600
  src url('./assets/fonts/lcdmono2/lcdmono2ultra.ttf')*/

@font-face
  font-family LcdMono2
  font-weight 700
  src url('./assets/fonts/lcdmono2/lcdmono2bold.ttf')

@font-face
  font-family DinDisplay
  font-weight 100
  src url('./assets/fonts/DinDisplay/PFDinDisplayPro-Thin.woff2')

@font-face
  font-family DinDisplay
  font-weight 200
  src url('./assets/fonts/DinDisplay/PFDinDisplayPro-ExtraThin.woff2')

@font-face
  font-family DinDisplay
  font-weight 300
  src url('./assets/fonts/DinDisplay/PFDinDisplayPro-Light.woff2')

@font-face
  font-family DinDisplay
  font-weight 400
  src url('./assets/fonts/DinDisplay/PFDinDisplayPro-Regular.woff2')

@font-face
  font-family DinDisplay
  font-weight 500
  src url('./assets/fonts/DinDisplay/PFDinDisplayPro-Medium.woff2')

@font-face
  font-family DinDisplay
  font-weight 700
  src url('./assets/fonts/DinDisplay/PFDinDisplayPro-Bold.woff2')

@font-face
  font-family DinDisplay
  font-weight 900
  src url('./assets/fonts/DinDisplay/PFDinDisplayPro-Black.woff2')

@font-face
  font-family BebasNeue
  font-weight 300
  src url('./assets/fonts/BebasNeue/BebasNeueLight.woff2')

@font-face
  font-family BebasNeue
  font-weight 700
  src url('./assets/fonts/BebasNeue/BebasNeueBold.woff2')

@font-face
  font-family BebasNeue
  font-weight 100
  src url('./assets/fonts/BebasNeue/BebasNeueThin.woff2')

@font-face
  font-family BebasNeue
  font-weight 400
  src url('./assets/fonts/BebasNeue/BebasNeueRegular.woff2')
</style>