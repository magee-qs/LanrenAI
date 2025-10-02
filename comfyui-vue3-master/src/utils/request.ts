import useUserStore from "@/stores/useUserStore";
import axios, { AxiosError } from "axios";
import validate from "./validate";
import { toQueryString } from ".";
import message from "./message";
import { saveAs } from 'file-saver';


axios.defaults.headers['Content-Type'] = 'application/json;charset=utf-8'

const env = import.meta.env

//请求限制
const limitSize = env.VITE_REQUEST_MAXSIZE
const tokenExpireMessage = 'token已失效,请重新登录'


const baseAPI = env.VITE_APP_API
const baseServer = env.VITE_APP_SERVER


const websocketUrl = env.VITE_APP_WEBSOCKET

const service = axios.create({
    baseURL: baseAPI,
    timeout: 10000 * 100 * 100
})

service.interceptors.request.use(config => {
    const userStore = useUserStore()
    config.headers['X-Access-Token'] = userStore.token
    config.headers['X-WebId'] = userStore.webId
    // 客户端类型
    config.headers['X-ClientType'] = 'pc'

    const method = config.method

    if (method == 'get') {
        const queryString = toQueryString(config.params)
        config.url += '?_t=' + new Date().getTime() + '&' + queryString
        config.url = config.url?.slice(0, -1)
    }
    if (method == 'post') {
        const req = {
            url: config.url,
            data: validate.isObject(config.data) ? JSON.stringify(config.data) : config.data,
            time: new Date().getTime()
        }
        const size = Object.keys(JSON.stringify(req)).length
        if (size > limitSize) {
            console.warn(`[${config.url}]: ` + '请求数据超出限制')
            return Promise.reject(new Error('请求数据超出限制'))
        }
    }


    return config
})


service.interceptors.response.use(response => {
    //console.log(response)
    //非正常
    if (200 != response.status) {
        return response
    }
    // 二进制数据则直接返回
    if (response.request.responseType == 'blob' || response.request.responseType === 'arraybuffer') {
        return response
    }
    // ajax请求
    if (response.data.code != null && response.data.success != null) {
        const code = response.data.code
        let msg = response.data.message || '错误'
        if (msg.length > 100) {
            msg = msg.substring(0, 100) + '...'
        }

        switch (code) {
            case 200:
                if (response.data.success) {
                    return response.data
                } else {
                    message.error(msg)
                    return Promise.reject(response.data)
                }
            case 401:
                //未登录 
                message.confirm('登录状态已过期，请重新登录').then(() => {
                    location.href = '/login'
                }).catch(() => {
                    //无操作
                    return Promise.reject('会话已过期，请重新登录')
                })
                break
            case 403:
                message.error(msg)
                return Promise.reject(response.data)
            case 404:
                message.error('访问资源不存在')
                return Promise.reject(response.data)
            case 500:
                console.log(response.data)
                message.error(msg)
                return Promise.reject(response.data)
            default:
                console.log('其他错误')
                console.log(response.data)
                message.error(msg)
                return Promise.reject(response.data)
        }
    }
    //其他请求直接返回数据
    return response.data

}, err => {
    console.log(err)
    if (err.response) {
        switch (err.response.status) {
            case 404:
                message.error('访问资源不存在')
                break
            case 400:
                message.error('请求参数错误或格式无效')
                break
            case 401:
                message.error('未授权')
                break
            case 403:
                message.error('无权限')
                break
            case 405:
                message.error('请求方法不被允许')
                break
            case 408:
                message.error('请求超时')
                break
            case 500:
                message.error('服务器错误，请联系管理员')
                break
            case 502:
                message.error('服务暂不可用，请稍后重试')
                break
            case 503:
                message.error('服务正在维护，请稍后再试')
                break
            case 504:
                message.error('服务器响应超时')
                break
        }
    } else {

        if (err.request) {
            // 请求已发送但无响应（如网络问题）
            message.error("网络异常，请重试");
        } else {
            // 其他错误（如代码逻辑问题）
            message.error("请求发送失败：" + err.message);
        }
    }

    return Promise.reject(err)
})


/* 文件下载 */
export function download(url: string, params?: any, filename?: string, config = {}) {
    message.load('下载中')
    return service.post(url, params, {
        transformRequest: [(params) => { return toQueryString(params) }],
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        responseType: 'blob',
        ...config
    }).then(async response => {
        if (response.data.type == 'application/json') {
            message.close()

            const resText = await response.data.text();
            const rspObj = JSON.parse(resText);
            const errmsg = rspObj.message || '下载文件出错'

            message.error(errmsg)
        } else {
            const blob = new Blob([response.data])

            if (!filename) {
                // 从Content-Disposition中解析文件名
                if (response.headers['Content-Disposition']) {
                    const contentDisposition = response.headers['Content-Disposition'];
                    const match = contentDisposition.match(/filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/i);
                    if (match && match[1]) {
                        // 去除可能存在的引号
                        filename = match[1].replace(/['"]/g, '');
                    }
                }
            }

            saveAs(blob, filename)

        }
    }).finally(() => {
        message.close()
    })
}


export { service as request, baseAPI, websocketUrl as WebSocketURL }







