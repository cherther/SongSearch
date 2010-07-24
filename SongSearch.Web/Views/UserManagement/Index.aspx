﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.UserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.PageTitle %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

	menu.Add("Manage Users", new string[4] { "Index", "UserManagement", "Admin", "current" });
	menu.Add("Manage Catalogs", new string[4] { "Index", "CatalogManagement", "Admin", "" });
	menu.Add("Catalog Upload", new string[4] { "Upload", "CatalogUpload", "Admin", "" });
	menu.Add("Invite", new string[4] { "Invite", "UserManagement", "Admin", "" });

	Html.RenderPartial(MVC.Shared.Views.ctrlSubMenu, menu);
		
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
	<h2><%: Model.PageTitle %></h2>

	<%if (Model != null) {
		var users = Model.MyUsers;
		var invites = Model.MyInvites;
		users = users.OrderBy(u => u.ParentUserId).ThenBy(u => u.UserName).ToList();
	%>
	<div>&nbsp;</div>
	<h3>Users</h3>
	<div>&nbsp;</div>
	
	<table>
	<tr>
		<td style="vertical-align: top">
			<%if (users.Count() > 0) { %>
			<div class = "cw-outl cw-padded cw-rounded-corners" style="overflow:auto ; height: 300px; width: 300px;">
				<table id="user-list" class="cw-tbl-usr">
					<% Html.RenderPartial(MVC.UserManagement.Views.ctrlUserList, users); %>
				</table>
			</div>
			<%} else { %>
			You do not have any registered users yet.
			<%} %>
			<div>&nbsp;</div>
			<h3>Open Invites</h3><%: Html.ActionLink("Invite New", "Invite", "UserManagement") %>
			<div>&nbsp;</div>
			<div class = "cw-outl cw-padded cw-rounded-corners" style="overflow: auto; height: 150px; width: 300px;">
				<table id="invite-list" class="cw-tbl-usr">
				<%
				foreach (var invite in invites)
				{
				%>
					<tr>
						<td><%: invite.InvitationEmailAddress %></td>           
					</tr>
				<%  
				}        
				%>
				</table>
			</div>			
		</td>
		<td style="vertical-align: top">
			<div id="cw-user-detail" class="cw-outl cw-padded cw-rounded-corners" style="display: none">
			</div>
		</td>
	</tr>
	</table>
	<%} %>
</div>
</asp:Content>