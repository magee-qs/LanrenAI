import session from './session.js'
import {
	randomL
} from './index.js'

const tokenKey = 'sys_token'
const userInfoKey = 'sys_userInfo'
const loginKey = 'sys_login'
const webIdKey = 'sys_webId'

const auth = {
	saveToken(token) {
		session.save(tokenKey, token)
	},
	getToken() {
		return session.get(tokenKey)
	},
	saveUserInfo(userInfo) {
		session.save(userInfoKey, userInfo)
	},
	getUserInfo() {
		return session.get(userInfoKey)
	},
	login(token, userInfo) {
		this.saveToken(token)
		this.saveUserInfo(userInfo)
	},
	logout() {
		session.remove(tokenKey)
		session.remove(userInfoKey)
	},
	getWebId() {
		let id = session.get(webIdKey)
		if (!id) {
			id = randomL(64)
			session.save(webIdKey, id)
		}
		return id
	}
}

export default auth