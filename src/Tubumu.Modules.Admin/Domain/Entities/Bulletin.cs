using System;

namespace Tubumu.Modules.Admin.Domain.Entities
{
    public partial class Bulletin
    {
        public Guid BulletinId { get; set; }
        public bool IsShow { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
