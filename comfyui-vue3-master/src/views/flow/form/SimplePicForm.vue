<template>
    <TaskForm :show-keywords="showKeywords" :show-pic-size-select="showPicSizeSelect" :param="param">
        <template #prefix>
            <UploadForm :limit="1" @update="update"></UploadForm>
        </template>
    </TaskForm>
</template>
<script lang="ts" setup>
import TaskForm from '@/components/task/TaskForm.vue';
import UploadForm from '@/components/UploadImage.vue';
import { ref } from 'vue';

const showKeywords = ref(false)
const showPicSizeSelect = ref(false)
const param = ref({
    fileList: []
})
const update = (data) => {
    param.value.fileList = data
}
</script>