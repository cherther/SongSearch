<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>
<%: Html.Hidden("state.CatalogId", Model.CatalogUploadState.CatalogId) %>
<%: Html.Hidden("state.CatalogName", Model.CatalogUploadState.CatalogName)%>

<div>
You've successfully uploaded <%: Model.CatalogUploadState.UploadFiles.Count%> song(s) to the <%: Model.CatalogUploadState.CatalogName %> catalog. 
</div>
<div>&nbsp;</div>
<div>
<%: Html.ActionLink("Upload", MVC.CatalogUpload.Upload(Model.CatalogUploadState.CatalogId))%> more songs or see a <%: Html.ActionLink("list", MVC.CatalogManagement.Index()) %> of your catalogs.
</div>
<div>&nbsp;</div>
