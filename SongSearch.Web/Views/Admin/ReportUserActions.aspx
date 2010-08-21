<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ReportEventViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ReportUserActionsList
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% var events = Model.UserActionEvents.OrderByDescending(e => e.UserActionEventDate).ThenByDescending(e => e.UserActionEventId); %>
	<h2>ReportUserActionsList</h2>

	<table>
		<tr>
			<th>
				#
			</th>
			<th>
				Date
			</th>
			<th>
				User
			</th>
			<th>
				User Action
			</th>
		</tr>

	<% foreach (var item in events) { %>
	
		<tr>
			<td>
				<%: item.UserActionEventId %>
			</td>
			<td>
				<%: String.Format("{0:g}", item.UserActionEventDate) %>
			</td>
			<td>
				<%: item.User.UserName %>
			</td>
			<td>
				<%: (SongSearch.Web.Services.UserActions)item.UserActionId %>
			</td>
		</tr>
	
	<% } %>

	</table>

</asp:Content>
