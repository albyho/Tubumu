webpackJsonp([6],{"/jSc":function(e,t){},"/rEA":function(e,t,r){"use strict";(function(e){function n(e,t,r){this.name="ApiError",this.message=e||"Default Message",this.errorType=t||p.Default,this.innerError=r,this.stack=(new Error).stack}var o=r("//Fk"),a=r.n(o),i=r("mvHQ"),s=r.n(i),l=r("OvRC"),u=r.n(l),d=r("mtWM"),c=r.n(d),f=r("mw3O"),m=r.n(f),g=r("bzuE"),p={Default:"default",Sysetem:"sysetem"};(n.prototype=u()(Error.prototype)).constructor=n;var h=c.a.create({baseURL:g.a,timeout:2e4,responseType:"json",withCredentials:!0});h.interceptors.request.use(function(e){return"post"===e.method||"put"===e.method||"patch"===e.method?(e.headers={"Content-Type":"application/json; charset=UTF-8"},e.data=s()(e.data)):"delete"!==e.method&&"get"!==e.method&&"head"!==e.method||(e.paramsSerializer=function(e){return m.a.stringify(e,{arrayFormat:"indices"})}),localStorage.token&&(e.headers.Authorization="Bearer "+localStorage.token),e},function(e){return e}),h.interceptors.response.use(function(t){if(-1===t.headers["content-type"].indexOf("json"))return t;var r=void 0;if("arraybuffer"===t.request.responseType&&"[object ArrayBuffer]"===t.data.toString()){var o=e.from(t.data).toString("utf8");console.log(o),r=JSON.parse(o)}else r=t.data;if(r){if(r.token&&(localStorage.token=r.token),r.refreshToken&&(localStorage.refreshToken=r.refreshToken),r.url)return void(top.location=r.url);if(200!==r.code)return console.log(r),a.a.reject(new n(r.message))}return t},function(e){if(console.log(e.response.headers),401===e.response.status){if(-1!==e.response.headers["content-type"].indexOf("json")&&e.response.data.url)return void(top.location=e.response.data.url);if(e.response.headers["token-expired"]&&localStorage.token&&localStorage.refreshToken){console.log("Refresh Token");var t={token:localStorage.token,refreshToken:localStorage.refreshToken};return localStorage.removeItem("token"),localStorage.removeItem("refreshToken"),h.post("/admin/refreshToken",t),a.a.reject(new n("登录超时，自动登录中......",p.Sysetem,e))}}return a.a.reject(new n(e.message,p.Sysetem,e))}),t.a={install:function(e){arguments.length>1&&void 0!==arguments[1]&&arguments[1];e.httpClient=h,e.prototype.$httpClient=h}}}).call(t,r("EuP9").Buffer)},IV7n:function(e,t,r){"use strict";Object.defineProperty(t,"__esModule",{value:!0});var n=r("tvR6"),o=(r.n(n),r("qBF2")),a=r.n(o),i=r("7+uW"),s=r("/rEA"),l=r("uN2V"),u=(r.n(l),r("tqSN"));i.default.config.productionTip=!1,i.default.use(s.a),i.default.use(a.a,{size:"mini"}),new i.default({el:"#app",render:function(e){return e(u.a)}})},JWjH:function(e,t,r){"use strict";var n={render:function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("el-container",{directives:[{name:"loading",rawName:"v-loading.fullscreen.lock",value:e.isLoading,expression:"isLoading",modifiers:{fullscreen:!0,lock:!0}}]},[r("el-header",{staticClass:"header"},[r("el-breadcrumb",{staticClass:"breadcrumb",attrs:{"separator-class":"el-icon-arrow-right"}},[r("el-breadcrumb-item",[e._v("我的资料")])],1)],1),e._v(" "),r("el-main",{staticClass:"main"},[r("el-tabs",{attrs:{type:"card"},model:{value:e.activeTabName,callback:function(t){e.activeTabName=t},expression:"activeTabName"}},[r("el-tab-pane",{attrs:{label:"修改资料",name:"first"}},[r("el-form",{ref:"changeProfileForm",attrs:{model:e.changeProfileForm,rules:e.changeProfileFormRules,"label-position":"right","label-width":"120px"}},[r("el-form-item",{attrs:{label:"昵称",prop:"displayName"}},[r("el-input",{ref:"displayName",attrs:{"auto-complete":"off",placeholder:"请输入昵称"},model:{value:e.changeProfileForm.displayName,callback:function(t){e.$set(e.changeProfileForm,"displayName","string"==typeof t?t.trim():t)},expression:"changeProfileForm.displayName"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"头像",prop:"headUrl"}},[r("el-input",{ref:"headUrl",attrs:{"auto-complete":"off",placeholder:"请输入头像 Url"},model:{value:e.changeProfileForm.headUrl,callback:function(t){e.$set(e.changeProfileForm,"headUrl","string"==typeof t?t.trim():t)},expression:"changeProfileForm.headUrl"}},[r("el-button",{attrs:{slot:"append",icon:"el-icon-search"},on:{click:e.handleChangeHeadUrlBrowser},slot:"append"})],1)],1),e._v(" "),r("el-form-item",{attrs:{label:"Logo",prop:"logoUrl"}},[r("el-input",{ref:"logoUrl",attrs:{"auto-complete":"off",placeholder:"请输入Logo Url"},model:{value:e.changeProfileForm.logoUrl,callback:function(t){e.$set(e.changeProfileForm,"logoUrl","string"==typeof t?t.trim():t)},expression:"changeProfileForm.logoUrl"}},[r("el-button",{attrs:{slot:"append",icon:"el-icon-search"},on:{click:e.handleChangeLogoUrlBrowser},slot:"append"})],1)],1),e._v(" "),r("el-form-item",[r("el-button",{attrs:{type:"primary"},on:{click:e.handleChangeProfile}},[e._v("修改资料")])],1)],1)],1),e._v(" "),r("el-tab-pane",{attrs:{label:"修改密码",name:"second"}},[r("el-form",{ref:"changePasswordForm",attrs:{model:e.changePasswordForm,rules:e.changePasswordFormRules,"label-position":"right","label-width":"120px"}},[r("el-form-item",{attrs:{label:"当前密码",prop:"currentPassword"}},[r("el-input",{ref:"currentPassword",attrs:{type:"password","auto-complete":"off",placeholder:"请输入当前密码"},model:{value:e.changePasswordForm.currentPassword,callback:function(t){e.$set(e.changePasswordForm,"currentPassword","string"==typeof t?t.trim():t)},expression:"changePasswordForm.currentPassword"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"新密码",prop:"newPassword"}},[r("el-input",{ref:"newPassword",attrs:{type:"password","auto-complete":"off",placeholder:"请输入新密码"},model:{value:e.changePasswordForm.newPassword,callback:function(t){e.$set(e.changePasswordForm,"newPassword","string"==typeof t?t.trim():t)},expression:"changePasswordForm.newPassword"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"确认新密码",prop:"newPasswordConfirm"}},[r("el-input",{ref:"newPasswordConfirm",attrs:{type:"password","auto-complete":"off",placeholder:"请确认新密码"},model:{value:e.changePasswordForm.newPasswordConfirm,callback:function(t){e.$set(e.changePasswordForm,"newPasswordConfirm","string"==typeof t?t.trim():t)},expression:"changePasswordForm.newPasswordConfirm"}})],1),e._v(" "),r("el-form-item",[r("el-button",{attrs:{type:"primary"},on:{click:e.handleChangePassword}},[e._v("修改密码")])],1)],1)],1)],1)],1)],1)},staticRenderFns:[]};t.a=n},VsUZ:function(e,t,r){"use strict";var n=r("7+uW");t.a={login:function(e){return n.default.httpClient.post("/admin/login",e)},refreshToken:function(e){return n.default.httpClient.post("/admin/refreshToken",e)},logout:function(){return n.default.httpClient.post("/admin/logout")},getProfile:function(){return n.default.httpClient.get("/admin/getProfile")},changeProfile:function(e){return n.default.httpClient.post("/admin/changeProfile",e)},changePassword:function(e){return n.default.httpClient.post("/admin/changePassword",e)},getMenus:function(){return n.default.httpClient.get("/admin/getMenus")},getBulletin:function(){return n.default.httpClient.get("/admin/getBulletin")},editBulletin:function(e){return n.default.httpClient.post("/admin/editBulletin",e)},getModuleMetaDatas:function(){return n.default.httpClient.get("/admin/getModuleMetaDatas")},extractModuleMetaDatas:function(){return n.default.httpClient.get("/admin/extractModuleMetaDatas")},clearModulePermissions:function(){return n.default.httpClient.get("/admin/clearModulePermissions")},getRoleList:function(){return n.default.httpClient.get("/admin/getRoleList")},addRole:function(e){return n.default.httpClient.post("/admin/addRole",e)},editRole:function(e){return n.default.httpClient.post("/admin/editRole",e)},removeRole:function(e){return n.default.httpClient.post("/admin/removeRole",e)},moveRole:function(e){return n.default.httpClient.post("/admin/moveRole",e)},saveRoleName:function(e){return n.default.httpClient.post("/admin/saveRoleName",e)},getGroupTree:function(){return n.default.httpClient.get("/admin/getGroupTree")},addGroup:function(e){return n.default.httpClient.post("/admin/addGroup",e)},editGroup:function(e){return n.default.httpClient.post("/admin/editGroup",e)},removeGroup:function(e){return n.default.httpClient.post("/admin/removeGroup",e)},moveGroup:function(e){return n.default.httpClient.post("/admin/moveGroup",e)},getUserPage:function(e){return n.default.httpClient.post("/admin/getUserPage",e)},addUser:function(e){return n.default.httpClient.post("/admin/addUser",e)},editUser:function(e){return n.default.httpClient.post("/admin/editUser",e)},removeUser:function(e){return n.default.httpClient.post("/admin/removeUser",e)},getUserStatuList:function(){return n.default.httpClient.get("/admin/getUserStatuList")},getNotificationsForManager:function(e){return n.default.httpClient.post("/admin/getNotificationsForManager",e)},addNotification:function(e){return n.default.httpClient.post("/admin/addNotification",e)},editNotification:function(e){return n.default.httpClient.post("/admin/editNotification",e)},removeNotification:function(e){return n.default.httpClient.post("/admin/removeNotification",e)},getNotifications:function(e){return n.default.httpClient.post("/admin/getNotifications",e)},readNotifications:function(e){return n.default.httpClient.post("/admin/readNotifications",e)},deleteNotifications:function(e){return n.default.httpClient.post("/admin/deleteNotifications",e)},getNewestNotification:function(e){return n.default.httpClient.post("/admin/getNewestNotification",e)},getGroupList:function(){return n.default.httpClient.get("/admin/getGroupList")},getRoleBaseList:function(){return n.default.httpClient.get("/admin/getRoleBaseList")},getPermissionTree:function(){return n.default.httpClient.get("/admin/getPermissionTree")},callDirectly:function(e){return n.default.httpClient.get(e)},download:function(e,t){return n.default.httpClient.post(e,t,{responseType:"arraybuffer"})}}},bzuE:function(e,t,r){"use strict";r.d(t,"a",function(){return n}),r.d(t,"b",function(){return o}),r.d(t,"c",function(){return a});var n="/api",o="",a=""},tqSN:function(e,t,r){"use strict";var n=r("xuOk"),o=r("JWjH"),a=function(e){r("/jSc")},i=r("VU/8")(n.a,o.a,!1,a,null,null);t.a=i.exports},tvR6:function(e,t){},uN2V:function(e,t){},xuOk:function(e,t,r){"use strict";var n=r("VsUZ"),o=r("NC6I"),a=r.n(o);t.a={data:function(){return{isLoading:!1,activeTabName:"first",changeProfileForm:{displayName:null,headUrl:null,logoUrl:null},changeProfileFormRules:{displayName:[{max:20,message:"最多支持20个字符",trigger:"blur"},{pattern:/^[a-zA-Z\u4E00-\u9FA5\uF900-\uFA2D][a-zA-Z0-9-_\u4E00-\u9FA5\uF900-\uFA2D]*$/,message:"昵称包含非法字符",trigger:"blur"}],head:[{max:200,message:"最多支持200个字符",trigger:"blur"}],logo:[{max:200,message:"最多支持200个字符",trigger:"blur"}]},changePasswordForm:{currentPassword:null,newPassword:null,newPasswordConfirm:null},changePasswordFormRules:{currentPassword:[{required:!0,message:"请输入当前密码",trigger:"blur"},{min:6,message:"最少支持6个字符",trigger:"blur"},{max:32,message:"最多支持32个字符",trigger:"blur"}],newPassword:[{required:!0,message:"请输入新密码",trigger:"blur"},{min:6,message:"最少支持6个字符",trigger:"blur"},{max:32,message:"最多支持32个字符",trigger:"blur"}],newPasswordConfirm:[{required:!0,message:"请确认新密码",trigger:"blur"},{min:6,message:"最少支持6个字符",trigger:"blur"},{max:32,message:"最多支持32个字符",trigger:"blur"}]}}},mounted:function(){this.getProfile()},methods:{getProfile:function(){var e=this;this.isLoading=!0,n.a.getProfile().then(function(t){e.isLoading=!1;var r=t.data.item;e.changeProfileForm.displayName=r.displayName,e.changeProfileForm.headUrl=r.headUrl,e.changeProfileForm.logoUrl=r.logoUrl},function(t){e.isLoading=!1,e.showErrorMessage(t.message)})},handleChangeProfile:function(){var e=this;this.$refs.changeProfileForm.validate(function(t){if(!t)return!1;e.isLoading=!0;var r=e.changeProfileForm;n.a.changeProfile(r).then(function(t){e.isLoading=!1,e.$message({message:t.data.message,type:"success"})},function(t){e.isLoading=!1,e.showErrorMessage(t.message)})})},handleChangePassword:function(){var e=this;this.$refs.changePasswordForm.validate(function(t){if(!t)return!1;e.isLoading=!0;var r={currentPassword:a()(e.changePasswordForm.currentPassword),newPassword:a()(e.changePasswordForm.newPassword),newPasswordConfirm:a()(e.changePasswordForm.newPasswordConfirm)};n.a.changePassword(r).then(function(t){e.isLoading=!1,e.changePasswordForm.currentPassword=null,e.changePasswordForm.newPassword=null,e.changePasswordForm.newPasswordConfirm=null,e.$message({message:t.data.message,type:"success"})},function(t){e.isLoading=!1,e.showErrorMessage(t.message)})})},handleChangeHeadUrlBrowser:function(){this.popupFileManager("headUrl")},handleChangeLogoUrlBrowser:function(){this.popupFileManager("logoUrl")},popupFileManager:function(e){var t=this;try{CKFinder.popup({chooseFiles:!0,width:800,height:600,onInit:function(r){r.on("files:choose",function(r){var n=r.data.files.first();t.changeProfileForm[e]=n.getUrl()}),r.on("file:choose:resizedImage",function(r){t.changeProfileForm[e]=r.data.resizedUrl})}})}catch(e){console.log(e.message)}},showErrorMessage:function(e){this.$message({message:e,type:"error"})}}}}},["IV7n"]);
//# sourceMappingURL=profile.js.map