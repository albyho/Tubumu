using System.Collections.Generic;

namespace Tubumu.Modules.Admin.Domain.Entities
{
    public partial class Region
    {
        public Region()
        {
            InverseParent = new HashSet<Region>();
        }

        public int RegionId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Initial { get; set; }
        public string Initials { get; set; }
        public string Pinyin { get; set; }
        public string Extra { get; set; }
        public string Suffix { get; set; }
        public string ZipCode { get; set; }
        public string RegionCode { get; set; }
        public int DisplayOrder { get; set; }
        public bool HasChildren { get; set; }

        public virtual Region Parent { get; set; }
        public virtual ICollection<Region> InverseParent { get; set; }
    }
}
