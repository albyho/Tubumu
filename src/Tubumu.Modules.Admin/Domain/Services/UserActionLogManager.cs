using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Domain.Entities;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Core.Models;
using Tubumu.Modules.Framework.Extensions;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Domain.Services
{
    /// <summary>
    /// IUserActionLogManager
    /// </summary>
    public interface IUserActionLogManager
    {
        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="userActionLogId"></param>
        /// <returns></returns>
        Task<XM.UserActionLogInfo> GetUserActionLogInfoAsync(int userActionLogId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<Page<XM.UserActionLogInfo>> GetUserActionLogInfoPageAsync(XM.UserActionLogPageSearchCriteria criteria);

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="userActionLogInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(UserActionLogInput userActionLogInput, ModelStateDictionary modelState);
    }

    /// <summary>
    /// UserActionLogManager
    /// </summary>
    public class UserActionLogManager : IUserActionLogManager
    {
        private readonly TubumuContext _context;
        private readonly Expression<Func<UserActionLog, XM.UserActionLogInfo>> _userActionLogInfoSelector;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public UserActionLogManager(TubumuContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            _userActionLogInfoSelector = m => new XM.UserActionLogInfo
            {
                UserActionLogId = m.UserActionLogId,
                User = new XM.UserInfoWarpper
                {
                    UserId = m.User.UserId,
                    Username = m.User.Username,
                    DisplayName = m.User.DisplayName,
                    AvatarUrl = m.User.AvatarUrl,
                    LogoUrl = m.User.LogoUrl,
                },
                ActionTypeId = m.ActionTypeId,
                ClientTypeId = m.ClientTypeId,
                ClientAgent = m.ClientAgent,
                Remark = m.Remark,
            };
        }

        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <returns></returns>
        public async Task<XM.UserActionLogInfo> GetUserActionLogInfoAsync(int userActionLogId)
        {
            var item = await _context.UserActionLog.AsNoTracking().Where(m => m.UserActionLogId == userActionLogId).Select(_userActionLogInfoSelector).FirstOrDefaultAsync();
            return item;
        }

        /// <summary>
        /// GetUserActionLogInfoPageAsync
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<Page<XM.UserActionLogInfo>> GetUserActionLogInfoPageAsync(XM.UserActionLogPageSearchCriteria criteria)
        {
            IQueryable<UserActionLog> query = CreateQuery(criteria);
            IOrderedQueryable<UserActionLog> orderedQuery;
            if (criteria.PagingInfo.SortInfo.IsValid())
            {
                orderedQuery = query.Order(criteria.PagingInfo.SortInfo);
            }
            else
            {
                // 默认排序
                orderedQuery = query.OrderByDescending(m => m.UserActionLogId);
            }

            var page = await orderedQuery.Select(_userActionLogInfoSelector).GetPageAsync(criteria.PagingInfo);
            return page;
        }

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="userActionLogInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> SaveAsync(UserActionLogInput userActionLogInput, ModelStateDictionary modelState)
        {
            var newUserActionLog = new UserActionLog();
            _mapper.Map(userActionLogInput, newUserActionLog);
            newUserActionLog.CreationTime = DateTime.Now;

            _context.UserActionLog.Add(newUserActionLog);
            await _context.SaveChangesAsync();
            return true;
        }

        private IQueryable<UserActionLog> CreateQuery(XM.UserActionLogPageSearchCriteria criteria)
        {
            IQueryable<UserActionLog> query = _context.UserActionLog;
            return query;
        }
    }
}
