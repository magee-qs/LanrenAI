<template>
    <div class="wrapper">
        <template v-if="url">
            <div class="code">
                <img :width="width" :src="url">
                <!-- <lay-qrcode :text="text" :width="width"></lay-qrcode> -->
            </div>
            <div class="memo">
                <slot>扫码支付</slot>
            </div>
        </template>
        <template v-else>
        </template>
    </div>
</template>
<script lang="ts" setup>
import { ref, watch } from 'vue';
import QRCode from 'qrcode'

const props = defineProps({
    width: {
        type: Number,
        default: 200
    },
    text: {
        type: String,
        default: ''
    }
})
const url = ref(null)
watch(props, (newVal, oldVal) => {
    let t1 = newVal.text
    let t2 = oldVal.text

    QRCode.toDataURL(t1).then(value => {
        debugger
        url.value = value
    }).catch(() => {
        url.value = null
    })
})
</script>
<style lang="less" scoped>
.wrapper {
    text-align: center;
}
</style>