<template>
    <div class="wrapper">
        <el-card>
            <el-row style="margin-top: 20px;">
                <el-space direction="horizontal">
                    <el-avatar src="/static/avatar/avatar4.webp"></el-avatar>
                    {{ userStore.userInfo.userName }}
                </el-space>
            </el-row>
            <el-row class="t t1" style="margin-top: 20px;">
                <el-col :span="12" class="t3"><el-text>会员类型</el-text></el-col>
                <el-col :span="12" class="t3"><el-text>{{ userLevel(userStore.userInfo.userLevel) }}</el-text></el-col>
            </el-row>
            <template v-if="userStore.userInfo.fee">
                <el-row class="t t2">
                    <el-col :span="12" class="t3"><el-text>有效期</el-text></el-col>
                    <el-col :span="12" class="t3"><el-text>
                            {{ formatDate(userStore.userInfo.fee.expire, 'YYYY-MM-DD') }}</el-text></el-col>
                </el-row>
                <el-row class="t t1">
                    <el-col :span="12" class="t3"><el-text>云盘空间</el-text></el-col>
                    <el-col :span="12" class="t3"><el-text>{{ userStore.userInfo.fee.store }}G</el-text></el-col>
                </el-row>
                <el-row class="t t2">
                    <el-col :span="12" class="t3"><el-text>星力值</el-text></el-col>
                    <el-col :span="12" class="t3"><el-text>{{ userStore.userInfo.fee.cost }}/月</el-text></el-col>
                </el-row>
                <el-row class="t t1">
                    <el-col :span="12" class="t3"><el-text>并发任务数量</el-text></el-col>
                    <el-col :span="12" class="t3"><el-text>{{ userStore.userInfo.fee.multiple }}个</el-text></el-col>
                </el-row>
            </template>
            <!-- <el-button @click="showDialog">绘制蒙版</el-button> -->
            <!-- <MaskCanvas image-src="/src/assets/demo2.png" ref="mask"></MaskCanvas> -->

        </el-card>
    </div>

</template>
<script lang="ts" setup>
import MaskCanvas from '@/components/MaskCanvas.vue';
import useUserStore from '@/stores/useUserStore';
import { userLevel, formatDate } from '@/utils';
import { ref } from 'vue';


const userStore = useUserStore()
const mask = ref(null)
const showDialog = () => {
    mask.value.show()
}
</script>
<style lang="less" scoped>
.wrapper {
    width: 900px;
    margin: 0 auto;
    padding-top: 20px;
}

.t {
    text-align: center;
    line-height: 60px;

    border: 1px solid #E4E7ED;
}

.t+.t {
    border-top: none;
}

.t3+.t3 {
    border-left: 1px solid #E4E7ED;
}

.t2 {
    background-color: #F2F6FC;
}
</style>