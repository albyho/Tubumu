using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tubumu.Modules.Admin.Models.Input;

namespace Tubumu.Modules.Admin.Application.Services
{
    /// <summary>
    /// IAdminUserService
    /// </summary>
    public interface IAdminUserService
    {
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangePasswordAsync(int userId, UserChangePasswordInput userInput, ModelStateDictionary modelState);

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeProfileAsync(int userId, UserChangeProfileInput userInput, ModelStateDictionary modelState);
    }

    /// <summary>
    /// AdminUserService
    /// </summary>
    public class AdminUserService : IAdminUserService
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userService"></param>
        public AdminUserService(IUserService userService)
        {
            _userService = userService;
        }

        #region IAdminUserService Members

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="input"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangePasswordAsync(int userId, UserChangePasswordInput input, ModelStateDictionary modelState)
        {
            //判断当前密码是否输入正确
            var chkUser = await _userService.GetNormalUserAsync(userId, input.CurrentPassword);
            if (chkUser == null)
            {
                modelState.AddModelError("CurrentPassword", "当前密码不正确");
                return false;
            }

            return await _userService.ChangePasswordAsync(chkUser.UserId, input.NewPassword, modelState);

        }

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="input"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeProfileAsync(int userId, UserChangeProfileInput input, ModelStateDictionary modelState)
        {
            return await _userService.ChangeProfileAsync(userId, input, modelState);
        }

        #endregion
    }
}
