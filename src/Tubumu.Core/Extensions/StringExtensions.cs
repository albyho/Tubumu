using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.PinYinConverter;

namespace Tubumu.Core.Extensions
{
    /// <summary>
    /// StringExtensions
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex TagRegex = new Regex("<[^<>]*>", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex GuidRegex = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex SpaceRegex = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex NonWordCharsRegex = new Regex(@"[^\w]+", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex UrlRegex = new Regex("(^|[^\\w'\"]|\\G)(?<uri>(?:https?|ftp)(?:&#58;|:)(?:&#47;&#47;|//)(?:[^./\\s'\"<)\\]]+\\.)+[^./\\s'\"<)\\]]+(?:(?:&#47;|/).*?)?)(?:[\\s\\.,\\)\\]'\"]?(?:\\s|\\.|\\)|\\]|,|<|$))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SubDirectoryRegex = new Regex(@"^[a-zA-Z0-9-_]+(/[a-zA-Z0-9-_]+)*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex VirtualDirectoryRegex = new Regex(@"^~(/[a-zA-Z0-9-_]+)+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #region Guid相关

        /// <summary>
        /// 校验字符串是否是 Guid 格式
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsGuid(this string source)
        {
            return !source.IsNullOrWhiteSpace() && GuidRegex.IsMatch(source);
        }

        /// <summary>
        /// 字符串转换为Guid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool GuidTryParse(this string source, out Guid result)
        {
            if (source.IsNullOrWhiteSpace())
            {
                result = Guid.Empty;
                return false;
            }

            try
            {
                result = new Guid(source);
                return true;
            }
            catch (FormatException)
            {
                result = Guid.Empty;
                return false;
            }
            catch (OverflowException)
            {
                result = Guid.Empty;
                return false;
            }
            catch
            {
                result = Guid.Empty;
                return false;
            }
        }

        #endregion

        #region 字符串截取

        /// <summary>
        /// Substr
        /// </summary>
        /// <param name="source"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Substr(this string source, int len)
        {
            return source.Substr(len, "...");
        }

        /// <summary>
        /// Substr
        /// </summary>
        /// <param name="source"></param>
        /// <param name="len"></param>
        /// <param name="att"></param>
        /// <returns></returns>
        public static string Substr(this string source, int len, string att)
        {
            if (string.IsNullOrEmpty(source)) return String.Empty;

            att = att ?? String.Empty;

            var rChinese = new Regex(@"[\u4e00-\u9fa5]"); //验证中文
            var rEnglish = new Regex(@"^[A-Za-z0-9]+$");  //验证字母

            if (rChinese.IsMatch(source))
            {
                //中文
                return (source.Length > len) ? source.Substring(0, len) + att : source;
            }
            else if (rEnglish.IsMatch(source))
            {
                //英文
                return (source.Length > len * 2) ? source.Substring(0, len * 2) + att : source;
            }
            return (source.Length > len) ? source.Substring(0, len) + att : source;


        }

        #endregion

        #region 字符串空 / null 校验

        /// <summary>
        /// IsNullOrEmpty
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return String.IsNullOrEmpty(source);
        }

        /// <summary>
        /// IsNullOrWhiteSpace
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        #endregion

        #region 字符串格式化

        /// <summary>
        /// FormatWith
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, object arg0)
        {
            return String.Format(format, arg0);
        }

        /// <summary>
        /// FormatWith
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, object arg0, object arg1)
        {
            return String.Format(format, arg0, arg1);
        }

        /// <summary>
        /// FormatWith
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, object arg0, object arg1, object arg2)
        {
            return String.Format(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// FormatWith
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return String.Format(format, args);
        }

        /// <summary>
        /// FormatWith
        /// </summary>
        /// <param name="format"></param>
        /// <param name="provider"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            return String.Format(provider, format, args);
        }

        #endregion

        #region 串联字符串集合

        /// <summary>
        /// Join
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<String> source, string separator)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (separator == null)
            {
                throw new ArgumentNullException(nameof(separator));
            }

            var enumerable = source as string[] ?? source.ToArray();
            if (enumerable.IsNullOrEmpty()) return String.Empty;
            return String.Join(separator, enumerable);
        }

        /// <summary>
        /// Join
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> source, string separator, Func<T, String> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (separator == null)
            {
                throw new ArgumentNullException(nameof(separator));
            }
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var enumerable = source as T[] ?? source.ToArray();
            if (enumerable.IsNullOrEmpty()) return String.Empty;

            return String.Join(separator, enumerable.Select(selector));
        }

        #endregion

        #region 目录相关

        /// <summary>
        /// 确保目录为子目录
        /// </summary>
        /// <example>
        /// <para>~/目录名/ -> 目录名</para>
        /// <para>~/目录名/子目录名/ -> 目录名/子目录名</para>
        /// </example>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string EnsureSubFolder(this string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.StartsWith("~"))
                path = path.Substring(1, path.Length - 1);

            string[] pathNames = path.Split('/');
            path = String.Empty;

            foreach (string p in pathNames)
            {
                if (p != String.Empty)
                    path += p + "/";
            }
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return path;
        }

        /// <summary>
        /// 确保目录为根目录
        /// </summary>
        /// <example>
        /// <para>目录名/ -> ~/目录名</para>
        /// <para>/目录名/子目录名/ -> ~/目录名/子目录名</para>
        /// </example>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string EnsureVirtualDirectory(this string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!path.StartsWith("~/"))
            {
                if (!path.StartsWith("/"))
                    path = "/" + path;
                if (!path.StartsWith("~"))
                    path = "~" + path;
            }
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return path;
        }

        /// <summary>
        /// 判断目录是否为子目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns><c>true</c>是子目录；<c>false</c>不是子目录</returns>
        public static bool IsSubDirectory(this string path)
        {
            return !path.IsNullOrWhiteSpace() && SubDirectoryRegex.IsMatch(path);
        }

        /// <summary>
        /// 判断目录是否为虚拟目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns><c>true</c>是虚拟目录；<c>false</c>不是虚拟目录</returns>
        public static bool IsVirtualDirectory(this string path)
        {
            return !path.IsNullOrWhiteSpace() && VirtualDirectoryRegex.IsMatch(path);
        }

        #endregion

        /// <summary>
        /// 字符串重复
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="times">重复次数</param>
        /// <returns></returns>
        public static string Repeat(this string source, int times)
        {
            if (String.IsNullOrEmpty(source) || times <= 0)
                return source;
            var sb = new StringBuilder();
            while (times > 0)
            {
                sb.Append(source);
                times--;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 简单过滤 SQL 语句
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static string SqlFilter(this string sqlString)
        {
            if (sqlString == null)
                return null;
            sqlString = sqlString.ToLower();
            string words = "and|exec|insert|select|delete|update|chr|mid|master|or|truncate|char|declare|join";
            foreach (string i in words.Split('|'))
            {
                if ((sqlString.IndexOf(i + " ", StringComparison.Ordinal) > -1) || (sqlString.IndexOf(" " + i, StringComparison.Ordinal) > -1))
                {
                    sqlString = sqlString.Replace(i, String.Empty);
                }
            }
            return sqlString;
        }

        /// <summary>
        /// 如果源对象为 null ，则返回 null ，否则返回其 ToString 方法返回值
        /// </summary>
        /// <param name="source">源对象</param>
        /// <returns>字符串</returns>
        public static string ToNullableString<T>(this T source) where T : class
        {
            return source?.ToString();
        }

        /// <summary>
        /// 如果源对象为 null ，则返回 String.Empty ，否则返回其 ToString 方法返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToEmptyableString<T>(this T source) where T : class
        {
            return source != null ? source.ToString() : String.Empty;
        }

        /// <summary>
        /// WithUrl
        /// </summary>
        /// <param name="source"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string WithUrl(this string source, string url)
        {
            if (source == null || url == null)
                return null;

            return $"<a href=\"{url}\">{source}</a>";
        }

        #region 拼音

        /// <summary>
        /// ConvertToPinYin
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertToPinYin(this string source)
        {
            var pinYin = "";
            foreach (var item in source)
            {
                if (ChineseChar.IsValidChar(item))
                {
                    var cc = new ChineseChar(item);
                    //PYstr += string.Join("", cc.Pinyins.ToArray());
                    pinYin += cc.Pinyins[0].Substring(0, cc.Pinyins[0].Length - 1).ToLowerInvariant();
                    //PYstr += cc.Pinyins[0].Substring(0, cc.Pinyins[0].Length - 1).Substring(0, 1).ToLower();
                }
                else
                {
                    pinYin += item.ToString();
                }
            }
            return pinYin;
        }

        /// <summary>
        /// ConvertToPinYinPY
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Tuple<string, string> ConvertToPinYinPY(this string source)
        {
            var pinYin = "";
            var py = "";
            foreach (char item in source)
            {
                if (ChineseChar.IsValidChar(item))
                {
                    var cc = new ChineseChar(item);
                    var pinYinString = cc.Pinyins[0].Substring(0, cc.Pinyins[0].Length - 1).ToLowerInvariant();
                    pinYin += pinYinString;
                    py += pinYinString.Substring(0, 1);
                }
                else
                {
                    var charString = item.ToString();
                    pinYin += charString;
                    py += charString;
                }
            }
            return new Tuple<string, string>(pinYin, py);
        }

        /// <summary>
        /// ConvertToPY
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertToPY(this string source)
        {
            string py = "";
            foreach (char item in source)
            {
                if (ChineseChar.IsValidChar(item))
                {
                    var cc = new ChineseChar(item);
                    py += cc.Pinyins[0].Substring(0, 1).ToLowerInvariant();
                }
                else
                {
                    var charString = item.ToString();
                    py += charString;
                }
            }
            return py;
        }

        #endregion
    }
}
