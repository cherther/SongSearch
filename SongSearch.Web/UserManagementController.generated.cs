// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace SongSearch.Web {
    public partial class UserManagementController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected UserManagementController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Detail() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Detail);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult UpdateRole() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.UpdateRole);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult ToggleSystemAdminAccess() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.ToggleSystemAdminAccess);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult UpdateCatalog() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.UpdateCatalog);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult UpdateAllCatalogs() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.UpdateAllCatalogs);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult UpdateAllUsers() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.UpdateAllUsers);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Delete() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Delete);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult TakeOwnership() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.TakeOwnership);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public UserManagementController Actions { get { return MVC.UserManagement; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "UserManagement";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Index = "Index";
            public readonly string Invite = "Invite";
            public readonly string Detail = "Detail";
            public readonly string UpdateRole = "UpdateRole";
            public readonly string ToggleSystemAdminAccess = "ToggleSystemAdminAccess";
            public readonly string UpdateCatalog = "UpdateCatalog";
            public readonly string UpdateAllCatalogs = "UpdateAllCatalogs";
            public readonly string UpdateAllUsers = "UpdateAllUsers";
            public readonly string Delete = "Delete";
            public readonly string TakeOwnership = "TakeOwnership";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Copy_of_ctrlUserList = "~/Views/UserManagement/Copy of ctrlUserList.ascx";
            public readonly string ctrlDetail = "~/Views/UserManagement/ctrlDetail.ascx";
            public readonly string ctrlUserList = "~/Views/UserManagement/ctrlUserList.ascx";
            public readonly string Index = "~/Views/UserManagement/Index.aspx";
            public readonly string Invite = "~/Views/UserManagement/Invite.aspx";
            public readonly string InviteComplete = "~/Views/UserManagement/InviteComplete.aspx";
            public readonly string InviteMessage = "~/Views/UserManagement/InviteMessage.ascx";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_UserManagementController: SongSearch.Web.UserManagementController {
        public T4MVC_UserManagementController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Index() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Invite() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Invite);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Invite(SongSearch.Web.InviteViewModel model) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Invite);
            callInfo.RouteValueDictionary.Add("model", model);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Detail(int id) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Detail);
            callInfo.RouteValueDictionary.Add("id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult UpdateRole(int userId, int roleId) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.UpdateRole);
            callInfo.RouteValueDictionary.Add("userId", userId);
            callInfo.RouteValueDictionary.Add("roleId", roleId);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult ToggleSystemAdminAccess(int userId) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.ToggleSystemAdminAccess);
            callInfo.RouteValueDictionary.Add("userId", userId);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult UpdateCatalog(int userId, int catalogId, int roleId) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.UpdateCatalog);
            callInfo.RouteValueDictionary.Add("userId", userId);
            callInfo.RouteValueDictionary.Add("catalogId", catalogId);
            callInfo.RouteValueDictionary.Add("roleId", roleId);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult UpdateAllCatalogs(int userId, int roleId) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.UpdateAllCatalogs);
            callInfo.RouteValueDictionary.Add("userId", userId);
            callInfo.RouteValueDictionary.Add("roleId", roleId);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult UpdateAllUsers(int catalogId, int roleId) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.UpdateAllUsers);
            callInfo.RouteValueDictionary.Add("catalogId", catalogId);
            callInfo.RouteValueDictionary.Add("roleId", roleId);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Delete(int id) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Delete);
            callInfo.RouteValueDictionary.Add("id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult TakeOwnership(int id) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.TakeOwnership);
            callInfo.RouteValueDictionary.Add("id", id);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
