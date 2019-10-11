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
            CreateMap<Bulletin, XM.Bulletin>().ReverseMap();
            CreateMap<Bulletin, XM.Input.BulletinInput>().ReverseMap();
            CreateMap<XM.Bulletin, XM.Input.BulletinInput>();

            CreateMap<XM.Input.UserActionLogInput, UserActionLog>();

            CreateMap<Permission, XM.Permission>().ReverseMap();

            CreateMap<Region, XM.RegionInfo>();
        }
    }
}
