using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Tubumu.Modules.Admin.Entities;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Repositories
{
    /// <summary>
    /// INotificationRepository
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// GetPageAsync
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<Page<XM.NotificationUser>> GetPageAsync(XM.NotificationSearchCriteria criteria);

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="notificationInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(XM.NotificationInput notificationInput, ModelStateDictionary modelState);

        /// <summary>
        /// RemoveAsync
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(int notificationId, ModelStateDictionary modelState);

        /// <summary>
        /// ReadAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="notificationIds"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ReadAsync(int userId, int[] notificationIds, ModelStateDictionary modelState);

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="notificationIds"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int userId, int[] notificationIds, ModelStateDictionary modelState);

        /// <summary>
        /// GetNewestAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentNotificationId"></param>
        /// <returns></returns>
        Task<XM.NotificationUser> GetNewestAsync(int userId, int? currentNotificationId = null);
    }

    /// <summary>
    /// NotificationRepository
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        private readonly TubumuContext _context;
        private readonly Expression<Func<Notification, XM.Notification>> _notificationSelector;
        private readonly Expression<Func<Notification, XM.NotificationUser>> _notificationUserSelector;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tubumuContext"></param>
        public NotificationRepository(TubumuContext tubumuContext)
        {
            _context = tubumuContext;

            _notificationSelector = m => new XM.Notification
            {
                NotificationId = m.NotificationId,
                FromUser = m.FromUser != null ? new XM.UserInfoWarpper
                {
                    UserId = m.FromUser.UserId,
                    Username = m.FromUser.Username,
                    DisplayName = m.FromUser.DisplayName,
                    HeadUrl = m.FromUser.HeadUrl,
                    LogoUrl = m.FromUser.LogoUrl,
                }: null,
                ToUser = m.ToUser != null ? new XM.UserInfoWarpper
                {
                    UserId = m.ToUser.UserId,
                    Username = m.ToUser.Username,
                    DisplayName = m.ToUser.DisplayName,
                    HeadUrl = m.ToUser.HeadUrl,
                    LogoUrl = m.ToUser.LogoUrl,
                }: null,
                Title = m.Title,
                Message = m.Message,
                CreationDate = m.CreationDate,
                Url = m.Url,
            };

            _notificationUserSelector = m => new XM.NotificationUser
            {
                NotificationId = m.NotificationId,
                FromUser = m.FromUser != null ? new XM.UserInfoWarpper
                {
                    UserId = m.FromUser.UserId,
                    Username = m.FromUser.Username,
                    DisplayName = m.FromUser.DisplayName,
                    HeadUrl = m.FromUser.HeadUrl,
                    LogoUrl = m.FromUser.LogoUrl,
                }: null,
                ToUser = m.ToUser != null ? new XM.UserInfoWarpper
                {
                    UserId = m.ToUser.UserId,
                    Username = m.ToUser.Username,
                    DisplayName = m.ToUser.DisplayName,
                    HeadUrl = m.ToUser.HeadUrl,
                    LogoUrl = m.ToUser.LogoUrl,
                }: null,
                Title = m.Title,
                Message = m.Message,
                CreationDate = m.CreationDate,
                Url = m.Url,

                ReadTime = null,
                DeleteTime = null,
            };
        }

        /// <summary>
        /// GetPageAsync
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<Page<XM.NotificationUser>> GetPageAsync(XM.NotificationSearchCriteria criteria)
        {
            if (criteria.ToUserId.HasValue)
            {
                return await GetNotificationUserPageAsync(criteria);
            }

            // 备注：忽略搜索条件的 IsReaded, ToUserId
            // 备注：因为查询所有 ToUserId, 所有不会标记已读未读

            IQueryable<Notification> query = _context.Notification;
            if (criteria.FromUserId.HasValue)
            {
                query = query.Where(m => m.FromUserId == criteria.FromUserId);
            }
            if (criteria.Keyword != null)
            {
                var keyword = criteria.Keyword.Trim();
                if (keyword.Length != 0)
                {
                    query = query.Where(m => m.Title.Contains(keyword));
                }
            }
            if (criteria.CreationDateBegin.HasValue)
            {
                var begin = criteria.CreationDateBegin.Value.Date;
                query = query.Where(m => m.CreationDate >= begin);
            }
            if (criteria.CreationDateEnd.HasValue)
            {
                var end = criteria.CreationDateEnd.Value.Date.AddDays(1);
                query = query.Where(m => m.CreationDate < end);
            }

            IOrderedQueryable<Notification> orderedQuery;
            if (criteria.PagingInfo.SortInfo != null && !criteria.PagingInfo.SortInfo.Sort.IsNullOrWhiteSpace())
            {
                orderedQuery = query.Order(criteria.PagingInfo.SortInfo.Sort, criteria.PagingInfo.SortInfo.SortDir == SortDir.DESC);
            }
            else
            {
                // 默认排序
                orderedQuery = query.OrderByDescending(m => m.NotificationId);
            }

            var page = await orderedQuery.Select(_notificationUserSelector).GetPageAsync(criteria.PagingInfo);
            return page;
        }

        /// <summary>
        /// GetNotificationUserPageAsync
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private async Task<Page<XM.NotificationUser>> GetNotificationUserPageAsync(XM.NotificationSearchCriteria criteria)
        {
            if (!criteria.ToUserId.HasValue)
            {
                throw new ArgumentNullException(nameof(criteria.ToUserId), "必须输入 ToUserId");
            }
            var userCreationDate = await _context.User.AsNoTracking().Where(m => m.UserId == criteria.ToUserId.Value).Select(m => m.CreationDate).FirstOrDefaultAsync();

            // 备注：查询发送给所有人的以及本人的、未删除的记录
            var query1 = from n in _context.Notification
                         where n.CreationDate > userCreationDate && (!n.ToUserId.HasValue || n.ToUserId == criteria.ToUserId.Value)
                         select n;

            if (criteria.FromUserId.HasValue)
            {
                query1 = query1.Where(m => m.FromUserId == criteria.FromUserId);
            }
            if (criteria.Keyword != null)
            {
                var keyword = criteria.Keyword.Trim();
                if (keyword.Length != 0)
                {
                    query1 = query1.Where(m => m.Title.Contains(keyword));
                }
            }
            if (criteria.CreationDateBegin.HasValue)
            {
                var begin = criteria.CreationDateBegin.Value.Date;
                query1 = query1.Where(m => m.CreationDate >= begin);
            }
            if (criteria.CreationDateEnd.HasValue)
            {
                var end = criteria.CreationDateEnd.Value.Date.AddDays(1);
                query1 = query1.Where(m => m.CreationDate < end);
            }

            // 剔除已逻辑删除的记录
            var query2 = from m in query1
                         from pu in m.NotificationUser.Where(n => n.UserId == criteria.ToUserId.Value).DefaultIfEmpty()
                         where pu == null || !pu.DeleteTime.HasValue
                         //join pu in m.NotificationUsers.Where(n=>n.UserId == criteria.ToUserId.Value) on m equals pu.Notification into purd
                         //from x in purd.DefaultIfEmpty()
                         //where x == null || !x.DeleteTime.HasValue
                         select new XM.NotificationUser
                         {
                             NotificationId = m.NotificationId,
                             FromUser = new XM.UserInfoWarpper
                             {
                                 UserId = m.FromUserId.HasValue ? m.FromUser.UserId : 0,
                                 Username = m.FromUserId.HasValue ? m.FromUser.Username : "",
                                 DisplayName = m.FromUserId.HasValue ? m.FromUser.DisplayName : "",
                                 HeadUrl = m.FromUserId.HasValue ? m.FromUser.HeadUrl : "",
                                 LogoUrl = m.FromUserId.HasValue ? m.FromUser.LogoUrl : "",
                             },
                             ToUser = new XM.UserInfoWarpper
                             {
                                 UserId = m.ToUserId.HasValue ? m.ToUser.UserId : 0,
                                 Username = m.ToUserId.HasValue ? m.ToUser.Username : "",
                                 DisplayName = m.ToUserId.HasValue ? m.ToUser.DisplayName : "",
                                 HeadUrl = m.ToUserId.HasValue ? m.ToUser.HeadUrl : "",
                                 LogoUrl = m.ToUserId.HasValue ? m.ToUser.LogoUrl : "",
                             },
                             Title = m.Title,
                             Message = m.Message,
                             Url = m.Url,
                             CreationDate = m.CreationDate,

                             ReadTime = pu != null ? pu.ReadTime : null,
                             DeleteTime = pu != null ? pu.DeleteTime : null,
                         };

            if (criteria.IsReaded.HasValue)
            {
                if (criteria.IsReaded.Value)
                {
                    // 备注，读取已读，也可通过用户的 NotificationsToUser 取
                    query2 = query2.Where(m => m.ReadTime.HasValue);
                }
                else
                {
                    query2 = query2.Where(m => !m.ReadTime.HasValue);
                }
            }

            IOrderedQueryable<XM.NotificationUser> orderedQuery;
            if (criteria.PagingInfo.SortInfo != null && !criteria.PagingInfo.SortInfo.Sort.IsNullOrWhiteSpace())
            {
                orderedQuery = query2.Order(criteria.PagingInfo.SortInfo.Sort, criteria.PagingInfo.SortInfo.SortDir == SortDir.DESC);
            }
            else
            {
                // 默认排序
                orderedQuery = query2.OrderByDescending(m => m.NotificationId);
            }

            var page = await orderedQuery.GetPageAsync(criteria.PagingInfo);
            return page;
        }

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="notificationInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> SaveAsync(XM.NotificationInput notificationInput, ModelStateDictionary modelState)
        {
            User fromUser = null;
            User toUser = null;
            if (notificationInput.FromUserId.HasValue)
            {
                fromUser = await _context.User.FirstOrDefaultAsync(m => m.UserId == notificationInput.FromUserId);
                if (fromUser == null)
                {
                    modelState.AddModelError("FromUserId", "无法获取通知发布者");
                    return false;
                }
            }
            if (notificationInput.ToUserId.HasValue)
            {
                toUser = await _context.User.FirstOrDefaultAsync(m => m.UserId == notificationInput.ToUserId);
                if (toUser == null)
                {
                    modelState.AddModelError("FromUserId", "无法获取通知接收者");
                    return false;
                }
            }
            Notification itemToSave;
            if (notificationInput.NotificationId.HasValue)
            {
                itemToSave = await _context.Notification.FirstOrDefaultAsync(m => m.NotificationId == notificationInput.NotificationId);
                if (itemToSave == null)
                {
                    modelState.AddModelError("FromUserId", "无法获取编辑的记录");
                    return false;
                }
            }
            else
            {
                itemToSave = new Notification
                {
                    FromUser = fromUser,
                    ToUser = toUser,
                    CreationDate = DateTime.Now,
                    Url = notificationInput.Url,
                };

                _context.Notification.Add(itemToSave);
            }

            itemToSave.Title = notificationInput.Title;
            itemToSave.Message = notificationInput.Message;

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// RemoveAsync
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(int notificationId, ModelStateDictionary modelState)
        {
            // 需删除 NotificationUser 的记录

            var sql = "DELETE [NotificationUser] WHERE NotificationId = @NotificationId; DELETE [Notification] WHERE NotificationId = @NotificationId;";
            await _context.Database.ExecuteSqlCommandAsync(sql
                , new SqlParameter("NotificationId", notificationId)
                );

            return true;
        }

        /// <summary>
        /// ReadAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="notificationIds"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ReadAsync(int userId, int[] notificationIds, ModelStateDictionary modelState)
        {
            var notifications = await _context.Notification.AsNoTracking().Where(m => notificationIds.Contains(m.NotificationId)).
                Select(m => new
                {
                    m.NotificationId,
                    m.ToUserId,
                }).
                ToArrayAsync();
            if (notifications.Any(m => m.ToUserId.HasValue && m.ToUserId != userId))
            {
                modelState.AddModelError("Error", "尝试读取不存在或非发给本人的通知");
                return false;
            }

            // TODO: 批量查询出 NotificationUsers，或以其他方式实现
            foreach (var notification in notifications)
            {
                var notificationUser = await _context.NotificationUser.Where(m => m.NotificationId == notification.NotificationId && m.UserId == userId).FirstOrDefaultAsync();
                if (notificationUser == null)
                {
                    var nu = new NotificationUser
                    {
                        UserId = userId,
                        NotificationId = notification.NotificationId,
                        ReadTime = DateTime.Now,
                    };
                    _context.NotificationUser.Add(nu);
                }
                else if (!notificationUser.ReadTime.HasValue)
                {
                    notificationUser.ReadTime = DateTime.Now;
                }

            }
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="notificationIds"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int userId, int[] notificationIds, ModelStateDictionary modelState)
        {
            var notifications = await _context.Notification.AsNoTracking().Where(m => notificationIds.Contains(m.NotificationId)).
                Select(m => new
                {
                    m.NotificationId,
                    m.ToUserId,
                }).
                ToArrayAsync();
            if (notifications.Any(m => m.ToUserId.HasValue && m.ToUserId != userId))
            {
                modelState.AddModelError("Error", "尝试读取不存在或非发给本人的通知");
                return false;
            }

            // TODO: 批量查询出 NotificationUsers，或以其他方式实现
            foreach (var notification in notifications)
            {
                var notificationUser = await _context.NotificationUser.Where(m => m.NotificationId == notification.NotificationId && m.UserId == userId).FirstOrDefaultAsync();
                if (notificationUser == null)
                {
                    var nu = new NotificationUser
                    {
                        UserId = userId,
                        NotificationId = notification.NotificationId,
                        DeleteTime = DateTime.Now,
                    };
                    _context.NotificationUser.Add(nu);
                }
                else if (!notificationUser.DeleteTime.HasValue)
                {
                    notificationUser.DeleteTime = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// GetNewestAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentNotificationId"></param>
        /// <returns></returns>
        public async Task<XM.NotificationUser> GetNewestAsync(int userId, int? currentNotificationId = null)
        {
            var userCreationDate = await _context.User.AsNoTracking().Where(m => m.UserId == userId).Select(m => m.CreationDate).FirstOrDefaultAsync();

            var query1 = from n in _context.Notification.AsNoTracking()
                         where n.CreationDate > userCreationDate && (!n.ToUserId.HasValue || n.ToUserId == userId)
                         select n;

            if (currentNotificationId.HasValue)
            {
                query1 = query1.Where(n => n.NotificationId > currentNotificationId.Value);
            }

            IQueryable<XM.NotificationUser> query2;
            query2 = from m in query1
                     from pu in m.NotificationUser.Where(n => n.UserId == userId).DefaultIfEmpty()
                     where pu == null || (!pu.DeleteTime.HasValue && !pu.ReadTime.HasValue)
                     //join pu in DbContext.NotificationUsers.Where(n => n.UserId == userId) on m equals pu.Notification into purd
                     //from x in purd.DefaultIfEmpty()
                     //where x == null || (!x.DeleteTime.HasValue && !x.ReadTime.HasValue)
                     orderby m.NotificationId descending
                     select new XM.NotificationUser
                     {
                         NotificationId = m.NotificationId,
                         FromUser = new XM.UserInfoWarpper
                         {
                             UserId = m.FromUserId.HasValue ? m.FromUser.UserId : 0,
                             Username = m.FromUserId.HasValue ? m.FromUser.Username : "",
                             DisplayName = m.FromUserId.HasValue ? m.FromUser.DisplayName : "",
                             HeadUrl = m.FromUserId.HasValue ? m.FromUser.HeadUrl : "",
                             LogoUrl = m.FromUserId.HasValue ? m.FromUser.LogoUrl : "",
                         },
                         ToUser = new XM.UserInfoWarpper
                         {
                             UserId = m.ToUserId.HasValue ? m.ToUser.UserId : 0,
                             Username = m.ToUserId.HasValue ? m.ToUser.Username : "",
                             DisplayName = m.ToUserId.HasValue ? m.ToUser.DisplayName : "",
                             HeadUrl = m.ToUserId.HasValue ? m.ToUser.HeadUrl : "",
                             LogoUrl = m.ToUserId.HasValue ? m.ToUser.LogoUrl : "",
                         },
                         Title = m.Title,
                         Message = m.Message,
                         Url = m.Url,
                         CreationDate = m.CreationDate,

                         ReadTime = pu != null ? pu.ReadTime : null,
                         DeleteTime = pu != null ? pu.DeleteTime : null,
                     };

            return await query2.FirstOrDefaultAsync();
        }
    }
}
