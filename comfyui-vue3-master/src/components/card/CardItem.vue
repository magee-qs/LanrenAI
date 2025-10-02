<template>
    <div class="item">
        <div id="header">
            <template v-if="!!card && card.icon">
                <img :src="card.icon" width="20" height="20" style="margin-right: 8px;vertical-align: middle;">
            </template>
            <template v-else>
                <img src="/static/diamond.png" width="20" height="20"
                    style="margin-right: 8px; vertical-align: middle;">
            </template>
            <span>{{ card.name }}</span>
        </div>

        <div id="fee">
            <span class="t1">¥</span>
            <span class="t2">{{ card.price }}</span>
            <span class="t3">¥{{ card.unitPrice }}</span>
        </div>
        <div class="row t4" style="line-height:32px;  ">
            到期自动续费,可以随时取消
        </div>
        <div class="row" style="padding-top: 10px; padding-bottom: 10px; ">
            <el-button type="primary" @click="submit()">立即开通</el-button>
        </div>
        <div>
            <PayForm ref="orderForm" :card="card" :order="order"></PayForm>
        </div>
    </div>
</template>
<script lang="ts" setup>
import { ref } from 'vue';
import PayForm from '@/components/PayForm.vue';
import { saveOrder } from '@/api/order';


// 定义参数
const props = defineProps({
    card: {
        type: Object,
        default: {
            id: '',
            icon: '/static/diamond.png',
            name: '普通会员包月',
            unitPrice: 50,
            discount: 80,
            price: 40
        }
    }
})

const orderForm = ref(null)
const order = ref({
    orderId: '',
    name: '',
    unitPrice: 0,
    discount: 0,
    price: 0,
    content: ''
})


const submit = function () {
    //保存订单
    saveOrder(props.card.id, 'alipay').then(res => {
        order.value = res.data
        //弹窗显示订单信息和二维码
        orderForm.value.show()
    })
}
</script>
<style lang="less" scoped>
.item {
    border-radius: 10px;
    border: 1px solid #d2d2d2;
    padding: 20px;
    width: 185px;
    text-align: center;
}

#header {
    font-size: 14px;
    line-height: 40px;
    font-weight: bold;
}

.t1 {
    font-size: 24px;
}

.t2 {
    font-size: 32px;
}

.t3 {
    font-size: 16px;
    color: #c2c2c2;
    text-decoration: line-through;
}

.t4 {
    font-size: 12px;
}

#fee {
    span+span {
        margin-left: 6px;
    }

    font-weight: 500;
}
</style>