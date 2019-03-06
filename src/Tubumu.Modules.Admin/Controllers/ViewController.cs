using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tubumu.Modules.Admin.Frontend;
using Tubumu.Modules.Framework.Extensions;

namespace Tubumu.Modules.Admin.Controllers
{
    /// <summary>
    /// 视图 Controller
    /// </summary>
    public class ViewController : Controller
    {
        private readonly string ProductionCoreTemplate = "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
            //"  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no\">" +
            "  <meta charset=\"utf-8\">" +
            "  <title>{0}</title >" +
            "  <link href=\"{1}/staticcore/css/modules/{2}.css\" rel=\"stylesheet\">" +
            "</head>" +
            "<body>" +
            "<div id=\"app\"></div>{3}" +
            "<script type = \"text/javascript\" src=\"{1}/staticcore/js/manifest.js\"></script>" +
            "<script type = \"text/javascript\" src=\"{1}/staticcore/js/vendor.js\"></script>" +
            "<script type = \"text/javascript\" src=\"{1}/staticcore/js/modules/{2}.js\"></script>" +
            "</body>" +
            "</html>"
            ;
        private readonly string DevelopmentCoreTemplate = "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
            //"  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no\">" +
            "  <meta charset=\"utf-8\">" +
            "  <title>{0}</title >" +
            "</head>" +
            "<body>" +
            "<div id=\"app\"></div>{3}" +
            "<script type = \"text/javascript\" src=\"{1}/modules/{2}.js\"></script>" +
            "</body>" +
            "</html>"
            ;
        private readonly string ProductionTemplate = "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
            //"  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no\">" +
            "  <meta charset=\"utf-8\">" +
            "  <title>{0}</title >" +
            "  <link href=\"{1}/static/css/modules/{2}.css\" rel=\"stylesheet\">" +
            "</head>" +
            "<body>" +
            "<div id=\"app\"></div>{3}" +
            "<script type = \"text/javascript\" src=\"{1}/static/js/manifest.js\"></script>" +
            "<script type = \"text/javascript\" src=\"{1}/static/js/vendor.js\"></script>" +
            "<script type = \"text/javascript\" src=\"{1}/static/js/modules/{2}.js\"></script>" +
            "</body>" +
            "</html>"
            ;
        private readonly string DevelopmentTemplate = "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
            //"  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no\">" +
            "  <meta charset=\"utf-8\">" +
            "  <title>{0}</title >" +
            "</head>" +
            "<body>" +
            "<div id=\"app\"></div>{3}" +
            "<script type = \"text/javascript\" src=\"{1}/modules/{2}.js\"></script>" +
            "</body>" +
            "</html>"
            ;

        private readonly FrontendSettings _frontendSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="frontendSettingsOptions"></param>
        public ViewController(IOptions<FrontendSettings> frontendSettingsOptions)
        {
            _frontendSettings = frontendSettingsOptions.Value;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            HttpContext.Session.SetString("bbb", "123");
            return Content(GenerateHtml(new ViewInput
            {
                IsCore = true,
                Name = "login",
                Title = "系统登录"
            }), "text/html");
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return Content(GenerateHtml(new ViewInput
            {
                IsCore = true,
                Name = "index",
                Title = "系统管理",
                Components = "signalr",
            }), "text/html");
        }

        /// <summary>
        /// 视图
        /// </summary>
        /// <param name="viewInput"></param>
        /// <returns></returns>
        public ActionResult View(ViewInput viewInput)
        {
            return Content(GenerateHtml(viewInput), "text/html");
        }

        private string GenerateHtml(ViewInput viewInput)
        {
            bool isDevelopment;
            string developmentHost;
            string productionHost;
            if (viewInput.IsCore)
            {
                isDevelopment = _frontendSettings.CoreEnvironment.IsDevelopment;
                developmentHost = _frontendSettings.CoreEnvironment.DevelopmentHost;
                productionHost = _frontendSettings.CoreEnvironment.ProductionHost;
            }
            else
            {
                isDevelopment = _frontendSettings.ProjectEnvironment.IsDevelopment;
                developmentHost = _frontendSettings.ProjectEnvironment.DevelopmentHost;
                productionHost = _frontendSettings.ProjectEnvironment.ProductionHost;
            }

            var componentScripts = new StringBuilder();
            if (!viewInput.Components.IsNullOrWhiteSpace())
            {
                var components = viewInput.Components.Split(',', ';');
                foreach (var compent in components)
                {
                    switch (compent.ToLower())
                    {
                        case "ckfinder":
                            // ckfinder 需要获取语言包，涉及跨域，故总是从服务器取脚本
                            componentScripts.AppendFormat("<script type = \"text/javascript\" src=\"{0}/ckfinderscripts/ckfinder.js\"></script>", productionHost);
                            break;
                        case "signalr":
                            componentScripts.AppendFormat("<script type = \"text/javascript\" src=\"{0}/lib/signalr/dist/browser/signalr.js\"></script>", productionHost);
                            break;
                        default:
                            break;
                    }
                }
            }

            // 模板有4种
            string template;
            if (viewInput.IsCore)
            {
                template = isDevelopment ? DevelopmentCoreTemplate : ProductionCoreTemplate;
            }
            else
            {
                template = isDevelopment ? DevelopmentTemplate : ProductionTemplate;
            }

            var html = String.Format(template, viewInput.Title, isDevelopment ? developmentHost : productionHost, viewInput.Name, componentScripts);
            return html;
        }

        /// <summary>
        /// ViewInput
        /// </summary>
        public class ViewInput
        {
            /// <summary>
            /// 是否是核心模块的视图
            /// </summary>
            public bool IsCore { get; set; } // IsCore：让核心模块的前端和项目模块的前端能分开开发

            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 使用的组件
            /// </summary>
            public string Components { get; set; } // 以半角逗号或分号分隔
        }
    }
}