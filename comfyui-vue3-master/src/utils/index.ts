import dayjs from "dayjs"
import validate from "./validate"
import axios from "axios"
import { saveAs } from "file-saver"
import message from "./message"

const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
const charactersL = 'abcdefghijklmnopqrstuvwxyz0123456789'

/*  生成随机数 */
export function random(length: number): string {
    let str = ''
    for (let i = 0; i < length; i++) {
        const index = Math.floor(Math.random() * characters.length)
        str += characters.charAt(index)
    }
    return str
}

export function randomL(length: number): string {
    let str = ''
    for (let i = 0; i < length; i++) {
        const index = Math.floor(Math.random() * charactersL.length)
        str += charactersL.charAt(index)
    }
    return str
}

/* 日期格式化 */
export function formatDate(date: Date | null, formatter: string = 'YYYY-MM-DD HH:mm:ss') {
    return dayjs(date).format(formatter)
}

/* 打开窗口 */
export function openWindow(url: string, opt?: { target?: string; noopener?: boolean; noreferrer?: boolean }) {
    const { target = '__blank', noopener = true, noreferrer = true } = opt || {};
    const feature: string[] = [];

    noopener && feature.push('noopener=yes');
    noreferrer && feature.push('noreferrer=yes');

    window.open(url, target, feature.join(','));
}

/* 参数处理 
    {id: 111, name: ''} => id=111&name=aaa
*/
export function toQueryString(params: any) {
    let result = ''
    if (!!params) {
        for (const prop of Object.keys(params)) {
            const value = params[prop]
            var part = encodeURIComponent(prop) + '='
            if (validate.isEmpty(value)) {
                if (validate.isObject(value)) {
                    for (const key of Object.keys(value)) {
                        if (!validate.isEmpty(value[key])) {
                            const subPart = encodeURIComponent(key) + '=' + encodeURIComponent(value[key])
                            result += subPart + '&'
                        }
                    }
                } else {
                    result += part + encodeURIComponent(value) + '&'
                }
            }
        }
    }

    return result
}

/* 对象空判断 */
export function isEmpty(value: any) {
    if (value == null || value == "" || value == undefined || value == "undefined") {
        return true
    }
    return false
}


export function isUrl(url: String) {
    if (url == null || url == undefined || url == '') {
        return false
    }
    var res = url.match(/(http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/);
    return res !== null;
}
//获取缩微图路径
export function thumbPath(filePath: any) {
    var path = filePath as string
    var index = path.lastIndexOf('/')

    if (index == -1) {
        index = path.lastIndexOf('\\')
    }
    var src1 = path.substring(0, index + 1)
    var src2 = path.substring(index + 1)
    return src1 + 'thumb-' + src2
}


export function userLevel(level: any) {
    if ('vip' == level) {
        return 'VIP用户'
    } else if ('svip' == level) {
        return 'SVIP用户'
    } else {
        return '普通用户'
    }
}

//指定url下载文件
export function download(url, filename?) {
    message.load()
    axios.get(url, {
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        responseType: 'blob'
    }).then(async response => {
        const blob = new Blob([response.data])
        if (!filename) {
            var index = url.lastIndexOf('/')
            filename = url.substring(index + 1)
        }
        saveAs(blob, filename)
    }).finally(() => {
        message.close()
    })
}


export function emptyUserInfo() {
    return {
        userName: '',
        userLevel: 'normal',
        fee: {
            cost: 0,
            store: 0,
            multiple: 0,
            expire: new Date('2025-01-01')
        }
    }
}

const baseAPI = import.meta.env.VITE_APP_API
const fileAPI = import.meta.env.VITE_FILE_SERVER
export function getFilePath(filePath) {
    filePath = filePath.split('\\').join('/')
    if (filePath[0] == '/' || filePath[0] == '\\') {
        return fileAPI + filePath
    }
    return fileAPI + '/' + filePath
}


export { baseAPI, fileAPI }






