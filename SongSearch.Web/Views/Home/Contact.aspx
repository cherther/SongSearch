<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ContactModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Contact Us
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
	<h2>Contact Us</h2>
	<div>&nbsp;</div>

	<div class="six_column section">
	
	<div class="three column">
				
		<div class="cw-outl cw-padded cw-rounded-corners">
			<h3><%: Model.PageTitle %></h3>
			<div>&nbsp;</div>
			<%if (!String.IsNullOrWhiteSpace(Model.PageMessage)) {%>
			<div>&nbsp;</div>
			<div>&nbsp;</div>
			<p><%: Model.PageMessage %></p>
			<div>&nbsp;</div>
			<div>&nbsp;</div>
			<%} else { %>
			<% using (Html.BeginForm(MVC.Home.Contact(), FormMethod.Post)) { %>
				<% Html.EnableClientValidation(); %>
				<%: Html.AntiForgeryToken() %>
				<fieldset>
					<div>&nbsp;</div>
					<div><%: Html.LabelFor(m => m.Name) %></div>
					<div><%: Html.TextBoxFor(m => m.Name, new { @class = "cw-field-large" })%></div>
					<div><%= Html.ValidationMessageFor(m => m.Name)%></div>
					<div>&nbsp;</div>
					<div><%: Html.LabelFor(m => m.Email) %></div>
					<div><%: Html.TextBoxFor(m => m.Email, new { @class = "cw-field-large" })%></div>
					<div><%= Html.ValidationMessageFor(m => m.Email)%></div>
					<div>&nbsp;</div>
					<div><%: Html.LabelFor(m => m.Company) %></div>
					<div><%: Html.TextBoxFor(m => m.Company, new { @class = "cw-field-large" })%></div>
					<div><%= Html.ValidationMessageFor(m => m.Company)%></div>
					<div>&nbsp;</div>
					<div><%: Html.LabelFor(m => m.Subject) %></div>
					<div><%: Html.TextBoxFor(m => m.Subject, new { @class = "cw-field-xlarge" })%></div>
					<div><%= Html.ValidationMessageFor(m => m.Subject)%></div>

					<div>&nbsp;</div>
					<div><%: Html.LabelFor(m => m.Body)%></div>
					<div><%: Html.TextAreaFor(m => m.Body, new { @class = "cw-field-xlarge", rows = 8 })%></div>
					<div><%= Html.ValidationMessageFor(m => m.Name)%></div>
					<div>&nbsp;</div>
					<button id="invite-send" type="submit" title="Send" class="cw-button cw-simple cw-blue">
						<span class="b-email">Send</span>
					</button>
				</fieldset>
			<%} %>	
			<%} %>
			</div>
		</div>
		<div class="three column">

			<div class="cw-outl cw-padded cw-buffered-left cw-rounded-corners">
				<h3>Contact Information:</h3>
				<div>&nbsp;</div>
				Ford Music Services<br />
				Office: (323) 939-2955<br />
				Fax: (323) 939-2951<br />
				E-Mail: artfordmusic@yahoo.com<br />
			</div>

		</div>
	</div>

</div>
</asp:Content>

