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
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
	<h2>Invite New Users</h2>
	<div>&nbsp;</div>
	<div class="cw-outl cw-padded cw-rounded-corners">
		<p>Please enter one or many e-mail addresses (separated by commas).</p> 
		<p>After you click 'Send', an e-mail invitation will be sent to each address with a personalized invitation code and instructions on how to complete the registration process.</p>
		<p> This invitation code, along with their e-mail address will allow the recipient to register on the site using a password of their choice.</p>
	
		<% using (Html.BeginForm("Invite", "UserManagement", FormMethod.Post, new { @class = "cw-form-small" }))
		   {
			   //string baseUrl = Html.ActionLink("register", "Register").ToString();
			   %>
		   <%:Html.AntiForgeryToken() %>
		 <div>&nbsp;</div>
		 <fieldset id="invite">
			<legend>
			   Invite
			</legend>           
		   <div>&nbsp;</div>
		   <div><%:Html.Label("From:")%>&nbsp;<%: String.Format("{0} <{1}>", Html.Friendly(), User.Identity.Name) %></div>
		   <div>&nbsp;</div>
		   <div><%:Html.Label("To:")%></div>
		   <%: Html.ValidationMessageFor(m => m.Recipient)%>
		   <div>
		   <%:Html.TextArea("Recipient", new { cols = "50", rows = 10 })%>     
		   </div>
		   <div>
			<button id="invite-send" type="submit" title="Send" class="cw-button cw-simple cw-blue">
			<span class="b-email">Send</span>
			</button>
		   </div>
	   
		 </fieldset>  
		<%}%>
	</div>
</div>
</asp:Content>