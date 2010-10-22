<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogViewModel>" %>
<%
	var catalog = Model.Catalog;
	var users = Model.MyUsers;// != null ? Model.Users.OrderBy(x => x.FullName()) : null;
	var roleClasses = new string[] { "cw-black", "cw-orange", "cw-green", "cw-purple", "cw-blue" };
	var catalogDetailUrl = Request.Url;//String.Concat(Url.Content("/"), Url.Action("Detail", new { id = user.UserId }));

	var catalogSearchUrl = String.Concat("/Search/Results?f[0].P=16&f[0].T=1&f[0].V=", Url.Encode(catalog.CatalogName));
	var catalogContents = catalog != null ? Model.CatalogContents : null;
	var contentListViewModel = new ContentListViewModel() {
		List = catalogContents,
		ListHeaders = new string[] { "Title", "Artist", "Year" },
		HeaderActions = new GridAction[] { GridAction.Delete },
		GridActions = new GridAction[] { GridAction.ShowDetails, GridAction.Media },
		IsSortable = true
	};
	
%>
<div>
	Catalog: <strong><%: catalog.CatalogName %></strong>
	<span>
	&nbsp;&nbsp;&nbsp;&nbsp;-&nbsp;&nbsp;&nbsp;Created by <strong><%: catalog.Creator.FullName() %></strong> on <%: catalog.CreatedOn.ToShortDateString() %>
	</span>
</div>
<%:Html.Hidden("catalogid", catalog.CatalogId)%>
<div>&nbsp;</div>
<hr />
<div id="catalog-detail-tabs">
	<ul>
		<li><a href="#tab-1">Catalog Contents</a></li>
		<li><a href="#tab-2">Access & Permissions</a></li>
		<%if (Model.AllowEdit) { %>
		<li><a href="#tab-3">Advanced Options</a></li>
		<%} %>
	</ul>				
	<div id="tab-1">
		<%if (catalogContents.Count > 0){ %>
<%--			<a href="#" id="cw-catalog-contents-show-link" class="cw-button cw-simple cw-small cw-blue">Show Song List</a>--%>
			<span id="cw-catalog-contents-msg" class="cw-hidden">This catalog does not contain any songs.</span>
		<%} else {%>
			<span>This catalog does not contain any songs.</span>
		<%} %>
		<% var uploadLink = Url.Action(MVC.CatalogUpload.Upload(catalog.CatalogId)); %>
		<a href="<%: uploadLink %>" class="cw-button cw-simple cw-small cw-blue" title="Go to the Catalog Upload Wizard"><span class="b-add">Add New Songs</span></a>
		<%--(<%: String.Format("{0} {1}", catalogContents.Count, catalogContents.Count == 1 ? "song" : "songs")%>)--%>
		<div>&nbsp;</div>
		<%if (catalogContents.Count > 0){ %>
		<%//using (Html.BeginForm(MVC.Content.DeleteMultiple(), FormMethod.Post, new { id = "cw-catalog-contents-form" })) { %>
		<%: Html.Hidden("id", catalog.CatalogId)%>
		<%//= Html.AntiForgeryToken()%>
		<%: Html.Partial(MVC.CatalogManagement.Views.ctrlCatalogContentsTable, contentListViewModel) %>
		<%//} %>
		<%} %>
	</div>
	<div id="tab-2">
		<div>
		<%if (Model.MyUsers.Count() > 0) { %>
			<table id="user-list" class="">
				<tr>
					<td>
					Set Role for All Users:
					</td>
					<td class="text-center">
					<%
			foreach (var role in Model.CatalogRoles) {
				string roleName = ((SongSearch.Web.Roles)role).ToString();
				string roleClass = "cw-tag-box cw-cat-role-edit-all cw-button cw-simple cw-small"; 
						
							%>
							<%:Html.ActionLink(roleName, MVC.UserManagement.UpdateAllUsers(catalog.CatalogId, role), new { @class = roleClass, rel = catalogDetailUrl, title = "Make all users " + roleName + "s for this catalog" })%>
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
					<%: Html.Partial(MVC.CatalogManagement.Views.ctrlUserList, Model)%>   
			</table>
			<%} else { %>
			You do not have any registered users yet.
			<%} %>
		</div>
	</div>
	<%if (Model.AllowEdit) { %>
		<div id="tab-3">
		<%using (Html.BeginForm(MVC.CatalogManagement.Delete(), FormMethod.Post, new { id = "cw-catalog-delete-form" })) { %>
		<%: Html.Hidden("id", catalog.CatalogId)%>
		<%: Html.AntiForgeryToken()%>
		<div class="cw-outl cw-padded" >
			<label><em>Delete Catalog</em></label>
			<div>&nbsp;</div>
			<div style="text-align: center">
				Note: By deleting this catalog, you are deleting all associated song mp3 files and metadata. You will NOT be able to recover this catalog.
				<div>&nbsp;</div>
				<button type="submit" id="cw-catalog-delete-link" class="cw-button cw-simple cw-small cw-red"><span class="b-delete">Delete Catalog?</span></button>
				<div>&nbsp;</div>
			</div>
		</div>
		<%} %>	
		</div>
	<%} %>
</div>
<script language="javascript" type="text/javascript">
	$(document).ready(function () {
		//alert('here');
		$('#catalog-detail-tabs').tabs();
		setupTooltips();
		//setupMediaUploader('fullUploadContainer', 'fullVersionUpload', 'fullVersionFilelist', 'Full', 0);
		//setupMediaUploader('previewVersionUploadContainer', 'previewVersionUpload','previewVersionFilelist','Preview', 1);
	});
</script>