webpackJsonp([1],{"/rEA":function(e,t,r){"use strict";(function(e){function a(e,t,r){this.name="ApiError",this.message=e||"Default Message",this.errorType=t||f.Default,this.innerError=r,this.stack=(new Error).stack}var i=r("//Fk"),n=r.n(i),o=r("mvHQ"),l=r.n(o),s=r("OvRC"),u=r.n(s),m=r("mtWM"),c=r.n(m),d=r("mw3O"),p=r.n(d),h=r("bzuE"),f={Default:"default",Sysetem:"sysetem"};(a.prototype=u()(Error.prototype)).constructor=a;var g=c.a.create({baseURL:h.a,timeout:2e4,responseType:"json",withCredentials:!0});g.interceptors.request.use(function(e){return"post"===e.method||"put"===e.method||"patch"===e.method?(e.headers={"Content-Type":"application/json; charset=UTF-8"},e.data=l()(e.data)):"delete"!==e.method&&"get"!==e.method&&"head"!==e.method||(e.paramsSerializer=function(e){return p.a.stringify(e,{arrayFormat:"indices"})}),localStorage.token&&(e.headers.Authorization="Bearer "+localStorage.token),e},function(e){return e}),g.interceptors.response.use(function(t){if(-1===t.headers["content-type"].indexOf("json"))return t;var r=void 0;if("arraybuffer"===t.request.responseType&&"[object ArrayBuffer]"===t.data.toString()){var i=e.from(t.data).toString("utf8");console.log(i),r=JSON.parse(i)}else r=t.data;if(r){if(r.token&&(localStorage.token=r.token),r.refreshToken&&(localStorage.refreshToken=r.refreshToken),r.url)return void(top.location=r.url);if(200!==r.code)return console.log(r),n.a.reject(new a(r.message))}return t},function(e){if(console.log(e.response.headers),401===e.response.status){if(-1!==e.response.headers["content-type"].indexOf("json")&&e.response.data.url)return void(top.location=e.response.data.url);if(e.response.headers["token-expired"]&&localStorage.token&&localStorage.refreshToken){console.log("Refresh Token");var t={token:localStorage.token,refreshToken:localStorage.refreshToken};return localStorage.removeItem("token"),localStorage.removeItem("refreshToken"),g.post("/admin/refreshToken",t),n.a.reject(new a("登录超时，自动登录中......",f.Sysetem,e))}}return n.a.reject(new a(e.message,f.Sysetem,e))}),t.a={install:function(e){arguments.length>1&&void 0!==arguments[1]&&arguments[1];e.httpClient=g,e.prototype.$httpClient=g}}}).call(t,r("EuP9").Buffer)},"47Lh":function(e,t,r){"use strict";t.a={name:"XLUserStatusRadioGroup",props:{value:{type:String,default:"Normal"}},data:function(){return{currentValue:this.value,userStatusData:[{label:"默认",value:"Normal"},{label:"待审",value:"PendingApproval"},{label:"删除",value:"Removed"}]}},watch:{value:function(e){this.currentValue=e},currentValue:function(e){this.$emit("input",e)}}}},CPfC:function(e,t,r){"use strict";var a=r("Ss4G"),i=r("rMx+"),n=r("VU/8")(a.a,i.a,!1,null,null,null);t.a=n.exports},"D+l0":function(e,t,r){"use strict";var a=r("lY/r"),i=r("l7xB"),n=function(e){r("oqYR")},o=r("VU/8")(a.a,i.a,!1,n,null,null);t.a=o.exports},DUa8:function(e,t,r){"use strict";var a=r("vT38"),i=r("RdGK"),n=r("VU/8")(a.a,i.a,!1,null,null,null);t.a=n.exports},IlS5:function(e,t,r){"use strict";Object.defineProperty(t,"__esModule",{value:!0});var a=r("tvR6"),i=(r.n(a),r("qBF2")),n=r.n(i),o=r("7+uW"),l=r("/rEA"),s=r("t7Yk"),u=r("YNH2"),m=r("CPfC"),c=r("DUa8"),d=r("uN2V"),p=(r.n(d),r("D+l0"));o.default.config.productionTip=!1,o.default.use(l.a),o.default.use(n.a,{size:"mini"}),o.default.component("xl-userStatusSelect",s.a),o.default.component("xl-userStatusRadioGroup",u.a),o.default.component("xl-groupCascader",m.a),o.default.component("xl-datePicker",c.a),new o.default({el:"#app",render:function(e){return e(p.a)}})},"N0+e":function(e,t,r){"use strict";t.a={name:"XLUserStatusSelect",props:{value:{type:String,default:"Normal"}},data:function(){return{currentValue:this.value,userStatusData:[{label:"默认",value:"Normal"},{label:"待审",value:"PendingApproval"},{label:"删除",value:"Removed"}]}},watch:{value:function(e){this.currentValue=e},currentValue:function(e){this.$emit("input",e)}}}},RdGK:function(e,t,r){"use strict";var a={render:function(){var e=this,t=e.$createElement;return(e._self._c||t)("el-date-picker",{attrs:{"picker-options":e.pickerOptions,"value-format":"yyyy-MM-dd",type:"daterange","range-separator":"至","start-placeholder":"创建日期开始","end-placeholder":"创建日期结束",align:"right"},model:{value:e.currentValue,callback:function(t){e.currentValue=t},expression:"currentValue"}})},staticRenderFns:[]};t.a=a},Ss4G:function(e,t,r){"use strict";var a=r("BO1k"),i=r.n(a),n=r("VsUZ");t.a={name:"XLGroupCascader",props:{value:{type:Array}},data:function(){return{currentValue:this.value,tempValue:[],treeData:[],defaultProps:{children:"children",value:"id",label:"name"}}},watch:{value:function(e){this.currentValue=e},currentValue:function(e){this.$emit("input",e)}},mounted:function(){this.getGroupTree()},methods:{getGroupTree:function(){var e=this;n.a.getGroupTree().then(function(t){var r=t.data.tree;e.treeData=r,e.currentValue&&1===e.currentValue.length&&(e.getIdPath(r,e.currentValue[0]),e.currentValue=e.tempValue)},function(t){e.$message({message:t.message,type:"error"})})},getIdPath:function(e,t){if(e){var r=!0,a=!1,n=void 0;try{for(var o,l=i()(e);!(r=(o=l.next()).done);r=!0){var s=o.value;if(s.id===t){this.tempValue=s.parentIdPath?s.parentIdPath.concat():[],this.tempValue.push(t);break}this.getIdPath(s.children,t)}}catch(e){a=!0,n=e}finally{try{!r&&l.return&&l.return()}finally{if(a)throw n}}}},handleChange:function(e){this.$emit("change",e)}}}},VsUZ:function(e,t,r){"use strict";var a=r("7+uW");t.a={login:function(e){return a.default.httpClient.post("/admin/login",e)},refreshToken:function(e){return a.default.httpClient.post("/admin/refreshToken",e)},logout:function(){return a.default.httpClient.post("/admin/logout")},getProfile:function(){return a.default.httpClient.get("/admin/getProfile")},changeProfile:function(e){return a.default.httpClient.post("/admin/changeProfile",e)},changePassword:function(e){return a.default.httpClient.post("/admin/changePassword",e)},getMenus:function(){return a.default.httpClient.get("/admin/getMenus")},getBulletin:function(){return a.default.httpClient.get("/admin/getBulletin")},editBulletin:function(e){return a.default.httpClient.post("/admin/editBulletin",e)},getModuleMetaDatas:function(){return a.default.httpClient.get("/admin/getModuleMetaDatas")},extractModuleMetaDatas:function(){return a.default.httpClient.get("/admin/extractModuleMetaDatas")},clearModulePermissions:function(){return a.default.httpClient.get("/admin/clearModulePermissions")},getRoleList:function(){return a.default.httpClient.get("/admin/getRoleList")},addRole:function(e){return a.default.httpClient.post("/admin/addRole",e)},editRole:function(e){return a.default.httpClient.post("/admin/editRole",e)},removeRole:function(e){return a.default.httpClient.post("/admin/removeRole",e)},moveRole:function(e){return a.default.httpClient.post("/admin/moveRole",e)},saveRoleName:function(e){return a.default.httpClient.post("/admin/saveRoleName",e)},getGroupTree:function(){return a.default.httpClient.get("/admin/getGroupTree")},addGroup:function(e){return a.default.httpClient.post("/admin/addGroup",e)},editGroup:function(e){return a.default.httpClient.post("/admin/editGroup",e)},removeGroup:function(e){return a.default.httpClient.post("/admin/removeGroup",e)},moveGroup:function(e){return a.default.httpClient.post("/admin/moveGroup",e)},getUserPage:function(e){return a.default.httpClient.post("/admin/getUserPage",e)},addUser:function(e){return a.default.httpClient.post("/admin/addUser",e)},editUser:function(e){return a.default.httpClient.post("/admin/editUser",e)},removeUser:function(e){return a.default.httpClient.post("/admin/removeUser",e)},getUserStatuList:function(){return a.default.httpClient.get("/admin/getUserStatuList")},getNotificationsForManager:function(e){return a.default.httpClient.post("/admin/getNotificationsForManager",e)},addNotification:function(e){return a.default.httpClient.post("/admin/addNotification",e)},editNotification:function(e){return a.default.httpClient.post("/admin/editNotification",e)},removeNotification:function(e){return a.default.httpClient.post("/admin/removeNotification",e)},getNotifications:function(e){return a.default.httpClient.post("/admin/getNotifications",e)},readNotifications:function(e){return a.default.httpClient.post("/admin/readNotifications",e)},deleteNotifications:function(e){return a.default.httpClient.post("/admin/deleteNotifications",e)},getNewestNotification:function(e){return a.default.httpClient.post("/admin/getNewestNotification",e)},getGroupList:function(){return a.default.httpClient.get("/admin/getGroupList")},getRoleBaseList:function(){return a.default.httpClient.get("/admin/getRoleBaseList")},getPermissionTree:function(){return a.default.httpClient.get("/admin/getPermissionTree")},callDirectly:function(e){return a.default.httpClient.get(e)},download:function(e,t){return a.default.httpClient.post(e,t,{responseType:"arraybuffer"})}}},YNH2:function(e,t,r){"use strict";var a=r("47Lh"),i=r("hm6y"),n=r("VU/8")(a.a,i.a,!1,null,null,null);t.a=n.exports},bzuE:function(e,t,r){"use strict";r.d(t,"a",function(){return a}),r.d(t,"b",function(){return i}),r.d(t,"c",function(){return n});var a="/api",i="",n=""},hm6y:function(e,t,r){"use strict";var a={render:function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("el-radio-group",{model:{value:e.currentValue,callback:function(t){e.currentValue=t},expression:"currentValue"}},e._l(e.userStatusData,function(t){return r("el-radio",{key:t.value,attrs:{label:t.value}},[e._v(e._s(t.label))])}),1)},staticRenderFns:[]};t.a=a},l7xB:function(e,t,r){"use strict";var a={render:function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("el-container",{directives:[{name:"loading",rawName:"v-loading.fullscreen.lock",value:e.isLoading,expression:"isLoading",modifiers:{fullscreen:!0,lock:!0}}]},[r("el-header",{staticClass:"header"},[r("el-breadcrumb",{staticClass:"breadcrumb",attrs:{"separator-class":"el-icon-arrow-right"}},[r("el-breadcrumb-item",[e._v("用户管理")]),e._v(" "),r("el-breadcrumb-item",[e._v("用户列表")])],1)],1),e._v(" "),r("el-main",{staticClass:"main"},[r("el-form",{ref:"searchCriteriaForm",staticClass:"searchCriteriaForm",attrs:{model:e.searchCriteriaForm,inline:""}},[r("el-row",[r("el-form-item",[r("el-input",{staticClass:"filterText",attrs:{placeholder:"关键字(用户名/真实名称/昵称/邮箱/手机号)",clearable:""},model:{value:e.searchCriteriaForm.keyword,callback:function(t){e.$set(e.searchCriteriaForm,"keyword",t)},expression:"searchCriteriaForm.keyword"}})],1),e._v(" "),r("el-form-item",[r("el-cascader",{attrs:{options:e.editGroupTreeData,props:e.editGroupTreeDefaultProps,clearable:"","change-on-select":"",filterable:"",placeholder:"分组"},model:{value:e.searchCriteriaForm.groupIdPath,callback:function(t){e.$set(e.searchCriteriaForm,"groupIdPath",t)},expression:"searchCriteriaForm.groupIdPath"}})],1),e._v(" "),r("el-form-item",[r("xl-userStatusSelect",{model:{value:e.searchCriteriaForm.status,callback:function(t){e.$set(e.searchCriteriaForm,"status",t)},expression:"searchCriteriaForm.status"}})],1),e._v(" "),r("el-form-item",[r("el-button",{attrs:{plain:"",icon:e.isSearchCriteriaFormExpand?"el-icon-caret-top":"el-icon-caret-bottom"},on:{click:function(t){e.isSearchCriteriaFormExpand=!e.isSearchCriteriaFormExpand}}}),e._v(" "),r("el-button-group",[r("el-button",{attrs:{type:"primary",icon:"el-icon-search"},on:{click:function(t){e.handleSearch()}}},[e._v("搜索")]),e._v(" "),r("el-button",{attrs:{type:"primary",icon:"el-icon-search"},on:{click:function(t){e.handleSearchAll()}}},[e._v("全部")])],1),e._v(" "),r("el-button",{attrs:{type:"primary",icon:"el-icon-circle-plus-outline"},on:{click:function(t){e.handleAdd()}}},[e._v("添加")])],1)],1),e._v(" "),r("el-row",{directives:[{name:"show",rawName:"v-show",value:e.isSearchCriteriaFormExpand,expression:"isSearchCriteriaFormExpand"}]},[r("el-form-item",[r("xl-datePicker",{model:{value:e.searchCriteriaForm.creationTime,callback:function(t){e.$set(e.searchCriteriaForm,"creationTime",t)},expression:"searchCriteriaForm.creationTime"}})],1)],1)],1),e._v(" "),r("el-row",[r("el-table",{staticStyle:{width:"100%"},attrs:{data:e.page.list,"empty-text":e.mainTableEmptyText},on:{"sort-change":e.handleSortChange}},[r("el-table-column",{attrs:{prop:"userId",label:"#",width:"60",sortable:"custom"}}),e._v(" "),r("el-table-column",{attrs:{prop:"username",label:"用户名",width:"160",sortable:"custom"}}),e._v(" "),r("el-table-column",{attrs:{prop:"group.name",label:"分组",width:"160"}}),e._v(" "),r("el-table-column",{attrs:{label:"角色",width:"100"},scopedSlots:e._u([{key:"default",fn:function(t){return[r("span",[e._v(e._s(t.row.role?t.row.role.name:""))])]}}])}),e._v(" "),r("el-table-column",{attrs:{prop:"displayName",label:"昵称",width:"100"}}),e._v(" "),r("el-table-column",{attrs:{label:"真实名称",width:"100"},scopedSlots:e._u([{key:"default",fn:function(t){return[r("i",{directives:[{name:"show",rawName:"v-show",value:t.row.realName&&!t.row.realNameIsValid,expression:"scope.row.realName && !scope.row.realNameIsValid"}],staticClass:"el-icon-question"}),e._v(" "),r("span",[e._v(e._s(t.row.realName))])]}}])}),e._v(" "),r("el-table-column",{attrs:{label:"手机号码",width:"120"},scopedSlots:e._u([{key:"default",fn:function(t){return[r("i",{directives:[{name:"show",rawName:"v-show",value:t.row.mobile&&!t.row.mobileIsValid,expression:"scope.row.mobile && !scope.row.mobileIsValid"}],staticClass:"el-icon-question"}),e._v(" "),r("span",[e._v(e._s(t.row.mobile))])]}}])}),e._v(" "),r("el-table-column",{attrs:{label:"Email",width:"140"},scopedSlots:e._u([{key:"default",fn:function(t){return[r("i",{directives:[{name:"show",rawName:"v-show",value:t.row.email&&!t.row.emailIsValid,expression:"scope.row.email && !scope.row.emailIsValid"}],staticClass:"el-icon-question"}),e._v(" "),r("span",[e._v(e._s(t.row.email))])]}}])}),e._v(" "),r("el-table-column",{attrs:{prop:"statusText",label:"状态",width:"60"}}),e._v(" "),r("el-table-column",{attrs:{prop:"creationTime",label:"创建时间",width:"160"}}),e._v(" "),r("el-table-column",{attrs:{label:"开发",width:"60"},scopedSlots:e._u([{key:"default",fn:function(t){return[e._v("\n            "+e._s(t.row.isDeveloper?"√":"×")+"\n          ")]}}])}),e._v(" "),r("el-table-column",{attrs:{label:"测试",width:"60"},scopedSlots:e._u([{key:"default",fn:function(t){return[e._v("\n            "+e._s(t.row.isTester?"√":"×")+"\n          ")]}}])}),e._v(" "),r("el-table-column",{attrs:{align:"center",fixed:"right",width:"84"},scopedSlots:e._u([{key:"default",fn:function(t){return[r("el-button",{attrs:{type:"text",size:"small",icon:"el-icon-edit"},on:{click:function(r){e.handleEdit(t.row)}}}),e._v(" "),r("el-button",{attrs:{type:"text",size:"small",icon:"el-icon-delete"},on:{click:function(r){e.handleRemove(t.row)}}})]}}])})],1)],1),e._v(" "),r("el-dialog",{attrs:{visible:e.mainFormDialogVisible,"close-on-click-modal":!1,width:"600px"},on:{"update:visible":function(t){e.mainFormDialogVisible=t}},nativeOn:{submit:function(e){e.preventDefault()}}},[r("span",{attrs:{slot:"title"},slot:"title"},[e._v("\n        "+e._s(e.editActive?"编辑":"添加")+"\n      ")]),e._v(" "),r("el-form",{ref:"mainForm",attrs:{model:e.mainForm,rules:e.mainFormRules,"label-position":"right","label-width":"160px"}},[r("el-tabs",{attrs:{type:"card"},model:{value:e.activeTabName,callback:function(t){e.activeTabName=t},expression:"activeTabName"}},[r("el-tab-pane",{attrs:{label:"基本信息",name:"first"}},[r("el-form-item",{attrs:{label:"主要分组",prop:"groupIdPath"}},[r("el-cascader",{attrs:{options:e.editGroupTreeData,props:e.editGroupTreeDefaultProps,clearable:"","change-on-select":"",filterable:"",placeholder:"请选择主要分组"},on:{change:e.handleGroupCascaderChange},model:{value:e.mainForm.groupIdPath,callback:function(t){e.$set(e.mainForm,"groupIdPath",t)},expression:"mainForm.groupIdPath"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"主要角色",prop:"roleId"}},[r("el-select",{attrs:{clearable:"",placeholder:"请选择主要角色"},model:{value:e.mainForm.roleId,callback:function(t){e.$set(e.mainForm,"roleId",t)},expression:"mainForm.roleId"}},e._l(e.editGroupRoleListData,function(e){return r("el-option",{key:e.roleId,attrs:{label:e.name,value:e.roleId}})}),1)],1),e._v(" "),r("el-form-item",{attrs:{label:"用户名",prop:"username"}},[r("el-input",{ref:"username",attrs:{autocomplete:"off",placeholder:"请输入用户名"},model:{value:e.mainForm.username,callback:function(t){e.$set(e.mainForm,"username","string"==typeof t?t.trim():t)},expression:"mainForm.username"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"状态",required:!0}},[r("xl-userStatusRadioGroup",{model:{value:e.mainForm.status,callback:function(t){e.$set(e.mainForm,"status",t)},expression:"mainForm.status"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"登录密码",prop:"password",required:!e.editActive}},[r("el-input",{attrs:{type:"password",placeholder:e.editActive?"如果不修改密码，请保持为空":"请输入登录密码"},model:{value:e.mainForm.password,callback:function(t){e.$set(e.mainForm,"password",t)},expression:"mainForm.password"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"确认密码",prop:"passwordConfirm",required:!e.editActive}},[r("el-input",{attrs:{type:"password",placeholder:e.editActive?"如果不修改密码，请保持为空":"请输入确认密码"},model:{value:e.mainForm.passwordConfirm,callback:function(t){e.$set(e.mainForm,"passwordConfirm",t)},expression:"mainForm.passwordConfirm"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"昵称"}},[r("el-input",{attrs:{type:"text"},model:{value:e.mainForm.displayName,callback:function(t){e.$set(e.mainForm,"displayName",t)},expression:"mainForm.displayName"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"真实名称"}},[r("el-input",{attrs:{type:"text"},model:{value:e.mainForm.realName,callback:function(t){e.$set(e.mainForm,"realName",t)},expression:"mainForm.realName"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"真实名称是否验证"}},[r("el-switch",{model:{value:e.mainForm.realNameIsValid,callback:function(t){e.$set(e.mainForm,"realNameIsValid",t)},expression:"mainForm.realNameIsValid"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"邮箱",prop:"email"}},[r("el-input",{attrs:{type:"text"},model:{value:e.mainForm.email,callback:function(t){e.$set(e.mainForm,"email",t)},expression:"mainForm.email"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"邮箱是否验证"}},[r("el-switch",{model:{value:e.mainForm.emailIsValid,callback:function(t){e.$set(e.mainForm,"emailIsValid",t)},expression:"mainForm.emailIsValid"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"手机号码",prop:"mobile"}},[r("el-input",{attrs:{type:"text"},model:{value:e.mainForm.mobile,callback:function(t){e.$set(e.mainForm,"mobile",t)},expression:"mainForm.mobile"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"手机号码是否验证"}},[r("el-switch",{model:{value:e.mainForm.mobileIsValid,callback:function(t){e.$set(e.mainForm,"mobileIsValid",t)},expression:"mainForm.mobileIsValid"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"描述"}},[r("el-input",{attrs:{type:"textarea",rows:2},model:{value:e.mainForm.description,callback:function(t){e.$set(e.mainForm,"description","string"==typeof t?t.trim():t)},expression:"mainForm.description"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"头像",prop:"headUrl"}},[r("el-input",{ref:"headUrl",attrs:{autocomplete:"off",placeholder:"请输入头像 Url"},model:{value:e.mainForm.headUrl,callback:function(t){e.$set(e.mainForm,"headUrl","string"==typeof t?t.trim():t)},expression:"mainForm.headUrl"}},[r("el-button",{attrs:{slot:"append",icon:"el-icon-search"},on:{click:e.handleChangeHeadUrlBrowser},slot:"append"})],1)],1),e._v(" "),r("el-form-item",{attrs:{label:"Logo",prop:"logoUrl"}},[r("el-input",{ref:"logoUrl",attrs:{autocomplete:"off",placeholder:"请输入Logo Url"},model:{value:e.mainForm.logoUrl,callback:function(t){e.$set(e.mainForm,"logoUrl","string"==typeof t?t.trim():t)},expression:"mainForm.logoUrl"}},[r("el-button",{attrs:{slot:"append",icon:"el-icon-search"},on:{click:e.handleChangeLogoUrlBrowser},slot:"append"})],1)],1),e._v(" "),r("el-form-item",{attrs:{label:"是否是开发人员"}},[r("el-switch",{model:{value:e.mainForm.isDeveloper,callback:function(t){e.$set(e.mainForm,"isDeveloper",t)},expression:"mainForm.isDeveloper"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"是否是测试人员"}},[r("el-switch",{model:{value:e.mainForm.isTester,callback:function(t){e.$set(e.mainForm,"isTester",t)},expression:"mainForm.isTester"}})],1)],1),e._v(" "),r("el-tab-pane",{attrs:{label:"附加分组",name:"second"}},[r("el-form-item",{attrs:{label:"附加分组"}},[r("el-tree",{ref:"editGroupTree",attrs:{data:e.editGroupTreeData,props:e.editGroupTreeDefaultProps,"node-key":"id","empty-text":"","show-checkbox":"","default-expand-all":"","check-strictly":""},on:{"check-change":e.handleGroupTreeCheckChange}})],1)],1),e._v(" "),r("el-tab-pane",{attrs:{label:"附加角色",name:"third"}},[r("el-form-item",{attrs:{label:"附加角色"}},[r("el-checkbox-group",{model:{value:e.mainForm.roleIds,callback:function(t){e.$set(e.mainForm,"roleIds",t)},expression:"mainForm.roleIds"}},e._l(e.editRoleListData,function(t){return r("el-checkbox",{key:t.roleId,attrs:{label:t.roleId}},[e._v(e._s(t.name))])}),1)],1)],1),e._v(" "),r("el-tab-pane",{attrs:{label:"附加权限",name:"fourth"}},[r("el-form-item",{attrs:{label:"附加权限"}},[r("el-tree",{ref:"editPermissionTree",attrs:{data:e.editPermissionTreeData,props:e.editPermissionTreeDefaultProps,"node-key":"id","empty-text":"","show-checkbox":"","default-expand-all":"","check-strictly":""},on:{"check-change":e.handlePermissionTreeCheckChange}})],1)],1)],1)],1),e._v(" "),r("div",{staticClass:"dialog-footer",attrs:{slot:"footer"},slot:"footer"},[r("el-button",{on:{click:function(t){e.handleMainFormSure(!1)}}},[e._v("取 消")]),e._v(" "),r("el-button",{attrs:{type:"primary"},on:{click:function(t){e.handleMainFormSure(!0)}}},[e._v("确 定")])],1)],1)],1),e._v(" "),r("el-footer",{staticClass:"footer"},[e.page.totalItemCount?r("el-pagination",{attrs:{"current-page":e.searchCriteriaForm.pagingInfo.pageNumber,"page-sizes":[20,50,100,200,400],"page-size":e.searchCriteriaForm.pagingInfo.pageSize,layout:"total, sizes, prev, pager, next, jumper",total:e.page.totalItemCount},on:{"size-change":e.handlePaginationSizeChange,"current-change":e.handlePaginationCurrentChange}}):e._e()],1)],1)},staticRenderFns:[]};t.a=a},"lY/r":function(e,t,r){"use strict";var a=r("BO1k"),i=r.n(a),n=r("VsUZ"),o=r("M4fF"),l=r.n(o),s=r("NC6I"),u=r.n(s);t.a={data:function(){var e=this;return{isLoading:!1,activeTabName:"first",page:{list:null,totalItemCount:null,totalPageCount:null},isSearchCriteriaFormExpand:!1,searchCriteriaForm:{keyword:null,groupIds:null,creationTime:null,creationTimeBegin:null,creationTimeEnd:null,status:null,groupIdPath:[],pagingInfo:{pageNumber:1,pageSize:20,isExcludeMetaData:!1,sortInfo:{sort:"userId",sortDir:"ASC"}}},removeActive:null,editActive:null,mainFormDialogVisible:!1,mainForm:{userId:null,status:null,username:null,displayName:null,realName:null,realNameIsValid:!1,email:null,emailIsValid:!1,mobile:null,mobileIsValid:!1,groupIdPath:[],groupId:null,groupIds:[],roleId:null,roleIds:[],permissionIds:null,password:null,passwordConfirm:null,description:null,headUrl:null,logoUrl:null,isDeveloper:!1,isTester:!1},mainFormRules:{username:[{required:!0,message:"请输入用户名",trigger:"blur"},{max:20,message:"最多支持20个字符",trigger:"blur"}],displayName:[{max:20,message:"最多支持20个字符",trigger:"blur"}],realName:[{max:100,message:"最多支持100个字符",trigger:"blur"}],mobile:[{pattern:/^1\d{10}$/,message:"请输入正确的手机号码",trigger:"blur"}],email:[{pattern:/^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/,message:"请输入正确的邮箱",trigger:"blur"}],groupIdPath:[{required:!0,type:"array",message:"请选择主要分组",trigger:"change"}],password:[{validator:function(t,r,a){!e.editActive||r&&0!==r.length||a(),r?r.length<6?a(new Error("请输入至少6位密码")):r.length>32?a(new Error("密码请保持在32位以内")):a():a(new Error("请输入登录密码"))},trigger:"blur"}],passwordConfirm:[{validator:function(t,r,a){!e.editActive||e.mainForm.password&&0!==e.mainForm.password.length||a(),r?r!==e.mainForm.password?a(new Error("两次输入密码不一致!")):a():a(new Error("请输入确认密码"))},trigger:"blur"}]},editPermissionTreeData:null,editPermissionTreeDefaultProps:{children:"children",label:"name"},editGroupRoleListData:null,editRoleListData:null,editGroupTreeData:[],editGroupTreeDefaultProps:{children:"children",value:"id",label:"name"}}},mounted:function(){this.getPage(),this.getGroupTree(),this.getRoleBaseList(),this.getPermissionTree()},computed:{mainTableEmptyText:function(){return this.isLoading?"加载中...":"暂无数据"}},watch:{},methods:{getPage:function(){var e=this;this.isLoading=!0;var t=this.searchCriteriaForm;n.a.getUserPage(t).then(function(t){e.isLoading=!1,e.page=t.data.page},function(t){e.isLoading=!1,e.showErrorMessage(t.message)})},handlePaginationSizeChange:function(e){this.searchCriteriaForm.pagingInfo.pageSize=e,this.searchCriteriaForm.pagingInfo.pageNumber=1,this.getPage()},handlePaginationCurrentChange:function(e){this.searchCriteriaForm.pagingInfo.pageNumber=e,this.getPage()},getGroupTree:function(){var e=this;n.a.getGroupTree().then(function(t){e.editGroupTreeData=t.data.tree},function(t){e.showErrorMessage(t.message)})},getRoleBaseList:function(){var e=this;n.a.getRoleBaseList().then(function(t){e.editRoleListData=t.data.list},function(t){e.showErrorMessage(t.message)})},getPermissionTree:function(){var e=this;n.a.getPermissionTree().then(function(t){e.editPermissionTreeData=t.data.tree},function(t){e.showErrorMessage(t.message)})},handleSearchAll:function(){this.searchCriteriaForm.pagingInfo.pageNumber=1,this.searchCriteriaForm.keyword=null,this.searchCriteriaForm.groupIds=null,this.searchCriteriaForm.creationTime=null,this.searchCriteriaForm.creationTimeBegin=null,this.searchCriteriaForm.creationTimeEnd=null,this.searchCriteriaForm.status=null,this.searchCriteriaForm.groupIdPath=[],this.getPage()},handleSearch:function(){this.searchCriteriaForm.pagingInfo.pageNumber=1,this.searchCriteriaForm.creationTime&&2===this.searchCriteriaForm.creationTime.length&&(this.searchCriteriaForm.creationTimeBegin=this.searchCriteriaForm.creationTime[0],this.searchCriteriaForm.creationTimeEnd=this.searchCriteriaForm.creationTime[1]),this.searchCriteriaForm.groupIds=this.searchCriteriaForm.groupIdPath&&this.searchCriteriaForm.groupIdPath.length?[this.searchCriteriaForm.groupIdPath[this.searchCriteriaForm.groupIdPath.length-1]]:null,this.getPage()},handleAdd:function(){var e=this;this.validateBaseData()&&(this.activeTabName="first",this.editActive=null,this.mainFormDialogVisible=!0,this.mainForm.userId=null,this.mainForm.status="Normal",this.mainForm.username=null,this.mainForm.displayName=null,this.mainForm.realName=null,this.mainForm.realNameIsValid=!1,this.mainForm.email=null,this.mainForm.emailIsValid=!1,this.mainForm.mobile=null,this.mainForm.mobileIsValid=!1,this.mainForm.groupIdPath=[],this.mainForm.groupId=null,this.mainForm.groupIds=[],this.mainForm.roleId=null,this.editGroupRoleListData=[],this.mainForm.roleIds=[],this.mainForm.permissionIds=null,this.mainForm.password=null,this.mainForm.passwordConfirm=null,this.mainForm.description=null,this.mainForm.headUrl=null,this.mainForm.logoUrl=null,this.mainForm.isDeveloper=!1,this.mainForm.isTester=!1,this.$nextTick(function(){e.$refs.editGroupTree.setCheckedKeys([],!0),e.$refs.editPermissionTree.setCheckedKeys([],!0),e.clearValidate("mainForm")}))},handleEdit:function(e){var t=this;console.log("handleEdit",e),this.validateBaseData()&&e&&(this.activeTabName="first",this.editActive=e,this.mainFormDialogVisible=!0,this.mainForm.userId=e.userId,this.mainForm.status=e.status,this.mainForm.username=e.username,this.mainForm.displayName=e.displayName,this.mainForm.realName=e.realName,this.mainForm.realNameIsValid=e.realNameIsValid,this.mainForm.email=e.email,this.mainForm.emailIsValid=e.emailIsValid,this.mainForm.mobile=e.mobile,this.mainForm.mobileIsValid=e.mobileIsValid,this.getGroupIdPath(this.editGroupTreeData,e.group.groupId),this.mainForm.groupId=e.group.groupId,this.mainForm.groupIds=e.groups.map(function(e){return e.groupId}),this.mainForm.roleId=e.role?e.role.roleId:null,this.getGroupAvailableRoles(this.editGroupTreeData,e.group.groupId),this.mainForm.roleIds=e.roles.map(function(e){return e.roleId}),this.mainForm.permissionIds=e.permissions.map(function(e){return e.permissionId}),this.mainForm.password=null,this.mainForm.passwordConfirm=null,this.mainForm.description=e.description,this.mainForm.headUrl=e.headUrl,this.mainForm.logoUrl=e.logoUrl,this.mainForm.isDeveloper=e.isDeveloper,this.mainForm.isTester=e.isTester,this.$nextTick(function(){t.$refs.editGroupTree.setCheckedKeys(t.mainForm.groupIds,!0),t.$refs.editPermissionTree.setCheckedKeys(t.mainForm.permissionIds,!0),t.clearValidate("mainForm")}))},handleMainFormSure:function(e){console.log("handleMainFormSure",e),e?this.editActive?this.edit():this.add():this.mainFormDialogVisible=!1},handleRemove:function(e){var t=this;this.removeActive=e,this.$confirm("删除该用户后，相关的数据也将被删除。是否继续?","提示",{confirmButtonText:"确定",cancelButtonText:"取消",type:"warning"}).then(function(){t.remove()}).catch(function(){t.removeActive=null})},handleGroupCascaderChange:function(e){console.log(e),this.mainForm.roleId=null,this.editGroupRoleListData=[],0!==e.length&&this.getGroupAvailableRoles(this.editGroupTreeData,e[e.length-1])},add:function(){var e=this;this.$refs.mainForm.validate(function(t){if(!t)return!1;e.isLoading=!0,e.mainForm.groupId=e.mainForm.groupIdPath[e.mainForm.groupIdPath.length-1];var r=l.a.cloneDeep(e.mainForm);e.params.password&&(r.password=u()(r.password)),r.passwordConfirm&&(r.passwordConfirm=u()(r.passwordConfirm)),n.a.addUser(r).then(function(t){e.isLoading=!1,e.mainFormDialogVisible=!1,e.getPage()},function(t){e.isLoading=!1,e.showErrorMessage(t.message)})})},edit:function(){var e=this;this.editActive?this.$refs.mainForm.validate(function(t){if(!t)return!1;e.isLoading=!0,e.mainForm.groupId=e.mainForm.groupIdPath[e.mainForm.groupIdPath.length-1];var r=l.a.cloneDeep(e.mainForm);r.password&&(r.password=u()(r.password)),r.passwordConfirm&&(r.passwordConfirm=u()(r.passwordConfirm)),n.a.editUser(r).then(function(t){e.isLoading=!1,e.editActive=null,e.mainFormDialogVisible=!1,e.getPage()},function(t){e.isLoading=!1,e.showErrorMessage(t.message)})}):this.showErrorMessage("异常：无编辑目标")},remove:function(){var e=this;if(this.removeActive){var t={userId:this.removeActive.userId};this.isLoading=!0,n.a.removeUser(t).then(function(t){e.isLoading=!1,e.removeActive=null,e.getPage()},function(t){e.isLoading=!1,e.showErrorMessage(t.message)})}},validateBaseData:function(){return this.editGroupTreeData&&0!==this.editGroupTreeData.length?this.editRoleListData?!!this.editPermissionTreeData||(this.showErrorMessage("基础数据缺失：权限列表"),!1):(this.showErrorMessage("基础数据缺失：角色列表"),!1):(this.showErrorMessage("基础数据缺失：分组列表"),!1)},handleGroupTreeCheckChange:function(e,t,r){this.mainForm.groupIds=this.$refs.editGroupTree.getCheckedKeys()},handlePermissionTreeCheckChange:function(e,t,r){this.mainForm.permissionIds=this.$refs.editPermissionTree.getCheckedKeys()},getGroupIdPath:function(e,t){if(e){var r=!0,a=!1,n=void 0;try{for(var o,l=i()(e);!(r=(o=l.next()).done);r=!0){var s=o.value;if(s.id===t){this.mainForm.groupIdPath=s.parentIdPath?s.parentIdPath.concat():[],this.mainForm.groupIdPath.push(t);break}this.getGroupIdPath(s.children,t)}}catch(e){a=!0,n=e}finally{try{!r&&l.return&&l.return()}finally{if(a)throw n}}}},getGroupAvailableRoles:function(e,t){if(this.editGroupRoleListData=[],e){var r=!0,a=!1,n=void 0;try{for(var o,l=i()(e);!(r=(o=l.next()).done);r=!0){var s=o.value;if(s.id===t){this.editGroupRoleListData=s.availableRoles;break}this.getGroupAvailableRoles(s.children,t)}}catch(e){a=!0,n=e}finally{try{!r&&l.return&&l.return()}finally{if(a)throw n}}}},resetForm:function(e){this.$refs[e].resetFields()},clearValidate:function(e){this.$refs[e].clearValidate()},showErrorMessage:function(e){this.$message({message:e,type:"error"})},handleSortChange:function(e){this.searchCriteriaForm.pagingInfo.sortInfo.sort=e.prop,this.searchCriteriaForm.pagingInfo.sortInfo.sortDir="descending"===e.order?"DESC":"ASC",this.searchCriteriaForm.pagingInfo.pageNumber=1,this.getPage()},handleChangeHeadUrlBrowser:function(){this.popupFileManager("headUrl")},handleChangeLogoUrlBrowser:function(){this.popupFileManager("logoUrl")},popupFileManager:function(e){var t=this;try{CKFinder.popup({chooseFiles:!0,width:800,height:600,onInit:function(r){r.on("files:choose",function(r){var a=r.data.files.first();t.mainForm[e]=a.getUrl()}),r.on("file:choose:resizedImage",function(r){t.mainForm[e]=r.data.resizedUrl})}})}catch(e){console.log(e.message)}}}}},oqYR:function(e,t){},"rMx+":function(e,t,r){"use strict";var a={render:function(){var e=this,t=e.$createElement;return(e._self._c||t)("el-cascader",{attrs:{options:e.treeData,props:e.defaultProps,clearable:"",filterable:"",placeholder:"请选择分组","change-on-select":""},on:{change:e.handleChange},model:{value:e.currentValue,callback:function(t){e.currentValue=t},expression:"currentValue"}})},staticRenderFns:[]};t.a=a},t7Yk:function(e,t,r){"use strict";var a=r("N0+e"),i=r("uCBo"),n=r("VU/8")(a.a,i.a,!1,null,null,null);t.a=n.exports},tvR6:function(e,t){},uCBo:function(e,t,r){"use strict";var a={render:function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("el-select",{attrs:{clearable:"",placeholder:"状态"},model:{value:e.currentValue,callback:function(t){e.currentValue=t},expression:"currentValue"}},e._l(e.userStatusData,function(e){return r("el-option",{key:e.value,attrs:{label:e.label,value:e.value}})}),1)},staticRenderFns:[]};t.a=a},uN2V:function(e,t){},vT38:function(e,t,r){"use strict";t.a={name:"XLDatePicker",props:{value:{type:Array}},data:function(){return{currentValue:this.value,pickerOptions:{shortcuts:[{text:"最近一周",onClick:function(e){var t=new Date,r=new Date;r.setTime(r.getTime()-6048e5),e.$emit("pick",[r,t])}},{text:"最近两周",onClick:function(e){var t=new Date,r=new Date;r.setTime(r.getTime()-12096e5),e.$emit("pick",[r,t])}},{text:"最近一个月",onClick:function(e){var t=new Date,r=new Date;r.setTime(r.getTime()-2592e6),e.$emit("pick",[r,t])}},{text:"最近两个月",onClick:function(e){var t=new Date,r=new Date;r.setTime(r.getTime()-5184e6),e.$emit("pick",[r,t])}}]}}},watch:{value:function(e){this.currentValue=e},currentValue:function(e){this.$emit("input",e)}}}}},["IlS5"]);
//# sourceMappingURL=user.js.map