using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Domain.Services;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Core.Models;
using Tubumu.Modules.Framework.Extensions;

namespace Tubumu.Modules.Admin.Application.Services
{
    /// <summary>
    /// IUserActionLogService
    /// </summary>
    public interface IUserActionLogService
    {
        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="userActionLogId"></param>
        /// <returns></returns>
        Task<UserActionLogInfo> GetUserActionLogInfoAsync(int userActionLogId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<Page<UserActionLogInfo>> GetUserActionLogInfoPageAsync(UserActionLogPageSearchCriteria criteria);

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="userActionLogInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(UserActionLogInput userActionLogInput, ModelStateDictionary modelState);
    }

    /// <summary>
    /// UserActionLogService
    /// </summary>
    public class UserActionLogService : IUserActionLogService
    {
        private readonly IUserActionLogManager _manager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="manager"></param>
        public UserActionLogService(IUserActionLogManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// GetUserActionLogInfoAsync
        /// </summary>
        /// <param name="userActionLogId"></param>
        /// <returns></returns>
        public Task<UserActionLogInfo> GetUserActionLogInfoAsync(int userActionLogId)
        {
            return _manager.GetUserActionLogInfoAsync(userActionLogId);
        }

        /// <summary>
        /// GetUserActionLogInfoPageAsync
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Task<Page<UserActionLogInfo>> GetUserActionLogInfoPageAsync(UserActionLogPageSearchCriteria criteria)
        {
            return _manager.GetUserActionLogInfoPageAsync(criteria);
        }

        /// <summary>
        /// SaveAsyncSaveAsync
        /// </summary>
        /// <param name="userActionLogInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public Task<bool> SaveAsync(UserActionLogInput userActionLogInput, ModelStateDictionary modelState)
        {
            return _manager.SaveAsync(userActionLogInput, modelState);
        }
    }
}
