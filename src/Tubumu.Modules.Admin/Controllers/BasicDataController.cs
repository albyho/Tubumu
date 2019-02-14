using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Modules.Admin.Frontend;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Api;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.ModuleMenus;
using Tubumu.Modules.Admin.Services;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Extensions.Object;
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
    public class BasicDataController : ControllerBase
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
        /// <param name="parentIdInput">父节点 Id 。 不为 null ,可获取该节点的子节点；否则获取第一级节点列表</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiListResult<RegionInfo>> GetRegionInfoList(ParentIdNullableInput parentIdInput)
        {
            var returnResult = new ApiListResult<RegionInfo>();
            var list = await _regionService.GetRegionInfoListAsync(parentIdInput.ParentId);
            returnResult.List = list;
            returnResult.Code = 200;
            returnResult.Message = "获取成功";
            return returnResult;
        }

        /// <summary>
        /// 获取区域信息列表（全部）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ContentResult> GetRegionInfoListFull()
        {
            // 出于效率上的考虑，直接返回字符串；而不是从缓存中反序列化后再序列化
            var sb = new StringBuilder();
            sb.Append("{\"list\":");
            var list = await _regionService.GetRegionInfoListJsonAsync();
            sb.Append(list);
            sb.Append(",\"code\":200,\"message\":\"获取成功\"}");
            return Content(sb.ToString(), "application/json; charset=utf-8");
        }

        /// <summary>
        /// 获取区域信息树（全部）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiTreeResult<RegionTreeNode>> GetRegionTree()
        {
            var returnResult = new ApiTreeResult<RegionTreeNode>();
            var tree = await _regionService.GetRegiontTreeAsync();
            returnResult.Tree = tree;
            returnResult.Code = 200;
            returnResult.Message = "获取成功";
            return returnResult;
        }

        /// <summary>
        /// 获取区域信息父级树 (含自身及兄弟节点)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiTreeResult<RegionTreeNode>> GetRegiontParentTreeByParentIdPath(ParentIdPathInput parentIdPath)
        {
            var returnResult = new ApiTreeResult<RegionTreeNode>();
            var tree = await _regionService.GetRegiontParentTreeAsync(parentIdPath.ParentIdPath);
            returnResult.Tree = tree;
            returnResult.Code = 200;
            returnResult.Message = "获取成功";
            return returnResult;
        }

        /// <summary>
        /// 获取区域信息父级树 (含自身及兄弟节点)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiTreeResult<RegionTreeNode>> GetRegiontParentTree(RegionIdInput regionIdInput)
        {
            var returnResult = new ApiTreeResult<RegionTreeNode>();
            var tree = await _regionService.GetRegiontParentTreeAsync(regionIdInput.RegionId);
            returnResult.Tree = tree;
            returnResult.Code = 200;
            returnResult.Message = "获取成功";
            return returnResult;
        }

        /// <summary>
        /// 获取区域树节点下的子节点信息列表
        /// </summary>
        /// <param name="parentIdInput"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiListResult<RegionTreeNode>> GetRegiontTreeChildNodeList(ParentIdNullableInput parentIdInput)
        {
            var returnResult = new ApiListResult<RegionTreeNode>();
            var list = await _regionService.GetRegiontTreeChildNodeListAsync(parentIdInput.ParentId);
            returnResult.List = list;
            returnResult.Code = 200;
            returnResult.Message = "获取成功";
            return returnResult;
        }
    }
}
