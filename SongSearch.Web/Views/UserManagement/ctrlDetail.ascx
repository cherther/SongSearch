<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.UserViewModel>" %>
<%
	var user = Model.MyUsers.First();
	var roleClasses = new string[] { "cw-black", "cw-orange", "cw-green", "cw-purple", "cw-blue" };
	var userDetailUrl = Request.Url;//String.Concat(Url.Content("/"), Url.Action("Detail", new { id = user.UserId }));
	var admin = (int)SongSearch.Web.Roles.Admin;
	var isCatAdmin = user.UserCatalogRoles.Any(r => r.RoleId == admin);
	var isAdmin = user.IsSuperAdmin() || isCatAdmin || user.RoleId == admin;
%>
<div>
	<h3>User:
	<span><%= user.FullName() %></span>
	</h3>
<%if (Model.IsThisUser){%>
	<span>(<%=Html.ActionLink("Edit your Profile", MVC.Account.UpdateProfile()) %>)</span>
<%}%>
</div>

<%=Html.Hidden("userid", user.UserId) %>
<div>&nbsp;</div>
<div>
	<label>E-mail:</label>
	 <span><%= user.UserName%></span>
</div>
<div>&nbsp;</div>
<div>
	<label>Registered On:</label>
	 <span><%= user.RegisteredOn.ToShortDateString()%></span>
</div>
<div>&nbsp;</div>
<hr />
<div>&nbsp;</div>
<label><strong>Allow this user to add <u>new</u> Catalogs & Users?</strong></label>&nbsp;&nbsp;
	<%
		//var roleName = ((SongSearch.Web.Roles)admin).ToString();
		var labelClass = isAdmin ? " cw-label-red" : "";
		var roleUrl = Url.Action(MVC.UserManagement.ToggleSystemAdminAccess(user.UserId));
		var roleMsg = isAdmin ? "Yes" : "No";
		var disable = user.IsSuperAdmin() || isCatAdmin;
		var adminMsg = user.IsSuperAdmin() ? " (SuperAdmin)" : 
			(isCatAdmin ? " (Admin in at least one Catalog)" : "");
	%>
		<input type="checkbox" id="cw-system-access" class="cw-role-edit" <%: isAdmin ? "checked=checked" : "" %> <%: disable ? "disabled=disabled" : "" %> value="<%: roleUrl %>" />
		<label for="cw-system-access" class="<%: labelClass %>"><%: roleMsg %></label>
		<span><%: adminMsg %></span>
<div>&nbsp;</div>

<hr />
<div>&nbsp;</div>
<label><strong>Catalog Privileges:</strong></label>
<div style="overflow:auto ; height: 400px; width: 500px">
		<table id="catalog-list" class="">
			<tr>
				<td>
				Set Role for All Catalogs:
				</td>
				<td>
				<%
					foreach (var role in Model.CatalogRoles)
					{
						var roleName = ((SongSearch.Web.Roles)role).ToString();
						var roleClass = "cw-tag-box cw-usrcat-role-edit-all cw-button cw-simple cw-small"; 
						%>
						<%=Html.ActionLink(roleName, MVC.UserManagement.UpdateAllCatalogs(user.UserId, role), new { @class = roleClass, rel = userDetailUrl })%>
					<%} %>
				</td>
			</tr>
			<tr>
				<td>
				&nbsp;
				</td>
				<td>
					<hr />
				</td>
			</tr>
			<% 
			foreach (var cat in Model.Catalogs.OrderBy(c => c.CatalogName))
			{
				var catalogId = cat.CatalogId;
				var userCatalog = user.UserCatalogRoles.Where(c => c.Catalog.CatalogId == cat.CatalogId).SingleOrDefault();
				var rowId = String.Concat("catalog-", catalogId);
  
				var rowClass = String.Concat("c-", catalogId);
				
				var userCatRoleId = userCatalog == null ? 0 : userCatalog.RoleId;
				//string catClass = roleClasses[userCatRoleId];
				
				%>
				<tr id="<%= rowId%>" class="catalog-listing <%= rowClass%>">
					<td>
						<%: cat.CatalogName%>
							
					</td>
					<td>
					
					<%=Html.Hidden(String.Concat("cr-", catalogId), userCatRoleId)%>
					<%
					foreach (var role in Model.CatalogRoles)
					{
						var roleClass = String.Concat("cw-tag-box cw-usrcat-role-edit cw-button cw-simple cw-small", role == userCatRoleId ? " cw-green" : " cw-gray");
						var roleId = !role.Equals(userCatRoleId) ? role : 0;
						var roleName = ((SongSearch.Web.Roles)role).ToString();
						
						%>
						<%=Html.ActionLink(roleName, MVC.UserManagement.UpdateCatalog(user.UserId, cat.CatalogId, roleId), new { @class = roleClass, rel = userDetailUrl })%>
					<%} %>
					</td>
				</tr>
			<%} %>	    
		</table>
</div>


<%if (Model.AllowEdit){ %>
	<div>&nbsp;</div>
	<%using (Html.BeginForm(MVC.UserManagement.Delete(), FormMethod.Post)){ %>
	<%= Html.Hidden("id", user.UserId) %>
	<%= Html.AntiForgeryToken() %>
	<div class="cw-outl cw-padded" >
		<label><em>Delete User</em></label>
		<div>&nbsp;</div>
		<div style="text-align: center">
			Note: By deleting this user, you are taking ownership of this user's sub-users (if any). Also, you will NOT be able to recover this user or any of their saved settings, such as song carts or saved song archives.
			<div>&nbsp;</div>
			<button type="submit" class="cw-button cw-simple cw-small cw-red"><span class="b-delete">Delete User?</span></button>
			<div>&nbsp;</div>
		</div>
	</div>
	<%} %>
	<div>&nbsp;</div>
	
	<%using (Html.BeginForm(MVC.UserManagement.TakeOwnership(), FormMethod.Post)) { %>
	<%=Html.Hidden("id", user.UserId)%>
	<%= Html.AntiForgeryToken() %>
	<div class="cw-outl cw-padded">
		<label><em>Take Ownership</em></label>
		<div>&nbsp;</div>
		<div style="text-align: center">
			Note: taking ownership means moving all of this user's sub-users into your user hierarchy. It does not delete this user or any of their settings.
			<div>&nbsp;</div>
			<button type="submit" class="cw-button cw-simple cw-small cw-red"><span class="b-delete">Take Ownership of this User + Sub-users?</span></button>
			<div>&nbsp;</div>
		</div>
	</div>
	<%} %>
<%} %>