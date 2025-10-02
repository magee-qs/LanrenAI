<template>
	<view style="padding-top: 20px; padding-bottom: 20px; padding-left: 10px; padding-right: 10px;">
		<uni-forms ref="form" :modelValue="form" :rules="rules">
			<uni-forms-item label="手机号" name="telephone">
				<uni-easyinput v-model="form.telephone" type="number"></uni-easyinput>
			</uni-forms-item>
			<uni-forms-item label="会员类型" name="feeId">
				<fee-level-select v-model="form.feeId"></fee-level-select>
			</uni-forms-item>
			<uni-forms-item label="金额" name="cost">
				<uni-easyinput v-model="form.cost" type="number" :clearable="false"></uni-easyinput>
			</uni-forms-item>
			<uni-forms-item label="月份" name="month">
				<uni-easyinput v-model="form.month" type="number" :clearable="false"></uni-easyinput>
			</uni-forms-item>
			<button type="primary" @click="handleSubmit">提交</button>
		</uni-forms>
	</view>
</template>

<script>
	import {
		queryUserByTelephone,
		saveOrder
	} from '../../api/user'
	import FeeLevelSelect from '../../components/FeeLevelSelect.vue'
	import message from '../../utils/message'
	import nav from '../../utils/nav'
	export default {
		components: {
			FeeLevelSelect
		},
		data() {
			return {
				form: {
					telephone: '',
					cost: 99,
					feeId: '',
					month: 12,
					userId: ''
				},
				rules: {
					feeId: {
						rules: [{
							required: true,
							errorMessage: '请选择会员类型'
						}]
					},
					telephone: {
						rules: [{
							required: true,
							errorMessage: '用户电话不能为空'
						}]
					}
				}
			}
		},
		methods: {
			handleSubmit() {
				this.$refs.form.validate().then(() => {
					queryUserByTelephone(this.form.telephone).then(res => {
						if (!!res.data) {
							this.form.userId = res.data.id
							//提交订单
							saveOrder(this.form).then(res => {
								message.success('订单提交成功')
								//保存成功
								nav.to('/pages/order/list')
							})
						} else {
							message.error('用户不存在')
							return
						}
					})
				})
			}
		}
	}
</script>

<style>

</style>