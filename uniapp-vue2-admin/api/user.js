import {
	post
} from '../utils/request.js'


export function login(form) {
	return post('/admin/login', form)
}

export function getOrderList(param) {
	return post('/Fee/OrderList', param)
}

export function payOrder(orderId) {
	return post('/Fee/PayOrder', orderId)
}

export function saveOrder(form) {
	return post('/Fee/SaveOrder', form)
}

export function abandonOrder(orderId) {
	return post('/Fee/AbandonOrder', orderId)
}

export function getFeeLevels() {
	return post('/Fee/GetFeeLevels')
}

export function getCostList(data) {
	return post('/Cost/GetCostList', data)
}

export function queryUserByTelephone(telephone) {
	return post('/user/GetUserByTelephone', telephone)
}

export function autoExecuteCost(month) {
	return post('/Fee/AutoExecute', {
		month: month
	}, 3 * 60 * 1000)
}

export function execute(form) {
	return post('/Fee/Execute', form)
}



export function getServerState() {
	return post('/task/GetServerState')
}