<template>
    <TaskForm :param="param" :show-keywords="false">
        <template #prefix>
            <el-form-item label="上传图片">
                <ImageEdit @update="update"></ImageEdit>
            </el-form-item>
            <el-text>扩图尺寸</el-text>
            <el-row style="width: 100%; margin-top: 5px;">
                <el-col :span="4">
                    <el-form-item label="上边" label-position="left">
                        <el-input-number v-model="param.top" :controls="false" :precision="0" />
                    </el-form-item>
                </el-col>
                <el-col :span="1"></el-col>
                <el-col :span="4">
                    <el-form-item label="左边" label-position="left">
                        <el-input-number v-model="param.left" :controls="false" :precision="0" />
                    </el-form-item>
                </el-col>
                <el-col :span="1"></el-col>
                <el-col :span="4">
                    <el-form-item label="右边" label-position="left">
                        <el-input-number v-model="param.right" :controls="false" :precision="0" />
                    </el-form-item>
                </el-col>
                <el-col :span="1"></el-col>
                <el-col :span="4">
                    <el-form-item label="下边" label-position="left">
                        <el-input-number v-model="param.bottom" :controls="false" :precision="0" />
                    </el-form-item>
                </el-col>
                <el-col :span="1"></el-col>
                <el-col :span="4">
                    <el-form-item label="边缘" label-position="left">
                        <el-input-number v-model="param.edge" :controls="false" :precision="0" />
                    </el-form-item>
                </el-col>
            </el-row>
        </template>
        <template #suffix>
            <PicSizeSelect :form="param" :show-scale="false" :show-size="false"></PicSizeSelect>
        </template>
    </TaskForm>
</template>
<script lang="ts" setup>
import ImageEdit from '@/components/ImageEdit.vue';
import PicSizeSelect from '@/components/task/PicSizeSelect.vue';
import TaskForm from '@/components/task/TaskForm.vue';
import { ref } from 'vue';


const param = ref({
    left: 0,
    right: 0,
    bottom: 0,
    top: 0,
    edge: 24,
    image: null,
    unit: 4
})

const update = (data) => {
    param.value.image = data
}

</script>
<style lang="less" scoped></style>