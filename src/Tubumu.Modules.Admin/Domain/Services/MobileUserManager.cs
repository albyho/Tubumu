using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Domain.Entities;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Domain.Services
{
    /// <summary>
    /// IMobileUserManager
    /// </summary>
    public interface IMobileUserManager
    {
        /// <summary>
        /// ChangeMobileAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newMobile"></param>
        /// <param name="mobileIsValid"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeMobileAsync(int userId, string newMobile, bool mobileIsValid, ModelStateDictionary modelState);

        /// <summary>
        /// GenerateItemAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="status"></param>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<XM.UserInfo> GenerateItemAsync(Guid groupId, XM.UserStatus status, string mobile, string password, ModelStateDictionary modelState);

        /// <summary>
        /// ResetPasswordAsync
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<int> ResetPasswordAsync(string mobile, string password, ModelStateDictionary modelState);

        /// <summary>
        /// GetOrGenerateItemByMobileAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="mobile"></param>
        /// <param name="mobileIsValid"></param>
        /// <returns></returns>
        Task<XM.UserInfo> GetOrGenerateItemByMobileAsync(Guid groupId, XM.UserStatus generateStatus, string mobile, bool mobileIsValid);
    }

    /// <summary>
    /// MobileUserManager
    /// </summary>
    public class MobileUserManager : IMobileUserManager
    {
        private readonly TubumuContext _context;
        private readonly IUserManager _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public MobileUserManager(TubumuContext context, IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// ChangeMobileAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newMobile"></param>
        /// <param name="mobileIsValid"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeMobileAsync(int userId, string newMobile, bool mobileIsValid, ModelStateDictionary modelState)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userId);
            if (user == null)
            {
                modelState.AddModelError("UserId", "当前用户不存在");
                return false;
            }
            if (!user.Mobile.IsNullOrWhiteSpace() &&
                user.Mobile.Equals(newMobile, StringComparison.InvariantCultureIgnoreCase))
            {
                modelState.AddModelError("UserId", "目标手机号和当前手机号相同");
                return false;
            }
            if (await _context.User.AnyAsync(m => m.UserId != userId && m.Mobile == newMobile))
            {
                modelState.AddModelError("UserId", $"手机号[{newMobile}]已经被使用");
                return false;
            }
            user.MobileIsValid = mobileIsValid;
            user.Mobile = newMobile;
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// GenerateItemAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="status"></param>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<XM.UserInfo> GenerateItemAsync(Guid groupId, XM.UserStatus status, string mobile, string password, ModelStateDictionary modelState)
        {
            if (await _context.User.AnyAsync(m => m.Mobile == mobile))
            {
                modelState.AddModelError(nameof(mobile), $"手机号 {mobile} 已被注册。");
                return null;
            }
            var newUser = new User
            {
                Status = status,
                CreationTime = DateTime.Now,
                Mobile = mobile,
                MobileIsValid = true,
                GroupId = groupId,
                Username = "U" + Guid.NewGuid().ToString("N").Substring(19),
                Password = password,
            };

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();
            return await _userManager.GetItemByUserIdAsync(newUser.UserId);
        }

        /// <summary>
        /// ResetPasswordAsync
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<int> ResetPasswordAsync(string mobile, string password, ModelStateDictionary modelState)
        {
            if (!await _context.User.AnyAsync(m => m.Mobile == mobile))
            {
                modelState.AddModelError(nameof(mobile), $"手机号 {mobile} 尚未注册。");
                return 0;
            }
            var user = await _context.User.Where(m => m.Mobile == mobile).FirstOrDefaultAsync();
            if (user == null)
            {
                modelState.AddModelError(nameof(mobile), $"手机号 {mobile} 尚未注册。");
                return 0;
            }
            if (user.Status != XM.UserStatus.Normal)
            {
                modelState.AddModelError(nameof(mobile), $"手机号 {mobile} 的用户状态不允许重置密码。");
                return 0;
            }

            user.Password = password;
            await _context.SaveChangesAsync();
            return user.UserId;
        }

        /// <summary>
        /// GetOrGenerateItemByMobileAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="mobile"></param>
        /// <param name="mobileIsValid"></param>
        /// <returns></returns>
        public async Task<XM.UserInfo> GetOrGenerateItemByMobileAsync(Guid groupId, XM.UserStatus generateStatus, string mobile, bool mobileIsValid)
        {
            if (mobile.IsNullOrWhiteSpace()) return null;
            var user = await _userManager.GetItemByMobileAsync(mobile, null, null);
            if (user == null)
            {
                var newUser = new User
                {
                    Status = generateStatus,
                    CreationTime = DateTime.Now,
                    Mobile = mobile,
                    MobileIsValid = mobileIsValid,
                    GroupId = groupId, // new Guid("11111111-1111-1111-1111-111111111111") 等待分配组
                    Username = "U" + Guid.NewGuid().ToString("N").Substring(19),
                    Password = mobile,
                };

                _context.User.Add(newUser);
                await _context.SaveChangesAsync();
                user = await _userManager.GetItemByMobileAsync(mobile, null, null);
            }
            return user;
        }
    }
}
