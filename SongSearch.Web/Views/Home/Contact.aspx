<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ContactModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Contact Us
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded">
	<table width="90%">
	<tr>
		<th class="text-left" width="50%"><%: Model.PageTitle %></th>
		<th class="text-left" width="50%">Contact Information:</th>
	</tr>
	<tr>
		<td>
			<%if (!String.IsNullOrWhiteSpace(Model.PageMessage)) {%>
				<div>&nbsp;</div>
				<div>&nbsp;</div>
				<p><%: Model.PageMessage %></p>
				<div>&nbsp;</div>
				<div>&nbsp;</div>
			<%} else { %>
			<% using (Html.BeginForm()) { %>
				
				<%=Html.AntiForgeryToken() %>

				<table width="100%">
				<tr>
					<td valign="top"><%: Html.LabelFor(m => m.Name) %></td>
					<td valign="top"><%: Html.TextBoxFor(m => m.Name) %><div><%= Html.ValidationMessageFor(m => m.Name)%></div></td>
				</tr>
				<tr>
					<td valign="top"><%: Html.LabelFor(m => m.Email)%></td>
					<td valign="top"><%: Html.TextBoxFor(m => m.Email) %><div><%= Html.ValidationMessageFor(m => m.Email)%></div></td>
				</tr>
				<tr>
					<td valign="top"><%: Html.LabelFor(m => m.Company) %></td>
					<td valign="top"><%: Html.TextBoxFor(m => m.Company)%><div><%= Html.ValidationMessageFor(m => m.Company)%></div></td>
				</tr>
				<tr>
					<td valign="top"><%: Html.LabelFor(m => m.Subject) %></td>
					<td valign="top"><%: Html.TextBoxFor(m => m.Subject)%><div><%= Html.ValidationMessageFor(m => m.Subject)%></div></td>
				</tr>
				<tr>
					<td valign="top"><%: Html.LabelFor(m => m.Body) %></td>
					<td valign="top"><%: Html.TextAreaFor(m => m.Body)%><div><%= Html.ValidationMessageFor(m => m.Body)%></div></td>
				</tr>
				<tr>
					<td>&nbsp;</td>
					<td>
					<button id="invite-send" type="submit" title="Send" class="cw-button cw-simple cw-blue">
						<span class="b-email">Send</span>
					</button>
					</td>
				</tr>
				</table>

				<%} %>
			<%} %>
		</td>
		<td>
		<div>
			<p>
				Ford Music Services<br />
				Office: (323) 939-2955<br />
				Fax: (323) 939-2951<br />
				E-Mail: artfordmusic@yahoo.com<br />
			</p>
		</div>
		</td>
	</tr>
	</table>
	
	<div>&nbsp;</div>
	<div>&nbsp;</div>
	
	<div>&nbsp;</div>
	<div>&nbsp;</div>
	<div>&nbsp;</div>
	

</div>
</asp:Content>

