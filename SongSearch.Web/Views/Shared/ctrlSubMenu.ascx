<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IDictionary<string, string[]>>" %>
<%

    IDictionary<string, string[]> menu = Model as IDictionary<string, string[]>;
           
    %>
<ul id="sub-menu" class="cw-menu cw-menu-sub">                        
<%
foreach(var menuItem in menu)
{
    string[] values = menuItem.Value;        
                                
    if ((values[2] != "Admin") || ((values[2] == "Admin") && Page.User.UserIsAnyAdmin()))
    {                    
    %>
        <li class="<%: values[3] == "current" ? "current" : ""%>">
        <%if (values[1] != null)
        {%>
            <%: Html.ActionLink(menuItem.Key, values[0], values[1])%>
        <%}
        else
        { %>
            <a href="#"><%:menuItem.Key %></a>
        <%} %>
        </li>
        <%
    }
} %>
</ul>