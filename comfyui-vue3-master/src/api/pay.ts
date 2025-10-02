import message from "@/utils/message";
import { request } from "@/utils/request";
// 阿里订单支付
export function alipay(orderId: String) {
    message.load()
    return request({
        url: '/alipay/order/',
        data: orderId,
        timeout: 30000,
        method: 'post'
    }).then(res => {
        let qrCode = res.data
        return qrCode;
    }).finally(() => {
        message.close()
    })
}
//轮询支付状态
export function pollPayState(orderId: string) {
    return request({
        url: '/card/isPayed',
        data: orderId,
        method: 'post'
    }).then(res => {
        return res.data
    })
}