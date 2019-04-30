using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Tubumu.Modules.Admin.Application.Services;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.SignalR.Hubs
{
    /// <summary>
    /// ApiResult: Notification
    /// </summary>
    /// <remarks>错误码：200 连接通知成功 201 新消息(可带url参数) 202 清除新消息标记 400 连接通知失败等错误</remarks>
    public class ApiResultNotification : ApiResult
    {
        [JsonProperty(PropertyName = "title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }

    public interface INotificationClient
    {
        Task ReceiveMessage(ApiResultNotification message);
    }

    [Authorize]
    public partial class NotificationHub : Hub<INotificationClient>
    {
        private readonly INotificationService _notificationService;

        public NotificationHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public override async Task OnConnectedAsync()
        {
            //await SendMessageToCaller(new ApiResultNotification { Code = 200, Message = "连接通知成功" });
            var userId = int.Parse(Context.User.Identity.Name);
            await SendNewNotificationAsync(userId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

    }

    public partial class NotificationHub
    {
        public async Task SendMessageByUserIdAsync(int userId, ApiResultNotification message)
        {
            var client = Clients.User(userId.ToString());
            await client.ReceiveMessage(message);
        }

        public async Task SendMessageAsync(string connectionId, ApiResultNotification message)
        {
            var client = Clients.Client(connectionId);
            await client.ReceiveMessage(message);
        }

        public Task SendMessageToCaller(ApiResultNotification message)
        {
            return Clients.Caller.ReceiveMessage(message);
        }

        public async Task BroadcastMessageAsync(ApiResultNotification message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }

    public partial class NotificationHub
    {
        private async Task SendNewNotificationAsync(int userId)
        {
            var newest = await _notificationService.GetNewestAsync(userId);
            if (newest != null)
            {
                await SendMessageByUserIdAsync(userId, new ApiResultNotification
                {
                    Code = 201,
                    Title = newest.Title,
                    Message = newest.Message,
                });
            }
        }

    }
}
