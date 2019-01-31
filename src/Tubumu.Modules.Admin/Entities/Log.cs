using System;
using System.Collections.Generic;

namespace Tubumu.Modules.Admin.Entities
{
    public partial class Log
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public string Ip { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
