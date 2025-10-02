<template>
	<view class="form-content">
		<uni-forms :modelValue="form" :rules="rules" @handleSubmit="handleSubmit" ref="form">
			<uni-forms-item label="手机号" name="telephone">
				<uni-easyinput type="text" v-model="form.telephone" placeholder="请输入手机号"></uni-easyinput>
			</uni-forms-item>
			<uni-forms-item label="密码" name="password">
				<uni-easyinput type="password" v-model="form.password" placeholder="请输入密码"></uni-easyinput>
			</uni-forms-item>
			<button type="primary" @click="handleSubmit">提交</button>
		</uni-forms>
	</view>
</template>

<script>
	import {
		login
	} from '@/api/user.js'

	import auth from '@/utils/auth.js';
	import nav from '@/utils/nav.js'

	export default {
		data() {
			return {
				form: {
					telephone: '17343323979',
					password: '1733979'
				},
				rules: {
					telephone: {
						rules: [{
							required: true,
							errorMessage: '请输入手机号'
						}]
					},
					password: {
						rules: [{
							required: true,
							errorMessage: '请输入密码'
						}]
					}
				}
			};
		},
		methods: {
			handleSubmit() {
				this.$refs.form.validate().then(res => {
					login(this.form).then(res => {
						// token, userInfo
						// 登录成功
						console.log(res)
						auth.saveToken(res.data.token)
						auth.saveUserInfo(res.data.userInfo)
						//跳转到首页
						nav.toHome()

					})
				}).catch(err => {
					console.log('表单错误信息', err)
				})
			}
		}
	}
</script>

<style lang="less">

</style>