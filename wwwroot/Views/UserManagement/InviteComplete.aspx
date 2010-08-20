<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.InviteViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Invite
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
ViewData["SubMenuLocation"] = "Invite";
Html.RenderPartial(MVC.Shared.Views.ctrlAdminMenu);
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded  cw-rounded-corners-bottom">

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
	   <li><%: recipient %></li>
	   <%} %>
	 </ul>
	 </fieldset>  
	 <%} %>
	 </div>
</asp:Content>

