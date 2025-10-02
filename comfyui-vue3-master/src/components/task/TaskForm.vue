<template>

    <div class="form-body">
        <el-form :model="param" label-position="top">
            <slot name="prefix"></slot>
            <KeywordSelect :form="form" v-if="showKeywords"></KeywordSelect>
            <!-- 基础组件 -->
            <slot></slot>
            <!-- 其他组件 -->
            <slot name="suffix"></slot>

            <el-form-item>
                <el-button type="primary" @click="submit" style="margin-top: 5px;">立即生成</el-button>
            </el-form-item>
        </el-form>
    </div>

</template>
<script lang="ts" setup>
import { saveTask } from '@/api/task';
import useAppStore from '@/stores/useAppStore';
import message from '@/utils/message';
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import KeywordSelect from './KeywordSelect.vue';
import emitter from '@/utils/eventBus';


const form = ref({
    keyword: '',
    negtive: '',
    unit: 1,
    scale: 3,
    width: 768,
    height: 1024,
    size: 1,
    noise: 10,
    step: 20,
    cfg: 7,
    seed: 0,
    seedType: 'rand',
    translacte: true
})

const props = defineProps({
    param: {
        type: Object,
    },
    showKeywords: {
        type: Boolean,
        default: true
    },
    valid: {
        type: Function,
        default: () => { return true }
    }
})

const flow: any = ref(null)

onMounted(() => {
    var path = route.path
    for (let i = 0; i < appStore.flowList.length; i++) {
        let _temp = appStore.flowList[i]
        if (path == _temp.route) {
            flow.value = _temp
            break
        }
    }
})



const appStore = useAppStore()
const route = useRoute()

//生成图片
const submit = () => {
    if (flow.value == null) {
        message.error('没有找到工作流')
        return
    }

    if (props.valid()) {
        debugger
        let data = Object.assign({ flowId: flow.value.id, cost: flow.value.cost, service: flow.value.service }, form.value, props.param)
        saveTask(data).then(res => {
            console.log('提交数据', res)
            //发送提交任务消息
            emitter.emit('saveTask', res.data)
        })
    }
} 
</script>