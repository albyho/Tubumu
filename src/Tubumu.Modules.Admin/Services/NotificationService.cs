using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Tubumu.Modules.Admin.Hubs;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Repositories;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Services
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
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

        public NotificationService(INotificationRepository notificationRepository, IHubContext<NotificationHub, INotificationClient> hubContext)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }

        public async Task<Page<NotificationUser>> GetPageAsync(NotificationPageSearchCriteria criteria)
        {
            return await _notificationRepository.GetPageAsync(criteria);
        }

        public async Task<bool> SaveAsync(NotificationInput notificationInput, ModelStateDictionary modelState)
        {
            var result = await _notificationRepository.SaveAsync(notificationInput, modelState);
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
                    await client.ReceiveMessage(apiResultNotification);
                }
                else
                {
                    await _hubContext.Clients.All.ReceiveMessage(apiResultNotification);
                }
            }
            return result;
        }

        public async Task<bool> RemoveAsync(int notificationId, ModelStateDictionary modelState)
        {
            return await _notificationRepository.RemoveAsync(notificationId, modelState);
        }

        public async Task<bool> ReadAsync(int userId, int[] notificationIds, ModelStateDictionary modelState)
        {
            return await _notificationRepository.ReadAsync(userId, notificationIds, modelState);
        }

        public async Task<bool> DeleteAsync(int userId, int[] notificationIds, ModelStateDictionary modelState)
        {
            return await _notificationRepository.DeleteAsync(userId, notificationIds, modelState);
        }

        public async Task<NotificationUser> GetNewestAsync(int userId, int? currentNotificationId = null)
        {
            return await _notificationRepository.GetNewestAsync(userId, currentNotificationId);
        }

    }
}
