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
    public partial class HomeController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public HomeController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected HomeController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Profile() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Profile);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public HomeController Actions { get { return MVC.Home; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Home";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Index = "Index";
            public readonly string Contact = "Contact";
            public readonly string Profile = "Profile";
            public readonly string PrivacyPolicy = "PrivacyPolicy";
            public readonly string TermsOfUse = "TermsOfUse";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Contact = "~/Views/Home/Contact.aspx";
            public readonly string Index = "~/Views/Home/Index.aspx";
            public readonly string PrivacyPolicy = "~/Views/Home/PrivacyPolicy.aspx";
            public readonly string TermsOfUse = "~/Views/Home/TermsOfUse.aspx";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_HomeController: SongSearch.Web.Controllers.HomeController {
        public T4MVC_HomeController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Index() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Contact() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Contact);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Contact(SongSearch.Web.ContactUsModel model) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Contact);
            callInfo.RouteValueDictionary.Add("model", model);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Profile(string profileName) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Profile);
            callInfo.RouteValueDictionary.Add("profileName", profileName);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult PrivacyPolicy() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.PrivacyPolicy);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult TermsOfUse() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.TermsOfUse);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
