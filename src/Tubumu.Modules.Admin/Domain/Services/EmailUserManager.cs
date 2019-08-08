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
    /// IEmailUserManager
    /// </summary>
    public interface IEmailUserManager
    {
        /// <summary>
        /// ChangeEmailAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newEmail"></param>
        /// <param name="emailIsValid"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeEmailAsync(int userId, string newEmail, bool emailIsValid, ModelStateDictionary modelState);

        /// <summary>
        /// GenerateItemAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="status"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<XM.UserInfo> GenerateItemAsync(Guid groupId, XM.UserStatus status, string email, string password, ModelStateDictionary modelState);

        /// <summary>
        /// ResetPasswordAsync
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<int> ResetPasswordAsync(string email, string password, ModelStateDictionary modelState);

        /// <summary>
        /// GetOrGenerateItemByEmailAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="email"></param>
        /// <param name="emailIsValid"></param>
        /// <returns></returns>
        Task<XM.UserInfo> GetOrGenerateItemByEmailAsync(Guid groupId, XM.UserStatus generateStatus, string email, bool emailIsValid);
    }

    /// <summary>
    /// EmailUserManager
    /// </summary>
    public class EmailUserManager : IEmailUserManager
    {
        private readonly TubumuContext _context;
        private readonly IUserManager _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public EmailUserManager(TubumuContext context, IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// ChangeEmailAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newEmail"></param>
        /// <param name="emailIsValid"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeEmailAsync(int userId, string newEmail, bool emailIsValid, ModelStateDictionary modelState)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userId);
            if (user == null)
            {
                modelState.AddModelError("UserId", "当前用户不存在");
                return false;
            }
            if (!user.Email.IsNullOrWhiteSpace() &&
                user.Email.Equals(newEmail, StringComparison.InvariantCultureIgnoreCase))
            {
                modelState.AddModelError("UserId", "目标邮箱和当前邮箱相同");
                return false;
            }
            if (_context.User.Any(m => m.UserId != userId && m.Email == newEmail))
            {
                modelState.AddModelError("UserId", $"邮箱[{newEmail}]已经被使用");
                return false;
            }
            user.EmailIsValid = emailIsValid;
            user.Email = newEmail;
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// GenerateItemAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="status"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<XM.UserInfo> GenerateItemAsync(Guid groupId, XM.UserStatus status, string email, string password, ModelStateDictionary modelState)
        {
            if (await _context.User.AnyAsync(m => m.Email == email))
            {
                modelState.AddModelError(nameof(email), $"邮箱 {email} 已被注册。");
                return null;
            }

            var newUser = new User
            {
                Status = status,
                CreationTime = DateTime.Now,
                Email = email,
                EmailIsValid = true,
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
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<int> ResetPasswordAsync(string email, string password, ModelStateDictionary modelState)
        {
            if (!await _context.User.AnyAsync(m => m.Email == email))
            {
                modelState.AddModelError(nameof(email), $"邮箱 {email} 尚未注册。");
                return 0;
            }

            var user = await _context.User.Where(m => m.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                modelState.AddModelError(nameof(email), $"邮箱 {email} 尚未注册。");
                return 0;
            }
            if (user.Status != XM.UserStatus.Normal)
            {
                modelState.AddModelError(nameof(email), $"邮箱 {email} 的用户状态不允许重置密码。");
                return 0;
            }

            user.Password = password;
            await _context.SaveChangesAsync();
            return user.UserId;
        }

        /// <summary>
        /// GetOrGenerateItemByEmailAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="email"></param>
        /// <param name="emailIsValid"></param>
        /// <returns></returns>
        public async Task<XM.UserInfo> GetOrGenerateItemByEmailAsync(Guid groupId, XM.UserStatus generateStatus, string email, bool emailIsValid)
        {
            if (email.IsNullOrWhiteSpace()) return null;
            var user = await _userManager.GetItemByEmailAsync(email, null, null);
            if (user == null)
            {
                var newUser = new User
                {
                    Status = generateStatus,
                    CreationTime = DateTime.Now,
                    Email = email,
                    EmailIsValid = emailIsValid,
                    GroupId = groupId, // new Guid("11111111-1111-1111-1111-111111111111") 等待分配组
                    Username = "U" + Guid.NewGuid().ToString("N").Substring(19),
                    Password = email,
                };

                _context.User.Add(newUser);
                await _context.SaveChangesAsync();
                user = await _userManager.GetItemByEmailAsync(email, null, null);
            }
            return user;
        }
    }
}
