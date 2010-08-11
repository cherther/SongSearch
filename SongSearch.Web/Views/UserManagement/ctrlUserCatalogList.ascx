<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.UserViewModel>" %>
<%
	var user = Model.MyUsers.First();
	var userDetailUrl = Request.Url;//String.Concat(Url.Content("/"), Url.Action("Detail", new { id = user.UserId }));
	var cats = Model.Catalogs.OrderBy(c => c.CatalogName);

	foreach (var cat in cats)
	{
		var catalogId = cat.CatalogId;
		var userCatalog = user.UserCatalogRoles.Where(c => c.Catalog.CatalogId == cat.CatalogId).SingleOrDefault();
		var rowId = String.Concat("catalog-", catalogId);
  
		var rowClass = String.Concat("c-", catalogId);
				
		var userCatRoleId = userCatalog == null ? 0 : userCatalog.RoleId;
		var isCreatedByThisUser = user.UserId == cat.CreatedByUserId;
				
		//string catClass = roleClasses[userCatRoleId];
				
		%>
		<tr id="<%: rowId%>" class="catalog-listing <%: rowClass%>">
			<td>
				<%: cat.CatalogName%>
							
			</td>
			<td style="width:18px;">
			<%if (isCreatedByThisUser) { %>
			<img src="/public/images/icons/silk/tick.png" alt="Ok" title="This catalog was created by <%: user.FullName() %>"/>
			<%} else { %>&nbsp;<%} %></td>
			<td class="text-center">
					
			<%:Html.Hidden(String.Concat("cr-", catalogId), userCatRoleId)%>
			<%
			foreach (var role in Model.CatalogRoles)
			{
				var roleClass = String.Concat("cw-tag-box cw-usrcat-role-edit cw-button cw-simple cw-small", role == userCatRoleId ? " cw-green" : " cw-gray");
				var roleId = !role.Equals(userCatRoleId) ? role : 0;
				var roleName = ((SongSearch.Web.Roles)role).ToString();
						
				%>
				<%:Html.ActionLink(roleName, MVC.UserManagement.UpdateCatalog(user.UserId, cat.CatalogId, roleId), 
					new { @class = roleClass, rel = userDetailUrl,
					title = String.Format("Make this user {0} {1} in this catalog", roleName.IndefArticle(), roleName) })%>
							
			<%} %>
			</td>
		</tr>
	<%} %>	 