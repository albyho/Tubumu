using System;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// 字母开头，由字母、数字、连词符或下滑线组成的字符串
    /// </summary>
    public class SlugAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SlugAttribute() :
            base(@"^[a-zA-Z][a-zA-Z0-9-_]*$")
        { }
    }

    /// <summary>
    /// 字母开头，由字母、数字、连词符或下滑线组成的字符串；或者1开头的11位数字的手机号码
    /// </summary>
    public class SlugWithMobileAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SlugWithMobileAttribute() :
            base(@"^(([a-zA-Z][a-zA-Z0-9-_]*)|(1\d{10}))$")
        { }
    }

    /// <summary>
    /// 字母开头，由字母、数字、连词符或下滑线组成的字符串；正整数
    /// </summary>
    public class SlugWithIntAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SlugWithIntAttribute() :
            base(@"^(([a-zA-Z][a-zA-Z0-9-_]*)|(\d+))$")
        { }
    }

    /// <summary>
    /// 字母或中文开头，由字母、数字、连词符或下滑线组成的字符串
    /// </summary>
    public class SlugWithChineseAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SlugWithChineseAttribute() :
            base(@"^[a-zA-Z\u4E00-\u9FA5\uF900-\uFA2D][a-zA-Z0-9-_\u4E00-\u9FA5\uF900-\uFA2D]*$")
        { }
    }

    /// <summary>
    /// prefix 开头，由字母、数字、连词符或下滑线组成的字符串
    /// </summary>
    public class SlugWithPrefixAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SlugWithPrefixAttribute(string prefix) :
            base(@"^" + prefix + @"[a-zA-Z0-9-_]*$")
        { }
    }

    /// <summary>
    /// 字母开头，由字母、数字、连词符或下滑线组成的字符串；或者1开头的11位数字的手机号码；或邮箱地址
    /// </summary>
    public class SlugWithMobileEmailAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SlugWithMobileEmailAttribute() :
            base(@"^(([a-zA-Z][a-zA-Z0-9-_]*)|(1\d{10}))|([\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?)$")
        { }
    }
}
