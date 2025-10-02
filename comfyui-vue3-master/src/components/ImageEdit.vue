<template>
    <div class="wrapper">
        <div class="img-wrapper">
            <img :src="imageURL" alt="上传图片">
            <input type="file" style="display: none;" ref="uploadRef" accept="image/*" @change="selectImage">
            <div class="btnWapper">
                <el-button @click="uploadClick">上传图片</el-button>
                <el-button @click="previewClick" style="margin-left: 0px; margin-top: 5px;">预览图片</el-button>
            </div>
        </div>
        <el-dialog v-model="showPreview">
            <div class="preview"> <img :src="imageURL" /></div>

        </el-dialog>
    </div>
</template>
<script lang="ts" setup>
import { saveImage } from '@/api/task';
import { getFilePath } from '@/utils';
import { ref, computed } from 'vue';

const image = ref(null)
const showPreview = ref(false)

const uploadRef = ref(null)

const uploadClick = () => {
    uploadRef.value.click()
}

const imageURL = computed(() => {
    if (image.value) {
        return getFilePath(image.value.filePath)
    } else {
        return '/static/temp.png'
    }
})

const selectImage = (event) => {
    const file = event.target.files[0]
    if (!file) return

    const formData = new FormData()
    formData.append('file', file)
    saveImage(formData).then(res => {
        //上传图片
        image.value = res.data
        //触发update事件
        emit('update', image.value)
    })
}

const previewClick = () => {
    showPreview.value = true
}

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

    .btnWapper {
        position: absolute;
        z-index: 1000;
        display: none;
        text-align: center;
        top: 35px;
    }


    img {
        width: 100%;
        height: 100%;
        object-fit: contain;

        cursor: pointer;
    }
}

.img-wrapper:hover .btnWapper {
    display: block;
}

.preview {
    width: 500px;
    height: 650px;
    text-align: center;

    img {
        width: 100%;
        height: 100%;
        object-fit: contain;
    }

}
</style>