<template>
	<view>
		<uni-nav-bar dark leftText="返回首页" rightText="新增订单" @clickRight="handleAdd"
			@clickLeft="handleHome"></uni-nav-bar>
		<!-- 基于 uni-list 的页面布局 -->
		<uni-list>
			<!-- to 属性携带参数跳转详情页面，当前只为参考 -->
			<uni-list-item direction="column" v-for="item in formData.data" :key="item.id">
				<!-- 通过header插槽定义列表的标题 -->
				<template v-slot:header>
					<view class="uni-title">订单编号: {{item.orderId}}</view>
				</template>
				<!-- 通过body插槽定义列表内容显示 -->
				<template v-slot:body>
					<uni-row>
						<uni-col :span="12" class="item-text">用户: {{item.userName}}</uni-col>
						<uni-col :span="12" class="item-text">手机号: {{item.telephone}}</uni-col>
					</uni-row>
					<uni-row>
						<uni-col :span="12" class="item-text">金额: {{item.cost }}</uni-col>
						<uni-col :span="12" class="item-text">有效期: {{item.month}}个月</uni-col>
					</uni-row>
					<uni-row>
						<uni-col :span="12" class="item-text">状态: {{getOrderState(item.state)}}</uni-col>
						<uni-col :span="12" class="item-text">日期: {{item.createTime}}</uni-col>
					</uni-row>

					<uni-row v-if="item.state == 0">
						<uni-col :span="12"><button type="primary" @click="handlePay(item)"
								size="mini">订单支付</button></uni-col>
						<uni-col :span="12"><button type="warn" @click="handleAbandon(item)"
								size="mini">订单作废</button></uni-col>
					</uni-row>
				</template>
			</uni-list-item>
		</uni-list>
		<view class="uni-footer"
			style="padding-left: 20px; padding-right: 20px; line-height: 30px; padding-bottom: 10px;">
			<text class="uni-footer-text">总记录:{{page.total}}</text>
			<button v-if="formData.status == 'more'" @click="loadNext()" type="primary" size="mini">下一页</button>
			<text class="uni-footer-text">已经到底了</text>
			<text class="uni-footer-text">当前页: {{page.current}}</text>
		</view>

	</view>
</template>

<script>
	import {
		getOrderList,
		payOrder
	} from '../../api/user';
	import message from '../../utils/message';
	import nav from '../../utils/nav';
	export default {
		data() {
			return {
				formData: {
					status: 'more', // 加载状态
					data: []
				},
				page: {
					current: 1,
					total: 0,
					rows: 10,
					sidx: 'createTime',
					sord: 'desc'
				}
			};
		},

		onLoad() {
			this.loadData()
		},
		onPullDownRefresh() {
			this.page = {
				current: 1,
				total: 0,
				rows: 10,
				sidx: 'createTime',
				sord: 'desc'
			}

			this.formData.data = []
			this.loadData().finally(() => {
				uni.stopPullDownRefresh()
			})
		},
		methods: {
			loadData() {

				let that = this
				return getOrderList(that.page).then(res => {
					if (res.data.rows == null || res.data.rows.length == 0) {
						that.formData.status = 'noMore'
						that.page.current--
					} else {
						that.formData.status = 'more'
						that.formData.data = [...that.formData.data, ...res.data.rows]
					}
					that.page.total = res.data.total

				}).catch(() => {
					that.formData.status = 'more'
				})
			},
			getOrderState(state) {
				if (state == 1) {
					return '已支付'
				} else if (state == 0) {
					return '待支付'
				} else if (state = -1) {
					return '已作废'
				}
			},
			handlePay(item) {
				payOrder(item.orderId).then(res => {
					if (res.success == true) {
						message.success('操作成功')
						item.state = 1
					} else {
						message.error('操作失败')
					}
				})
			},
			handleAdd() {
				nav.to('/pages/order/form')
			},
			handleAbandon(item) {
				message.confirm('确定作废订单?').then(() => {

				})
			},
			handleHome() {
				nav.toHome()
			},
			refresh() {
				uni.navigateTo({
					url: '/pages/order/list'
				})
			},
			loadNext() {
				if (this.formData.status == 'more') {
					this.page.current++
					this.loadData()
				} else {
					message.info('已经到最后一页')
				}
			}
		}
	}
</script>


<style lang="scss">
	@import '@/common/uni-ui.scss';

	page {
		display: flex;
		flex-direction: column;
		box-sizing: border-box;
		background-color: #efeff4;
		min-height: 100%;
		height: auto;
	}

	.tips {
		color: #67c23a;
		font-size: 14px;
		line-height: 40px;
		text-align: center;
		background-color: #f0f9eb;
		height: 0;
		opacity: 0;
		transform: translateY(-100%);
		transition: all 0.3s;
	}

	.tips-ani {
		transform: translateY(0);
		height: 40px;
		opacity: 1;
	}

	.content {
		width: 100%;
		display: flex;
	}

	.list-picture {
		width: 100%;
		height: 145px;
	}

	.thumb-image {
		width: 100%;
		height: 100%;
	}

	.ellipsis {
		display: flex;
		overflow: hidden;
	}

	.uni-ellipsis-1 {
		overflow: hidden;
		white-space: nowrap;
		text-overflow: ellipsis;
	}

	.uni-ellipsis-2 {
		overflow: hidden;
		text-overflow: ellipsis;
		display: -webkit-box;
		-webkit-line-clamp: 2;
		-webkit-box-orient: vertical;
	}

	.item-text {
		width: 50%;
		font-size: 14px;
		color: #999;
		line-height: 28px;
	}



	.item-wrapper {
		display: flex;
		flex-direction: row;
	}

	.t1 {
		width: 100%;
	}

	.t2 {
		width: 50%;
	}

	.t3 {
		width: 33%;
	}

	.t4 {
		width: 25%;
	}
</style>