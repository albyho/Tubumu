using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Domain.Services;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.SignalR.Hubs;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Application.Services
{
    public interface INotificationService
    {
        Task<Page<NotificationUser>> GetPageAsync(NotificationPageSearchCriteria criteria);

        Task<bool> SaveAsync(NotificationInput notificationInput, ModelStateDictionary modelState);

        Task<bool> RemoveAsync(int notificationId, ModelStateDictionary modelState);

        Task<bool> ReadAsync(int userId, int[] notificationIds, ModelStateDictionary modelState);

        Task<bool> DeleteAsync(int userId, int[] notificationIds, ModelStateDictionary modelState);

        Task<NotificationUser> GetNewestAsync(int userId, int? currentNotificationId = null);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationManager _notificationManager;
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

        public NotificationService(INotificationManager notificationManager, IHubContext<NotificationHub, INotificationClient> hubContext)
        {
            _notificationManager = notificationManager;
            _hubContext = hubContext;
        }

        public Task<Page<NotificationUser>> GetPageAsync(NotificationPageSearchCriteria criteria)
        {
            return _notificationManager.GetPageAsync(criteria);
        }

        public async Task<bool> SaveAsync(NotificationInput notificationInput, ModelStateDictionary modelState)
        {
            var result = await _notificationManager.SaveAsync(notificationInput, modelState);
            if (result && !notificationInput.NotificationId.HasValue)
            {
                var apiResultNotification = new ApiResultNotification
                {
                    Code = 201,
                    Title = notificationInput.Title,
                    Message = notificationInput.Message,
                };
                if (notificationInput.ToUserId.HasValue)
                {
                    var client = _hubContext.Clients.User(notificationInput.ToUserId.Value.ToString());
                    client.ReceiveMessage(apiResultNotification).NoWarning();
                }
                else
                {
                    _hubContext.Clients.All.ReceiveMessage(apiResultNotification).NoWarning();
                }
            }
            return result;
        }

        public Task<bool> RemoveAsync(int notificationId, ModelStateDictionary modelState)
        {
            return _notificationManager.RemoveAsync(notificationId, modelState);
        }

        public Task<bool> ReadAsync(int userId, int[] notificationIds, ModelStateDictionary modelState)
        {
            return _notificationManager.ReadAsync(userId, notificationIds, modelState);
        }

        public Task<bool> DeleteAsync(int userId, int[] notificationIds, ModelStateDictionary modelState)
        {
            return _notificationManager.DeleteAsync(userId, notificationIds, modelState);
        }

        public Task<NotificationUser> GetNewestAsync(int userId, int? currentNotificationId = null)
        {
            return _notificationManager.GetNewestAsync(userId, currentNotificationId);
        }
    }
}
