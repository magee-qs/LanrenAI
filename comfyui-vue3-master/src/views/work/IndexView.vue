<template>
    <div v-infinite-scroll="loadData" class="list">
        <el-space wrap>
            <template v-for="(item) in list">
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
        <div v-if="isLastPage" style="padding-top: 20px; padding-bottom: 20px; text-align: center;">
            <el-text>已经到底部啦!</el-text>
        </div>
        <el-backtop :right="100" :bottom="100">

        </el-backtop>
    </div>


</template>
<script lang="ts" setup>
import { getTaskList } from '@/api/task';
import { download, getFilePath, thumbPath } from '@/utils';
import { onMounted, ref } from 'vue';

const list = ref(<any>[])

const page = ref({
    current: 1,
    rows: 30,
    sidx: 'createTime',
    sord: 'desc'
})
const baseURL = import.meta.env.VITE_APP_API
const isLastPage = ref(false)
const loadData = () => {
    getTaskList(page.value).then(res => {
        const rows = res.data.rows
        if (rows == null || rows.length == 0) {
            isLastPage.value = true
            return
        }
        //遍历rows读取fileJson 
        for (let i = 0; i < rows.length; i++) {
            let row = rows[i]
            try {
                let json = JSON.parse(row['fileJson'])
                const fileList = <any>[]
                const thumbList = <any>[]
                if (json && json.length > 0) {
                    json.forEach(item => {
                        //平姐地址
                        let path = getFilePath(item['FilePath'])
                        fileList.push(path)
                        let thumb = thumbPath(item['FilePath'])
                        let tPath = getFilePath(thumb)
                        thumbList.push(tPath)
                    });
                }

                row['fileList'] = fileList
                row['thumbList'] = thumbList
            } catch (ex) {
                console.log('filejson error', row, ex)
            }
        }

        list.value = [...list.value, ...rows]
        page.value.current++
    })
}

onMounted(() => {
    // loadData()
})

const downloadImage = (url) => {
    download(url)
}
</script>
<style lang="less">
.list {
    width: 1170px;
    margin: 0 auto;
    padding: 20px;
}

.img-item {
    width: 225px;
    height: 300px;
    border: 1px solid #DCDFE6;
    border-radius: 3px;
}
</style>