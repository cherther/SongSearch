﻿// <auto-generated />
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

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
public static class MVC {
    public static SongSearch.Web.Controllers.AccountController Account = new SongSearch.Web.Controllers.T4MVC_AccountController();
    public static SongSearch.Web.Controllers.AdminController Admin = new SongSearch.Web.Controllers.T4MVC_AdminController();
    public static SongSearch.Web.Controllers.CartController Cart = new SongSearch.Web.Controllers.T4MVC_CartController();
    public static SongSearch.Web.Controllers.CatalogManagementController CatalogManagement = new SongSearch.Web.Controllers.T4MVC_CatalogManagementController();
    public static SongSearch.Web.Controllers.CatalogUploadController CatalogUpload = new SongSearch.Web.Controllers.T4MVC_CatalogUploadController();
    public static SongSearch.Web.Controllers.ContentController Content = new SongSearch.Web.Controllers.T4MVC_ContentController();
    public static SongSearch.Web.Controllers.ErrorController Error = new SongSearch.Web.Controllers.T4MVC_ErrorController();
    public static SongSearch.Web.Controllers.HomeController Home = new SongSearch.Web.Controllers.T4MVC_HomeController();
    public static SongSearch.Web.Controllers.MediaController Media = new SongSearch.Web.Controllers.T4MVC_MediaController();
    public static SongSearch.Web.Controllers.SearchController Search = new SongSearch.Web.Controllers.T4MVC_SearchController();
    public static SongSearch.Web.UserManagementController UserManagement = new SongSearch.Web.T4MVC_UserManagementController();
    public static T4MVC.SharedController Shared = new T4MVC.SharedController();
}

namespace T4MVC {
}

namespace System.Web.Mvc {
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class T4Extensions {
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, ActionResult result) {
            return htmlHelper.RouteLink(linkText, result.GetRouteValueDictionary());
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, ActionResult result, object htmlAttributes) {
            return ActionLink(htmlHelper, linkText, result, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, ActionResult result, IDictionary<string, object> htmlAttributes) {
            return htmlHelper.RouteLink(linkText, result.GetRouteValueDictionary(), htmlAttributes);
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, ActionResult result, FormMethod formMethod) {
            return htmlHelper.BeginForm(result, formMethod, null);
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, ActionResult result, FormMethod formMethod, object htmlAttributes) {
            return BeginForm(htmlHelper, result, formMethod, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, ActionResult result, FormMethod formMethod, IDictionary<string, object> htmlAttributes) {
            var callInfo = result.GetT4MVCResult();
            return htmlHelper.BeginForm(callInfo.Action, callInfo.Controller, callInfo.RouteValueDictionary, formMethod, htmlAttributes);
        }

        public static void RenderAction(this HtmlHelper htmlHelper, ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            htmlHelper.RenderAction(callInfo.Action, callInfo.Controller, callInfo.RouteValueDictionary);
        }

        public static MvcHtmlString Action(this HtmlHelper htmlHelper, ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return htmlHelper.Action(callInfo.Action, callInfo.Controller, callInfo.RouteValueDictionary);
        }
        public static string Action(this UrlHelper urlHelper, ActionResult result) {
            return urlHelper.RouteUrl(result.GetRouteValueDictionary());
        }

        public static string ActionAbsolute(this UrlHelper urlHelper, ActionResult result) {
            return string.Format("{0}{1}",urlHelper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority),
                urlHelper.RouteUrl(result.GetRouteValueDictionary()));
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, string linkText, ActionResult result, AjaxOptions ajaxOptions) {
            return ajaxHelper.RouteLink(linkText, result.GetRouteValueDictionary(), ajaxOptions);
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, string linkText, ActionResult result, AjaxOptions ajaxOptions, object htmlAttributes) {
            return ajaxHelper.RouteLink(linkText, result.GetRouteValueDictionary(), ajaxOptions, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, string linkText, ActionResult result, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            return ajaxHelper.RouteLink(linkText, result.GetRouteValueDictionary(), ajaxOptions, htmlAttributes);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result) {
            return MapRoute(routes, name, url, result, null /*namespaces*/);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result, object defaults) {
            return MapRoute(routes, name, url, result, defaults, null /*constraints*/, null /*namespaces*/);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result, string[] namespaces) {
            return MapRoute(routes, name, url, result, null /*defaults*/, namespaces);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result, object defaults, object constraints) {
            return MapRoute(routes, name, url, result, defaults, constraints, null /*namespaces*/);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result, object defaults, string[] namespaces) {
            return MapRoute(routes, name, url, result, defaults, null /*constraints*/, namespaces);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result, object defaults, object constraints, string[] namespaces) {
            // Create and add the route
            var route = CreateRoute(url, result, defaults, constraints, namespaces);
            routes.Add(name, route);
            return route;
        }

        // Note: can't name the AreaRegistrationContext methods 'MapRoute', as that conflicts with the existing methods
        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result) {
            return MapRouteArea(context, name, url, result, null /*namespaces*/);
        }

        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result, object defaults) {
            return MapRouteArea(context, name, url, result, defaults, null /*constraints*/, null /*namespaces*/);
        }

        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result, string[] namespaces) {
            return MapRouteArea(context, name, url, result, null /*defaults*/, namespaces);
        }

        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result, object defaults, object constraints) {
            return MapRouteArea(context, name, url, result, defaults, constraints, null /*namespaces*/);
        }

        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result, object defaults, string[] namespaces) {
            return MapRouteArea(context, name, url, result, defaults, null /*constraints*/, namespaces);
        }

        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result, object defaults, object constraints, string[] namespaces) {
            // Create and add the route
            var route = CreateRoute(url, result, defaults, constraints, namespaces);
            context.Routes.Add(name, route);
            route.DataTokens["area"] = context.AreaName;
            return route;
        }

        private static Route CreateRoute(string url, ActionResult result, object defaults, object constraints, string[] namespaces) {
            // Start by adding the default values from the anonymous object (if any)
            var routeValues = new RouteValueDictionary(defaults);

            // Then add the Controller/Action names and the parameters from the call
            foreach (var pair in result.GetRouteValueDictionary()) {
                routeValues.Add(pair.Key, pair.Value);
            }

            var routeConstraints = new RouteValueDictionary(constraints);

            // Create and add the route
            var route = new Route(url, routeValues, routeConstraints, new MvcRouteHandler());

            route.DataTokens = new RouteValueDictionary();

            if (namespaces != null && namespaces.Length > 0) {
                route.DataTokens["Namespaces"] = namespaces;
            }

            return route;
        }

        public static IT4MVCActionResult GetT4MVCResult(this ActionResult result) {
            var t4MVCResult = result as IT4MVCActionResult;
            if (t4MVCResult == null) {
                throw new InvalidOperationException("T4MVC methods can only be passed pseudo-action calls (e.g. MVC.Home.About()), and not real action calls.");
            }
            return t4MVCResult;
        }

        public static RouteValueDictionary GetRouteValueDictionary(this ActionResult result) {
            return result.GetT4MVCResult().RouteValueDictionary;
        }

        public static ActionResult AddRouteValues(this ActionResult result, object routeValues) {
            return result.AddRouteValues(new RouteValueDictionary(routeValues));
        }

        public static ActionResult AddRouteValues(this ActionResult result, RouteValueDictionary routeValues) {
            RouteValueDictionary currentRouteValues = result.GetRouteValueDictionary();

            // Add all the extra values
            foreach (var pair in routeValues) {
                currentRouteValues.Add(pair.Key, pair.Value);
            }

            return result;
        }

        public static ActionResult AddRouteValues(this ActionResult result, System.Collections.Specialized.NameValueCollection nameValueCollection) {
            // Copy all the values from the NameValueCollection into the route dictionary
            nameValueCollection.CopyTo(result.GetRouteValueDictionary());
            return result;
        }

        public static ActionResult AddRouteValue(this ActionResult result, string name, object value) {
            RouteValueDictionary routeValues = result.GetRouteValueDictionary();
            routeValues.Add(name, value);
            return result;
        }
        
        public static void InitMVCT4Result(this IT4MVCActionResult result, string area, string controller, string action) {
            result.Controller = controller;
            result.Action = action;
            result.RouteValueDictionary = new RouteValueDictionary();
             
            result.RouteValueDictionary.Add("Controller", controller);
            result.RouteValueDictionary.Add("Action", action);
        }

        public static bool FileExists(string virtualPath) {
            if (!HostingEnvironment.IsHosted) return false;
            string filePath = HostingEnvironment.MapPath(virtualPath);
            return System.IO.File.Exists(filePath);
        }

        static DateTime CenturyBegin=new DateTime(2001,1,1);
        public static string TimestampString(string virtualPath) {
            if (!HostingEnvironment.IsHosted) return string.Empty;
            string filePath = HostingEnvironment.MapPath(virtualPath);
            return Convert.ToString((System.IO.File.GetLastWriteTimeUtc(filePath).Ticks-CenturyBegin.Ticks)/1000000000,16);            
        }
    }
}

   
[GeneratedCode("T4MVC", "2.0")]   
public interface IT4MVCActionResult {   
    string Action { get; set; }   
    string Controller { get; set; }   
    RouteValueDictionary RouteValueDictionary { get; set; }   
}   
  

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
public class T4MVC_ActionResult : System.Web.Mvc.ActionResult, IT4MVCActionResult {
    public T4MVC_ActionResult(string area, string controller, string action): base()  {
        this.InitMVCT4Result(area, controller, action);
    }
     
    public override void ExecuteResult(System.Web.Mvc.ControllerContext context) { }
    
    public string Controller { get; set; }
    public string Action { get; set; }
    public RouteValueDictionary RouteValueDictionary { get; set; }
}
[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
public class T4MVC_JsonResult : System.Web.Mvc.JsonResult, IT4MVCActionResult {
    public T4MVC_JsonResult(string area, string controller, string action): base()  {
        this.InitMVCT4Result(area, controller, action);
    }
    
    public string Controller { get; set; }
    public string Action { get; set; }
    public RouteValueDictionary RouteValueDictionary { get; set; }
}



namespace Links {
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class Scripts {
        private const string URLPATH = "~/Scripts";
        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
        public static readonly string cw_app_ajax_chirp_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/cw.app.ajax.chirp.min.js") ? Url("cw.app.ajax.chirp.min.js") : Url("cw.app.ajax.chirp.js");
                      
        public static readonly string cw_app_ajax_min_js = Url("cw.app.ajax.min.js");
        public static readonly string cw_app_events_chirp_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/cw.app.events.chirp.min.js") ? Url("cw.app.events.chirp.min.js") : Url("cw.app.events.chirp.js");
                      
        public static readonly string cw_app_events_min_js = Url("cw.app.events.min.js");
        public static readonly string cw_app_main_chirp_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/cw.app.main.chirp.min.js") ? Url("cw.app.main.chirp.min.js") : Url("cw.app.main.chirp.js");
                      
        public static readonly string cw_app_main_min_js = Url("cw.app.main.min.js");
        public static readonly string cw_app_sound_chirp_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/cw.app.sound.chirp.min.js") ? Url("cw.app.sound.chirp.min.js") : Url("cw.app.sound.chirp.js");
                      
        public static readonly string cw_app_sound_min_js = Url("cw.app.sound.min.js");
        public static readonly string cw_app_upload_chirp_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/cw.app.upload.chirp.min.js") ? Url("cw.app.upload.chirp.min.js") : Url("cw.app.upload.chirp.js");
                      
        public static readonly string cw_app_upload_min_js = Url("cw.app.upload.min.js");
        public static readonly string gears_init_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/gears_init.min.js") ? Url("gears_init.min.js") : Url("gears_init.js");
                      
        public static readonly string jquery_1_4_2_min_js = Url("jquery-1.4.2.min.js");
        public static readonly string jquery_ui_min_js = Url("jquery-ui.min.js");
        public static readonly string jquery_blockUI_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.blockUI.min.js") ? Url("jquery.blockUI.min.js") : Url("jquery.blockUI.js");
                      
        public static readonly string jquery_feedbackbar_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.feedbackbar.min.js") ? Url("jquery.feedbackbar.min.js") : Url("jquery.feedbackbar.js");
                      
        public static readonly string jquery_feedbackbar_min_js = Url("jquery.feedbackbar.min.js");
        public static readonly string jquery_form_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.form.min.js") ? Url("jquery.form.min.js") : Url("jquery.form.js");
                      
        public static readonly string jquery_gritter_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.gritter.min.js") ? Url("jquery.gritter.min.js") : Url("jquery.gritter.js");
                      
        public static readonly string jquery_gritter_min_js = Url("jquery.gritter.min.js");
        public static readonly string jquery_plupload_queue_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.plupload.queue.min.js") ? Url("jquery.plupload.queue.min.js") : Url("jquery.plupload.queue.js");
                      
        public static readonly string jquery_plupload_queue_min_js = Url("jquery.plupload.queue.min.js");
        public static readonly string jquery_tools_min_js = Url("jquery.tools.min.js");
        public static readonly string jquery_validate_vsdoc_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.validate-vsdoc.min.js") ? Url("jquery.validate-vsdoc.min.js") : Url("jquery.validate-vsdoc.js");
                      
        public static readonly string jquery_validate_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.validate.min.js") ? Url("jquery.validate.min.js") : Url("jquery.validate.js");
                      
        public static readonly string jquery_validate_min_js = Url("jquery.validate.min.js");
        public static readonly string MicrosoftAjax_debug_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/MicrosoftAjax.debug.min.js") ? Url("MicrosoftAjax.debug.min.js") : Url("MicrosoftAjax.debug.js");
                      
        public static readonly string MicrosoftAjax_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/MicrosoftAjax.min.js") ? Url("MicrosoftAjax.min.js") : Url("MicrosoftAjax.js");
                      
        public static readonly string MicrosoftMvcAjax_debug_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/MicrosoftMvcAjax.debug.min.js") ? Url("MicrosoftMvcAjax.debug.min.js") : Url("MicrosoftMvcAjax.debug.js");
                      
        public static readonly string MicrosoftMvcAjax_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/MicrosoftMvcAjax.min.js") ? Url("MicrosoftMvcAjax.min.js") : Url("MicrosoftMvcAjax.js");
                      
        public static readonly string MicrosoftMvcValidation_debug_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/MicrosoftMvcValidation.debug.min.js") ? Url("MicrosoftMvcValidation.debug.min.js") : Url("MicrosoftMvcValidation.debug.js");
                      
        public static readonly string MicrosoftMvcValidation_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/MicrosoftMvcValidation.min.js") ? Url("MicrosoftMvcValidation.min.js") : Url("MicrosoftMvcValidation.js");
                      
        public static readonly string plupload_browserplus_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/plupload.browserplus.min.js") ? Url("plupload.browserplus.min.js") : Url("plupload.browserplus.js");
                      
        public static readonly string plupload_browserplus_min_js = Url("plupload.browserplus.min.js");
        public static readonly string plupload_flash_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/plupload.flash.min.js") ? Url("plupload.flash.min.js") : Url("plupload.flash.js");
                      
        public static readonly string plupload_flash_min_js = Url("plupload.flash.min.js");
        public static readonly string plupload_full_min_js = Url("plupload.full.min.js");
        public static readonly string plupload_gears_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/plupload.gears.min.js") ? Url("plupload.gears.min.js") : Url("plupload.gears.js");
                      
        public static readonly string plupload_gears_min_js = Url("plupload.gears.min.js");
        public static readonly string plupload_html4_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/plupload.html4.min.js") ? Url("plupload.html4.min.js") : Url("plupload.html4.js");
                      
        public static readonly string plupload_html4_min_js = Url("plupload.html4.min.js");
        public static readonly string plupload_html5_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/plupload.html5.min.js") ? Url("plupload.html5.min.js") : Url("plupload.html5.js");
                      
        public static readonly string plupload_html5_min_js = Url("plupload.html5.min.js");
        public static readonly string plupload_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/plupload.min.js") ? Url("plupload.min.js") : Url("plupload.js");
                      
        public static readonly string plupload_min_js = Url("plupload.min.js");
        public static readonly string plupload_silverlight_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/plupload.silverlight.min.js") ? Url("plupload.silverlight.min.js") : Url("plupload.silverlight.js");
                      
        public static readonly string plupload_silverlight_min_js = Url("plupload.silverlight.min.js");
        public static readonly string soundmanager2_nodebug_jsmin_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/soundmanager2-nodebug-jsmin.min.js") ? Url("soundmanager2-nodebug-jsmin.min.js") : Url("soundmanager2-nodebug-jsmin.js");
                      
        public static readonly string soundmanager2_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/soundmanager2.min.js") ? Url("soundmanager2.min.js") : Url("soundmanager2.js");
                      
    }

}

static class T4MVCHelpers {
    // You can change the ProcessVirtualPath method to modify the path that gets returned to the client.
    // e.g. you can prepend a domain, or append a query string:
    //      return "http://localhost" + path + "?foo=bar";
    private static string ProcessVirtualPathDefault(string virtualPath) {
        // The path that comes in starts with ~/ and must first be made absolute
        string path = VirtualPathUtility.ToAbsolute(virtualPath);
        
        // Add your own modifications here before returning the path
        return path;
    }

    // Calling ProcessVirtualPath through delegate to allow it to be replaced for unit testing
    public static Func<string, string> ProcessVirtualPath = ProcessVirtualPathDefault;


    // Logic to determine if the app is running in production or dev environment
    public static bool IsProduction() { 
        return (HttpContext.Current != null && !HttpContext.Current.IsDebuggingEnabled); 
    }
}




namespace T4MVC {
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class Dummy {
        private Dummy() { }
        public static Dummy Instance = new Dummy();
    }
}

#endregion T4MVC
#pragma warning restore 1591


