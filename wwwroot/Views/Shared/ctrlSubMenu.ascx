<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.MenuViewModel>" %>
<ul id="sub-menu" class="cw-menu cw-menu-sub">                        
<%
foreach (var menuItem in Model.MenuItems)
{

	if ((!menuItem.IsAdmin) || ((menuItem.IsAdmin) && Page.User.UserIsAnyAdmin()))
    {                    
    %>
        <li class="<%: menuItem.IsCurrent ? "current" : ""%>">
        <%if (menuItem.LinkControllerName != null)
        {%>
            <%: Html.ActionLink(menuItem.LinkDisplayName, menuItem.LinkActionName, menuItem.LinkControllerName)%>
        <%}
        else
        { %>
            <a href="#"><%:menuItem.LinkDisplayName%></a>
        <%} %>
        </li>
        <%
    }
} %>
</ul>