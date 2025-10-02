<template>
    <TaskForm :param="param">
        <template #prefix>
            <!-- <UploadForm :limit="1" @update="update" type="mask"></UploadForm> -->

            <ImageEdit @update="update"></ImageEdit>

            <el-form-item label="产品关键字">
                <el-input v-model="param.prodName" placeholder="请输入产品描述关键字"></el-input>
            </el-form-item>

        </template>
        <template #suffix>
            <el-form-item label="数量">
                <el-select v-model="param.unit">
                    <el-option :label="item.label" :value="item.value" v-for="(item) in counts"></el-option>
                </el-select>
            </el-form-item>
        </template>
    </TaskForm>
</template>
<script lang="ts" setup>
import ImageEdit from '@/components/ImageEdit.vue';
import TaskForm from '@/components/task/TaskForm.vue';
import { onMounted, ref } from 'vue';



const param = ref({
    mask: null,
    image: null,
    prodName: '',
    unit: 4
})

const update = (data) => {
    param.value.image = data
}

const counts = ref(
    [{ label: '1张', value: 1 },
    { label: '2张', value: 2 },
    { label: '3张', value: 3 },
    { label: '4张', value: 4 },
    { label: '5张', value: 5 },
    { label: '6张', value: 6 },
    { label: '7张', value: 7 },
    { label: '8张', value: 8 }
    ])

</script>
<style lang="less" scoped></style>