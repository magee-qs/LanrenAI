<template>
    <div class="body">
        <el-space wrap direction="horizontal" :size="30" alignment="flex-start">
            <div class="list-item" v-for="(item) in list">
                <div class="img"><img :src="item.img" :alt="item.description" /></div>
                <div class="title"> <el-text>{{ item.name }}</el-text></div>
                <div class="menu">
                    <el-button @click="start(item)" type="primary">
                        <el-space>
                            <el-text style="color: #fff;">{{ item.cost }} <el-icon>
                                    <StarFilled />
                                </el-icon>
                            </el-text>
                            <el-text style="color: #fff;">运行</el-text>
                        </el-space>
                    </el-button>
                    <!-- <el-button @click="save(item)" type="primary">
                        <el-space>
                            <el-icon>
                                <Star />
                            </el-icon>
                            <span>收藏</span>
                        </el-space>
                    </el-button> -->
                </div>
            </div>
        </el-space>
    </div>
</template>
<script lang="ts" setup>
import router from '@/router';
import useAppStore from '@/stores/useAppStore';
import { Star, StarFilled } from '@element-plus/icons-vue';
import { onMounted, ref } from 'vue';



const list = ref(null)
const appStore = useAppStore()

// 加载数据
onMounted(() => {
    if (appStore.flowList == null || appStore.flowList.length == 0) {
        appStore.loadFlow().then(() => {
            list.value = appStore.flowList
        })
    } else {
        list.value = appStore.flowList
    }

})
//启动
const start = function (item: any) {
    router.push({ path: item.route })
}


</script>
<style scoped lang="less">
.body {
    min-width: 750px;
    max-width: 1440px;
    margin: 0 auto;
    padding-top: 20px;
    padding-bottom: 20px;
}

.list-item {
    width: 225px;
    line-height: 28px;

    .img {
        width: 225px;
        height: 300px;



        img {
            border: 1px solid #DCDFE6;
            border-radius: 8px;
            width: 100%;
            height: 100%;
            object-fit: cover;
        }
    }



    .title {
        font-size: 14px;
        color: rgb(24, 25, 28);
        /* 父容器宽度范围内截断 */
        margin-top: 5px;
        margin-bottom: 5px;
        text-align: left;
        overflow: hidden;
    }
}
</style>