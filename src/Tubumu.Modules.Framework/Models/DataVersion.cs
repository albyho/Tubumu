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
        [ProtoMember(1)]
        public int TypeId { get; set; }

        [ProtoMember(2)]
        public int Version { get; set; }

        [ProtoMember(3)]
        public DateTime UpdateTime { get; set; }
    }

    public class DataVersionTypeIdInput
    {
        [Required(ErrorMessage = "请输入数据类型")]
        [Range(1, Int32.MaxValue, ErrorMessage = "请输入合法的数据类型")]
        public int TypeId { get; set; }
    }
}
