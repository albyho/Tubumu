using AutoMapper;
using Tubumu.Modules.Admin.Entities;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Mappings
{
    /// <summary>
    /// MappingProfile
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Bulletin, XM.Bulletin>();
            CreateMap<XM.Bulletin, Bulletin>();
            CreateMap<XM.Bulletin, XM.Input.BulletinInput>();

            CreateMap<Permission, XM.Permission>();
            CreateMap<XM.Permission, Permission>();

            CreateMap<Region, XM.RegionInfo>();
        }
    }
}
