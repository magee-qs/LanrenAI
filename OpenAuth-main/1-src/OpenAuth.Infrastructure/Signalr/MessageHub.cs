using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Infrastructure.Signalr
{
    /// <summary>
    /// 消息处理
    /// </summary>
    public class MessageHub : Hub
    {
        public static readonly Dictionary<string, string> users = new Dictionary<string, string>();

        protected IAuthService authService;
        public MessageHub(IAuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// 客户端连接
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            // 可以从Context中获取用户标识，这里假设通过查询字符串传递 
            var token = Context.GetHttpContext().Request.Query["token"].FirstOrDefault();

            if (token != null )
            {
                users[token] = Context.ConnectionId;
            }
            
            return base.OnConnectedAsync();
        }
        /// <summary>
        /// 连接终止时调用。
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var user =  users.FirstOrDefault(t => t.Value == Context.ConnectionId);
            if (user.Key != null)
            {
                users.Remove(user.Key);
            }
            return base.OnDisconnectedAsync(exception);
        }


        public static async Task SendTo(IHubContext<MessageHub> hubCotnext, string token, string method, string message)
        {
            if (users.ContainsKey(token))
            {
                users.TryGetValue(token, out var connectionId);
                await hubCotnext.Clients.Client(connectionId)
                    .SendAsync(method, connectionId, message);
            }
        }
    }
}
