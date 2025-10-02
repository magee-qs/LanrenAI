<template>
    <el-row>
        <el-col :span="7" v-if="showScale">
            <el-form-item label="图片比例" label-position="left">
                <el-select v-model="form.scale">
                    <el-option :label="item.label" :value="item.value" v-for="(item) in scales"></el-option>
                </el-select>
            </el-form-item>
        </el-col>
        <el-col :span="1" v-if="showScale"></el-col>
        <el-col :span="7" v-if="showUint">
            <el-form-item label="图片数量" label-position="left">
                <el-select v-model="form.unit">
                    <el-option :label="item.label" :value="item.value" v-for="(item) in counts"></el-option>
                </el-select>
            </el-form-item>
        </el-col>
        <!-- <el-col :span="1" v-if="showUint"></el-col>
        <el-col :span="8" v-if="showSize">
            <el-form-item label="清晰度" label-position="left">
                <el-select v-model="form.size">
                    <el-option :label="item.label" :value="item.value" v-for="(item) in sizes"></el-option>
                </el-select>
            </el-form-item>
        </el-col> -->
    </el-row>

    <el-row v-if="form.scale == 'define'">
        <el-col :span="7">
            <el-input v-model="form.width">
                <template #prepend>宽度</template>
                <template #append>px</template>
            </el-input>
        </el-col>
        <el-col :span="1"></el-col>
        <el-col :span="7">
            <el-input v-model="form.height">
                <template #prepend>高度</template>
                <template #append>px</template>
            </el-input>
        </el-col>
    </el-row>
</template>
<script lang="ts" setup>
import { ref } from 'vue';

const props = defineProps({
    form: {
        type: Object,
        default: {
            scale: '9:16',
            unit: 4,
            with: 768,
            height: 1024,
            size: 1
        }
    },
    showScale: {
        default: true
    },
    showUint: {
        default: true
    },
    showSize: {
        default: true
    }
})

const scales = ref([
    { label: '1:1', value: '1:1' },
    { label: '16:9', value: '16:9' },
    { label: '9:16', value: '9:16' },
    { label: '4:3', value: '4:3' },
    { label: '3:4', value: '3:4' },
    { label: '自定义', value: 'define' }
])

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

const sizes = ref([
    { label: '1K', value: 1 },
    { label: '2K', value: 2 },
    { label: '4K', value: 3 },
    { label: '8K', value: 4 }
])
</script>
<style lang="less">
.t1 {
    text-align: left;
}

.t1+.t1 {
    margin-left: 5px;
}
</style>