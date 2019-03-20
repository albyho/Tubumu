using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Controllers
{
    public partial class AdminController
    {
        #region 通知管理

        /// <summary>
        /// 管理员获取通知列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost("GetNotificationsForManager")]
        [PermissionAuthorize(Permissions = "通知管理")]
        public async Task<ApiPageResult<NotificationUser>> GetNotificationsForManager([FromBody]NotificationPageSearchCriteria criteria)
        {
            var page = await _notificationService.GetPageAsync(criteria);
            var result = new ApiPageResult<NotificationUser>
            {
                Code = 200,
                Message = "获取通知列表成功",
                Page = page,
            };
            return result;
        }

        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="notificationInput"></param>
        /// <returns></returns>
        [HttpPost("AddNotification")]
        [PermissionAuthorize(Permissions = "通知管理")]
        public async Task<ApiResult> AddNotification([FromBody]NotificationInput notificationInput)
        {
            notificationInput.FromUserId = HttpContext.User.GetUserId();
            var result = new ApiResult();
            if (notificationInput.NotificationId.HasValue)
            {
                result.Code = 400;
                result.Message = "编辑通知失败：无需通知Id";
                return result;
            }
            if (!await _notificationService.SaveAsync(notificationInput, ModelState))
            {
                result.Code = 400;
                result.Message = "发布通知失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "发布通知成功";
            return result;
        }

        /// <summary>
        /// 编辑通知
        /// </summary>
        /// <param name="notificationInput"></param>
        /// <returns></returns>
        [HttpPost("EditNotification")]
        [PermissionAuthorize(Permissions = "通知管理")]
        public async Task<object> EditNotification([FromBody]NotificationInput notificationInput)
        {
            notificationInput.FromUserId = HttpContext.User.GetUserId();
            var result = new ApiResult();
            if (!notificationInput.NotificationId.HasValue)
            {
                result.Code = 400;
                result.Message = "编辑通知失败：无通知ID";
                return result;
            }
            if (!await _notificationService.SaveAsync(notificationInput, ModelState))
            {
                result.Code = 400;
                result.Message = "编辑通知失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "编辑通知成功";
            return result;
        }

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="notificationIdInput"></param>
        /// <returns></returns>
        [HttpPost("RemoveNotification")]
        [PermissionAuthorize(Permissions = "通知管理")]
        public async Task<ApiResult> RemoveNotification([FromBody]NotificationIdInput notificationIdInput)
        {
            var result = new ApiResult();
            if (!await _notificationService.RemoveAsync(notificationIdInput.NotificationId, ModelState))
            {
                result.Code = 400;
                result.Message = "删除失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "删除成功";
            return result;
        }

        #endregion
    }
}
