<template>
	<view class="content">
		<view class="wrapper" style="margin-top: 15px;">
			<view class="item" @click="handleRefresh">
				<uni-icons type="smallcircle-filled" size="30" :color="state == 'ok'?'green': 'red'"></uni-icons>
				<text class=" text">服务状态: {{state == 'ok'? '运行中': '异常'}}</text>
			</view>
		</view>
		<view class="split" style="margin-top: 10px;">
			<text>日常应用</text>
		</view>
		<view class="wrapper">
			<view class="item" v-for="(item,index) in baseList" :key="index" @click="selectedMenu(index)">
				<image :src="item.image" class="image" />
				<text class="text">{{ item.text }}</text>
			</view>
		</view>
	</view>
</template>

<script>
	import {
		getServerState
	} from '../../api/user'
	export default {
		data() {
			return {
				title: 'Hello',
				baseList: [{
					image: '/static/menu/zb.png',
					url: '/pages/order/list',
					text: '订单管理'
				}, {
					image: '/static/menu/cw.png',
					url: '/pages/cost/list',
					text: '奖励发放'
				}],
				state: 'ok'
			}
		},
		onLoad() {
			this.handleRefresh()
		},
		methods: {
			handleTo() {
				uni.navigateTo({
					url: '/pages/login/login'
				})
			},
			selectedMenu(index) {
				let menu = this.baseList[index]
				uni.navigateTo({
					url: menu.url
				})
			},
			//刷新状态
			handleRefresh() {
				getServerState().then((res) => {
					if (res.data == 'ok') {
						this.state == 'ok'
					} else {
						this.state = 'fail'
					}
				})
			}
		}
	}
</script>

<style lang="less" scoped>
	.content {}

	.split {
		background-color: #F5F5F5;
		line-height: 48px;
		padding-left: 15px;
		font-size: 14px;

		margin-bottom: 15px;
	}

	.wrapper {
		display: flex;
		flex-direction: row;
		flex-wrap: wrap;
		gap: 20px 15px;

		.item {
			display: flex;
			flex-direction: column;
			align-content: center;
			justify-content: center;
			font-size: 16rpx;
			flex: 0 0 calc(25% - 11.25px);
			box-sizing: border-box;

			image {
				width: 48px;
				height: 48px;
				//border-radius: 24px;
				margin: auto;
			}

			.text {
				margin-top: 8px;
				text-align: center;
			}
		}
	}
</style>