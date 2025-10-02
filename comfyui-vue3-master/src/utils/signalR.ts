
import { WebSocketURL } from "./request";
import * as signalR from '@microsoft/signalr'
import message from "./message";
import emitter from '@/utils/eventBus';

const service = {
    connection: null,
    failNum: 3,
    init(token: string) {

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(WebSocketURL + '?token=' + encodeURIComponent(token))
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Warning)
            .build()

        this.connection = connection
        // 断线重连
        connection.onclose(async () => {
            console.log('断开连接了')
            console.assert(connection.state === signalR.HubConnectionState.Disconnected)
            // 建议用户重新刷新浏览器
            await this.start()
        })
        connection.onreconnected(() => {
            console.log('断线重新连接成功')
        })

        this.receiveMsg(connection)
    },
    async start() {
        var that = this
        try {
            await that.connection.start()
            console.log('signalR 连接成功了', that.connection.connectionId)
            return true
        } catch (error) {
            that.failNum--
            console.log(`失败重试剩余次数${that.failNum}`, error)
            if (that.failNum > 0) {
                setTimeout(async () => {
                    await that.connection.start()
                }, 5000)
            }
        }
    },
    receiveMsg(connection) {

        // 接收系统消息
        connection.on('receiveNotice', (title, content) => {
            console.log('receiveNotice', title, content)
            message.info(content)
        })

        // 接收任务完成
        connection.on('updateTask', () => {
            emitter.emit('updateTask')
        })
    }
}

export { service as signalR }
