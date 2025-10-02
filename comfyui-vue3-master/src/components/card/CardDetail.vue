<template>
    <div class="body">
        <el-row class="t t0">
            <el-col :span="8">类型</el-col>
            <el-col :span="8">
                <img :src="vip.icon" width="20" height="20" style="margin-right: 8px;  vertical-align: middle;">{{
                    vip.title }}</el-col>
            <el-col :span="8">
                <img :src="svip.icon" width="20" height="20" style="margin-right: 8px; vertical-align: middle;">{{
                    svip.title
                }}</el-col>
        </el-row>
        <el-row class="t t1">
            <el-col :span="8">算力</el-col>
            <el-col :span="8">每月{{ vip.cost }}点</el-col>
            <el-col :span="8">每月{{ svip.cost }}点</el-col>
        </el-row>
        <el-row class="t t2">
            <el-col :span="8">云存储</el-col>
            <el-col :span="8"> {{ vip.store }}GB</el-col>
            <el-col :span="8"> {{ svip.store }}GB</el-col>
        </el-row>
        <el-row class="t t1">
            <el-col :span="8">模型训练</el-col>
            <el-col :span="8"> <template v-if="vip.train > 0">
                    模型训练 {{ vip.train }} 张
                </template>
                <template v-else>
                    <img src="/static/close.svg">
                </template>
            </el-col>
            <el-col :span="8"> <template v-if="svip.train > 0">
                    模型训练 {{ svip.train }} 张
                </template>
                <template v-else>
                    <img src="/static/close.svg">
                </template>
            </el-col>
        </el-row>
        <el-row class="t t2">
            <el-col :span="8">并行出图</el-col>
            <el-col :span="8"> {{ vip.multi }}个任务</el-col>
            <el-col :span="8"> {{ svip.multi }}个任务</el-col>
        </el-row>
    </div>
</template>
<script lang="ts" setup>
import { getCardConfig } from '@/api/order';
import { onMounted, ref } from 'vue';
const vip = ref({ title: '基础版', icon: '/static/diamond.png', cost: 1500, store: 5, train: 0, multi: 5 })
const svip = ref({ title: '专业版', icon: '/static/svip.png', cost: 3000, store: 10, train: 100, multi: 15 })

onMounted(() => {
    getCardConfig().then(res => {
        vip.value = res.data.vip
        svip.value = res.data.svip
    })
})
</script>
<style lang="less" scoped>
.body {
    width: 980px;
    margin: 0 auto;
}

.t {
    text-align: center;
    line-height: 60px;
    color: #303133;
    font-size: 14px;
    border: 1px solid #DCDFE6;

    .el-col+.el-col {
        border-left: 1px solid #DCDFE6;
    }
}

.t+.t {
    border-top: none;
}

.t0,
.t2 {
    background-color: #F2F6FC;
}

.t0 {
    font-size: 16px;
    font-weight: 600;
}
</style>