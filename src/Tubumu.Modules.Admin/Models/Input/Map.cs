using System.ComponentModel;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 经维度 Input
    /// </summary>
    public class CoordinateInput
    {
        /// <summary>
        /// 经度
        /// </summary>
        [DisplayName("经度")]
        public double Longitude { get; set; }

        /// <summary>
        /// 维度
        /// </summary>
        [DisplayName("维度")]
        public double Latitude { get; set; }
    }

    public class NullableCoordinate
    {
        /// <summary>
        /// 经度
        /// </summary>
        [DisplayName("经度")]
        public double? Longitude { get; set; }

        /// <summary>
        /// 维度
        /// </summary>
        [DisplayName("维度")]
        public double? Latitude { get; set; }
    }
}
