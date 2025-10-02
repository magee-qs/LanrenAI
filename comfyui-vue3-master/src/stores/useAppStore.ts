
import { getFlowList } from "@/api/flow";
import { getFilePath } from "@/utils";
import { defineStore } from "pinia";

const useAppStore = defineStore('__app__', {
    state: () => ({
        id: '',
        flowList: []
    }),
    actions: {
        //加载数据
        loadFlow() {
            return getFlowList('').then(res => {
                this.flowList = res.data

                this.flowList.forEach(item => {
                    item.img = getFilePath('/images/' + item.img)
                })
                //加载路由
                return this.flowList
            })
        }
    }
})

export default useAppStore