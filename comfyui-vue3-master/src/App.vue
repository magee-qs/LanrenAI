<template>
  <div class="common-layout">
    <el-affix class="affix">
      <Header></Header>
    </el-affix>
    <div class="main">
      <RouterView></RouterView>
    </div>
    <Footer></Footer>
  </div>
</template>

<script lang="ts" setup>
import { RouterView } from 'vue-router'
import Header from './components/layouts/Header.vue';
import Footer from './components/layouts/Footer.vue';
import useUserStore from './stores/useUserStore';
import { ref, watch, watchEffect } from 'vue';
import { signalR } from './utils/signalR'
const userStore = useUserStore()
const token = ref(userStore.token)
if (token.value) {
  signalR.init(token.value)
  signalR.start()
}

watch(token, (newVal, oldVal) => {
  if (newVal != oldVal && newVal != null) {
    //重启signalR
    signalR.init(token.value)
    signalR.start()
  }
})
</script>
<style scoped lang="less">
.affix {
  z-index: 999;
}

.main {
  min-height: calc(100vh - 145px);
}
</style>
