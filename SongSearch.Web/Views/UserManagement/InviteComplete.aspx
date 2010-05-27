<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.InviteViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Invite
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

    menu.Add("Manage Users", new string[4] { "Index", "UserManagement", "Admin", "" });
    menu.Add("Manage Catalogs", new string[4] { "Index", "Catalog", "Admin", "" });
    menu.Add("Invite", new string[4] { "Invite", "UserManagement", "Admin", "current" });
	
	Html.RenderPartial("ctrlSubMenu", menu);
		
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class = "cw-outl">

    <h2>Invite Sent</h2>
    <%if (Model.Recipients.Count() > 0)
      { %>
    <p>&nbsp;</p>
    <p>You've succesfully sent registration invites to the following recipients:</p> 
    <p>&nbsp;</p>
    <fieldset id="invite">
    <legend>
       Recipients
    </legend>
       <ul id="invite-list">
       <%foreach (string recipient in Model.Recipients)
         { %>
       <li><%=Html.Encode(recipient)%></li>
       <%} %>
     </ul>
     </fieldset>  
     <%} %>
     </div>
</asp:Content>

