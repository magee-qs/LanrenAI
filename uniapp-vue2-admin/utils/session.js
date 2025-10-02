const session = {
	save(key, data) {
		try {
			uni.setStorageSync(key, data);
		} catch (e) {
			message.error('保存缓存失败')
			console.log(e)
		}
	},
	get(key) {
		try {
			return uni.getStorageSync(key)
		} catch (e) {
			message.error('读取缓存失败')
			console.log(e)
		}
	},
	remove(key) {
		try {
			uni.removeStorageSync(key)
		} catch (e) {
			message.error('读取缓存失败')
			console.log(e)
		}
	}
}

export default session