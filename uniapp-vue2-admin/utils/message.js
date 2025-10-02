const message = {
	info(content) {
		let option = {
			title: content || '提示内容',
			mask: true
		}
		uni.showToast(option)
	},
	success(content) {
		let option = {
			title: content || '操作成功',
			mask: true
		}
		uni.showToast(option)
	},
	error(content) {
		let option = {
			icon: 'none',
			//image:'/static/img/error.png',
			title: content || '操作失败',
			mask: true
		}
		uni.showToast(option)
	},
	warn(content) {
		let option = {
			icon: 'none',
			title: content || '操作警告',
			mask: true
		}
		uni.showToast(option)
	},
	confirm(content) {
		return new Promise(function(resolve, reject) {
			let option = Object.assign({
				title: '确认',
				content: content || '确认内容',
				showCancel: true,
				success: function(res) {
					if (res.confirm) {
						resolve(res)
					} else {
						reject(res)
					}
				},
				fail: function(res) {
					reject(res)
				}
			})

			uni.showModal(option)
		})
	},
	alert(content) {
		return new Promise(function(resolve, reject) {
			let option = Object.assign({
				title: '确认',
				content: content || '确认内容',
				showCancel: false,
				success: function(res) {
					if (res.confirm) {
						resolve(res)
					} else {
						reject(res)
					}
				},
				fail: function(res) {
					reject(res)
				}
			})

			uni.showModal(option)
		})
	},
	showLoading(content) {

		uni.showLoading({
			mask: true,
			title: content || '加载中'
		})

	},
	hideLoading() {
		uni.hideLoading()
	},
	showActionSheet(data, success, err) {
		uni.showActionSheet({
			itemList: data,
			success: success,
			fail: err
		})
	}
}


export default message