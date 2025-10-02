import { fileURLToPath, URL } from 'node:url'

import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'
import path from 'path'


export default defineConfig(({ mode, command }) => {
  const env = loadEnv(mode, process.cwd())
  return {
    plugins: [vue(), vueDevTools()],
    base: process.env.NODE_ENV === 'production' ? './' : '/',
    resolve: {
      alias: {
        //'@': fileURLToPath(new URL('./src', import.meta.url))
        '@': path.resolve(__dirname, './src'),
      }
    },
    server: {
      port: 3000
    },
    build: {
      target: 'es2015',
    }
  }
})

