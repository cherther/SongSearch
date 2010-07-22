<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ContactUsModel>" %>

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
					<div>&nbsp;</div>
					<div><%: Html.LabelFor(m => m.Name) %></div>
					<div><%: Html.TextBoxFor(m => m.Name, new { @class = "cw-field-large", title = "Please enter you full name", autofocus="true", placeholder="Your Name", autocomplete="off", required="true" })%></div>
					<div><%= Html.ValidationMessageFor(m => m.Name)%></div>
					<div>&nbsp;</div>
					<div><%: Html.LabelFor(m => m.Email)%></div>
					<div><%: Html.TextBoxFor(m => m.Email, new { @class = "cw-field-large", title = "Please enter a valid e-mail address so we can get back to you", placeholder = "Your Email", autocomplete = "off", required = "true" })%></div>
					<div><%= Html.ValidationMessageFor(m => m.Email)%></div>
					<div>&nbsp;</div>
					<div><%: Html.LabelFor(m => m.Company)%></div>
					<div><%: Html.TextBoxFor(m => m.Company, new { @class = "cw-field-large", placeholder="Company Name" })%></div>
					<div><%= Html.ValidationMessageFor(m => m.Company)%></div>
					<div>&nbsp;</div>
					<div><%: Html.LabelFor(m => m.Subject)%></div>
					<div><%: Html.TextBoxFor(m => m.Subject, new { @class = "cw-field-xlarge", placeholder = "Subject Line", title = "Please enter a short subject line", required = "true" })%></div>
					<div><%= Html.ValidationMessageFor(m => m.Subject)%></div>

					<div>&nbsp;</div>
					<div><%: Html.LabelFor(m => m.Body)%></div>
					<div><%: Html.TextAreaFor(m => m.Body, new { @class = "cw-field-xlarge", rows = 8, placeholder = "Your Message", title = "Please let us know what's on your mind", required = "true" })%></div>
					<div><%= Html.ValidationMessageFor(m => m.Name)%></div>
					<div>&nbsp;</div>
					<button id="invite-send" type="submit" title="Click to Send" class="cw-button cw-simple cw-blue">
						<span class="b-email">Send</span>
					</button>
				<%} %>	
				<%} %>
				</div>
			</div>
			<div class="three column">
				<div class="cw-outl cw-padded cw-buffered-left cw-rounded-corners">
					<h3>Contact Information:</h3>
					<div>&nbsp;</div>
					<%if (!String.IsNullOrWhiteSpace(Model.ContactInfo.ContactName)) {%>
					<div class="cw-padded"><strong><%: Model.ContactInfo.ContactName%></strong></div>
					<%} %>
					<%if (!String.IsNullOrWhiteSpace(Model.ContactInfo.CompanyName)) {%>
					<div class="cw-padded"><%: Model.ContactInfo.CompanyName%></div>
					<%} %>
					<%if (!String.IsNullOrWhiteSpace(Model.ContactInfo.Phone1)) {%>
					<div class="cw-padded">Phone: <%: Model.ContactInfo.Phone1%></div>
					<%} %>
					<%if (!String.IsNullOrWhiteSpace(Model.ContactInfo.Fax)) {%>
					<div class="cw-padded">Fax: <%: Model.ContactInfo.Fax%></div>
					<%} %>
					<%if (!String.IsNullOrWhiteSpace(Model.ContactInfo.Email)) {%>
					<div class="cw-padded">Email: <%: Model.ContactInfo.Email%></div>
					<%} %>
				</div>

			</div>
		</div>
	</div>
</asp:Content>

