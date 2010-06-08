<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogViewModel>" %>
<%
    var catalog = Model.MyCatalogs.First();
	var users = Model.Users;// != null ? Model.Users.OrderBy(x => x.FullName()) : null;
    var roleClasses = new string[] { "cw-black", "cw-orange", "cw-green", "cw-purple", "cw-blue" };
    var catalogDetailUrl = Request.Url;//String.Concat(Url.Content("/"), Url.Action("Detail", new { id = user.UserId }));
%>
<div>
    Catalog: <strong><%= catalog.CatalogName %></strong>
</div>

<%=Html.Hidden("catalogid", catalog.CatalogId)%>
<div>&nbsp;</div>

<hr />
<div>&nbsp;</div>
<label><em>Catalog Access</em></label>
<div>&nbsp;</div>
<div style="overflow:auto ; height: 400px; width: 500px">
		<table id="catalog-list" class="">
            <tr>
                <td>
                Set Role for All Users:
                </td>
                <td>
                <%
                    foreach (var role in Model.CatalogRoles)
			        {
                        string roleName = ((SongSearch.Web.Roles)role).ToString();
                        string roleClass = "cw-tag-box cw-cat-role-edit-all cw-button cw-simple cw-small"; 
						
				        %>
                        <%=Html.ActionLink(roleName, MVC.UserManagement.UpdateAllUsers(catalog.CatalogId, role), new { @class = roleClass, rel = catalogDetailUrl })%>
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
			 <% Html.RenderPartial(MVC.CatalogManagement.Views.ctrlUserList, Model); %>   
		</table>
</div>


<%if (Model.AllowEdit){ %>
    <div>&nbsp;</div>
    <%using (Html.BeginForm(MVC.CatalogManagement.Delete(), FormMethod.Post)){ %>
    <%= Html.Hidden("id", catalog.CatalogId)%>
    <%= Html.AntiForgeryToken() %>
    <div class="cw-outl cw-padded" >
	    <label><em>Delete Catalog</em></label>
	    <div>&nbsp;</div>
	    <div style="text-align: center">
            Note: By deleting this catalog, you are deleting all associated song mp3 files and metadata. You will NOT be able to recover this catalog.
	        <div>&nbsp;</div>
            <button type="submit" class="cw-button cw-simple cw-small cw-red"><span class="b-delete">Delete Catalog?</span></button>
	        <div>&nbsp;</div>
	    </div>
    </div>
    <%} %>
    
<%} %>