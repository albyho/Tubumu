using System;
using System.ComponentModel.DataAnnotations;
using Tubumu.Core.Extensions;

namespace Tubumu.DataAnnotations
{
    /// <summary>
    /// 身份证号码
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ChineseIdCardNumberAttribute : ValidationAttribute
    {
        private const string AddressCode = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var stringValue = value.ToString();
            if (stringValue.IsNullOrWhiteSpace()) return true;
            return CheckIDCard(stringValue);
        }

        private static bool CheckIDCard(string id)
        {
            return id?.Length == 18 ? CheckIDCard18(id) : id?.Length == 15 ? CheckIDCard15(id) : false;
        }

        private static bool CheckIDCard18(string id)
        {
            if (!long.TryParse(id.Remove(17), out var n) || n < Math.Pow(10, 16) || !long.TryParse(id.Replace('x', '0').Replace('X', '0'), out _))
            {
                return false;//数字验证
            }

            if (AddressCode.IndexOf(id.Remove(2)) == -1)
            {
                return false;//省份验证

            }
            var birth = id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            if (!DateTime.TryParse(birth, out _))
            {
                return false;//生日验证
            }

            var varifyCodes = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            var wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            var ai = id.Remove(17).ToCharArray();
            var sum = 0;
            for (var i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }

            Math.DivRem(sum, 11, out int y);
            if (varifyCodes[y] != id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }

            return true;//符合GB11643-1999标准
        }

        private static bool CheckIDCard15(string id)
        {
            if (!long.TryParse(id, out var n) || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }

            if (AddressCode.IndexOf(id.Remove(2)) == -1)
            {
                return false;//省份验证
            }

            var birth = id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            if (!DateTime.TryParse(birth, out var _))
            {
                return false;//生日验证
            }

            return true;//符合15位身份证标准
        }
    }
}
