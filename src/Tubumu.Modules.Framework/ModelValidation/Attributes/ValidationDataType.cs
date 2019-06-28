namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// ValidationDataType
    /// </summary>
    public enum ValidationDataType : byte
    {
        /// <summary>
        /// 字符串
        /// </summary>
        String,

        /// <summary>
        /// 整型
        /// </summary>
        Integer,

        /// <summary>
        /// 双精度幅度点数
        /// </summary>
        Double,

        /// <summary>
        /// 日期
        /// </summary>
        Date,

        /// <summary>
        /// 时间
        /// </summary>
        Time,

        /// <summary>
        /// Decimal
        /// </summary>
        Currency,
    }
}
