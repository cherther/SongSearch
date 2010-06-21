<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.CatalogUploadViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Upload
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded">
	<h2>Catalog Upload Wizard</h2>
	<%: Html.ActionLink("(Start Over)", MVC.CatalogUpload.Upload()) %>
	<h3><%: Model.PageTitle %></h3>
	<div>&nbsp;</div>
	<div>
	<%using (Html.BeginForm("Upload", "CatalogUpload", FormMethod.Post, new { id = "catalogUploadForm" } )) { %>
	<% var stepIndex = Model.CatalogUploadState.CurrentStepIndex; %>
	<%//: Html.Hidden("state.CurrentStepIndex", stepIndex.ToString()) %>
	<% Html.RenderPartial("ctrlHiddenFormInputs"); %>

	<% if (!String.IsNullOrWhiteSpace(Model.CatalogUploadState.CatalogName)) {%>
		<label>Catalog:</label> <%: Model.CatalogUploadState.CatalogName%>
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
		<input type="submit" value="<%: Model.StepActionName %>" />
	</div>
	<%} %>
	</div>
</div>
	
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
<script language="javascript" type="text/javascript">
	$(document).ready(function () {

		

 
		//		$('form').submit(function (e) {
		//			var uploader = $('#uploader').pluploadQueue();

		//			// Validate number of uploaded files
		//			if (uploader.total.uploaded == 0) {
		//				// Files in queue upload them first
		//				if (uploader.files.length > 0) {
		//					// When all files are uploaded submit form
		//					uploader.bind('UploadProgress', function () {
		//						if (uploader.total.uploaded == uploader.files.length)
		//							$('form').submit();
		//					});

		//					uploader.start();
		//				} else
		//					alert('You must at least upload one file.');

		//				e.preventDefault();
		//			}
		//		});
	});
</script>
</asp:Content>
