import session from "./session.js"
const queryKey = 'sys_query'

const nav = {
	to(url, type, param) {
		let query = Object.assign({}, param)
		session.save(queryKey, query)
		if (type == 'tab') {
			uni.switchTab({
				url: url
			})
		} else {
			uni.navigateTo({
				url: url
			})
		}
	},
	getQuery() {
		return session.get(queryKey)
	},
	toHome() {
		uni.navigateTo({
			url: '/pages/index/index'
		})
	},
	back(param) {
		let query = Object.assign({}, param)
		session.save(queryKey, query)
		uni.navigateBack()
	}
}

export default nav