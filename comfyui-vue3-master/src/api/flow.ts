import { request } from "@/utils/request";

export function getFlowList(code: any) {
    return request({
        url: '/flow/list',
        method: 'post'
    })
}

export function getFlowById(id: any) {
    return request({
        url: '/flow/getFlow',
        method: 'post',
        params: {
            id: id
        }
    })
}