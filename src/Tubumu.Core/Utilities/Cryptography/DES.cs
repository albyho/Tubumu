using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Tubumu.Core.Extensions;

namespace Tubumu.Core.Utilities.Cryptography
{
    /// <summary>
    /// DES加密解密算法(默认采用的是ECB模式)
    /// </summary>
    public static class DES
    {
        private const string DefaultKey = "$uo@5%8*";

        #region 加密

        // 字节数组 -> 字节数组
        // 字节数组 -> Base64
        // 字符串   -> 字节数组
        // 字符串   -> Base64
        // 字符串   -> Base64(指定填充模式)
        // 字符串   -> Hex

        /// <summary>
        /// 核心方法 EncryptFromByteArrayToByteArray
        /// </summary>
        /// <param name="inputByteArray"></param>
        /// <param name="mode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Byte[] EncryptFromByteArrayToByteArray(Byte[] inputByteArray, CipherMode mode, PaddingMode paddingMode, string key = null)
        {
            if (inputByteArray == null)
            {
                throw new ArgumentNullException(nameof(inputByteArray));
            }

            Byte[] keyBytes = EnsureKey(key);
            Byte[] keyIV = keyBytes;
            var provider = new DESCryptoServiceProvider
            {
                Mode = mode,
                Padding = paddingMode
            };
            var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return mStream.ToArray();
        }

        /// <summary>
        /// EncryptFromByteArrayToByteArray
        /// </summary>
        /// <param name="inputByteArray"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Byte[] EncryptFromByteArrayToByteArray(Byte[] inputByteArray, string key = null)
        {
            return EncryptFromByteArrayToByteArray(inputByteArray, CipherMode.ECB, PaddingMode.Zeros, key);
        }

        /// <summary>
        /// EncryptFromByteArrayToBase64String
        /// </summary>
        /// <param name="inputByteArray"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptFromByteArrayToBase64String(Byte[] inputByteArray, string key = null)
        {
            return Convert.ToBase64String(EncryptFromByteArrayToByteArray(inputByteArray, key));
        }

        /// <summary>
        /// EncryptFromStringToByteArray
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Byte[] EncryptFromStringToByteArray(string encryptString, string key = null)
        {
            if (encryptString == null)
            {
                throw new ArgumentNullException(nameof(encryptString));
            }

            Byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);

            return EncryptFromByteArrayToByteArray(inputByteArray, key);
        }

        /// <summary>
        /// EncryptFromStringToBase64String
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptFromStringToBase64String(string encryptString, string key = null)
        {
            if (encryptString == null)
            {
                throw new ArgumentNullException(nameof(encryptString));
            }

            Byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);

            return EncryptFromByteArrayToBase64String(inputByteArray, key);
        }

        /// <summary>
        /// EncryptFromStringToBase64String
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="paddingMode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptFromStringToBase64String(string encryptString, PaddingMode paddingMode, string key = null)
        {
            if (encryptString == null)
            {
                throw new ArgumentNullException(nameof(encryptString));
            }

            Byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);

            Byte[] keyBytes = EnsureKey(key);
            Byte[] keyIV = keyBytes;

            var provider = new DESCryptoServiceProvider
            {
                Mode = CipherMode.ECB,
                Padding = paddingMode
            };
            var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// EncryptFromStringToHex
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptFromStringToHex(string encryptString, string key = null)
        {
            var encryptBuffer = EncryptFromStringToByteArray(encryptString, key);
            return HexFromByteArray(encryptBuffer);
        }

        #endregion

        #region 解密

        /// <summary>
        /// 核心方法 DecryptFromByteArrayToByteArray
        /// </summary>
        /// <param name="inputByteArray"></param>
        /// <param name="mode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Byte[] DecryptFromByteArrayToByteArray(Byte[] inputByteArray, CipherMode mode, PaddingMode paddingMode, string key = null)
        {
            if (inputByteArray == null)
            {
                throw new ArgumentNullException(nameof(inputByteArray));
            }

            Byte[] keyBytes = EnsureKey(key);
            Byte[] keyIV = keyBytes;
            var provider = new DESCryptoServiceProvider
            {
                Mode = mode,
                Padding = paddingMode
            };
            var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return mStream.ToArray();
        }

        /// <summary>
        /// DecryptFromByteArrayToByteArray
        /// </summary>
        /// <param name="inputByteArray"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Byte[] DecryptFromByteArrayToByteArray(Byte[] inputByteArray, string key = null)
        {
            return DecryptFromByteArrayToByteArray(inputByteArray, CipherMode.ECB, PaddingMode.Zeros);
        }

        /// <summary>
        /// DecryptFromByteArrayToString
        /// </summary>
        /// <param name="inputByteArray"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DecryptFromByteArrayToString(Byte[] inputByteArray, string key = null)
        {
            if (inputByteArray == null)
            {
                throw new ArgumentNullException(nameof(inputByteArray));
            }

            return Encoding.UTF8.GetString(DecryptFromByteArrayToByteArray(inputByteArray, key));
        }

        /// <summary>
        /// DecryptFromBase64StringToByteArray
        /// </summary>
        /// <param name="decryptBase64String"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Byte[] DecryptFromBase64StringToByteArray(string decryptBase64String, string key = null)
        {
            if (decryptBase64String == null)
            {
                throw new ArgumentNullException(nameof(decryptBase64String));
            }

            Byte[] inputByteArray = Convert.FromBase64String(decryptBase64String);
            return DecryptFromByteArrayToByteArray(inputByteArray, key);
        }

        /// <summary>
        /// DecryptFromBase64StringToString
        /// </summary>
        /// <param name="decryptBase64String"></param>
        /// <param name="paddingMode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DecryptFromBase64StringToString(string decryptBase64String, PaddingMode paddingMode, string key)
        {
            if (decryptBase64String == null)
            {
                throw new ArgumentNullException(nameof(decryptBase64String));
            }

            Byte[] inputByteArray = Convert.FromBase64String(decryptBase64String);
            Byte[] keyBytes = EnsureKey(key);
            Byte[] keyIV = keyBytes;
            var provider = new DESCryptoServiceProvider
            {
                Mode = CipherMode.ECB,
                Padding = paddingMode
            };
            var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        /// <summary>
        /// DecryptFromHexToString
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DecryptFromHexToString(string decryptString, string key = null)
        {
            var decryptBuffer = new Byte[decryptString.Length / 2];
            for (int i = 0; i < decryptBuffer.Length; i++)
            {
                decryptBuffer[i] = Convert.ToByte(decryptString.Substring(i * 2, 2), 16);
            }
            return Encoding.UTF8.GetString(DecryptFromByteArrayToByteArray(decryptBuffer, key)).Replace("\0", "");

        }

        #endregion

        /// <summary>
        /// HexFromByteArray
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string HexFromByteArray(Byte[] value)
        {
            if (value == null || value.Length == 0) return String.Empty;
            var sb = new StringBuilder();
            foreach (var item in value)
            {
                sb.AppendFormat("{0:X2}", item);
            }
            return sb.ToString();
        }

        #region Private Methods

        private static Byte[] EnsureKey(string key)
        {
            if (key != null)
            {
                Byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                if (keyBytes.Length < 8)
                    throw new ArgumentOutOfRangeException(nameof(key), "key应该是经过UTF8编码后的长度至少需要8个字节的字符串");

                return keyBytes.Length == 8 ? keyBytes : keyBytes.SubArray(8);
            }
            return Encoding.UTF8.GetBytes(DefaultKey);
        }

        #endregion
    }
}
