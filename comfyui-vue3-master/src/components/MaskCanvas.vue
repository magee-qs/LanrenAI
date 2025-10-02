<template>
    <el-dialog v-model="showForm" draggable>
        <div class="mask-editor">
            <div class="editor-container" ref="editorContainer">
                <div class="image-container" ref="imageContainer" :style="containerStyle">
                    <img ref="image" :src="imageSrc" @load="onImageLoad" v-show="imageLoaded" />
                    <canvas ref="canvas" v-show="imageLoaded" @mousedown="startDrawing" @mousemove="draw"
                        @mouseup="stopDrawing"></canvas>
                </div>
            </div>
            <div class="tool-group">
                <el-row>
                    <el-col :span="12">
                        <el-button-group>
                            <el-button :type="brushMode == 'draw' ? 'primary' : 'default'"
                                @click="setBrushMode('draw')">画笔</el-button>
                            <el-button :type="brushMode == 'erase' ? 'primary' : 'default'"
                                @click="setBrushMode('erase')">橡皮擦</el-button>
                            <el-button @click="clearCanvas">清除遮罩</el-button>
                        </el-button-group>
                    </el-col>
                    <el-col :span="4">
                        <el-text>笔刷大小:{{ brushSize }}</el-text>
                    </el-col>
                    <el-col :span="8"><el-slider v-model="brushSize" :min="0" :max="300" /></el-col>
                </el-row>

                <el-row style="margin-top: 8px;">
                    <el-col :span="12">
                        <el-button-group>
                            <el-button @click="zoomOut">-</el-button>
                            <el-button>{{ Math.round(scale * 100) }}%</el-button>
                            <el-button @click="zoomIn">+</el-button>
                            <el-button @click="resetZoom">重置</el-button>
                        </el-button-group>
                    </el-col>
                    <el-col :span="4">
                        <el-text>透明度:{{ brushOpacity }}</el-text>
                    </el-col>
                    <el-col :span="8"><el-slider v-model="brushOpacity" :min="0" :max="100" /> </el-col>
                </el-row>
            </div>


        </div>
        <template #footer>
            <div>
                <el-button @click="submitDialog" type="primary">保存</el-button>
                <el-button @click="downloadMask" type="primary">下载遮罩</el-button>
            </div>
        </template>
    </el-dialog>

</template>

<script lang="ts" setup>
import { saveMask } from '@/api/task'
import emitter from '@/utils/eventBus'
import { ref, computed, onMounted, nextTick, watchEffect, watch } from 'vue'


type BrushMode = 'draw' | 'erase'
// DOM 引用
const editorContainer = ref<HTMLElement | null>(null)
const imageContainer = ref<HTMLElement | null>(null)
const image = ref<HTMLImageElement | null>(null)
const canvas = ref<HTMLCanvasElement | null>(null)

const props = defineProps({
    imageSrc: {
        default: ''
    }
})

const showForm = ref(false)

// 响应式状态 
const imageLoaded = ref(false)
const isDrawing = ref(false)
const brushMode = ref<BrushMode>('draw')
const brushSize = ref(50)
const brushOpacity = ref(100)
const lastX = ref(0)
const lastY = ref(0)
const scale = ref(1)
const originalWidth = ref(0)
const originalHeight = ref(0)
const containerWidth = ref(0)
const containerHeight = ref(0)

// Canvas 上下文
const ctx = ref<CanvasRenderingContext2D | null>(null)

// 计算属性
const containerStyle = computed(() => ({
    transform: `scale(${scale.value})`,
    transformOrigin: '0 0',
    width: `${originalWidth.value}px`,
    height: `${originalHeight.value}px`,
}))



const onImageLoad = () => {
    if (!image.value) return

    image.value.crossOrigin = "Anonymous";
    originalWidth.value = image.value.naturalWidth
    originalHeight.value = image.value.naturalHeight
    imageLoaded.value = true

    nextTick(() => {
        initCanvas()
    })
}

const fitToContainer = () => {
    if (!editorContainer.value) return

    containerWidth.value = editorContainer.value.clientWidth
    containerHeight.value = editorContainer.value.clientHeight

    const scaleX = containerWidth.value / originalWidth.value
    const scaleY = containerHeight.value / originalHeight.value
    scale.value = Math.min(scaleX, scaleY, 1)

}

const initCanvas = () => {
    if (!canvas.value) return

    ctx.value = canvas.value.getContext('2d')
    if (!ctx.value) return

    canvas.value.width = originalWidth.value
    canvas.value.height = originalHeight.value

    ctx.value.fillStyle = 'rgba(0, 0, 0, 0)'
    ctx.value.fillRect(0, 0, canvas.value.width, canvas.value.height)


    fitToContainer()
}



const startDrawing = (e: MouseEvent) => {
    if (!canvas.value) return

    isDrawing.value = true
    const rect = canvas.value.getBoundingClientRect()
    lastX.value = (e.clientX - rect.left) / scale.value
    lastY.value = (e.clientY - rect.top) / scale.value
    drawDot(lastX.value, lastY.value)
}

const draw = (e: MouseEvent) => {
    if (!isDrawing.value || !canvas.value || !ctx.value) return

    const rect = canvas.value.getBoundingClientRect()
    const x = (e.clientX - rect.left) / scale.value
    const y = (e.clientY - rect.top) / scale.value

    setDrawingStyle()

    ctx.value.beginPath()
    ctx.value.moveTo(lastX.value, lastY.value)
    ctx.value.lineTo(x, y)
    ctx.value.stroke()

    lastX.value = x
    lastY.value = y

}

const drawDot = (x: number, y: number) => {
    if (!ctx.value) return

    setDrawingStyle()
    ctx.value.beginPath()
    ctx.value.arc(x, y, brushSize.value / 2, 0, Math.PI * 2)
    ctx.value.fill()
}

const setDrawingStyle = () => {
    if (!ctx.value) return

    ctx.value.globalCompositeOperation = brushMode.value === 'draw' ? 'source-over' : 'destination-out'
    ctx.value.lineJoin = 'round'
    ctx.value.lineCap = 'round'
    ctx.value.lineWidth = brushSize.value
    ctx.value.globalAlpha = brushMode.value === 'draw' ? brushOpacity.value / 100 : 1
    ctx.value.strokeStyle = 'rgba(0, 0, 0, 1)'
    ctx.value.fillStyle = 'rgba(0, 0, 0, 1)'
}

const stopDrawing = () => {
    isDrawing.value = false
}

const setBrushMode = (mode: BrushMode) => {
    brushMode.value = mode
}

const clearCanvas = () => {
    if (!ctx.value || !canvas.value) return
    ctx.value.clearRect(0, 0, canvas.value.width, canvas.value.height)
}

const zoomIn = () => {
    scale.value = Math.min(scale.value + 0.1, 3)
}

const zoomOut = () => {
    scale.value = Math.max(scale.value - 0.1, 0.1)
}

const resetZoom = () => {
    fitToContainer()
}



const uploadMask = async () => {
    if (!canvas.value || !image.value || !ctx.value) return



    try {
        // 创建合并后的canvas
        const mergedCanvas = document.createElement('canvas')

        // 初始化遮罩画布为全黑 

        mergedCanvas.width = canvas.value.width
        mergedCanvas.height = canvas.value.height
        const mergedCtx = mergedCanvas.getContext('2d')
        if (!mergedCtx) throw new Error('无法创建画布上下文')

        mergedCtx.fillStyle = 'black'
        mergedCtx.fillRect(0, 0, mergedCanvas.width, mergedCanvas.height)
        // 合并图像和蒙版
        //mergedCtx.drawImage(image.value, 0, 0)
        //设置合成模式 'source-in' - 只保留蒙版与原图重叠的部分 
        //mergedCtx.globalCompositeOperation = 'destination-in'
        // 先反相蒙版 
        mergedCtx.filter = 'invert(1)';
        mergedCtx.drawImage(canvas.value, 0, 0)
        mergedCtx.filter = 'none';
        // 恢复默认合成模式
        //mergedCtx.globalCompositeOperation = 'source-over';



        // 转换为Blob
        const blob = await new Promise<Blob | null>((resolve) => {
            mergedCanvas.toBlob((blob) => resolve(blob), 'image/png')
        })

        if (!blob) throw new Error('无法生成图像')

        // 创建FormData并上传
        const formData = new FormData()
        formData.append('file', blob, 'masked-image.png')

        saveMask(formData).then(res => {
            console.log('save mask event', res.data)
            emitter.emit('saveMask', res.data)
        })
    } catch (error) {
        console.error('上传失败:', error)
    }
}


const downloadMask = async () => {
    if (!canvas.value || !image.value || !ctx.value) return


    try {
        // 创建合并后的canvas
        const mergedCanvas = document.createElement('canvas')

        // 初始化遮罩画布为全黑 

        mergedCanvas.width = canvas.value.width
        mergedCanvas.height = canvas.value.height
        const mergedCtx = mergedCanvas.getContext('2d')
        if (!mergedCtx) throw new Error('无法创建画布上下文')

        mergedCtx.fillStyle = 'black'
        mergedCtx.fillRect(0, 0, mergedCanvas.width, mergedCanvas.height)
        // 合并图像和蒙版
        //mergedCtx.drawImage(image.value, 0, 0)
        //设置合成模式 'source-in' - 只保留蒙版与原图重叠的部分 
        //mergedCtx.globalCompositeOperation = 'destination-in'
        // 先反相蒙版 
        mergedCtx.filter = 'invert(1)';
        mergedCtx.drawImage(canvas.value, 0, 0)
        mergedCtx.filter = 'none';
        // 恢复默认合成模式
        //mergedCtx.globalCompositeOperation = 'source-over';


        const link = document.createElement('a')
        link.download = `mask-${new Date().toISOString().slice(0, 10)}.png`
        link.href = mergedCanvas.toDataURL('image/png')
        link.click()
    } catch (error) {
        console.error('上传失败:', error)
    }
}

const submitDialog = () => {
    uploadMask()

    //上传蒙版
    showForm.value = false
}



const show = () => {
    //上传蒙版
    showForm.value = true
    initCanvas()
}

defineExpose({ show }) 
</script>

<style scoped>
.editor-container {
    position: relative;
    margin-bottom: 20px;
    height: 500px;
    border: 1px solid #ddd;
    overflow: auto;
    background-color: #f0f0f0;
    background-image:
        linear-gradient(45deg, #ccc 25%, transparent 25%),
        linear-gradient(-45deg, #ccc 25%, transparent 25%),
        linear-gradient(45deg, transparent 75%, #ccc 75%),
        linear-gradient(-45deg, transparent 75%, #ccc 75%);
    background-size: 20px 20px;
    background-position: 0 0, 0 10px, 10px -10px, -10px 0px;
}

.image-container {
    position: relative;
    display: inline-block;
    margin: 0 auto;
}

canvas {
    position: absolute;
    top: 0;
    left: 0;
    cursor: crosshair;
}

.tool-group {
    line-height: 32px;
}
</style>