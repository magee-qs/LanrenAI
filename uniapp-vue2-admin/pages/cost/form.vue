<template>
	<view style="padding-top: 20px; padding-bottom: 20px; padding-left: 10px; padding-right: 10px;">
		<view class="t1">自动发放奖励</view>
		<uni-forms ref="form1" :modelValue="form1" :rules="rules1">
			<uni-forms-item label="月份" name="month">
				<uni-datetime-picker v-model="form1.month" type="date"></uni-datetime-picker>
			</uni-forms-item>
			<button type="primary" @click="handlerAutoExecute()">提交</button>
		</uni-forms>

		<view class="t1" style="margin-top: 60px ;">手动发放奖励</view>
		<uni-forms ref="form2" :modelValue="form2" :rules="rules2">
			<uni-forms-item label="账号" name="telephone">
				<uni-easyinput type="number" v-model="form2.telephone"></uni-easyinput>
			</uni-forms-item>
			<uni-forms-item label="月份" name="month">
				<uni-datetime-picker v-model="form2.month" type="date"></uni-datetime-picker>
			</uni-forms-item>
			<uni-forms-item label="点数" name="cost">
				<uni-easyinput type="number" v-model="form2.cost"></uni-easyinput>
			</uni-forms-item>
			<uni-forms-item label="备注" name="content">
				<uni-easyinput type="text" v-model="form2.content"></uni-easyinput>
			</uni-forms-item>
			<button type="primary" @click="handlerExecute()">提交</button>
		</uni-forms>

	</view>
</template>

<script>
	import {
		autoExecuteCost,
		execute,
		queryUserByTelephone
	} from '../../api/user'
	import message from '../../utils/message'
	export default {
		data() {
			return {
				form1: {
					month: ''
				},
				rules1: {
					month: {
						rules: [{
							required: true,
							errorMessage: '月份不能为空'
						}]
					}
				},
				form2: {
					userId: '',
					telephone: '',
					month: '',
					content: '发放奖励',
					cost: 100
				},
				rules2: {
					telephone: {
						rules: [{
							required: true,
							errorMessage: '账号不能为空'
						}]
					},
					month: {
						rules: [{
							required: true,
							errorMessage: '月份不能为空'
						}]
					},
					cost: {
						rules: [{
							required: true,
							errorMessage: '点数不能为空'
						}]
					},
				}
			}
		},
		methods: {
			handlerAutoExecute() {
				this.$refs.form1.validate().then(() => {
					message.showLoading('提交中')
					autoExecuteCost(this.form1.month).then((res => {
						if (res.success == true) {
							message.success('任务已提交')
						}
					})).catch().finally(() => {
						message.hideLoading()
					})
				})
			},
			handlerExecute() {
				this.$refs.form2.validate().then(() => {
					queryUserByTelephone(this.form2.telephone).then(res => {
						if (!!res.data) {
							this.form2.userId = res.data.id
							//提交订单
							execute(this.form2).then(res => {
								if (res.success) {
									message.success('操作成功')
									nav.to('/pages/cost/list')
								}
							})
						} else {
							message.error('用户不存在')
						}
					})
				})
			}
		}
	}
</script>

<style scoped lang="less">
	.t1 {
		font-size: 14px;
		font-weight: bold;

		border-bottom: 1px solid #ccc;
		padding-bottom: 4px;
		margin-bottom: 24px;
	}
</style>