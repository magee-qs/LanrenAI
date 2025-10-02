<template>
	<JSelect :option="list" pid="id" label="title" v-model="selectedKey" @selectedChange="selectedChange"
		:showClear="false"></JSelect>
</template>

<script>
	import {
		getFeeLevels
	} from '../api/user';

	import JSelect from './JSelect.vue'
	export default {
		name: "FeeLevelSelect",
		components: {
			JSelect
		},
		props: {
			value: {
				required: true
			},
			disabled: {
				default: false
			}
		},
		data() {
			return {
				selectedKey: '',
				list: []
			};
		},
		watch: {
			value(val) {
				this.selectedKey = val
			}
		},
		created() {
			getFeeLevels().then(res => {
				this.list = res.data
			})
		},
		methods: {
			selectedChange(key, rows) {
				this.$emit('input', key)
				this.$emit('selectedChange', key, rows)
				this.$emit('update:value', key)
			}
		}
	}
</script>

<style>

</style>