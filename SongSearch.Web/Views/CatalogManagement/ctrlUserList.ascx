<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogViewModel>" %>
<%
	var catalog = Model.MyCatalogs.First();
	var users = Model.Users != null ? Model.Users.OrderBy(x => x.FullName()) : null;
	var catalogDetailUrl = Request.Url;
%>

<% foreach (var user in users.OrderBy(u => u.RoleId).ThenBy(u => u.UserName))
   {
	   var userId = user.UserId;
	   var rowId = String.Concat("user-", userId);
	   //var rowClass = user.ParentUserId.HasValue ? String.Concat("c-", user.ParentUserId.ToString()) : "";
	   var rowClass = String.Concat("u-", userId);

	   //var childClass = user.ParentUserId.HasValue ? String.Concat("children-", user.UserId.ToString()) : "";

	   var userCatalog = catalog.UserCatalogRoles.SingleOrDefault(c => c.UserId == user.UserId);
	   
	   var userCatRoleId = userCatalog == null ? 0 : userCatalog.RoleId;
	   var userDisplayName = user.FullName();
	   
	   %>

    <tr id="<%: rowId%>" class="cw-user-listing <%: rowClass%>">
		<td>
		<%for (var i = 0; i < Model.HierarchyLevel; i++) {%>
		&nbsp;&nbsp;
		<%} %>
		<span title="<%: user.UserName %>">
		<%if (Model.HierarchyLevel > 0) { %>
			<%if (App.IsLicensedVersion && user.IsPlanUser) {%>
			<%:userDisplayName%>
			<img src="../../Public/Images/Icons/Silk/money_dollar.png" alt="Plan User" title="Plan User" />
			<%} else {%>
			&nbsp;-&nbsp;<%:userDisplayName%>
			<%} %>
		<%} else {%>
			<strong><%:userDisplayName%></strong>
			<img src="../../Public/Images/Icons/Silk/star_gold.png" alt="SuperAdmin" title="SuperAdmin"/>
		<%} %>
		</span>	
		</td>
		 <td class="text-center">
            <%:Html.Hidden(String.Concat("ur-", userId), userCatRoleId)%>
            <%
            foreach (var role in Model.CatalogRoles)
			{
                var roleClass = String.Concat("cw-tag-box cw-cat-role-edit cw-button cw-simple cw-small", role == userCatRoleId ? " cw-green" : " cw-gray");
                var roleId = !role.Equals(userCatRoleId) ? role : 0;
                var roleName = ((SongSearch.Web.Roles)role).ToString();
                        
				%>
                <%:Html.ActionLink(roleName, MVC.UserManagement.UpdateCatalog(user.UserId, catalog.CatalogId, roleId), new { @class = roleClass, rel = catalogDetailUrl, title="Click to select this role, unlick to remove this role" })%>
			<%} %>
            </td>
	</tr>
	<%
        if (user.ChildUsers.Count() > 0)
		{
			var childModel = new CatalogViewModel() { MyCatalogs = Model.MyCatalogs, Users = Model.Users, CatalogRoles = Model.CatalogRoles };
			childModel.Users = user.ChildUsers;
			childModel.HierarchyLevel = Model.HierarchyLevel + 1;
	 %>
		<% Html.RenderPartial(MVC.CatalogManagement.Views.ctrlUserList, childModel); %>
		<% childModel = null; %>
	<% } %>
<% } %>
