using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Modules.Admin.Frontend;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.ModuleMenus;
using Tubumu.Modules.Admin.Services;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;
using Tubumu.Modules.Framework.Swagger;

namespace Tubumu.Modules.Admin.Controllers
{
    /// <summary>
    /// BasicData Controller
    /// </summary>
    /// <remarks>基础数据</remarks>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class BasicDataController : ControllerBase
    {
        private readonly IRegionService _regionService;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="regionService"></param>
        public BasicDataController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        /// <summary>
        /// 获取区域信息列表
        /// </summary>
        /// <param name="parentId">父节点 Id 。 不为 null ,可获取该节点的子节点；否则获取第一级节点列表</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiListResult<RegionInfoBase>> GetRegionBaseInfoList(int? parentId)
        {
            var returnResult = new ApiListResult<RegionInfoBase>();
            var list = await _regionService.GetRegionInfoBaseListAsync(parentId);
            returnResult.List = list;
            returnResult.Code = 200;
            returnResult.Message = "获取成功";
            return returnResult;
        }
    }
}
