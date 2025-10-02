<template>
    <div class="image-mask-container" style="width: 700px; height: 80vh;">
        <!-- 工具栏 -->
        <div class="toolbar">
            <div class="tool-group">
                <span class="tool-label">笔刷大小:</span>
                <el-slider v-model="brushSize" :min="1" :max="50" :step="1" show-input style="width: 150px" />
            </div>

            <div class="tool-group">
                <el-button-group>
                    <el-button @click="zoomOut" :icon="Minus" title="缩小" />
                    <el-button disabled>
                        {{ (scale * 100).toFixed(0) }}%
                    </el-button>
                    <el-button @click="zoomIn" :icon="Plus" title="放大" />
                    <el-button @click="resetZoom" title="重置缩放">↻</el-button>
                </el-button-group>
            </div>

            <div class="tool-group">
                <el-button @click="clearMask" type="danger" :icon="Delete">
                    清除蒙版
                </el-button>
                <el-button @click="saveMask" type="primary" :icon="Download">
                    保存蒙版
                </el-button>
            </div>
        </div>

        <!-- 画布容器 -->
        <div class="canvas-wrapper" ref="container">
            <canvas ref="canvas" @mousedown="startDrawing" @mousemove="draw" @mouseup="stopDrawing"
                @mouseleave="stopDrawing"></canvas>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'
import { Minus, Plus, Delete, Download } from '@element-plus/icons-vue'

const props = defineProps({
    imageUrl: {
        type: String,
        default: '/static/temp.png'
    }
})

// 元素引用
const canvas = ref(null)
const container = ref(null)

// 状态
const brushSize = ref(10)
const scale = ref(1)
const isDrawing = ref(false)
const lastPos = ref({ x: 0, y: 0 })

// Canvas上下文和图像
let ctx = null
let maskCtx = null
const maskCanvas = document.createElement('canvas')
let image = null
let originalWidth = 0
let originalHeight = 0
let displayWidth = 0
let displayHeight = 0
let offsetX = 0
let offsetY = 0

// 初始化画布
const initCanvas = () => {
    if (!container.value) return

    // 加载图片
    image = new Image()
    image.crossOrigin = 'anonymous'
    image.src = props.imageUrl

    image.onload = () => {
        originalWidth = image.naturalWidth
        originalHeight = image.naturalHeight

        // 计算适应容器的尺寸
        calculateDisplaySize()

        // 设置主画布
        setupMainCanvas()

        // 设置蒙版画布
        setupMaskCanvas()

        // 初始绘制
        redraw()
    }

    image.onerror = (e) => {
        console.error('图片加载失败:', e)
    }
}

// 计算显示尺寸
const calculateDisplaySize = () => {
    const containerWidth = container.value.clientWidth
    const containerHeight = container.value.clientHeight
    const ratio = originalWidth / originalHeight

    // 计算保持宽高比的尺寸
    if (originalWidth > containerWidth || originalHeight > containerHeight) {
        if (containerWidth / containerHeight > ratio) {
            displayHeight = containerHeight
            displayWidth = displayHeight * ratio
        } else {
            displayWidth = containerWidth
            displayHeight = displayWidth / ratio
        }
    } else {
        displayWidth = originalWidth
        displayHeight = originalHeight
    }

    // 计算居中偏移量
    offsetX = (containerWidth - displayWidth) / 2
    offsetY = (containerHeight - displayHeight) / 2
}

// 设置主画布
const setupMainCanvas = () => {
    canvas.value.width = container.value.clientWidth
    canvas.value.height = container.value.clientHeight
    ctx = canvas.value.getContext('2d')
}

// 设置蒙版画布
const setupMaskCanvas = () => {
    maskCanvas.width = originalWidth
    maskCanvas.height = originalHeight
    maskCtx = maskCanvas.getContext('2d')
    maskCtx.fillStyle = 'rgba(0, 0, 0, 0)'
    maskCtx.fillRect(0, 0, originalWidth, originalHeight)
}

// 重绘图像和蒙版
const redraw = () => {
    if (!ctx || !image) return

    // 清空画布
    ctx.clearRect(0, 0, canvas.value.width, canvas.value.height)

    // 绘制白色背景
    ctx.fillStyle = '#ffffff'
    ctx.fillRect(0, 0, canvas.value.width, canvas.value.height)

    // 绘制图像
    ctx.save()
    ctx.translate(offsetX, offsetY)
    ctx.scale(scale.value, scale.value)
    ctx.drawImage(image, 0, 0, displayWidth, displayHeight)
    ctx.restore()

    // 应用蒙版
    ctx.globalCompositeOperation = 'source-atop'
    ctx.drawImage(
        maskCanvas,
        0, 0, originalWidth, originalHeight,
        offsetX, offsetY, displayWidth, displayHeight
    )
    ctx.globalCompositeOperation = 'source-over'
}

// 开始绘制
const startDrawing = (e) => {
    if (e.button !== 0) return // 只响应左键

    const pos = getCanvasPosition(e)
    if (!pos) return

    isDrawing.value = true
    lastPos.value = pos

    // 绘制初始点
    drawPoint(pos.x, pos.y)
    redraw()
}

// 绘制点
const drawPoint = (x, y) => {
    maskCtx.beginPath()
    maskCtx.arc(x, y, brushSize.value / 2, 0, Math.PI * 2)
    maskCtx.fillStyle = 'rgba(0, 0, 255, 0.5)'
    maskCtx.fill()
}

// 绘制线
const drawLine = (x1, y1, x2, y2) => {
    maskCtx.beginPath()
    maskCtx.moveTo(x1, y1)
    maskCtx.lineTo(x2, y2)
    maskCtx.lineWidth = brushSize.value
    maskCtx.lineCap = 'round'
    maskCtx.strokeStyle = 'rgba(0, 0, 255, 0.5)'
    maskCtx.stroke()
}

// 绘制过程
const draw = (e) => {
    if (!isDrawing.value) return

    const pos = getCanvasPosition(e)
    if (!pos) return

    drawLine(lastPos.value.x, lastPos.value.y, pos.x, pos.y)
    lastPos.value = pos
    redraw()
}

// 停止绘制
const stopDrawing = () => {
    isDrawing.value = false
}

// 获取画布坐标（相对于原始图像）
const getCanvasPosition = (e) => {
    const rect = canvas.value.getBoundingClientRect()
    const x = (e.clientX - rect.left - offsetX) / scale.value
    const y = (e.clientY - rect.top - offsetY) / scale.value

    // 转换为原始图像坐标
    const imgX = (x / displayWidth) * originalWidth
    const imgY = (y / displayHeight) * originalHeight

    // 检查是否在图像范围内
    if (imgX < 0 || imgX > originalWidth || imgY < 0 || imgY > originalHeight) {
        return null
    }

    return { x: imgX, y: imgY }
}

// 缩放功能
const zoomIn = () => {
    scale.value = Math.min(scale.value * 1.2, 5) // 最大500%
    redraw()
}

const zoomOut = () => {
    scale.value = Math.max(scale.value / 1.2, 0.2) // 最小20%
    redraw()
}

const resetZoom = () => {
    scale.value = 1
    redraw()
}

// 清除蒙版
const clearMask = () => {
    maskCtx.clearRect(0, 0, originalWidth, originalHeight)
    maskCtx.fillStyle = 'rgba(0, 0, 0, 0)'
    maskCtx.fillRect(0, 0, originalWidth, originalHeight)
    redraw()
}

// 保存蒙版
const saveMask = () => {
    const link = document.createElement('a')
    link.download = `mask-${new Date().toISOString().slice(0, 10)}.png`
    link.href = maskCanvas.toDataURL('image/png')
    link.click()
}

// 监听窗口大小变化
const handleResize = () => {
    initCanvas()
}

onMounted(() => {
    initCanvas()
    window.addEventListener('resize', handleResize)
})

onUnmounted(() => {
    window.removeEventListener('resize', handleResize)
})
</script>

<style scoped>
.image-mask-container {
    display: flex;
    flex-direction: column;
    height: 100%;
    width: 100%;
    border: 1px solid #dcdfe6;
    border-radius: 4px;
    overflow: hidden;
}

.toolbar {
    display: flex;
    flex-wrap: wrap;
    gap: 16px;
    padding: 12px;
    background: #f5f7fa;
    border-bottom: 1px solid #dcdfe6;
}

.tool-group {
    display: flex;
    align-items: center;
    gap: 8px;
}

.tool-label {
    font-size: 14px;
    color: #606266;
    white-space: nowrap;
}

.canvas-wrapper {
    flex: 1;
    display: flex;
    justify-content: center;
    align-items: center;
    overflow: hidden;
    background-color: #f2f6fc;
}

canvas {
    background-color: white;
    cursor: crosshair;
    box-shadow: 0 0 8px rgba(0, 0, 0, 0.1);
}
</style>