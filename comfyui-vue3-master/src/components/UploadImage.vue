<template>
    <el-form-item :label="title">
        <el-upload ref="upload" :action="url" list-type="picture-card" v-model="fileList" :limit="limit"
            :on-preview="handlePictureCardPreview" :on-remove="handleRemove" :on-success="handleSuccess"
            :on-error="handleError" :on-exceed="handleExceed">
            <el-icon v-if="fileList.length < limit">
                <Plus />
            </el-icon>
        </el-upload>
        <el-dialog v-model="dialogVisible">
            <img w-full :src="dialogImageUrl" width="100%" alit="Preview Image" />
        </el-dialog>

        <MaskCanvas ref="mask"></MaskCanvas>
    </el-form-item>

</template>
<script lang="ts" setup>
import type { UploadProps } from 'element-plus';
import { computed, ref, watch } from 'vue';
import { Plus } from '@element-plus/icons-vue'
import { baseAPI } from '@/utils/request';
import useUserStore from '@/stores/useUserStore';
import message from '@/utils/message';
import { genFileId } from 'element-plus'
import MaskCanvas from './MaskCanvas.vue';


const props = defineProps({
    limit: {
        type: Number,
        default: 1
    },
    title: {
        type: String,
        default: '上传图片'
    },
    method: {
        type: String,
        default: 'image'
    }
})

const upload = ref(null)


const fileList = ref([])
const files = ref([])



const handleRemove: UploadProps['onRemove'] = (uploadFile, uploadFiles) => {
    console.log(uploadFile, uploadFiles)
    let response = uploadFile.response as any
    if (response && response.success) {
        //返回数据 { name: '', fileName: '', fielPath: '', ...} 
        let fileDTO = response.data
        if (!!fileDTO) {
            for (let i = 0; i < fileList.value.length; i++) {
                let filePath = fileList.value[i]
                if (filePath == fileDTO["filePath"]) {
                    fileList.value.splice(i, 1)
                    files.value.splice(i, 1)
                    i--
                }
            }
        }
    }
}

const handlePictureCardPreview: UploadProps['onPreview'] = (uploadFile) => {
    dialogImageUrl.value = uploadFile.url!
    dialogVisible.value = true
}

const handleSuccess = (res) => {
    if (res && res.success) {
        message.success('上传成功')
        // fileDTO {fileID: '', fileName: '', filePath: ''}
        files.value.push(res.data)
        fileList.value.push(res.data.filePath)
    } else {
        message.error(res.message)
    }
}

const handleError = () => {
    message.error('文件上传失败')
}

const handleExceed = (files) => {
    upload.value!.clearFiles()
    const file = files[0]
    file.uid = genFileId()
    upload.value!.handleStart(file)
}

const dialogImageUrl = ref('')
const dialogVisible = ref(false)
const userApp = useUserStore()

const url = computed(() => {
    return baseAPI + '/Task/UploadImage?X-Access-Token=' + encodeURI(userApp.token)
})

const emit = defineEmits(['update'])
watch(fileList.value, (newVal) => {
    console.log(newVal)
    emit('update', files.value)
})
</script>