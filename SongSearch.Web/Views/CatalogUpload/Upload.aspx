<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.CatalogUploadViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Upload
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%ViewData["SubMenuLocation"] = "CatalogUpload";%>
<%: Html.Partial(MVC.Shared.Views.ctrlAdminMenu) %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-fill-lite cw-padded cw-rounded-corners-bottom">
<%
	var percentComplete = ((decimal)(Model.CatalogUploadState.CurrentStepIndex + 1) / (decimal)Model.CatalogUploadState.WorkflowStepsStatus.Count()) * 100;

	var stepStatusMessage = String.Format("Step {0} / {1}", Model.CatalogUploadState.CurrentStepIndex+1, Model.CatalogUploadState.WorkflowStepsStatus.Count());
%>
	<h2>Catalog Upload Wizard</h2>
	<div>&nbsp;</div>
	
	<%if (App.IsLicensedVersion && percentComplete < 100 && Model.MyUserBalances.NumberOfSongs.IsAtTheLimit) {%>
	<div>
	You've uploaded a total of <%: Model.MyUserBalances.NumberOfSongs.Allowed.ToBalanceDescription() %> songs. Based on your Plan, you cannot upload any more songs.
	</div>
	<div>&nbsp;</div>
	<div>
	Please upgrade to the next higher plan, if you'd like to take advantage of our higher upload limits.
	</div>
	<%} else { %>

		<%: Html.ActionLink("(Start Over)", MVC.CatalogUpload.Upload(Model.CatalogUploadState.CatalogId)) %>
		<div>&nbsp;</div>
		<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
			<div>&nbsp;</div>
			<div class="six_column section">
				<div class="three column text-top">
					<div id="progressbarWrapper" style="height:16px;width:480px" class="ui-widget-default" title="You're on <%: stepStatusMessage%>">
						<div id="progressbar" style="height:100%;"></div> 
					</div>
					<div>&nbsp;</div>
					<h3><%: stepStatusMessage %>: <%: Model.PageTitle %></h3>
				</div>
				<div class="three column text-top">
					<%if (App.IsLicensedVersion) {%>
						<%: Html.Partial(MVC.Shared.Views.ctrlUserBalancesWidget, Account.User().MyBalances())%>
					<%} %>
				</div>
			</div>
			<div>&nbsp;</div>
			<div>
				<%using (Html.BeginForm("Upload", "CatalogUpload", FormMethod.Post, new { id = "catalogUploadForm" } )) { %>
				<% var stepIndex = Model.CatalogUploadState.CurrentStepIndex; %>
				<%//: Html.Hidden("state.CurrentStepIndex", stepIndex.ToString()) %>
				<%: Html.Partial(MVC.CatalogUpload.Views.ctrlHiddenFormInputs) %>

				<% if (!String.IsNullOrWhiteSpace(Model.CatalogUploadState.CatalogName)) {%>
					<label>Catalog:</label> <%: Model.CatalogUploadState.CatalogName%>
					<div>&nbsp;</div>
				<%} %>
				<%: Html.Partial(Model.StepView) %>
			<%--	<% var i = 0; %>
				<% foreach (var stepStatus in Model.CatalogUploadState.WorkflowStepsStatus) { %>
				<%: Html.Hidden(String.Format("state.WorkflowStepsStatus.", i), stepStatus.Value)%>
				<%i++; %>
				<%} %>
			--%>	
				<%if (Model.CatalogUploadState.CurrentStepIndex < Model.CatalogUploadState.WorkflowStepsStatus.Count()-1) {%>
					<div>&nbsp;</div>
					<div>
						<button type="submit" id="stepAction" class="cw-button cw-simple cw-medium cw-blue">
							<span class="b-forward after"><%: Model.StepActionName%></span>
						</button>
					</div>
				<%} %>
				<%} %>
			</div>
		</div>
	<%} %>
</div>
	
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
<%
	var percentComplete = ((decimal)(Model.CatalogUploadState.CurrentStepIndex+1) / (decimal)Model.CatalogUploadState.WorkflowStepsStatus.Count()) * 100;
%>
<script language="javascript" type="text/javascript">
	$(document).ready(function () {
		$("#progressbar").progressbar({
			value: <%: percentComplete %>
		});		
	});
</script>

</asp:Content>
