using System;

namespace Tubumu.Core.Json
{
    /// <summary>
    /// Null 值序列化和反序列化
    /// 当对象的名为 propertyName 的 Guid 属性的值与 equalValue 相等时，序列化为 null
    /// </summary>
    public class NullValueJsonConverterGuid : NullValueJsonConverter<Guid>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="equaValue"></param>
        public NullValueJsonConverterGuid(string propertyName, string equaValue) : base(propertyName, new Guid(equaValue))
        {
        }
    }
}
