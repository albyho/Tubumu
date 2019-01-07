using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Tubumu.Modules.Admin.Entities;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Framework.Extensions.Object;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Repositories
{
    /// <summary>
    /// IRegionRepository
    /// </summary>
    public interface IRegionRepository
    {
        /// <summary>
        /// GetRegionInfoBaseListAsync
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<List<XM.RegionInfoBase>> GetRegionInfoBaseListAsync(int? parentId);

        /// <summary>
        /// GetRegionInfoBaseListAsync
        /// </summary>
        /// <returns></returns>
        Task<List<XM.RegionInfoBase>> GetRegionInfoBaseListAsync();
    }

    /// <summary>
    /// RegionRepository
    /// </summary>
    public class RegionRepository : IRegionRepository
    {
        private readonly TubumuContext _tubumuContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tubumuContext"></param>
        public RegionRepository(TubumuContext tubumuContext)
        {
            _tubumuContext = tubumuContext;
        }

        /// <summary>
        /// GetRegionInfoBaseListAsync
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<List<XM.RegionInfoBase>> GetRegionInfoBaseListAsync(int? parentId)
        {
            var list = await _tubumuContext.Region.AsNoTracking().
                Where(m => m.ParentId == parentId).
                OrderBy(m => m.DisplayOrder).
                ProjectTo<XM.RegionInfoBase>().
                ToListAsync();
            return list;
        }

        /// <summary>
        /// GetRegionInfoBaseListAsync
        /// </summary>
        /// <returns></returns>
        public async Task<List<XM.RegionInfoBase>> GetRegionInfoBaseListAsync()
        {
            var list = await _tubumuContext.Region.AsNoTracking().
                OrderBy(m => m.RegionId).
                ThenBy(m => m.DisplayOrder).
                ProjectTo<XM.RegionInfoBase>().
                ToListAsync();
            return list;
        }
    }
}
