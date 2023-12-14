<template>
  <svg :class="$style.svg" width="310" height="271" viewBox="0 0 310 271" fill="none" xmlns="http://www.w3.org/2000/svg">
    <defs>
      <mask id="testClip">
        <rect x='0' y='0' width='310' height='271' fill='white' />
        <path d="M188.493 208.541H40.1803L10 260.832H218.803L188.493 208.541Z" fill="black"/>
        <path d="M112.626 187.625L186.874 59.6275L156.5 7L52.251 187.625H112.626Z" fill="black"/>
        <path d="M303 260.832L198.929 80.5146L168.68 132.663L242.972 260.832H303Z" fill="black"/>
        <path d="M176.368 187.625L156.592 153.504L136.799 187.625H176.368Z" fill="black"/>
      </mask>
    </defs>
    <!--<rect fill='#323232' width='100%' height='100%' mask='url(#testClip)' />-->
  </svg>
<div :class="$style.container">
</div>
</template>

<script lang="ts" setup>
import clip from "@images/loading/cut.svg";
</script>

<style lang="stylus" module>
.container
  width 100vw
  height 100vh
  pos(left: 0, top: 0)
  background #000
  //background url("@images/loading/background.png") center center no-repeat
  //background-size cover
  mask url("#testClip")
  z-index 1000
</style>
