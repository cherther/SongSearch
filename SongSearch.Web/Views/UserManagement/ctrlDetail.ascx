<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.UserViewModel>" %>
<%
	var user = Model.MyUsers.First();
	var roleClasses = new string[] { "cw-black", "cw-orange", "cw-green", "cw-purple", "cw-blue" };
	var userDetailUrl = Request.Url;//String.Concat(Url.Content("/"), Url.Action("Detail", new { id = user.UserId }));
%>
<div>
	<strong><%= user.FullName() %></strong>
<%if (Model.IsThisUser){%>
	<span>(<%=Html.ActionLink("Edit your Profile", MVC.Account.UpdateProfile()) %>)</span>
<%}%>
</div>

<%=Html.Hidden("userid", user.UserId) %>
<div>&nbsp;</div>
<div>
	<label>User Name:</label>
	 <span><%= user.UserName%></span>
</div>
<div>&nbsp;</div>
<div>
	<label>Registered On:</label>
	 <span><%= user.RegisteredOn.ToShortDateString()%></span>
</div>
<div>&nbsp;</div>
<label><em>System Role</em></label>
<div>
	<%foreach (var role in Model.Roles) {%>
	<%
		string roleName = ((SongSearch.Web.Roles)role).ToString();
		string roleClass = String.Concat("cw-tag-box cw-role-edit cw-button cw-simple cw-small", role == user.RoleId ? " cw-green" : " cw-black"); 
		%>
		<%=Html.ActionLink(roleName, MVC.UserManagement.UpdateRole(user.UserId, role), new { @class = roleClass })%>
	<%} %>
</div>

<hr />
<div>&nbsp;</div>
<label><em>Catalog Access</em></label>
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
						string roleName = ((SongSearch.Web.Roles)role).ToString();
						string roleClass = "cw-tag-box cw-usrcat-role-edit-all cw-button cw-simple cw-small"; 
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