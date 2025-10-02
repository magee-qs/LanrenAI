<template>
    <div class="list">

        <el-space wrap>
            <template v-for="(item, index) in list">
                <el-image :src="src" fit="cover" style="width: 225px; height: 300px; border: 1px solid #DCDFE6;"
                    :preview-src-list="item.fileList" lazy show-progress v-for="(src) in item.thumbList">
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
                        <el-icon @click="downloadImage(item.fileList[activeIndex])">
                            <Download />
                        </el-icon>
                    </template>
                </el-image>
            </template>

        </el-space>


        <div ref="loader" class="loader" v-if="!isLastPage">
            <el-text>数据加载中...</el-text>
        </div>
        <div v-if="isLastPage" style="text-align: center; margin-top: 10px; ">
            <el-text>没有数据了</el-text>
        </div>
    </div>
</template>
<script lang="ts" setup>
import { getTaskList } from '@/api/task';
import { thumbPath, download } from '@/utils';
import { onMounted, ref, onBeforeUnmount } from 'vue';


const list = ref(<any>[])

const page = ref({
    current: 1,
    rows: 30,
    sidx: 'createTime',
    sord: 'desc'
})

// const srcList = [
//     'https://fuss10.elemecdn.com/a/3f/3302e58f9a181d2509f3dc0fa68b0jpeg.jpeg',
//     'https://fuss10.elemecdn.com/1/34/19aa98b1fcb2781c4fba33d850549jpeg.jpeg',
//     'https://fuss10.elemecdn.com/0/6f/e35ff375812e6b0020b6b4e8f9583jpeg.jpeg',
//     'https://fuss10.elemecdn.com/9/bb/e27858e973f5d7d3904835f46abbdjpeg.jpeg',
//     'https://fuss10.elemecdn.com/d/e6/c4d93a3805b3ce3f323f7974e6f78jpeg.jpeg',
//     'https://fuss10.elemecdn.com/3/28/bbf893f792f03a54408b3b7a7ebf0jpeg.jpeg',
//     'https://fuss10.elemecdn.com/2/11/6535bcfb26e4c79b48ddde44f4b6fjpeg.jpeg',
// ]

const baseURL = import.meta.env.VITE_APP_API


const isLoading = ref(false)
const isLastPage = ref(false)
const loader = ref(null)
let observer: any = null

const loadData = () => {
    //加载中或到最后一页
    if (isLoading.value || isLastPage.value) {
        return
    }
    isLoading.value = true
    getTaskList(page.value).then(res => {
        const rows = res.data.rows
        if (rows == null || rows.length == 0) {
            isLastPage.value = true
        }
        if (rows.length > 0) {
            //遍历rows读取fileJson 
            for (let i = 0; i < rows.length; i++) {
                let row = rows[i]
                try {
                    let json = JSON.parse(row['fileJson'])
                    const fileList = <any>[]
                    const thumbList = <any>[]
                    if (json && json.length > 0) {
                        json.forEach(item => {
                            fileList.push(baseURL + '/resources' + item['FilePath'])
                            let thumb = thumbPath(item['FilePath'])
                            thumbList.push(baseURL + '/resources' + thumb)
                        });
                    }

                    row['fileList'] = fileList
                    row['thumbList'] = thumbList
                } catch (ex) {
                    console.log('filejson error', row, ex)
                }
            }
        }

        list.value = [...list.value, ...rows]
        page.value.current++
    }).finally(() => {
        isLoading.value = false
    })
}

const initObserver = () => {
    observer = new IntersectionObserver(
        (entries) => {
            if (entries[0].isIntersecting) {
                loadData()
            }
        },
        { threshold: 0.4 }
    )

    if (loader.value) {
        observer.observe(loader.value)
    }
}

onMounted(() => {
    //初始化数据
    loadData()
    //初始化observer
    initObserver()
})

onBeforeUnmount(() => {
    if (observer) {
        observer.disconnect()
    }

})


const downloadImage = (url) => {
    download(url)
}
</script>
<style lang="less" scoped>
.img-item {
    width: 225px;
    height: 300px;
    border: 1px solid #DCDFE6;
    border-radius: 3px;
}
</style>
