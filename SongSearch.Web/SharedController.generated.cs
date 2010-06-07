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
namespace T4MVC {
    public class SharedController {

        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string ctrlBuildInfo = "~/Views/Shared/ctrlBuildInfo.ascx";
            public readonly string ctrlLogIn = "~/Views/Shared/ctrlLogIn.ascx";
            public readonly string ctrlMainMenu = "~/Views/Shared/ctrlMainMenu.ascx";
            public readonly string ctrlMediaplayer = "~/Views/Shared/ctrlMediaplayer.ascx";
            public readonly string ctrlSubMenu = "~/Views/Shared/ctrlSubMenu.ascx";
            public readonly string ctrlTagCloud = "~/Views/Shared/ctrlTagCloud.ascx";
            public readonly string ctrlTerritoryCloud = "~/Views/Shared/ctrlTerritoryCloud.ascx";
            public readonly string Error = "~/Views/Shared/Error.aspx";
            static readonly _DisplayTemplates s_DisplayTemplates = new _DisplayTemplates();
            public _DisplayTemplates DisplayTemplates { get { return s_DisplayTemplates; } }
            public partial class _DisplayTemplates{
                public readonly string _Object = "~/Views/Shared/DisplayTemplates/_Object.ascx";
                public readonly string MultilineText = "~/Views/Shared/DisplayTemplates/MultilineText.ascx";
            }
            static readonly _EditorTemplates s_EditorTemplates = new _EditorTemplates();
            public _EditorTemplates EditorTemplates { get { return s_EditorTemplates; } }
            public partial class _EditorTemplates{
                public readonly string MultilineText = "~/Views/Shared/EditorTemplates/MultilineText.ascx";
                public readonly string StringAutoComplete = "~/Views/Shared/EditorTemplates/StringAutoComplete.ascx";
            }
        }
    }

}

#endregion T4MVC
#pragma warning restore 1591
