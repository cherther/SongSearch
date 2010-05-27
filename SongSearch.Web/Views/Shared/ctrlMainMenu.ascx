<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ViewModel>" %>
<%

    IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

    var navLocation = Model == null ? "" : Model.NavigationLocation;
    navLocation = String.IsNullOrEmpty(navLocation) ? ViewData["NavigationLocation"] as string : navLocation;

    menu.Add("Home", new string[3] { "Index", "Home", "Home" });
    menu.Add("Search", new string[3] { "Index", "Search", "Search" });
    if (Page.User.Identity.IsAuthenticated)
    {
        var cartCount = ViewData["MyActiveCartContentsCount"] as string ?? "";
        var cartMnuText = String.Concat("Song Cart", cartCount != null ? String.Format("({0})", cartCount) : "");
        menu.Add("Cart", new string[3] { "Index", "Cart", cartMnuText });
    }
    menu.Add("Help", new string[3] { "Index", "Help", "Help" });
    if (Page.User.UserIsAnyAdmin())
    {
        menu.Add("Admin", new string[3] { "Index", "UserManagement", "Admin" });
    }
    if (!Page.User.Identity.IsAuthenticated)
    {
        menu.Add("Register", new string[3] { "Register", "Account", "Register" });
    }
       
    %>
    
        <ul id="main-menu" class="cw-menu cw-menu-main">
        <%
        foreach(var menuItem in menu)
        {
            string[] values = menuItem.Value;        
            string menuId = String.Format("menu-{0}", menuItem.Key.ToLower());
                %>
            <li id="<%=menuId %>" class="<%= menuItem.Key == navLocation ? "current" : ""%>">
            <%= Html.ActionLink(values[2], values[0], values[1])%>
            </li>
            <%
                //}
            } %>
        </ul>
