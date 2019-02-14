webpackJsonp([9],{"+nZf":function(t,e,n){"use strict";var a=n("OnD8"),r=n("629P"),o=function(t){n("+rid")},i=n("VU/8")(a.a,r.a,!1,o,null,null);e.a=i.exports},"+rid":function(t,e){},"/rEA":function(t,e,n){"use strict";(function(t){function a(t,e,n){this.name="ApiError",this.message=t||"Default Message",this.errorType=e||g.Default,this.innerError=n,this.stack=(new Error).stack}var r=n("//Fk"),o=n.n(r),i=n("mvHQ"),s=n.n(i),l=n("OvRC"),u=n.n(l),d=n("mtWM"),c=n.n(d),f=n("mw3O"),p=n.n(f),m=n("bzuE"),g={Default:"default",Sysetem:"sysetem"};(a.prototype=u()(Error.prototype)).constructor=a;var h=c.a.create({baseURL:m.a,timeout:2e4,responseType:"json",withCredentials:!0});h.interceptors.request.use(function(t){return"post"===t.method||"put"===t.method||"patch"===t.method?(t.headers={"Content-Type":"application/json; charset=UTF-8"},t.data=s()(t.data)):"delete"!==t.method&&"get"!==t.method&&"head"!==t.method||(t.paramsSerializer=function(t){return p.a.stringify(t,{arrayFormat:"indices"})}),localStorage.token&&(t.headers.Authorization="Bearer "+localStorage.token),t},function(t){return t}),h.interceptors.response.use(function(e){if(-1===e.headers["content-type"].indexOf("json"))return e;var n=void 0;if("arraybuffer"===e.request.responseType&&"[object ArrayBuffer]"===e.data.toString()){var r=t.from(e.data).toString("utf8");console.log(r),n=JSON.parse(r)}else n=e.data;if(n){if(n.token&&(localStorage.token=n.token),n.refreshToken&&(localStorage.refreshToken=n.refreshToken),n.url)return void(top.location=n.url);if(200!==n.code)return console.log(n),o.a.reject(new a(n.message))}return e},function(t){if(console.log(t.response.headers),401===t.response.status){if(-1!==t.response.headers["content-type"].indexOf("json")&&t.response.data.url)return void(top.location=t.response.data.url);if(t.response.headers["token-expired"]&&localStorage.token&&localStorage.refreshToken){console.log("Refresh Token");var e={token:localStorage.token,refreshToken:localStorage.refreshToken};return localStorage.removeItem("token"),localStorage.removeItem("refreshToken"),h.post("/admin/refreshToken",e),o.a.reject(new a("登录超时，自动登录中......",g.Sysetem,t))}}return o.a.reject(new a(t.message,g.Sysetem,t))}),e.a={install:function(t){arguments.length>1&&void 0!==arguments[1]&&arguments[1];t.httpClient=h,t.prototype.$httpClient=h}}}).call(e,n("EuP9").Buffer)},"629P":function(t,e,n){"use strict";var a={render:function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("el-container",{directives:[{name:"loading",rawName:"v-loading.fullscreen.lock",value:t.isLoading,expression:"isLoading",modifiers:{fullscreen:!0,lock:!0}}]},[n("el-header",{staticClass:"header"},[n("el-breadcrumb",{staticClass:"breadcrumb",attrs:{"separator-class":"el-icon-arrow-right"}},[n("el-breadcrumb-item",[t._v("系统管理")]),t._v(" "),n("el-breadcrumb-item",[t._v("模块元数据")])],1)],1),t._v(" "),n("el-main",{staticClass:"main"},[n("el-row",[n("el-button-group",[n("el-button",{attrs:{type:"primary",icon:"el-icon-refresh"},on:{click:t.handleExtractModuleMetaDatas}},[t._v("提取模块元数据")]),t._v(" "),n("el-button",{attrs:{type:"primary",icon:"el-icon-time"},on:{click:t.handleClearModulePermissions}},[t._v("清理模块权限")])],1)],1),t._v(" "),n("el-row",[n("el-tabs",{attrs:{type:"card"},model:{value:t.activeTabName,callback:function(e){t.activeTabName=e},expression:"activeTabName"}},[n("el-tab-pane",{attrs:{label:"权限",name:"first"}},[n("el-table",{staticStyle:{width:"100%"},attrs:{data:t.permissionList,"empty-text":t.emptyText}},[n("el-table-column",{attrs:{prop:"displayOrder",label:"#",width:"40"}}),t._v(" "),n("el-table-column",{attrs:{prop:"moduleName",label:"模块",width:"360"}}),t._v(" "),n("el-table-column",{attrs:{prop:"name",label:"名称"}})],1)],1),t._v(" "),n("el-tab-pane",{attrs:{label:"角色",name:"second"}},[n("el-table",{staticStyle:{width:"100%"},attrs:{data:t.roleList,"empty-text":t.emptyText}},[n("el-table-column",{attrs:{prop:"name",label:"名称"}})],1)],1),t._v(" "),n("el-tab-pane",{attrs:{label:"分组",name:"third"}},[n("el-tree",{ref:"tree",attrs:{data:t.groupTreeData,props:t.groupTreeDefaultProps,"empty-text":t.emptyText,"node-key":"id","default-expand-all":!0}})],1)],1)],1)],1)],1)},staticRenderFns:[]};e.a=a},"68aa":function(t,e,n){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var a=n("tvR6"),r=(n.n(a),n("qBF2")),o=n.n(r),i=n("7+uW"),s=n("/rEA"),l=n("uN2V"),u=(n.n(l),n("+nZf"));i.default.config.productionTip=!1,i.default.use(s.a),i.default.use(o.a,{size:"mini"}),new i.default({el:"#app",render:function(t){return t(u.a)}})},OnD8:function(t,e,n){"use strict";var a=n("VsUZ");e.a={data:function(){return{isLoading:!1,activeTabName:"first",permissionList:null,roleList:null,groupTreeData:[],groupTreeDefaultProps:{children:"children",label:"name",value:"id"}}},mounted:function(){this.getModuleMetaDatas()},computed:{emptyText:function(){return this.isLoading?"加载中...":"暂无数据"}},methods:{getModuleMetaDatas:function(){var t=this;this.isLoading=!0,a.a.getModuleMetaDatas().then(function(e){t.isLoading=!1,t.permissionList=e.data.item.permissions,t.roleList=e.data.item.roles,t.groupTreeData=e.data.item.groups},function(e){t.isLoading=!1,t.showErrorMessage(e.message)})},handleExtractModuleMetaDatas:function(){var t=this;this.isLoading=!0,a.a.extractModuleMetaDatas().then(function(e){t.isLoading=!1,t.getModuleMetaDatas(),t.$message({message:e.data.message,type:"success"})},function(e){t.isLoading=!1,t.showErrorMessage(e.message)})},handleClearModulePermissions:function(){var t=this;this.isLoading=!0,a.a.clearModulePermissions().then(function(e){t.isLoading=!1,t.getModuleMetaDatas(),t.$message({message:e.data.message,type:"success"})},function(e){t.isLoading=!1,t.showErrorMessage(e.message)})},showErrorMessage:function(t){this.$message({message:t,type:"error"})}}}},VsUZ:function(t,e,n){"use strict";var a=n("7+uW");e.a={login:function(t){return a.default.httpClient.post("/admin/login",t)},refreshToken:function(t){return a.default.httpClient.post("/admin/refreshToken",t)},logout:function(){return a.default.httpClient.post("/admin/logout")},getProfile:function(){return a.default.httpClient.get("/admin/getProfile")},changeProfile:function(t){return a.default.httpClient.post("/admin/changeProfile",t)},changePassword:function(t){return a.default.httpClient.post("/admin/changePassword",t)},getMenus:function(){return a.default.httpClient.get("/admin/getMenus")},getBulletin:function(){return a.default.httpClient.get("/admin/getBulletin")},editBulletin:function(t){return a.default.httpClient.post("/admin/editBulletin",t)},getModuleMetaDatas:function(){return a.default.httpClient.get("/admin/getModuleMetaDatas")},extractModuleMetaDatas:function(){return a.default.httpClient.get("/admin/extractModuleMetaDatas")},clearModulePermissions:function(){return a.default.httpClient.get("/admin/clearModulePermissions")},getRoleList:function(){return a.default.httpClient.get("/admin/getRoleList")},addRole:function(t){return a.default.httpClient.post("/admin/addRole",t)},editRole:function(t){return a.default.httpClient.post("/admin/editRole",t)},removeRole:function(t){return a.default.httpClient.post("/admin/removeRole",t)},moveRole:function(t){return a.default.httpClient.post("/admin/moveRole",t)},saveRoleName:function(t){return a.default.httpClient.post("/admin/saveRoleName",t)},getGroupTree:function(){return a.default.httpClient.get("/admin/getGroupTree")},addGroup:function(t){return a.default.httpClient.post("/admin/addGroup",t)},editGroup:function(t){return a.default.httpClient.post("/admin/editGroup",t)},removeGroup:function(t){return a.default.httpClient.post("/admin/removeGroup",t)},moveGroup:function(t){return a.default.httpClient.post("/admin/moveGroup",t)},getUserPage:function(t){return a.default.httpClient.post("/admin/getUserPage",t)},addUser:function(t){return a.default.httpClient.post("/admin/addUser",t)},editUser:function(t){return a.default.httpClient.post("/admin/editUser",t)},removeUser:function(t){return a.default.httpClient.post("/admin/removeUser",t)},getUserStatuList:function(){return a.default.httpClient.get("/admin/getUserStatuList")},getNotificationsForManager:function(t){return a.default.httpClient.post("/admin/getNotificationsForManager",t)},addNotification:function(t){return a.default.httpClient.post("/admin/addNotification",t)},editNotification:function(t){return a.default.httpClient.post("/admin/editNotification",t)},removeNotification:function(t){return a.default.httpClient.post("/admin/removeNotification",t)},getNotifications:function(t){return a.default.httpClient.post("/admin/getNotifications",t)},readNotifications:function(t){return a.default.httpClient.post("/admin/readNotifications",t)},deleteNotifications:function(t){return a.default.httpClient.post("/admin/deleteNotifications",t)},getNewestNotification:function(t){return a.default.httpClient.post("/admin/getNewestNotification",t)},getGroupList:function(){return a.default.httpClient.get("/admin/getGroupList")},getRoleBaseList:function(){return a.default.httpClient.get("/admin/getRoleBaseList")},getPermissionTree:function(){return a.default.httpClient.get("/admin/getPermissionTree")},callDirectly:function(t){return a.default.httpClient.get(t)},download:function(t,e){return a.default.httpClient.post(t,e,{responseType:"arraybuffer"})}}},bzuE:function(t,e,n){"use strict";n.d(e,"a",function(){return a}),n.d(e,"b",function(){return r}),n.d(e,"c",function(){return o});var a="/api",r="",o=""},tvR6:function(t,e){},uN2V:function(t,e){}},["68aa"]);
//# sourceMappingURL=modulemetadatas.js.map