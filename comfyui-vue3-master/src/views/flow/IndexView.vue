<template>
    <div class="form-warpper">
        <el-row>
            <el-col :span="12">
                <div class="form-body">
                    <RouterView></RouterView>
                </div>
            </el-col>
            <el-col :span="1"></el-col>
            <el-col :span="11">
                <el-card>
                    <template #header><el-text>任务列表</el-text></template>
                    <el-row v-if="!!task.keyword" style="margin-bottom: 5px;">
                        <el-text>{{ task.keyword }}</el-text>
                    </el-row>

                    <el-result icon="success" title="任务提交成功" sub-title="后台正在加速处理生成图片中..."
                        v-if="state == 'doing'"></el-result>
                    <el-result icon="error" title="任务执行失败" v-if="state == 'error'"></el-result>

                    <div class="img-list" v-if="state == 'done'">
                        <el-space wrap>
                            <el-image :src="src" fit="contain" style="width: 205px;" v-for="(src) in thumbList"
                                :preview-src-list="srcList">

                                <template #toolbar="{ actions, prev, next, reset, activeIndex }">
                                    <el-icon @click="prev">
                                        <Back />
                                    </el-icon>
                                    <el-icon @click="next">
                                        <Right />
                                    </el-icon>

                                    <el-icon @click="actions('zoomOut')">
                                        <ZoomOut />
                                    </el-icon>
                                    <el-icon @click="actions('zoomIn', { enableTransition: false, zoomRate: 2 })">
                                        <ZoomIn />
                                    </el-icon>


                                    <el-icon @click="reset">
                                        <Refresh />
                                    </el-icon>
                                    <el-icon @click="downloadImage(srcList[activeIndex])">
                                        <Download />
                                    </el-icon>
                                </template>

                            </el-image>
                        </el-space>
                    </div>
                </el-card>
                <el-card style="margin-top: 20px;">
                    <template #header><el-text>历史记录</el-text></template>
                    <el-space wrap>
                        <el-image :src="src" fit="cover"
                            style="width: 225px; height: 200px;  border: 1px solid #DCDFE6;" :preview-src-list="hList"
                            lazy show-progress v-for="(src) in htList">
                            <template #toolbar="{ actions, prev, next, reset, activeIndex }">
                                <el-icon @click="prev">
                                    <Back />
                                </el-icon>
                                <el-icon @click="next">
                                    <Right />
                                </el-icon>

                                <el-icon @click="actions('zoomOut')">
                                    <ZoomOut />
                                </el-icon>
                                <el-icon @click="actions('zoomIn', { enableTransition: false, zoomRate: 2 })">
                                    <ZoomIn />
                                </el-icon>


                                <el-icon @click="reset">
                                    <Refresh />
                                </el-icon>

                                <el-icon @click="downloadImage(srcList[activeIndex])">
                                    <Download />
                                </el-icon>
                            </template>
                        </el-image>
                    </el-space>
                </el-card>
            </el-col>
        </el-row>
    </div>
</template>
<script lang="ts" setup>
import { computed, onBeforeUnmount, onMounted, ref } from 'vue';
import emitter from '@/utils/eventBus';
import { useRoute } from 'vue-router';
import useAppStore from '@/stores/useAppStore';
import { getHistory, getTask } from '@/api/task';
import { download, getFilePath, thumbPath } from '@/utils';
import useUserStore from '@/stores/useUserStore';

const flow = ref({
    id: '',
    name: '产品抠图',
    description: '适用于产品抠图、人物抠图、动物抠图'
})
const route = useRoute()
const appStore = useAppStore()
const userStore = useUserStore()
const srcList = ref([])
const hList = ref([])
const htList = ref([])
const thumbList = ref([])
const task = ref({
    state: 0,
    id: null,
    fileJson: '',
    keyword: '',
    negtive: ''
})
const state = computed(() => {
    if (!!task.value.id) {
        if (task.value.state == -1) {
            return 'error'
        } else if (task.value.state == 1) {
            return 'done'
        } else {
            return 'doing'
        }
    } else {
        return 'none'
    }

})

const baseURL = import.meta.env.VITE_APP_API

const loadTask = () => {
    srcList.value = []
    thumbList.value = []

    getTask(task.value.id).then(res => {
        task.value = res.data
        try {

            const json = JSON.parse(task.value['fileJson'])
            if (json.length > 0) {
                for (let i = 0; i < json.length; i++) {
                    let file = json[i]
                    var filePath = file['FilePath']
                    srcList.value.push(getFilePath(filePath))
                    let thumb = thumbPath(filePath)
                    thumbList.value.push(getFilePath(thumb))
                }
            }
        } catch (err) {
            console.log(err)
        }
    })
}

const loadHistory = () => {
    hList.value = []
    htList.value = []
    getHistory(flow.value.id).then(res => {
        if (res.data && res.data.length > 0) {
            for (let i = 0; i < res.data.length; i++) {
                let item = res.data[i]
                if (item.state != 1) {
                    return
                }
                try {
                    var files = JSON.parse(item['fileJson'])
                    files.forEach(file => {
                        var filePath = file['FilePath']
                        hList.value.push(getFilePath(filePath))
                        let thumb = thumbPath(filePath)
                        htList.value.push(getFilePath(thumb))
                    })
                } catch {
                    console.log('json格式不正确')
                }

            }
        }
    })
}

onMounted(() => {
    //监听任务提交
    emitter.on('saveTask', (e) => {
        console.log('提交任务', e)
        if (e) {
            task.value = e as any

            //加载历史记录
            loadHistory()
        }
    })

    //监听任务完成
    emitter.on('updateTask', (e) => {
        console.log('监听signalR消息', e)
        loadTask()

        //加载历史记录
        loadHistory()
    })


    var path = route.path
    for (let i = 0; i < appStore.flowList.length; i++) {
        let _temp = appStore.flowList[i]
        if (path == _temp.route) {
            flow.value = _temp
            break
        }
    }

    //加载点数
    userStore.updateCost()
})

onBeforeUnmount(() => {
    //移除监听器
    emitter.off('saveTask')
    emitter.off('udpateTask')
})


const downloadImage = (url) => {
    download(url)
}
</script>
<style lang="less" scoped>
.form-warpper {
    max-width: 1200px;
    min-width: 980px;
    margin: 0 auto;
    margin-top: 20px;
    margin-bottom: 20px;
}

.img-warrper {
    width: 225px;
    height: 300px;
    border: 1px solid #DCDFE6;

    img {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }
}
</style>