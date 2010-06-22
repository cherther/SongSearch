﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.CatalogUploadViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Upload
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

	menu.Add("Manage Users", new string[4] { "Index", "UserManagement", "Admin", "" });
	menu.Add("Manage Catalogs", new string[4] { "Index", "CatalogManagement", "Admin", "" });
	menu.Add("Catalog Upload", new string[4] { "Upload", "CatalogUpload", "Admin", "current" });
	menu.Add("Invite", new string[4] { "Invite", "UserManagement", "Admin", "" });

	Html.RenderPartial(MVC.Shared.Views.ctrlSubMenu, menu);
		
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded">
<%
	var stepStatusMessage = String.Format("Step {0} / {1}", Model.CatalogUploadState.CurrentStepIndex+1, Model.CatalogUploadState.WorkflowStepsStatus.Count());
%>
	<h2>Catalog Upload Wizard</h2>
	<div>&nbsp;</div>
	<%: Html.ActionLink("(Start Over)", MVC.CatalogUpload.Upload()) %>
	<div>&nbsp;</div>
	<div class="cw-outl cw-padded">
		<div>&nbsp;</div>
		<h3><%: stepStatusMessage %>: <%: Model.PageTitle %></h3>
		<div>&nbsp;</div>
		<div>
			<%using (Html.BeginForm("Upload", "CatalogUpload", FormMethod.Post, new { id = "catalogUploadForm" } )) { %>
			<% var stepIndex = Model.CatalogUploadState.CurrentStepIndex; %>
			<%//: Html.Hidden("state.CurrentStepIndex", stepIndex.ToString()) %>
			<% Html.RenderPartial("ctrlHiddenFormInputs"); %>

			<% if (!String.IsNullOrWhiteSpace(Model.CatalogUploadState.CatalogName)) {%>
				<label>Catalog:</label> <%: Model.CatalogUploadState.CatalogName%>
				<div>&nbsp;</div>
			<%} %>
			<% Html.RenderPartial(Model.StepView); %>
		<%--	<% var i = 0; %>
			<% foreach (var stepStatus in Model.CatalogUploadState.WorkflowStepsStatus) { %>
			<%: Html.Hidden(String.Format("state.WorkflowStepsStatus.", i), stepStatus.Value)%>
			<%i++; %>
			<%} %>
		--%>	
		<div>&nbsp;</div>
			<div>
				<input type="submit" id="stepAction" value="<%: Model.StepActionName %>" />
			</div>
			<%} %>
			<div>&nbsp;</div>
				<div id="progressbarWrapper" style="height:18px;width:220px" class="ui-widget-default">
					<div id="progressbar" style="height:100%;"></div>
				</div>
		</div>
	</div>
</div>
	
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
<%
	var percentComplete = ((float)(Model.CatalogUploadState.CurrentStepIndex + 1) / (float)Model.CatalogUploadState.WorkflowStepsStatus.Count())*100;
%>
<script language="javascript" type="text/javascript">
	$(document).ready(function () {
		$("#progressbar").progressbar({
			value: <%= percentComplete %>
		});		
	});
</script>
</asp:Content>
