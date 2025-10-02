<template>
	<view>
		<uni-nav-bar dark leftText="返回首页" rightText="发放奖励" @clickRight="handleAdd"
			@clickLeft="handleHome"></uni-nav-bar>
		<!-- 基于 uni-list 的页面布局 -->
		<uni-list>

			<uni-list-item direction="column" v-for="item in formData.data" :key="item.id">

				<template v-slot:body>
					<uni-row>
						<uni-col :span="12" class="item-text">用户: {{item.userName}}</uni-col>
						<uni-col :span="12" class="item-text">手机号: {{item.telephone}}</uni-col>
					</uni-row>
					<uni-row>
						<uni-col :span="12" class="item-text">点数: {{item.total }}</uni-col>
						<uni-col :span="12" class="item-text">有效期: {{item.exipre}}</uni-col>
					</uni-row>
					<uni-row>
						<uni-col :span="12" class="item-text">已使用: {{item.cost }}</uni-col>
						<uni-col :span="12" class="item-text">剩余: {{item.leave}}</uni-col>
					</uni-row>
					<uni-row>
						<uni-col :span="12" class="item-text">创建人: {{item.createUser}}</uni-col>
						<uni-col :span="12" class="item-text">日期: {{item.createTime}}</uni-col>
					</uni-row>
				</template>
			</uni-list-item>
		</uni-list>
		<view class="uni-footer"
			style="padding-left: 20px; padding-right: 20px; line-height: 30px;padding-bottom: 10px;">
			<text class="uni-footer-text">总记录:{{page.total}}</text>
			<button v-if="formData.status == 'more'" @click="loadNext()" type="primary" size="mini">下一页</button>
			<text class="uni-footer-text" v-if="formData.status=='noMore'">已经到底了</text>
			<text class="uni-footer-text">当前页: {{page.current}}</text>
		</view>

	</view>
</template>

<script>
	import {
		getCostList,
		getOrderList,
		payOrder
	} from '../../api/user';
	import message from '../../utils/message';
	import nav from '../../utils/nav';
	export default {
		components: {},
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
				return getCostList(that.page).then(res => {
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


			handleAdd() {
				nav.to('/pages/cost/form')
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
	};
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