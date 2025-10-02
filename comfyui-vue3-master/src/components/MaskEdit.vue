<template>
    <div class="wrapper">
        <div class="img-wrapper">
            <img :src="imageURL" alt="上传图片">
            <input type="file" style="display: none;" ref="uploadRef" accept="image/*" @change="selectImage">
            <el-button class="btn" size="small" @click="uploadClick">上传图片</el-button>
        </div>
        <div class="img-wrapper" v-if="imageURL != '/src/assets/temp.png'">
            <img :src="maskURL" alt="编辑遮罩">
            <el-space class="btn">
                <el-button size="small" @click="showMask">编辑遮罩</el-button>
            </el-space>
        </div>
        <MaskCanvas :image-src="imageURL" v-if="sourceImage" ref="maskRef"></MaskCanvas>
    </div>
</template>
<script lang="ts" setup>

import { computed, onMounted, ref } from 'vue';
import MaskCanvas from './MaskCanvas.vue';
import message from '@/utils/message';
import emitter from '@/utils/eventBus';
import { saveImage } from '@/api/task';
import { getFilePath } from '@/utils';


const sourceImage = ref()
const maskImage = ref(null)
const maskRef = ref(null)

const uploadRef = ref(null)

const uploadClick = () => {
    uploadRef.value.click()
}

const showMask = () => {
    if (!sourceImage.value) {
        message.info('先上传图片在编辑遮罩')
        return
    }
    if (maskRef.value) {
        maskRef.value.show()
    }
}

// const clearMask = () => {
//     message.confirm('提示', '确定删除遮罩').then(() => {
//         maskImage.value = null
//     })
// }

const imageURL = computed(() => {
    if (sourceImage.value) {
        return getFilePath(sourceImage.value.filePath)
    } else {
        return '/static/temp.png'
    }
})

const maskURL = computed(() => {
    if (maskImage.value) {
        return getFilePath(maskImage.value.filePath)
    } else {
        return '/src/assets/temp.png'
    }
})

const selectImage = (event) => {
    const file = event.target.files[0]
    if (!file) return

    const formData = new FormData()
    formData.append('file', file)
    saveImage(formData).then(res => {
        //上传图片
        sourceImage.value = res.data
    })
}

onMounted(() => {
    //监听遮罩上传事件
    emitter.on('saveMask', data => {
        maskImage.value = data
        //发送原图和遮罩图
        const arr = { mask: maskImage.value, image: sourceImage.value }
        //发送事件
        emit('update', arr)
    })
})


const emit = defineEmits(['update'])
</script>
<style lang="less" scoped>
.wrapper {
    width: 100%;
    display: flex;
    flex-direction: row;
}

.img-wrapper {
    width: 140px;
    height: 140px;
    border: 1px solid #E4E7ED;
    position: relative;

    .btn {
        position: absolute;
        top: 58px;
        left: 35px;
        z-index: 1000;
        display: none;
    }

    .btn1 {
        position: absolute;
        top: 38px;
        left: 35px;
        z-index: 1000;
        display: none;
    }

    img {
        width: 100%;
        height: 100%;
        object-fit: contain;

        cursor: pointer;
    }
}

.img-wrapper:hover .btn {
    display: block;
}

.img-wrapper:hover .btn1 {
    display: block;
}

.img-wrapper+.img-wrapper {
    margin-left: 15px;
}
</style>