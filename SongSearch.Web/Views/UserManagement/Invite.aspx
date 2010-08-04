<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.InviteViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Invite
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
ViewData["SubMenuLocation"] = "Invite";
Html.RenderPartial(MVC.Shared.Views.ctrlAdminMenu);
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
	<h2>Invite New Users</h2>
	<div>&nbsp;</div>
	<div class="cw-outl cw-padded cw-rounded-corners">

		<%if (App.IsLicensedVersion && !Model.ShowInviteForm) {%>
			<div class="cw-outl-orange cw-padded cw-rounded-corners">
				<div>We're sorry, but you have reached your limit for inivited users. If you'd like to invite additional users, please upgrade your <%: Html.ActionLink("Plan", MVC.Account.Plan()) %>.</div>
			</div>
		
		<%} else { %>
	
		<% using (Html.BeginForm("Invite", "UserManagement", FormMethod.Post, new { @class = "cw-form-small" })) {
		//string baseUrl = Html.ActionLink("register", "Register").ToString();
		%>
		<%:Html.AntiForgeryToken()%>
			<div>&nbsp;</div>
			<div class="six_column section">
				<div class="three column text-top">
					<h3>Invite</h3>           
					<div>&nbsp;</div>
					<div><%:Html.Label("From:")%>&nbsp;<%: String.Format("{0} <{1}>", Html.Friendly(), User.Identity.Name)%></div>
				</div>
				<div class="three column">
				<%if (App.IsLicensedVersion && Model.ShowQuotaWidget) {%>
					<%: Html.Partial("ctrlUserQuotasWidget", Account.User().MyQuotas()) %>
				<%} %>
				</div>
			</div>
			<div>&nbsp;</div>
			<%if (Model.ShowPlanMessage) {%>
			<div class="cw-outl cw-fill cw-padded cw-rounded-corners">
				<ul class="cw-bullets">
					<li>Initially, any registered users will have read-only access to any catalogs you manage.</li>
					<li>If you'd like to allow any users to administer their own catalogs, please do so from the User Management panel once they have registered</li>
					<li>Any uploaded songs and invited users will count towards the limit of your current plan.</li>
				</ul>
			</div>
			<%} %>
			<div><%:Html.Label("To:")%></div>
			<%: Html.ValidationMessageFor(m => m.Recipient)%>
			<div class="six_column section">
				<div class="three column text-top">
					<%:Html.TextArea("Recipient", new { style="width: 90%", rows = 10 })%>     
			   </div>
				<div class="three column text-top">
				<div class="cw-outl cw-padded cw-rounded-corners">
					<p>Please enter one or many e-mail addresses (separated by commas).</p> 
					<p>After you click 'Send', an e-mail invitation will be sent to each address with a personalized invitation code and instructions on how to complete the registration process.</p>
					<p> This invitation code, along with their e-mail address will allow the recipient to register on the site using a password of their choice.</p>
				</div>	
				</div>	
			</div>
			<div>
				<button id="invite-send" type="submit" title="Send" class="cw-button cw-simple cw-blue">
				<span class="b-email">Send</span>
				</button>
			</div>	   
		 </fieldset>  
		<%}%>

		<%} %>
	</div>
</div>
</asp:Content>