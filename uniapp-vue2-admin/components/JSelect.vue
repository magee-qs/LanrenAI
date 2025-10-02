<template>
	<view class="wrapper">
		<picker :mode="mode" :range-key="label" :disabled="disabled" :range="option" @change="selectedChange">
			<uni-easyinput v-model="selectedLabel"></uni-easyinput>
		</picker>
		<view class="icon-close" v-show="displayClear" @click="handleClear">
			<uni-icons type="close" size="18" class="icon-close-info"></uni-icons>
		</view>
	</view>
</template>

<script>
	export default {
		name: "JSelect",
		props: {
			option: {
				required: true
			},
			disabled: {
				default: false
			},
			value: {
				required: true
			},
			pid: {
				default: 'id'
			},
			label: {
				default: 'label'
			},
			showClear: {
				default: true
			}
		},
		data() {
			return {
				selectedKey: '',
				selectedRows: [],
				selectedLabel: '',
				mode: 'selector'
			}
		},
		computed: {
			displayClear() {
				if (this.disabled == true) {
					return false
				} else {
					return this.showClear == true ? true : false
				}
			},
		},
		watch: {
			value(newVal, oldVal) {
				let key = this.updateData(newVal)
				this.selectedKey = key
			},
			selectedKey(newVal, oldVal) {
				if (newVal != oldVal) {
					//更新下拉选框
					this.updateSelectedLabel(newVal)
				}
			}
		},
		methods: {
			handleClear() {
				this.selectedKey = ''
				this.selectedRows = []
				this.selectedLabel = ''
				this.$emit('input', '')
				this.$emit('selectedChange', '', [])
			},
			/* 获取值 */
			getItemId(item) {
				return item[this.pid]
			},
			getItemLabel(item) {
				return item[this.label]
			},
			/* 更新数据 */
			updateData(newVal) {
				if (newVal == undefined || newVal == null || newVal.length == 0) {
					return ''
				} else {
					return newVal
				}
			},
			/* 选中数据 */
			selectedChange(e) {

				let data = e.target.value
				let rows = []
				let option = this.option || []

				//单选  
				let item = option[data]
				let key = this.getItemId(item)

				rows.push(item)

				this.selectedRows = rows
				this.selectedKeys = key

				this.$emit('input', key)
				this.$emit('selectedChange', key, rows)
			},
			updateSelectedLabel(selectedKey) {
				let that = this
				let rows = []
				let option = this.option || []
				//单选
				let key = selectedKey || ''
				for (let i = 0; i < option.length; i++) {
					let item = option[i]
					let itemId = that.getItemId(item)
					if (itemId == key) {
						rows.push(item)
						break
					}
				}

				let arr = []
				rows.forEach(item => {
					let label = that.getItemLabel(item)
					arr.push(label)
				})
				this.selectedLabel = arr.toString()
			}
		}
	}
</script>

<style lang="less">
	.wrapper {
		.icon-close {
			position: absolute;
			right: 4px;
			top: 2px;

			z-index: 999;

			.icon-close-info {
				color: #c8c7cc !important;
			}
		}

	}
</style>