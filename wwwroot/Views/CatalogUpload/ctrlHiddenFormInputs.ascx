<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>

	<input id="state_CurrentStepIndex" name="state.CurrentStepIndex" type="hidden" value="<%: Model.CatalogUploadState.CurrentStepIndex %>" />

<% if (!String.IsNullOrWhiteSpace(Model.CatalogUploadState.CatalogName)) {%>
	<input id="state_CatalogId" name="state.CatalogId" type="hidden" value="<%: Model.CatalogUploadState.CatalogId %>" />
	<input id="state_CatalogName" name="state.CatalogName" type="hidden" value="<%: Model.CatalogUploadState.CatalogName %>" />
<%} %>
