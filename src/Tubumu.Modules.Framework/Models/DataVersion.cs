using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ProtoBuf;

namespace Tubumu.Modules.Framework.Models
{
    [ProtoContract]
    public class DataVersion
    {
        /// <summary>
        /// 类型 Id
        /// </summary>
        [ProtoMember(1)]
        public int TypeId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [ProtoMember(2)]
        public int Version { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [ProtoMember(3)]
        public DateTime UpdateTime { get; set; }
    }

    public class DataVersionTypeIdInput
    {
        /// <summary>
        /// 数据类型 Id
        /// </summary>
        [Required(ErrorMessage = "请输入数据类型")]
        [Range(1, Int32.MaxValue, ErrorMessage = "请输入合法的数据类型")]
        public int TypeId { get; set; }
    }
}
