<template>
    <div>
        <el-dialog v-model="visible" shade :width="width" title="会员充值" destroy-on-close :append-to-body="true">
            <div class="form">
                <el-descriptions title="订单信息" :column="2">
                    <el-descriptions-item label="订单编号:" :span="2">
                        {{ order.orderId }}
                    </el-descriptions-item>

                    <el-descriptions-item label="充值类型:">
                        {{ card.name }}
                    </el-descriptions-item>
                    <el-descriptions-item label="订单金额:">
                        {{ order.price }}元
                    </el-descriptions-item>
                    <el-descriptions-item label="备注信息:" :span="2">
                        {{ order.content }}
                    </el-descriptions-item>


                    <el-descriptions-item label="支付方式:" :span="2" v-if="orderState == 0">
                        <el-space>
                            <el-button :type="payType == 'alipay' ? 'primary' : 'default'" @click="toAlipay()">
                                支付宝
                            </el-button>
                            <el-button :type="payType == 'wxpay' ? 'primary' : 'default'" @click="toWxpay()">
                                微信
                            </el-button>
                        </el-space>
                    </el-descriptions-item>

                </el-descriptions>
            </div>
            <el-result describe="订单支付完成" v-if="orderState == 1" title="支付成功"></el-result>


            <el-result status="failure" describe="订单支付超时" title="支付失败" v-if="orderState == -1"></el-result>
            <div style="min-height: 210px;">
                <MyCode :text="qrCode" v-if="orderState == 0">
                    {{ payType == 'alipay' ? '支付宝扫码' : '微信扫码支付' }}
                </MyCode>
            </div>
        </el-dialog>
    </div>

</template>
<script lang="ts" setup>
import { alipay, pollPayState } from '@/api/pay';
import { onUnmounted, ref } from 'vue';
import MyCode from './MyCode.vue';
const visible = ref(false)

const props = defineProps({
    width: {
        type: String,
        default: '650px'
    },
    height: {
        type: String,
        default: '550px'
    },
    card: {
        type: Object,
        default: {
            id: '',
            icon: '/static/diamond.png',
            name: '普通会员包月'
        }
    },
    order: {
        type: Object,
        default: {
            orderId: '',
            content: '',
            unitPrice: 0,
            price: 0,
            discount: 100,
            payType: 'alipay',
            state: 0,
            tradeId: ''
        }
    }
})


const show = () => {
    visible.value = true
}

defineExpose({ show })


const qrCode = ref('https://qr.alipay.com/bax03214nxrblvy0xvsh0007')
const payType = ref('alipay')
let pollFunc: Function = () => { }

const toAlipay = () => {
    payType.value = 'alipay'
    alipay(props.order.orderId).then(data => {
        qrCode.value = data
        //开启轮询
        //pollFunc = pollState()
    }).catch(() => {
        qrCode.value = ''
    })
}
const toWxpay = () => {
    payType.value = 'wxpay'
}

const orderState = ref(0)

const pollState = function () {
    const step = 12; // 步长 12
    const space = 5000; // 间隔5秒
    let current = 0;

    const poll = setInterval(async () => {
        current++;
        let result = await pollPayState(props.order.orderId)
        if (result == 1 || result == -1) {
            //订单响应
            orderState.value = result
            //清空
            clearInterval(poll)
        } else {
            if (current > step) {
                //超时处理
                clearInterval(poll)

                orderState.value = -1
            }
        }
    }, space)

    return () => {
        clearInterval(poll)
        console.log('停止轮询')
    }
}

onUnmounted(() => {
    if (pollFunc) {
        pollFunc()
    }
})
</script>
<style lang="less" scoped>
.form {
    width: 580px;
    overflow: hidden;
    margin: 0 auto;
    padding-left: 30px;
    padding-right: 30px;
    padding-top: 10px;
}
</style>