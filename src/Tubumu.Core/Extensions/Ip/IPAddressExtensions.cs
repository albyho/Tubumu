using System.Net;
using System.Text.RegularExpressions;

namespace Tubumu.Core.Extensions.Ip
{
    /// <summary>
    /// IpAddressExtensions
    /// </summary>
    public static class IpAddressExtensions
    {
        private static readonly Regex IpV4Regex = new Regex(@"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$", RegexOptions.Compiled);

        /// <summary>
        /// IPAddress 转 Int32 (可能产生负数)
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static int ToInt32(this IPAddress ip)
        {
            var x = 3;
            var v = 0;
            var bytes = ip.GetAddressBytes();
            for (int i = 0; i < bytes.Length; i++)
            {
                byte f = bytes[i];
                v += f << 8 * x--;
            }
            return v;
        }

        /// <summary>
        /// IPAddress 转 Int64
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long ToInt64(this IPAddress ip)
        {
            int x = 3;
            long v = 0;
            var bytes = ip.GetAddressBytes();
            for (var i = 0; i < bytes.Length; i++)
            {
                byte f = bytes[i];
                v += (long)f << 8 * x--;
            }
            return v;
        }

        /// <summary>
        /// Int32 转 IPAddress (注意：由于是基于int的扩展方法，故会造成一定的污染)
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IPAddress ToIPAddress(this int ip)
        {
            var bytes = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                bytes[3 - i] = (byte)(ip >> 8 * i & 255);
            }
            return new IPAddress(bytes);
        }

        /// <summary>
        /// Int64 转 IPAddress (注意：由于是基于Int64的扩展方法，故会造成一定的污染)
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IPAddress ToIPAddress(this long ip)
        {
            var bytes = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                bytes[3 - i] = (byte)(ip >> 8 * i & 255);
            }
            return new IPAddress(bytes);
        }

        /// <summary>
        /// 是否 IPv4 格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPv4(this string ip)
        {
            if (ip.IsNullOrWhiteSpace() || ip.Length < 7 || ip.Length > 15) return false;
            return IpV4Regex.IsMatch(ip);
        }
    }
}
