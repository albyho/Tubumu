using AutoMapper;
using Tubumu.Modules.Admin.Domain.Entities;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Mappings
{
    /// <summary>
    /// MappingProfile
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Bulletin, XM.Bulletin>();
            CreateMap<XM.Bulletin, Bulletin>();
            CreateMap<Bulletin, XM.Input.BulletinInput>();
            CreateMap<XM.Input.BulletinInput, Bulletin>();

            CreateMap<XM.Input.UserActionLogInput, UserActionLog>();

            CreateMap<Permission, XM.Permission>();
            CreateMap<XM.Permission, Permission>();

            CreateMap<Region, XM.RegionInfo>();
        }
    }
}
