<template>
    <TaskForm :param="param">
        <template #suffix>
            <PicSizeSelect :form="param"></PicSizeSelect>
        </template>
    </TaskForm>
</template>
<script lang="ts" setup>
import PicSizeSelect from '@/components/task/PicSizeSelect.vue';
import TaskForm from '@/components/task/TaskForm.vue';
import { ref } from 'vue';

const param = ref({
    translacte: false,
    unit: 4,
    scale: '9:16',
    width: 1024,
    height: 1024
})
</script>
<style lang="less" scoped></style>