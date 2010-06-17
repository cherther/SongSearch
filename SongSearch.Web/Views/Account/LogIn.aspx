<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.LogOnModel>" %>
<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
Log In
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

	menu.Add("Log In", new string[4] { "LogIn", "Account", "", "current" });
	//    menu.Add("My Playlists", new string[4] { "Index", "List", "", "" });
//    menu.Add("Add New", new string[4] { "Create", "Song", "", "" });

	Html.RenderPartial(MVC.Shared.Views.ctrlSubMenu, menu);
		
	%>
	
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded">
		<h3>Log in</h3>
		<p>
		Please enter your e-mail address and password. <%= Html.ActionLink("Register", MVC.Account.Register())%> if you don't have an account.
		</p>
	<%//= Html.ValidationSummary("Login was unsuccessful. Please correct the errors and try again.") %>
	<% Html.EnableClientValidation(); %>
	<% using (Html.BeginForm(MVC.Account.LogIn(), FormMethod.Post, new { @class = "cw-form-small" }))
	   { %><%=Html.AntiForgeryToken() %>
			<fieldset>
				
				<legend>Account Information</legend>
					<div class="cw-fe-vert cw-fe-req">
						<%= Html.LabelFor(m => m.Email) %>
					</div>
					<div class="cw-fe-vert">
						<%= Html.TextBoxFor(m => m.Email)%>
						<%= Html.ValidationMessageFor(m => m.Email)%>
					</div>
					
					<div class="cw-fe-vert cw-fe-req">
						<%= Html.LabelFor(m => m.Password) %>
					</div>
					<div class="cw-fe-vert">
						<%= Html.PasswordFor(m => m.Password) %>
						<%= Html.ValidationMessageFor(m => m.Password) %>
					</div>
					
					<div class="cw-fe-vert">
						
						<%= Html.CheckBoxFor(m => m.RememberMe) %> &nbsp<%= Html.LabelFor(m => m.RememberMe) %>
					</div>
					<%= Html.HiddenFor(m => m.ReturnUrl)%>
					<p>
					<button type="submit" class="cw-button cw-simple cw-blue">
					<span class="b-ok">Log in</span>
					</button>
					</p>
					 <p>&nbsp;</p>
					<p> <%= Html.ActionLink("Forgot your password?", MVC.Account.ResetPassword())%></p>
			</fieldset>
	<% } %>
   
</div>
</asp:Content>