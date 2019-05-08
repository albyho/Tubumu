using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Application.Services;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.SignalR.Hubs
{
    /// <summary>
    /// ApiResult: Notification (错误码：200 连接通知成功 201 新消息(可带url参数) 202 清除新消息标记 400 连接通知失败等错误)
    /// </summary>
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
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(INotificationService notificationService, ILogger<NotificationHub> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            // return SendMessageToCaller(new ApiResultNotification { Code = 200, Message = "连接通知成功" });
            var userId = int.Parse(Context.User.Identity.Name);
            return SendNewNotificationAsync(userId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }

    public partial class NotificationHub
    {
        public Task SendMessageByUserIdAsync(int userId, ApiResultNotification message)
        {
            var client = Clients.User(userId.ToString());
            return client.ReceiveMessage(message);
        }

        public Task SendMessageAsync(string connectionId, ApiResultNotification message)
        {
            var client = Clients.Client(connectionId);
            return client.ReceiveMessage(message);
        }

        public Task SendMessageToCaller(ApiResultNotification message)
        {
            return Clients.Caller.ReceiveMessage(message);
        }

        public Task BroadcastMessageAsync(ApiResultNotification message)
        {
            return Clients.All.ReceiveMessage(message);
        }
    }

    public partial class NotificationHub
    {
        private async Task SendNewNotificationAsync(int userId)
        {
            var newest = await _notificationService.GetNewestAsync(userId);
            if (newest != null)
            {
                SendMessageByUserIdAsync(userId, new ApiResultNotification
                {
                    Code = 201,
                    Title = newest.Title,
                    Message = newest.Message,
                }).ContinueWithOnFailedLog(_logger);
            }
        }
    }
}
