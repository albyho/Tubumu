using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Tubumu.Modules.Admin.Domain.Entities;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Domain.Services
{
    /// <summary>
    /// IRegionManager
    /// </summary>
    public interface IRegionManager
    {
        /// <summary>
        /// GetRegionInfoBaseListAsync
        /// </summary>
        /// <returns></returns>
        Task<List<XM.RegionInfo>> GetRegionInfoListAsync();

        /// <summary>
        /// GetRegionInfoBaseListAsync
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<List<XM.RegionInfo>> GetRegionInfoListAsync(int? parentId);
    }

    /// <summary>
    /// RegionManager
    /// </summary>
    public class RegionManager : IRegionManager
    {
        private readonly TubumuContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public RegionManager(TubumuContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// GetRegionInfoBaseListAsync
        /// </summary>
        /// <returns></returns>
        public async Task<List<XM.RegionInfo>> GetRegionInfoListAsync()
        {
            var list = await _context.Region.AsNoTracking().
                OrderBy(m => m.DisplayOrder).
                ProjectTo<XM.RegionInfo>(_mapper.ConfigurationProvider).
                ToListAsync();
            return list;
        }

        /// <summary>
        /// GetRegionInfoBaseListAsync
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<List<XM.RegionInfo>> GetRegionInfoListAsync(int? parentId)
        {
            var list = await _context.Region.AsNoTracking().
                Where(m => m.ParentId == parentId).
                OrderBy(m => m.DisplayOrder).
                ProjectTo<XM.RegionInfo>(_mapper.ConfigurationProvider).
                ToListAsync();
            return list;
        }
    }
}
