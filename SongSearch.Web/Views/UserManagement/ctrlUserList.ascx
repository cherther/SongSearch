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

	<tr id="<%: rowId%>" class="cw-user-listing <%: rowClass%>">
		<td>
			<%if (user.IsSuperAdmin()) {%>
				<strong>
				<%: Html.ActionLink(userDisplayName, MVC.UserManagement.Detail(user.UserId), new { @class = "cw-user-detail-link", title = user.UserName })%>
				</strong>
<%--				<img src="../../Public/Images/Icons/Silk/star_gold.png" alt="SuperAdmin" title="SuperAdmin"/>
--%>			<%} else {
				if (App.IsLicensedVersion && user.IsPlanUser) {%>
					&nbsp;&nbsp;
					<%: Html.ActionLink(userDisplayName, MVC.UserManagement.Detail(user.UserId), new { @class = "cw-user-detail-link", title = user.UserName })%>
<%--					<img src="../../Public/Images/Icons/Silk/money_dollar.png" alt="Plan User" title="Plan User" />--%>
				<%} else {%>
				&nbsp;&nbsp;&nbsp;-&nbsp;&nbsp;&nbsp;<%: Html.ActionLink(userDisplayName, MVC.UserManagement.Detail(user.UserId), new { @class = "cw-user-detail-link", title = user.UserName })%>
				<%}%>
			<%}%>
		</td>
	</tr>
	<%
		if (user.ChildUsers.Count() > 0)
	{
	 %>
	<tr class="cw-user-child-listing <%: rowClass%>">
		<td>
			<table class="cw-children cw-tbl-usr">
				<% Html.RenderPartial("ctrlUserList", user.ChildUsers); %>
			</table>
		</td>
	</tr>		
	<% } %>
<% } %>
