import { ElLoading, ElMessage, ElMessageBox } from "element-plus"


let loadingInstance: any = null
const message = {
    load: (content = '加载中') => {
        loadingInstance = ElLoading.service({
            text: content
        })
    },
    close() {
        if (loadingInstance != null) {
            loadingInstance.close()
        }
    },
    error(msg = '操作失败') {
        ElMessage.error(msg)
    },
    success(msg = '操作成功') {
        ElMessage({
            message: msg,
            type: 'success'
        })
    },
    warn(msg = '提示') {
        ElMessage({
            message: msg,
            type: 'warning'
        })
    },
    info(msg = '提示') {
        ElMessage({
            message: msg,
            type: 'info'
        })
    },
    confirm(msg = '提示', title = '提示') {
        return ElMessageBox.confirm(msg, title, {
            confirmButtonText: '确认',
            cancelButtonText: '取消',
            type: 'info',
        })

    }
}

export default message