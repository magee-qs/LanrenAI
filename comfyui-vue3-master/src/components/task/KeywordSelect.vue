<template>
    <el-form-item label="创意描述">
        <el-input v-model="form.keyword" type="textarea" :autosize="{ minRows: 2, maxRows: 4 }" />
    </el-form-item>
    <el-form-item label="不希望呈现的内容">
        <el-input v-model="form.negtive" type="textarea" :autosize="{ minRows: 2, maxRows: 4 }" />
    </el-form-item>
</template>
<script lang="ts" setup>
const props = defineProps({
    form: {
        type: Object,
        default: {
            keyword: '',
            negtive: ''
        }
    }
})
</script>