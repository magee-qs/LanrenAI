const validate = {
	isHttp(url) {
		return url.indexOf('http://') !== -1 || url.indexOf('https://') !== -1
	},
	isExternal(path) {
		return /^(https?:|mailto:|tel:)/.test(path)
	},
	isURL(url) {
		const reg =
			/^(https?|ftp):\/\/([a-zA-Z0-9.-]+(:[a-zA-Z0-9.&%$-]+)*@)*((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])){3}|([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(:[0-9]+)*(\/($|[a-zA-Z0-9.,?'\\+&%$#=~_-]+))*$/
		return reg.test(url)
	},
	isEmail(url) {
		const reg =
			/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
		return reg.test(url)
	},
	isPhoneNumber(phone) {
		return /^1(3[0-9]|4[01456879]|5[0-35-9]|6[2567]|7[0-8]|8[0-9]|9[0-35-9])\d{8}$/.test(phone);
	},
	isEmpty(obj) {
		if (obj == null || obj == undefined || obj == '') {
			return true
		}
		return false
	},
	isObject(obj) {
		if (obj == null || obj == undefined) {
			return false
		}
		return typeof(obj) == 'object'
	},
	isString(str) {
		if (typeof str === 'string' || str instanceof String) {
			return true
		}
		return false
	},
	isArray(arr) {
		if (typeof Array.isArray === 'undefined') {
			return Object.prototype.toString.call(arr) === '[object Array]'
		}
		return Array.isArray(arr)
	}
}

export default validate