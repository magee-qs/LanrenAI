import { request } from "@/utils/request";
import type { Page } from "./type";

export function getCardList() {
    return request({
        url: '/card/list',
        method: 'post'
    })
}

export function saveOrder(cardId: String, payType: String) {
    return request({
        url: '/card/order',
        params: {
            cardId: cardId,
            payType: payType
        },
        method: 'post'
    })
}

export function orderList(page: Page) {
    return request('/card/orderList', {
        data: page,
        method: 'post'
    })
}

export function getCardConfig() {
    return request('/card/getCardConfig', {
        method: 'post'
    })
}

export function getCost() {
    return request('/cost/getCost', {
        method: 'post'
    })
}