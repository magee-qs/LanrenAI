<template>
    <TaskForm :show-keywords="showKeywords" :show-pic-size-select="showPicSizeSelect" :param="param">
        <template #prefix>
            <UploadForm :limit="1" @update="update"></UploadForm>
        </template>
        <template #suffix>
            <el-form-item label="倍数" label-position="left" style="margin-top: 5px;">
                <el-col :span="6">
                    <el-select v-model="param.upscale">
                        <el-option label="2X" :value="2"></el-option>
                        <el-option label="4X" :value="4"></el-option>
                    </el-select>
                </el-col>
            </el-form-item>
        </template>
    </TaskForm>
</template>

<script setup lang="ts">
import TaskForm from '@/components/task/TaskForm.vue';
import UploadForm from '@/components/UploadImage.vue';
import { ref } from 'vue';

const showKeywords = ref(false)
const showPicSizeSelect = ref(false)
const param = ref({
    fileList: [],
    upscale: 2
})
const update = (data) => {
    param.value.fileList = data
}

</script>

<style lang="less" scoped></style>
