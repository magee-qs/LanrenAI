import auth from './auth.js'
import message from './message.js'
import config from './config.js'

const http = function(url, method, data, timeout) {

	const apiURL = config.apiURL
	if (!timeout) {
		timeout = 10 * 1000
	}
	return new Promise((resolve, reject) => {
		let options = {
			url: apiURL + url,
			data: data,
			header: {
				'X-Access-Token': auth.getToken(),
				'X-WebId': auth.getWebId()
			},
			method: method,
			dataType: 'json',
			sslVerify: false,
			timeout: timeout,
			success: (res) => {
				success(res).then(data => {
					resolve(data)
				})
			},
			fail: (res) => {
				fail(res)
				reject(data)
			},
			complete: () => {
				//message.hideLoading()  
			}
		}
		uni.request(options)
	})
}

const success = (response) => {
	console.log(response)

	// ajax请求
	let code = response.statusCode


	switch (code) {
		case 200:
			let data = response.data
			if (data.success) {
				return Promise.resolve(data)
			} else {
				if (data.code == 401) {
					//未登录
					message.confirm('登录状态已过期，请重新登录').then(() => {
						uni.navigateTo({
							url: '/pages/login/login'
						})
					}).catch(() => {
						//无操作
						return Promise.reject('会话已过期，请重新登录')
					})
				} else if (data.code == 403) {
					message.error('无权限')
				} else {
					message.error(data.message || '出错了')
				}
				return Promise.reject(data)
			}
		case 401:
			//未登录 
			message.confirm('登录状态已过期，请重新登录').then(() => {
				uni.navigateTo({
					url: '/pages/login/login'
				})
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
	//其他请求直接返回数据
	return response.data
}

const fail = (err) => {
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
			message.error("请求发送失败：" + err.errMsg);
		}
	}

	return Promise.reject(err)
}

export function post(url, data, timeout) {
	return http(url, 'post', data, timeout)
}

export function get(url, data, timeout) {
	return http(url, 'get', data, timeout)
}