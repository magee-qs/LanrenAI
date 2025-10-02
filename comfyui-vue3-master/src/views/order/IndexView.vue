<template>
    <div class="wrapper">
        <el-table :data="dataSource" stripe border>
            <el-table-column prop="content" label="订单明细" show-overflow-tooltip min-width="160"></el-table-column>
            <el-table-column prop="unitPrice" label="原价" width="80" align="center"></el-table-column>
            <el-table-column prop="discount" label="折扣" width="80" align="center"></el-table-column>
            <el-table-column prop="price" label="折后价" width="80" align="center"></el-table-column>
            <el-table-column prop="state" label="订单状态" width="120" align="center">
                <template #default="scope">
                    {{ toState(scope.row.state) }}
                </template>
            </el-table-column>
            <el-table-column prop="payType" label="支付方式" width="100" align="center">
                <template #default="scope">
                    {{ toPay(scope.row.payType) }}
                </template>
            </el-table-column>
            <el-table-column prop="tradeId" label="支付号" show-overflow-tooltip></el-table-column>
            <el-table-column prop="createTime" label="下单时间" align="center" min-width="120"></el-table-column>
        </el-table>
    </div>
</template>
<script lang="ts" setup>
import { orderList } from '@/api/order';
import { computed, onMounted, ref } from 'vue';

const page = ref({ current: 1, limit: 10, total: 100 })
const sort = { sidx: 'createTime', sord: 'desc' }
const columns = ref([
    { title: '订单编号', key: 'orderId', width: "120" },
    { title: '订单明细', key: 'content', width: "120" },
    { title: '原价', key: 'unitPrice' },
    { title: '折扣', key: 'discount' },
    { title: '折后价', key: 'price' },
    { title: '订单状态', key: 'state', customeSlot: "status" },
    { title: '支付方式', key: 'payType' },
    { title: '支付号', key: 'tradeId' }
])

const dataSource = ref([])
const loadData = () => {
    orderList({
        page: page.value.current,
        rows: page.value.limit,
        sidx: sort.sidx,
        sord: sort.sord
    }).then(res => {
        page.value.total = res.data.total
        dataSource.value = res.data.rows
    })
}
onMounted(() => {
    loadData()
})

const toState = (state: any) => {
    if (state == 0) {
        return '待支付'
    } else if (state == 1) {
        return '已支付'
    } else {
        return '已关闭'
    }
}

const toPay = (payType: any) => {
    if (payType == 'alipay') {
        return '支付宝'
    } else if (payType = 'wxpay') {
        return '微信'
    } else if (payType == 'union') {
        return '银联'
    }
}


</script>
<style lang="less" scoped>
.wrapper {
    min-width: 900px;
    max-width: 1400px;
    padding: 20px;
    text-align: center;
    margin: 0 auto;
}
</style>