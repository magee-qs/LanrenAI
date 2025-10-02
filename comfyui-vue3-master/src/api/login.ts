import { request } from "@/utils/request"

// 用户登录
export function login(formData: any) {
    return request({
        url: '/login',
        method: 'post',
        data: formData
    })
}

export function getCaptcha() {
    return request({
        url: '/getCaptcha',
        method: 'get'
    })
}

export function getSMSCode(phoneNumber: string) {
    return request({
        url: '/getSmsCode',
        method: 'post',
        data: phoneNumber
    })
}


export function verifyCheckCode(checkCode: string) {
    return request({
        url: '/verifyCheckCode',
        method: 'post',
        data: checkCode
    })
}