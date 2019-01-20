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
        Task<List<XM.RegionInfo>> GetRegionInfoListAsync();

        /// <summary>
        /// GetRegionInfoBaseListAsync
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<List<XM.RegionInfo>> GetRegionInfoListAsync(int? parentId);
    }

    /// <summary>
    /// RegionRepository
    /// </summary>
    public class RegionRepository : IRegionRepository
    {
        private readonly TubumuContext _context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public RegionRepository(TubumuContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GetRegionInfoBaseListAsync
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<List<XM.RegionInfo>> GetRegionInfoListAsync()
        {
             var list = await _context.Region.AsNoTracking().
                 OrderBy(m => m.DisplayOrder).
                 ProjectTo<XM.RegionInfo>().
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
                ProjectTo<XM.RegionInfo>().
                ToListAsync();
            return list;
        }
    }
}
