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
namespace SongSearch.Web.Controllers {
    public partial class MediaController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public MediaController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected MediaController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Download() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Download);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Stream() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Stream);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult StreamUrl() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.StreamUrl);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public MediaController Actions { get { return MVC.Media; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Media";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Download = "Download";
            public readonly string Stream = "Stream";
            public readonly string StreamUrl = "StreamUrl";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_MediaController: SongSearch.Web.Controllers.MediaController {
        public T4MVC_MediaController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Download(int id) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Download);
            callInfo.RouteValueDictionary.Add("id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Stream(int id, SongSearch.Web.MediaVersion version) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Stream);
            callInfo.RouteValueDictionary.Add("id", id);
            callInfo.RouteValueDictionary.Add("version", version);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult StreamUrl(int id, SongSearch.Web.MediaVersion version) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.StreamUrl);
            callInfo.RouteValueDictionary.Add("id", id);
            callInfo.RouteValueDictionary.Add("version", version);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
