<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.LogOnModel>" %>
<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
Log In
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
ViewData["SubMenuLocation"] = "LogIn";
Html.RenderPartial(MVC.Shared.Views.ctrlAccountMenu);
%>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
<% Html.EnableClientValidation(); %>
<div id="content" class="cw-outl cw-fill-lite cw-padded cw-rounded-corners-bottom">

	<%--<h2>Log in</h2>
	<div>&nbsp;</div>--%>
	<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners text-top">
		<h2 style="display:inline;">Log in</h2> using your e-mail address and <%: Model.SiteProfile.CompanyName %> password you've set up previously. <%: Html.ActionLink("Register", MVC.Account.Register())%> if you don't have an account.
		<div>&nbsp;</div>
		<% using (Html.BeginForm(MVC.Account.LogIn(), FormMethod.Post, new { @class = "cw-form-small" }))
			{ %><%:Html.AntiForgeryToken() %>
				<div>&nbsp;</div>
				<div>
					<%: Html.LabelFor(m => m.Email) %>
				</div>
				<div>
					<%: Html.TextBoxFor(m => m.Email, new { @class = "cw-field-large", placeholder="Your email address", required=true, title = "Your username/e-mail address" })%>
					<%: Html.ValidationMessageFor(m => m.Email)%>
				</div>
				<div>&nbsp;</div>
				<div>
					<%: Html.LabelFor(m => m.Password) %>
				</div>
				<div>
					<%: Html.PasswordFor(m => m.Password, new { @class = "cw-field-large", placeholder="Your password", required=true })%>
					<%: Html.ValidationMessageFor(m => m.Password) %>
				</div>
				<div>&nbsp;</div>
				<div>
						
					<%: Html.CheckBox("RememberMe", Model.RememberMe, new { title = "Check this box to help us remember you!" })%> &nbsp<label for="RememberMe">Remember me?</label>
				</div>
				<%: Html.HiddenFor(m => m.ReturnUrl)%>
				<div>&nbsp;</div>
				<button type="submit" class="cw-button cw-simple cw-blue">
				<span class="b-ok">Log in</span>
				</button>
				<div>&nbsp;</div>
				<%: Html.ActionLink("Forgot your password?", MVC.Account.ResetPassword(), new { title = "Did you forget your password? We can help you!" })%>
		<% } %>
	</div>
</div>
</asp:Content>
