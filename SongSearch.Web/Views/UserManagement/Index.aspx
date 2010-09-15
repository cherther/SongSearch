<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.UserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.PageTitle %>
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
ViewData["SubMenuLocation"] = "UserManagement";
Html.RenderPartial(MVC.Shared.Views.ctrlAdminMenu);
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-fill-lite cw-padded cw-rounded-corners-bottom">
	<div class="six_column section">
		<div class="three column text-top">
			<h2><%: Model.PageTitle %></h2>
	<div>&nbsp;</div>
	<h3>Registered Users</h3>
	<div>&nbsp;</div>
		</div>
		<div class="three column text-top">
			<%if (App.IsLicensedVersion) {%>
				<%: Html.Partial("ctrlUserQuotasWidget", Account.User().MyQuotas()) %>
			<%} %>
		</div>
	</div>

	<%if (Model != null) {
		var users = Model.MyUsers;
		var invites = Model.MyInvites;
		users = users.OrderBy(u => u.ParentUserId).ThenBy(u => u.UserName).ToList();
	%>
	
	<table>
	<tr>
		<td style="vertical-align: top">
			<%if (users.Count() > 0) { %>
			<div class = "cw-outl-thick cw-fill-white cw-padded cw-rounded-corners" style="overflow:auto ; height: 300px; width: 200px;">
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
			<div class = "cw-outl-thick cw-fill-white cw-padded cw-rounded-corners" style="overflow: auto; height: 150px; width: 200px;">
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
			<div id="cw-user-detail" class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners" style="display: none">
			</div>
		</td>
	</tr>
	</table>
	<%} %>
</div>
</asp:Content>