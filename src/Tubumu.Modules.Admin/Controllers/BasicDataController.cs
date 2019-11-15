using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Modules.Admin.Application.Services;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Api;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Framework.Application.Services;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Controllers
{
    /// <summary>
    /// BasicData Controller (基础数据)
    /// </summary>
    [Route("Api/[controller]/[action]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class BasicDataController : ControllerBase
    {
        private readonly IRegionService _regionService;
        private readonly IDataVersionService _dataVersionService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="regionService"></param>
        /// <param name="dataVersionService"></param>
        public BasicDataController(IRegionService regionService, IDataVersionService dataVersionService)
        {
            _regionService = regionService;
            _dataVersionService = dataVersionService;
        }

        /// <summary>
        /// 获取区域信息列表
        /// </summary>
        /// <param name="parentIdInput">父节点 Id 。 不为 null ,可获取该节点的子节点；否则获取第一级节点列表</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResultData<List<RegionInfo>>> GetRegionInfoList(ParentIdNullableInput parentIdInput)
        {
            var returnResult = new ApiResultData<List<RegionInfo>>();
            var list = await _regionService.GetRegionInfoListAsync(parentIdInput.ParentId);
            returnResult.Data = list;
            returnResult.Code = 200;
            returnResult.Message = "获取成功";
            return returnResult;
        }

        /// <summary>
        /// 获取区域信息列表 (全部)
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
        /// 获取区域信息树 (全部)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResultData<List<RegionTreeNode>>> GetRegionTree()
        {
            var returnResult = new ApiResultData<List<RegionTreeNode>>();
            var tree = await _regionService.GetRegiontTreeAsync();
            returnResult.Data = tree;
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
        public async Task<ApiResultData<List<RegionTreeNode>>> GetRegiontParentTreeByParentIdPath(ParentIdPathInput parentIdPath)
        {
            var returnResult = new ApiResultData<List<RegionTreeNode>>();
            var tree = await _regionService.GetRegiontParentTreeAsync(parentIdPath.ParentIdPath);
            returnResult.Data = tree;
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
        public async Task<ApiResultData<List<RegionTreeNode>>> GetRegiontParentTree(RegionIdInput regionIdInput)
        {
            var returnResult = new ApiResultData<List<RegionTreeNode>>();
            var tree = await _regionService.GetRegiontParentTreeAsync(regionIdInput.RegionId);
            returnResult.Data = tree;
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
        public async Task<ApiResultData<List<RegionTreeNode>>> GetRegiontTreeChildNodeList(ParentIdNullableInput parentIdInput)
        {
            var returnResult = new ApiResultData<List<RegionTreeNode>>();
            var list = await _regionService.GetRegiontTreeChildNodeListAsync(parentIdInput.ParentId);
            returnResult.Data = list;
            returnResult.Code = 200;
            returnResult.Message = "获取成功";
            return returnResult;
        }

        /// <summary>
        /// 获取数据版本
        /// </summary>
        /// <param name="dataVersionTypeIdInput"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResultData<DataVersion>> GetDataVersion(DataVersionTypeIdInput dataVersionTypeIdInput)
        {
            var returnResult = new ApiResultData<DataVersion>();
            var item = await _dataVersionService.GetGlobalAsync(dataVersionTypeIdInput.TypeId);
            returnResult.Data = item;
            returnResult.Code = 200;
            returnResult.Message = "获取成功";
            return returnResult;
        }

        /// <summary>
        /// 获取数据版本
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResultData<IEnumerable<DataVersion>>> GetDataVersionList()
        {
            var returnResult = new ApiResultData<IEnumerable<DataVersion>>();
            var list = await _dataVersionService.GetAllAsync();
            returnResult.Data = list;
            returnResult.Code = 200;
            returnResult.Message = "获取成功";
            return returnResult;
        }
    }
}
