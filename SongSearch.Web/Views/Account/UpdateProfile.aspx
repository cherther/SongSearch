<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.UpdateProfileModel>" %>

<asp:Content ID="updateProfileTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Update Profile
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

	menu.Add("Your Profile", new string[4] { "UpdateProfile", "Account", "", "current" });
	menu.Add("Change Password", new string[4] { "ChangePassword", "Account", "", "" });
	
	menu.Add("Log Out", new string[4] { "LogOut", "Account", "", "" });

//    menu.Add("My Playlists", new string[4] { "Index", "List", "", "" });
//    menu.Add("Add New", new string[4] { "Create", "Song", "", "" });

	Html.RenderPartial(MVC.Shared.Views.ctrlSubMenu, menu);
		
	%>
	
</asp:Content>
<asp:Content ID="updateProfileContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">

	<h2>Update Profile</h2>
	<div>&nbsp;</div>
	<div class="cw-outl cw-padded cw-rounded-corners">
	<p>
		Use the form below to update your user profile. 
	</p>
	<%//= Html.ValidationSummary("Profile update was unsuccessful. Please correct the errors and try again.")%>
	<div>&nbsp;</div>
	<% using (Html.BeginForm(MVC.Account.UpdateProfile(), FormMethod.Post, new { @class = "cw-form-small" }))
	   {%>
	   <%=Html.AntiForgeryToken() %>
	   <% Html.EnableClientValidation(); %>
			<fieldset>
				<legend>Profile Information</legend>
				<div>&nbsp;</div>
				<div class="cw-fe-vert">
					<%= Html.LabelFor(m => m.Email) %>
				</div>
				<div class="cw-fe-vert cw-fe-req">
					<%= Html.Encode(Model.Email)%>
					<%= Html.HiddenFor(m => Model.Email)%>
				</div>
				<div>&nbsp;</div>
				<div class="cw-fe-vert">
					<%= Html.LabelFor(m => m.FirstName)%>
				</div>
				<div class="cw-fe-vert">
					<%= Html.TextBoxFor(m => m.FirstName, new { @class = "cw-field-large", title = "Your First Name" })%>
					<%= Html.ValidationMessageFor(m => m.FirstName)%>
				</div>
				<div>&nbsp;</div>
				<div class="cw-fe-vert">
					<%= Html.LabelFor(m => m.LastName) %>
				</div>
				<div class="cw-fe-vert">
					<%= Html.TextBoxFor(m => m.LastName, new { @class = "cw-field-large", title = "Your Last Name" })%>
					<%= Html.ValidationMessageFor(m => m.LastName)%>
				</div>
				<%if (Model.ShowSignatureField)
				  {%>
				<div>&nbsp;</div>
				<div class="cw-fe-vert">
					<%= Html.LabelFor(m => m.Signature)%>
				</div>
				<div class="cw-fe-vert">
					<%= Html.TextBoxFor(m => m.Signature, new { @class = "cw-field-xlarge", maxlength = "80", title = "Your signature is automatically appended<br/>to the file names of songs your users download.<br/>It cannot contain special characters such as '/' and ':'." })%>
					<%= Html.ValidationMessageFor(m => m.Signature)%>
				</div>
				<%--<div>&nbsp;</div>
				<div class="cw-fe-vert">
					<%= Html.LabelFor(m => m.AppendSignatureToTitle)%>
					<%= Html.CheckBoxFor(m => m.AppendSignatureToTitle, new { title = "Check to embed your signature in all download/zipped mp3 id3 tags" })%>
					<%= Html.ValidationMessageFor(m => m.AppendSignatureToTitle)%>
				</div>--%>
				<%} %>
				
				 <p>&nbsp;</p>
				<p>
					<button type="submit" class="cw-button cw-simple cw-blue" title="Click to save your changes">
					<span class="b-save">Update Profile</span>
					</button>
				</p>
			</fieldset>
	<% } %>
	</div>
</div>    
</asp:Content>
