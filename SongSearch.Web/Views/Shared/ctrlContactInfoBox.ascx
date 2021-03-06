﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.Data.Contact>" %>
<%
	var contact = Model;
	
	%>
<div class="cw-outl cw-fill cw-padded cw-rounded-corners" style="width: 60%">
	<%if (!String.IsNullOrWhiteSpace(contact.ContactName)) {%>
	<div><strong><%: contact.ContactName%></strong></div>
		<div>&nbsp;</div>
	<%} %>
		<div>
		<%if (!String.IsNullOrWhiteSpace(Html.SiteProfile().CompanyName)) {%>
		<div><%: contact.CompanyName%></div>
		<%} %>
		<div>
		<%if (!String.IsNullOrWhiteSpace(contact.Address1)) {%>
			<%: contact.Address1%>
			<%if (!String.IsNullOrWhiteSpace(contact.Address2)) {%>
			,&nbsp;<%: contact.Address2%>
			<%} %>
		<%} %>
		</div>
		<div>
		<%if (!String.IsNullOrWhiteSpace(contact.City)) {%>
			<%if (!String.IsNullOrWhiteSpace(contact.StateRegion)) {%>
			<%: contact.City%>,&nbsp;<%: contact.StateRegion%>&nbsp;<%: contact.PostalCode%>
			<%} else {%>
			<%: contact.PostalCode%>&nbsp;<%: contact.City%>
			<%} %>
			<%if (!String.IsNullOrWhiteSpace(contact.Country)) {%>
			,&nbsp;<%: contact.Country%>
			<%} %>
		<%} %>
		</div>
		<div>&nbsp;</div>
		<div><%if (!String.IsNullOrWhiteSpace(contact.Phone1)) {%>
		Phone: <%: contact.Phone1%>
		<%} %>
		<%if (!String.IsNullOrWhiteSpace(contact.Phone2)) {%>
		- Phone: <%: contact.Phone2%>
		<%} %>
		<%if (!String.IsNullOrWhiteSpace(contact.Fax)) {%>
		- Fax: <%: contact.Fax%>
		<%} %>
		</div>
		<%if (!String.IsNullOrWhiteSpace(contact.Email)) {%>
		<div>E-mail: <%: contact.Email%></div>
		<%} %>
		</div>
</div>