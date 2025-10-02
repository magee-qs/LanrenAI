import { request } from "@/utils/request";

export function getTaskList(query: any) {
    return request({
        url: '/task/list',
        method: 'post',
        data: query
    })
}

export function getTaskFileList(query: any) {
    return request({
        url: '/task/GetFileList',
        method: 'post',
        data: query
    })
}

export function getTask(taskId: any) {
    return request({
        url: '/Task/GetTask',
        method: 'post',
        data: taskId
    })
}

export function getHistory(flowId: any) {
    return request({
        url: '/Task/getHistory',
        method: 'post',
        data: flowId
    })
}

export function saveTask(data: any) {
    return request({
        url: '/Task/SaveTask',
        method: 'post',
        data: data
    })
}

export function saveMask(formData: FormData) {
    return request.post('/Task/UploadImage', formData,
        {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
}

export function saveImage(formData: FormData) {
    return request.post('/Task/UploadImage', formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    })
}