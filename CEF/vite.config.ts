import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import * as path from "path"
import svgLoader from "vite-svg-loader"
const srcFolder = path.join(__dirname, './src/');

// https://vitejs.dev/config/
export default defineConfig({
  optimizeDeps: {
    exclude: ["arp-inventory"]
  },
  base: "./",
  build: {
    outDir: "../../resources/cef/assets",
  },
  plugins: [vue({
    reactivityTransform: true
  }), svgLoader({
    svgoConfig: {
      plugins: [
        {
          name: 'removeViewBox',
          active: false,
        },
        {
          name: "removeElementsByAttr",
          active: false
        }
      ],

    }
  })],
  resolve: {
    alias: {
      "@views": path.join(srcFolder, "./views/"),
      "@images": path.join(srcFolder, "./assets/images/")
    }
  },
  css: {
    preprocessorOptions: {
      stylus: {
        imports: [path.resolve(__dirname, 'src/assets/stylus/*.styl')]
      }
    }
  }
})
