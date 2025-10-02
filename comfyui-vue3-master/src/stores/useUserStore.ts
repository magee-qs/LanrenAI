import { getCost } from "@/api/order";
import { emptyUserInfo, isEmpty, randomL } from "@/utils";
import { defineStore } from "pinia";


const useUserStore = defineStore('__user__', {
    state: () => ({
        token: '',
        userInfo: emptyUserInfo(),
        webId: randomL(64),
        cost: 0
    }),
    actions: {
        logout() {
            this.token = ''
            this.userInfo = emptyUserInfo()
            this.cost = 0
        },
        login(data: any) {
            this.token = data.token
            this.userInfo = data.userInfo
        },
        getWebId() {
            if (isEmpty(this.webId)) {
                this.webId = randomL(64)
            }
            return this.webId
        },
        updateCost() {
            getCost().then(data => {
                this.cost = data.data
            })
        }
    },
    persist: true
})

export default useUserStore