<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<SongSearch.Web.Data.User>>" %>
<%
	var users = Model;
%>

<% foreach (var user in users.OrderBy(u => u.UserName))
   {
	   var rowId = String.Concat("user-", user.UserId.ToString());
	   //var rowClass = user.ParentUserId.HasValue ? String.Concat("c-", user.ParentUserId.ToString()) : "";
	   var rowClass = String.Concat("c-", user.UserId.ToString());
	   var userDisplayName = user.FullName();
	   
	   //var childClass = user.ParentUserId.HasValue ? String.Concat("children-", user.UserId.ToString()) : "";
	   %>

	<tr id="<%= rowId%>" class="cw-user-listing <%= rowClass%>">
		<td>-</td>
		<td>
			<%: Html.ActionLink(userDisplayName, MVC.UserManagement.Detail(user.UserId), new { @class = "cw-user-detail-link", title = user.UserName })%>
		</td>
	</tr>
	<%
		if (user.ChildUsers.Count() > 0)
	{
	 %>
	<tr class="cw-user-child-listing <%= rowClass%>">
		<td>&nbsp;</td>
		<td>
			<table class="cw-children cw-tbl-usr">
				<% Html.RenderPartial("ctrlUserList", user.ChildUsers); %>
			</table>
		</td>
	</tr>		
	<% } %>
<% } %>
