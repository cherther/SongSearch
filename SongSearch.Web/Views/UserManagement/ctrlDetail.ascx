<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.UserViewModel>" %>
<%
	var user = Model.MyUsers.First();
	var roleClasses = new string[] { "cw-black", "cw-orange", "cw-green", "cw-purple", "cw-blue" };
	var userDetailUrl = Request.Url;//String.Concat(Url.Content("/"), Url.Action("Detail", new { id = user.UserId }));
	var admin = (int)SongSearch.Web.Roles.Admin;
	var isCatAdmin = user.UserCatalogRoles.Any(r => r.RoleId == admin);
	var isAdmin = user.IsSuperAdmin() || isCatAdmin || user.RoleId == admin;
%>
<div class="six_column section">
	
	<div class="four column">
	<h3>User:
	<span><%: user.FullName() %></span>
	</h3>
	
	<%:Html.Hidden("userid", user.UserId) %>
	</div>
	<%if (Model.IsThisUser) {%>
		<div class="two column">
		<span>(<%:Html.ActionLink("Edit your Profile", MVC.Account.UpdateProfile())%>)</span>
		</div>
	<%} else {%>
	<div class="one column">
		<%if (Model.AllowEdit) { %>
			<%using (Html.BeginForm(MVC.UserManagement.Delete(), FormMethod.Post, new { id = "cw-user-delete-form" })) { %>
			<%: Html.Hidden("id", user.UserId)%>
			<%: Html.AntiForgeryToken()%>
			<button type="submit" id="cw-user-delete-link" class="cw-button cw-simple cw-small cw-red" title="Delete User"><span class="b-delete">Delete</span></button>
			<%} %>
	</div>
	<div class="one column">

			<%using (Html.BeginForm(MVC.UserManagement.TakeOwnership(), FormMethod.Post, new { id = "cw-user-takeowner-form" })) { %>
			<%:Html.Hidden("id", user.UserId)%>
			<%: Html.AntiForgeryToken()%>
			<button type="submit" id="cw-user-takeowner-link" class="cw-button cw-simple cw-small cw-red" title="Take Ownership"><span class="b-user">Take Ownership</span></button>
			<%} %>
		<%} %>
	</div>
	<%} %>
</div>
<div>&nbsp;</div>
<div class="six_column section">
	
	<div class="four column">
	<label>E-mail:</label>
	 <span><%: user.UserName%></span>
	</div>
	<div class="two column">
	<label>Registered On:</label>
	 <span><%: user.RegisteredOn.ToShortDateString()%></span>
	</div>

</div>
<%if (Page.User.UserIsSuperAdmin()) { %>
<div>&nbsp;</div>
<div class="six_column section">
	
	<div class="four column">
	<label>Current Pricing Plan:</label>
	 <span><%: user.PricingPlan.PricingPlanName %></span></div>
	<div class="two column">
	
	</div>

</div>
<%} %>
<div>&nbsp;</div>
<hr />
<div>&nbsp;</div>
<label><strong>User can add <span style="text-decoration:underline">new</span> Catalogs & Users?</strong></label>&nbsp;&nbsp;
	<%
		//var roleName = ((SongSearch.Web.Roles)admin).ToString();
		var labelClass = isAdmin ? " cw-label-red" : "";
		var roleUrl = Url.Action(MVC.UserManagement.ToggleSystemAdminAccess(user.UserId));
		var roleMsg = isAdmin ? "Yes" : "No";
		var disable = user.IsSuperAdmin() || isCatAdmin;
		var adminMsg = user.IsSuperAdmin() ? " (SuperAdmin)" : 
			(isCatAdmin ? " (Admin in at least one Catalog)" : "");
	%>
		<input type="radio" name="cw-system-access" id="cw-system-access-yes" class="cw-role-edit" <%: isAdmin ? "checked=checked" : "" %> <%: disable ? "disabled=disabled" : "" %> value="<%: roleUrl %>" />
		<label for="cw-system-access-yes">Yes</label>
		<input type="radio" name="cw-system-access" id="cw-system-access-no" class="cw-role-edit" <%: !isAdmin ? "checked=checked" : "" %> <%: disable ? "disabled=disabled" : "" %> value="<%: roleUrl %>" />
		<label for="cw-system-access-no">No</label>
		<%--<label for="cw-system-access" class="<%: labelClass %>"><%: roleMsg %></label>--%>
		&nbsp;<span><%: adminMsg %></span>
<div>&nbsp;</div>

<hr />
<div>&nbsp;</div>
<label><strong>Catalog Privileges:</strong></label>
<div style="overflow:auto ; height: 400px; width: 600px">
		<table id="catalog-list" class="">
			<tr>
				<td>
				Set Role for All Catalogs:
				</td>
				<td>&nbsp;</td>
				<td>
				<%
					foreach (var role in Model.CatalogRoles)
					{
						var roleName = ((SongSearch.Web.Roles)role).ToString();
						var roleClass = "cw-tag-box cw-usrcat-role-edit-all cw-button cw-simple cw-small"; 
						%>
						<%:Html.ActionLink(roleName, MVC.UserManagement.UpdateAllCatalogs(user.UserId, role), 
							new { @class = roleClass, rel = userDetailUrl, 
								title = String.Format("Make this user {0} {1} in all catalogs", roleName.IndefArticle(), roleName) })%>
					<%} %>
				</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
				<td>
					<hr />
				</td>
			</tr>
			<%
				var catsOwned = new UserViewModel() {
					MyUsers = Model.MyUsers,
					CatalogRoles = Model.CatalogRoles,
					Catalogs = Model.Catalogs.Where(c => c.CreatedByUserId == user.UserId).ToList()
				};	 
				%>
			<%if (catsOwned.Catalogs.Count > 0) {%>
			<%: Html.Partial("ctrlUserCataloglist", catsOwned)%>
			<tr>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
				<td>
					<hr />
				</td>
			</tr>
			<%} %>
			<%
				var catsOther = new UserViewModel() {
					MyUsers = Model.MyUsers,
					CatalogRoles = Model.CatalogRoles,
					Catalogs = Model.Catalogs.Where(c => c.CreatedByUserId != user.UserId).ToList()
				};	 
				%>
			<%: Html.Partial("ctrlUserCataloglist", catsOther)%>
		</table>
</div>


<div id="dialog-confirm-user-delete" class="cw-hidden-dialog" title="Delete User?">
	<p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>
	By deleting this user, you are taking ownership of this user's sub-users (if any). Also, you will NOT be able to recover this user or any of their saved settings, such as song carts or saved song archives. Are you sure?</p>
</div>
<div id="dialog-confirm-user-takeowner" class="cw-hidden-dialog" title="Take Ownership?">
	<p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>
	Taking ownership means moving all of this user's sub-users into your user hierarchy. It does not delete this user or any of their settings. Are you sure?</p>
</div>
<script language="javascript" type="text/javascript">
	$(document).ready(function () {
		//alert('here');
		setupTooltips();
		
		//setupMediaUploader('fullUploadContainer', 'fullVersionUpload', 'fullVersionFilelist', 'Full', 0);
		//setupMediaUploader('previewVersionUploadContainer', 'previewVersionUpload','previewVersionFilelist','Preview', 1);
	});
</script>