<template>
    <TaskForm :show-keywords="showKeywords" :show-pic-size-select="showPicSizeSelect" :param="param">
        <template #prefix>
            <el-form-item label="上传图片">
                <ImageEdit @update="update"></ImageEdit>
            </el-form-item>

            <el-form-item label="图片数量">
                <el-select v-model="param.unit">
                    <el-option :label="item.label" :value="item.value" v-for="(item) in counts"></el-option>
                </el-select>
            </el-form-item>

        </template>
    </TaskForm>
</template>
<script setup lang="ts">
import ImageEdit from '@/components/ImageEdit.vue';
import TaskForm from '@/components/task/TaskForm.vue';
import { ref } from 'vue';

const showKeywords = ref(false)
const showPicSizeSelect = ref(false)

const param = ref({
    unit: 4,
    image: null
})

const update = (data) => {
    param.value.image = data
}

const counts = ref([
    { label: '1张', value: 1 },
    { label: '2张', value: 2 },
    { label: '3张', value: 3 },
    { label: '4张', value: 4 },
    { label: '5张', value: 5 },
    { label: '6张', value: 6 },
    { label: '7张', value: 7 },
    { label: '8张', value: 8 }
])
</script>