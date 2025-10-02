import dayjs from 'dayjs'
import config from './config'

const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
const charactersL = 'abcdefghijklmnopqrstuvwxyz0123456789'

/*  生成随机数 */
export function random(length) {
	let str = ''
	for (let i = 0; i < length; i++) {
		const index = Math.floor(Math.random() * characters.length)
		str += characters.charAt(index)
	}
	return str
}

export function randomL(length) {
	let str = ''
	for (let i = 0; i < length; i++) {
		const index = Math.floor(Math.random() * charactersL.length)
		str += charactersL.charAt(index)
	}
	return str
}

// 日期格式化  
export function formatDate(date, formatter = 'YYYY-MM-DD HH:mm:ss') {
	return dayjs(date).format(formatter)
}


//参数处理 {id: 111, name: ''} => id=111&name=aaa 
export function toQueryString(params) {
	var result = ''
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

//获取缩微图路径
export function thumbPath(filePath) {
	var path = filePath
	var index = path.lastIndexOf('/')

	if (index == -1) {
		index = path.lastIndexOf('\\')
	}
	var src1 = path.substring(0, index + 1)
	var src2 = path.substring(index + 1)
	return src1 + 'thumb-' + src2
}

export function getFilePath(filePath) {
	filePath = filePath.split('\\').join('/')
	let fileAPI = config.apiURL
	if (filePath[0] == '/' || filePath[0] == '\\') {
		return fileAPI + filePath
	}
	return fileAPI + '/' + filePath
}