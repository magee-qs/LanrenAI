<template>
    <el-form-item>
        <el-row>
            <el-col :span="2"><el-text>噪波</el-text></el-col>
            <el-col :span="6">
                <el-input-number v-model="form.noise" :min="1" :max="10" size="small"></el-input-number>
            </el-col>

            <el-col :span="2"><el-text>噪波</el-text></el-col>
            <el-col :span="6">
                <el-input-number v-model="form.step" :min="1" :max="50" size="small"></el-input-number>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="2"><el-text>噪波</el-text></el-col>
            <el-col :span="4">
                <el-input-number v-model="form.cfg" :min="1" :max="10" size="small"></el-input-number>
            </el-col>
        </el-row>
    </el-form-item>
</template>
<script lang="ts" setup>
const props = defineProps({
    form: {
        type: Object,
        default: {
            noise: 10,
            step: 20,
            cfg: 7
        }
    }
})
</script>