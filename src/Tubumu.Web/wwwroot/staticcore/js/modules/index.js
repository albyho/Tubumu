webpackJsonp([4],{"/rEA":function(e,t,n){"use strict";(function(e){function i(e,t,n){this.name="ApiError",this.message=e||"Default Message",this.errorType=t||g.Default,this.innerError=n,this.stack=(new Error).stack}var o=n("//Fk"),a=n.n(o),r=n("mvHQ"),s=n.n(r),l=n("OvRC"),u=n.n(l),d=n("mtWM"),c=n.n(d),f=n("mw3O"),m=n.n(f),p=n("bzuE"),g={Default:"default",Sysetem:"sysetem"};(i.prototype=u()(Error.prototype)).constructor=i;var h=c.a.create({baseURL:p.a,timeout:2e4,responseType:"json",withCredentials:!0});h.interceptors.request.use(function(e){return"post"===e.method||"put"===e.method||"patch"===e.method?(e.headers={"Content-Type":"application/json; charset=UTF-8"},e.data=s()(e.data)):"delete"!==e.method&&"get"!==e.method&&"head"!==e.method||(e.paramsSerializer=function(e){return m.a.stringify(e,{arrayFormat:"indices"})}),localStorage.token&&(e.headers.Authorization="Bearer "+localStorage.token),e},function(e){return e}),h.interceptors.response.use(function(t){if(-1===t.headers["content-type"].indexOf("json"))return t;var n=void 0;if("arraybuffer"===t.request.responseType&&"[object ArrayBuffer]"===t.data.toString()){var o=e.from(t.data).toString("utf8");n=JSON.parse(o)}else n=t.data;if(n){if(n.data&&(n.data.token&&(localStorage.token=n.data.token),n.data.refreshToken&&(localStorage.refreshToken=n.data.refreshToken)),n.url)return void(top.location=n.url);if(200!==n.code)return console.log(n),a.a.reject(new i(n.message))}return t},function(e){if(console.log(e.response.headers),401===e.response.status){if(-1!==e.response.headers["content-type"].indexOf("json")&&e.response.data.url)return void(top.location=e.response.data.url);if(e.response.headers["token-expired"]&&localStorage.token&&localStorage.refreshToken){console.log("Refresh Token");var t={token:localStorage.token,refreshToken:localStorage.refreshToken};return localStorage.removeItem("token"),localStorage.removeItem("refreshToken"),h.post("/admin/refreshToken",t),a.a.reject(new i("登录超时，自动登录中......",g.Sysetem,e))}}return a.a.reject(new i(e.message,g.Sysetem,e))}),t.a={install:function(e){arguments.length>1&&void 0!==arguments[1]&&arguments[1];e.httpClient=h,e.prototype.$httpClient=h}}}).call(t,n("EuP9").Buffer)},"01L+":function(e,t,n){"use strict";var i=n("dpSG"),o=n("4Wj6"),a=function(e){n("vhE5")},r=n("VU/8")(i.a,o.a,!1,a,null,null);t.a=r.exports},"1Hg3":function(e,t){},"2eif":function(e,t,n){"use strict";t.a={name:"XLMenu",props:["model","index"],data:function(){return{}}}},"4Wj6":function(e,t,n){"use strict";var i={render:function(){var e=this,t=e.$createElement,n=e._self._c||t;return n("el-container",{directives:[{name:"loading",rawName:"v-loading.fullscreen.lock",value:e.isLoading,expression:"isLoading",modifiers:{fullscreen:!0,lock:!0}}]},[n("el-header",[n("el-row",[n("el-col",{attrs:{span:16}},[e._v("系统管理")]),e._v(" "),n("el-col",{staticClass:"userinfo",attrs:{span:8}},[n("el-badge",{attrs:{value:"new",hidden:!e.hasNewMessage}},[n("i",{staticClass:"el-icon-message newMessage",on:{click:e.handleNewMessage}})]),e._v(" "),n("el-dropdown",{attrs:{trigger:"hover","show-timeout":150}},[n("span",{staticClass:"el-dropdown-link userinfo-inner"},[n("img",{directives:[{name:"show",rawName:"v-show",value:e.profileDisplay.avatarUrl,expression:"profileDisplay.avatarUrl"}],attrs:{src:e.profileDisplay.avatarUrl}}),e._v(" [ "+e._s(e.profileDisplay.groups.map(function(e){return e.name}).join(" - "))+" ] "+e._s(e.profileDisplay.displayName||e.profileDisplay.username)+"\r\n          ")]),e._v(" "),n("el-dropdown-menu",{attrs:{slot:"dropdown"},slot:"dropdown"},[n("el-dropdown-item",{nativeOn:{click:function(t){return e.profile(t)}}},[e._v("我的资料")]),e._v(" "),n("el-dropdown-item",{attrs:{divided:""},nativeOn:{click:function(t){return e.logout(t)}}},[e._v("退出登录")])],1)],1)],1)],1)],1),e._v(" "),n("el-container",{staticClass:"el-container-inner"},[n("el-aside",{directives:[{name:"loading",rawName:"v-loading",value:e.isGetMenusLoading,expression:"isGetMenusLoading"}],attrs:{width:"200"}},[n("el-menu",{staticClass:"el-menu-vertical-main",attrs:{"unique-opened":"","default-active":"menuActiveIndex"},on:{open:e.handleOpen,close:e.handleClose,select:e.handleSelect}},e._l(e.menus,function(e,t){return n("xl-menu",{key:t,attrs:{model:e,index:t.toString()}})}),1)],1),e._v(" "),n("el-main",[n("iframe",{staticClass:"el-main-content",attrs:{src:e.mainFrameUrl,scrolling:"auto",frameBorder:"0",width:e.iframeWidth,height:e.iframeHeight}})])],1)],1)},staticRenderFns:[]};t.a=i},DjhF:function(e,t,n){"use strict";var i={render:function(){var e=this,t=e.$createElement,n=e._self._c||t;return 0===e.model.type?n("el-menu-item",{attrs:{index:e.index}},[n("span",{attrs:{slot:"title"},slot:"title"},[e._v(e._s(e.model.title))])]):1===e.model.type?n("el-submenu",{attrs:{index:e.index}},[n("template",{slot:"title"},[n("i",{staticClass:"el-icon-menu"}),e._v(" "),n("span",{attrs:{slot:"title"},slot:"title"},[e._v(e._s(e.model.title))])]),e._v(" "),e._l(e.model.children,function(t,i){return n("xl-menu",{key:e.index+"-"+i,attrs:{model:t,index:e.index+"-"+i}})})],2):2===e.model.type?n("el-menu-item-group",{attrs:{title:e.model.title}},e._l(e.model.children,function(t,i){return n("xl-menu",{key:e.index+"-"+i,attrs:{model:t,index:e.index+"-"+i}})}),1):e._e()},staticRenderFns:[]};t.a=i},"Rt+6":function(e,t,n){"use strict";var i=n("2eif"),o=n("DjhF"),a=n("VU/8")(i.a,o.a,!1,null,null,null);t.a=a.exports},VsUZ:function(e,t,n){"use strict";var i=n("7+uW");t.a={login:function(e){return i.default.httpClient.post("/admin/login",e)},refreshToken:function(e){return i.default.httpClient.post("/admin/refreshToken",e)},logout:function(){return i.default.httpClient.post("/admin/logout")},getProfile:function(){return i.default.httpClient.get("/admin/getProfile")},changeProfile:function(e){return i.default.httpClient.post("/admin/changeProfile",e)},changeAvatar:function(e){return i.default.uploadHttpClient.post("/admin/ChangeAvatar",e)},changeLogo:function(e){return i.default.uploadHttpClient.post("/admin/ChangeLogo",e)},changePassword:function(e){return i.default.httpClient.post("/admin/changePassword",e)},getMenus:function(){return i.default.httpClient.get("/admin/getMenus")},getBulletin:function(){return i.default.httpClient.get("/admin/getBulletin")},editBulletin:function(e){return i.default.httpClient.post("/admin/editBulletin",e)},getModuleMetaDatas:function(){return i.default.httpClient.get("/admin/getModuleMetaDatas")},extractModuleMetaDatas:function(){return i.default.httpClient.get("/admin/extractModuleMetaDatas")},clearModulePermissions:function(){return i.default.httpClient.get("/admin/clearModulePermissions")},getRoleList:function(){return i.default.httpClient.get("/admin/getRoleList")},addRole:function(e){return i.default.httpClient.post("/admin/addRole",e)},editRole:function(e){return i.default.httpClient.post("/admin/editRole",e)},removeRole:function(e){return i.default.httpClient.post("/admin/removeRole",e)},moveRole:function(e){return i.default.httpClient.post("/admin/moveRole",e)},saveRoleName:function(e){return i.default.httpClient.post("/admin/saveRoleName",e)},getGroupTree:function(){return i.default.httpClient.get("/admin/getGroupTree")},addGroup:function(e){return i.default.httpClient.post("/admin/addGroup",e)},editGroup:function(e){return i.default.httpClient.post("/admin/editGroup",e)},removeGroup:function(e){return i.default.httpClient.post("/admin/removeGroup",e)},moveGroup:function(e){return i.default.httpClient.post("/admin/moveGroup",e)},getUserPage:function(e){return i.default.httpClient.post("/admin/getUserPage",e)},addUser:function(e){return i.default.httpClient.post("/admin/addUser",e)},editUser:function(e){return i.default.httpClient.post("/admin/editUser",e)},removeUser:function(e){return i.default.httpClient.post("/admin/removeUser",e)},getUserStatuList:function(){return i.default.httpClient.get("/admin/getUserStatuList")},changeUserAvatar:function(e){return i.default.uploadHttpClient.post("/admin/ChangeUserAvatar",e)},changeUserLogo:function(e){return i.default.uploadHttpClient.post("/admin/ChangeUserLogo",e)},getNotificationsForManager:function(e){return i.default.httpClient.post("/admin/getNotificationsForManager",e)},addNotification:function(e){return i.default.httpClient.post("/admin/addNotification",e)},editNotification:function(e){return i.default.httpClient.post("/admin/editNotification",e)},removeNotification:function(e){return i.default.httpClient.post("/admin/removeNotification",e)},getNotifications:function(e){return i.default.httpClient.post("/admin/getNotifications",e)},readNotifications:function(e){return i.default.httpClient.post("/admin/readNotifications",e)},deleteNotifications:function(e){return i.default.httpClient.post("/admin/deleteNotifications",e)},getNewestNotification:function(e){return i.default.httpClient.post("/admin/getNewestNotification",e)},getGroupList:function(){return i.default.httpClient.get("/admin/getGroupList")},getRoleBaseList:function(){return i.default.httpClient.get("/admin/getRoleBaseList")},getPermissionTree:function(){return i.default.httpClient.get("/admin/getPermissionTree")},callDirectly:function(e){return i.default.httpClient.get(e)},download:function(e,t){return i.default.httpClient.post(e,t,{responseType:"arraybuffer"})}}},bzuE:function(e,t,n){"use strict";n.d(t,"a",function(){return i}),n.d(t,"b",function(){return o}),n.d(t,"c",function(){return a});var i="/api",o="",a=""},dpSG:function(e,t,n){"use strict";var i=n("VsUZ");t.a={data:function(){return{isLoading:!1,isGetMenusLoading:!1,hasNewMessage:null,mainFrameUrl:"",profileDisplay:{username:"",displayName:"",avatarUrl:null,groups:[]},menus:null,menuActiveIndex:"0-0",iframeWidth:document.documentElement.clientWidth-200,iframeHeight:document.documentElement.clientHeight-60}},created:function(){},mounted:function(){var e=this,t=this;window.onresize=function(){t.iframeWidth=document.documentElement.clientWidth-200,t.iframeHeight=document.documentElement.clientHeight-60},window.onresize(),this.isGetMenusLoading=!0,i.a.getMenus().then(function(t){e.isGetMenusLoading=!1,e.menus=t.data.data},function(t){e.isGetMenusLoading=!1,e.$message.error(t.message)}),i.a.getProfile().then(function(t){e.profileDisplay=t.data.data,e.connectNotifictionServer()},function(t){e.$message.error(t.message)})},methods:{handleOpen:function(e,t){},handleClose:function(e,t){},handleSelect:function(e,t){for(var n=e.split("-"),i=this.menus,o=null,a=0;a<n.length;a++)o=i[n[a]],a===n.length-1?o.directly?this.callDirectly(o.link):o.linkTarget?window.open(o.link,o.linkTarget):this.mainFrameUrl=o.link:i=o.children},profile:function(){this.mainFrameUrl="/Admin/View?IsCore=true&Title=%E6%88%91%E7%9A%84%E8%B5%84%E6%96%99&Name=profile&Components=ckfinder"},resources:function(){this.mainFrameUrl="/Admin/View?IsCore=true&Title=%E6%88%91%E7%9A%84%E8%B5%84%E6%96%99&Name=resources&Components=ckfinder"},logout:function(){var e=this;this.isLoading=!0,i.a.logout().then(function(e){localStorage.removeItem("token"),localStorage.removeItem("refreshToken")},function(t){e.isLoading=!1,e.$message.error(t.message)})},callDirectly:function(e){var t=this;this.isLoading=!0,i.a.callDirectly(e).then(function(e){t.isLoading=!1,t.$message.success(e.data.message)},function(e){t.isLoading=!1,t.$message.error(e.message)})},connectNotifictionServer:function(){var e=this;try{var t=(new signalR.HubConnectionBuilder).withUrl("/hubs/notificationHub",{accessTokenFactory:function(){return localStorage.token}}).build();t.on("ReceiveMessage",function(t){console.log(t),201===t.code?(e.hasNewMessage=!0,e.$notify.info({dangerouslyUseHTMLString:!0,offset:64,duration:5e3,title:t.title||"新的消息",message:t.message})):202===t.code?e.hasNewMessage=!1:400===t.code&&e.$message.error(t.message)}),t.start().catch(function(e){return console.error(e.toString())})}catch(e){console.log(e.message)}},handleNewMessage:function(){this.hasNewMessage=!1,this.mainFrameUrl="/Admin/View?IsCore=true&Title=%E9%80%9A%E7%9F%A5%E4%B8%AD%E5%BF%83&Name=notification&t="+(new Date).getTime()}}}},sSkr:function(e,t,n){"use strict";Object.defineProperty(t,"__esModule",{value:!0});var i=n("tvR6"),o=(n.n(i),n("qBF2")),a=n.n(o),r=n("7+uW"),s=n("/rEA"),l=n("Rt+6"),u=n("1Hg3"),d=(n.n(u),n("01L+"));r.default.config.productionTip=!1,r.default.use(s.a),r.default.use(a.a,{size:"mini"}),r.default.component("xl-menu",l.a),new r.default({el:"#app",render:function(e){return e(d.a)}})},tvR6:function(e,t){},vhE5:function(e,t){}},["sSkr"]);
//# sourceMappingURL=index.js.map