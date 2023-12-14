<template>
<div :class="$style.render">
  <canvas v-render :class="$style.canvas"></canvas>
</div>
</template>

<script lang="ts" setup>
import {Directive} from "vue";
import * as BABYLON from "babylonjs"
import 'babylonjs-loaders'

const createScene = (engine: BABYLON.Engine, canvas: HTMLElement) => {

  const scene = new BABYLON.Scene(engine);

  const camera = new BABYLON.FreeCamera('camera1', new BABYLON.Vector3(0, 0, -2), scene);

  camera.setTarget(BABYLON.Vector3.Zero());

  const light = new BABYLON.PointLight('pointLight', new BABYLON.Vector3(0, 0, -1.5), scene);
  // Create a built-in "sphere" shape using the SphereBuilder
  BABYLON.SceneLoader.ImportMesh(null, "/src/assets/", "untitled.glb", scene, (el) => {
    for(let i of el.slice(1)) {
      i.scaling.set(1, 1, 1);
      i.rotate(BABYLON.Axis.X, 1.5707963268, BABYLON.Space.WORLD);
      i.rotate(BABYLON.Axis.Y, 3.1415926536, BABYLON.Space.WORLD);
    }
  })
  return scene;
}

const vRender: Directive = {
  mounted: (el) => {
    const engine = new BABYLON.Engine(el, true, {preserveDrawingBuffer: true, stencil: true});
    const scene = createScene(engine, el);
    engine.runRenderLoop(function(){
      scene.render();
    });

    window.addEventListener('resize', function(){
      engine.resize();
    });
  }
}
</script>

<style lang="stylus" module>
.render
  width 100vw
  height 100vh

.canvas
  width 100%
  height 100%
</style>