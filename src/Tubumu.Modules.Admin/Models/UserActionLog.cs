using System;
using System.ComponentModel.DataAnnotations;
using Tubumu.Modules.Core.Models;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 用户操作日志
    /// </summary>
    public class UserActionLogInfo
    {
        /// <summary>
        /// 日志 Id
        /// </summary>
        public int UserActionLogId { get; set; }

        /// <summary>
        /// 用户 Id
        /// </summary>
        public UserInfoWarpper User { get; set; }

        /// <summary>
        /// 操作类型 Id: 1、登录 2、注销 3、App 进入前台 4、 App 进入后台
        /// </summary>
        public int ActionTypeId { get; set; }

        /// <summary>
        /// 操作系统文本
        /// </summary>
        public string ActionTypeText
        {
            get
            {
                switch(ActionTypeId)
                {
                    case 1:
                        return "登录";
                    case 2:
                        return "注销";
                    case 3:
                        return "App 进入前台";
                    case 4:
                        return "App 进入后台";
                    default:
                        return "";
                };
            }
        }

        /// <summary>
        /// 客户端类型 Id: 1、PC 2、Web 3、Android 4、iOS
        /// </summary>
        public int? ClientTypeId { get; set; }

        /// <summary>
        /// 客户端类型文本
        /// </summary>
        public string ClientTypeText
        {
            get
            {
                switch (ActionTypeId)
                {
                    case 1:
                        return "PC";
                    case 2:
                        return "Web";
                    case 3:
                        return "Android";
                    case 4:
                        return "iOS";
                    default:
                        return "";
                };
            }
        }

        /// <summary>
        /// 客户端代理
        /// </summary>
        public string ClientAgent { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class UserActionLogSearchCriteria
    {

    }

    public class UserActionLogPageSearchCriteria : UserActionLogSearchCriteria
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        [Required(ErrorMessage = "请输入分页信息")]
        public PagingInfo PagingInfo { get; set; }
    }
}
