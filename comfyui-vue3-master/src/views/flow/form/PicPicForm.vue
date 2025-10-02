<template>
    <TaskForm :param="param">
        <template #prefix>
            <ImageEdit @update="update"></ImageEdit>
        </template>
        <template #suffix>
            <PicSizeSelect :form="param"></PicSizeSelect>
        </template>
    </TaskForm>
</template>
<script setup lang="ts">
import ImageEdit from '@/components/ImageEdit.vue';
import PicSizeSelect from '@/components/task/PicSizeSelect.vue';
import TaskForm from '@/components/task/TaskForm.vue';
import { ref } from 'vue';

const param = ref({
    trancelate: false,
    image: null,
    unit: 4,
    scale: '9:16',
    width: 1024,
    height: 1024
})

const update = (data) => {
    param.value.image = data
}
</script>
