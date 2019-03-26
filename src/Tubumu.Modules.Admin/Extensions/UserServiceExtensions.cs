using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tubumu.Core.Extensions;
using Tubumu.Core.Utilities.Cryptography;
using Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Services
{
    /// <summary>
    /// UserServiceExtensions
    /// </summary>
    public static class UserServiceExtensions
    {
        /// <summary>
        /// GetNormalUserAsync
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<UserInfo> GetNormalUserAsync(this IUserService userService, string account, string password)
        {
            if (account.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace()) return null;

            // ^(([a-zA-Z][a-zA-Z0-9-_]*)|(1\d{10}))|([\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?)$
            UserInfo userInfo = null;
            if (IsValidUsername(account))
            {
                userInfo = await userService.GetItemByUsernameAsync(account, UserStatus.Normal);
            }
            else if (IsValidMobile(account))
            {
                userInfo = await userService.GetItemByMobileAsync(account, true, UserStatus.Normal);
            }
            else if (IsValidEmail(account))
            {
                userInfo = await userService.GetItemByEmailAsync(account, true, UserStatus.Normal);
            }

            if (userInfo == null || userInfo.Password.IsNullOrWhiteSpace()) return null;
            return CheckPassword(userInfo, password);
        }

        /// <summary>
        /// GetNormalUserAsync
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<UserInfo> GetNormalUserAsync(this IUserService userService, int userId, string password)
        {
            if (password.IsNullOrWhiteSpace()) return null;

            var userInfo = await userService.GetItemByUserIdAsync(userId, UserStatus.Normal);
            if (userInfo == null || userInfo.Password.IsNullOrWhiteSpace()) return null;
            return CheckPassword(userInfo, password);
        }

        /// <summary>
        /// GetNormalUserByUesrIdAsync
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<UserInfo> GetNormalUserByUesrIdAsync(this IUserService userService, string userId)
        {
            if (int.TryParse(userId, out var value))
            {
                return await userService.GetItemByUserIdAsync(value, UserStatus.Normal);
            }

            return null;
        }

        /// <summary>
        /// VerifyPassword
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Boolean VerifyPassword(this UserInfo userInfo, string password)
        {
            if (userInfo == null || password.IsNullOrWhiteSpace()) return false;
            if (userInfo.Password.IsNullOrWhiteSpace()) return false;

            string[] splitData = userInfo.Password.Split('|');
            if (splitData.Length != 2) throw new InvalidOperationException("Password and PasswordSalt could not be read from module data");

            string userPasswordSalt = splitData[0];
            string userPassword = splitData[1];

            return userPassword == SHA256.Encrypt(password, userPasswordSalt) ? true : false;
        }

        #region Private Method

        private static bool IsValidEmail(string source)
        {
            return Regex.IsMatch(source, @"^([\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?)$");
        }

        private static bool IsValidMobile(string source)
        {
            return Regex.IsMatch(source, @"^(1\d{10})$");
        }

        private static bool IsValidUsername(string source)
        {
            return Regex.IsMatch(source, @"^([a-zA-Z][a-zA-Z0-9-_]*)$");
        }

        private static UserInfo CheckPassword(UserInfo userInfo, string password)
        {
            string[] splitData = userInfo.Password.Split('|');
            if (splitData.Length != 2) throw new InvalidOperationException("Password and PasswordSalt could not be read from module data");

            string userPasswordSalt = splitData[0];
            string userPassword = splitData[1];

            return userPassword == SHA256.Encrypt(password, userPasswordSalt) ? userInfo : null;
        }

        #endregion
    }
}
