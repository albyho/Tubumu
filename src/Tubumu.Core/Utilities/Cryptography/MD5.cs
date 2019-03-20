using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Tubumu.Core.Extensions;

namespace Tubumu.Core.Utilities.Cryptography
{
    /// <summary>
    /// MD5加密算法
    /// </summary>
    public static class MD5
    {
        /// <summary>
        /// EncryptFromByteArrayToByteArray
        /// </summary>
        /// <param name="inputByteArray"></param>
        /// <returns></returns>
        public static Byte[] EncryptFromByteArrayToByteArray(Byte[] inputByteArray)
        {
            //MD5 md5 = System.Security.Cryptography.MD5.Create();
            var provider = new MD5CryptoServiceProvider();
            return provider.ComputeHash(inputByteArray);
        }

        /// <summary>
        /// EncryptFromStringToByteArray
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static Byte[] EncryptFromStringToByteArray(string encryptString)
        {
            if (encryptString.IsNullOrWhiteSpace()) return null;

            Byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            return EncryptFromByteArrayToByteArray(inputByteArray);
        }

        /// <summary>
        /// EncryptFromByteArrayToBase64
        /// </summary>
        /// <param name="inputByteArray"></param>
        /// <returns></returns>
        public static string EncryptFromByteArrayToBase64(Byte[] inputByteArray)
        {
            Byte[] encryptBuffer = EncryptFromByteArrayToByteArray(inputByteArray);
            return Convert.ToBase64String(encryptBuffer);
        }

        /// <summary>
        /// EncryptFromStringToBase64
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string EncryptFromStringToBase64(string encryptString)
        {
            if (encryptString.IsNullOrWhiteSpace()) return null;

            Byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            Byte[] encryptBuffer = EncryptFromByteArrayToByteArray(inputByteArray);
            return Convert.ToBase64String(encryptBuffer);
        }

        /// <summary>
        /// EncryptFromByteArrayToHex
        /// </summary>
        /// <param name="inputByteArray"></param>
        /// <returns></returns>
        public static string EncryptFromByteArrayToHex(Byte[] inputByteArray)
        {
            Byte[] encryptBuffer = EncryptFromByteArrayToByteArray(inputByteArray);
            return ByteArrayToHex(encryptBuffer);
        }

        /// <summary>
        /// EncryptFromStringToHex
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string EncryptFromStringToHex(string encryptString)
        {
            Byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            Byte[] encryptBuffer = EncryptFromByteArrayToByteArray(inputByteArray);
            return ByteArrayToHex(encryptBuffer);
        }

        private static string ByteArrayToHex(IEnumerable<byte> inputByteArray)
        {
            var sb = new StringBuilder();
            foreach (Byte item in inputByteArray)
            {
                sb.AppendFormat("{0:X2}", item);
            }
            return sb.ToString();
        }
    }
}
