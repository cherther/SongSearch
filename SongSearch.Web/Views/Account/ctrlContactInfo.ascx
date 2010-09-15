<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.UpdateProfileModel>" %>

<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
	<%: Html.HiddenFor(m => m.Contact.ContactId)%>
	<h3>Contact Info</h3>
	<div>This information will be displayed on the <%: Html.ActionLink("Contact Us", MVC.Home.Contact())%> page. Please enter at least option for users to contact you, such as Phone or Email.</div>
	<div class="six_column section">
	
		<div class="three column">
			<div>&nbsp;</div>

			<div>
				<%: Html.LabelFor(m => m.Contact.ContactName)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Contact.ContactName, new { @class = "cw-field-large", placeholder = "Site Contact Name", title = "Site Contact Name" })%>
				<%: Html.ValidationMessageFor(m => m.Contact.ContactName)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Contact.CompanyName)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Contact.CompanyName, new { @class = "cw-field-large", placeholder = "Site Company Name", title = "Site Company Name" })%>
				<%: Html.ValidationMessageFor(m => m.Contact.CompanyName)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Contact.Address1)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Contact.Address1, new { @class = "cw-field-large", placeholder = "Site Contact Address", title = "Site Contact Address" })%>
				<%: Html.ValidationMessageFor(m => m.Contact.Address1)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Contact.Address2)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Contact.Address2, new { @class = "cw-field-large", title = "Site Contact Address (cont'd)" })%>
				<%: Html.ValidationMessageFor(m => m.Contact.Address2)%>
			</div>
			<div>
				<%: Html.LabelFor(m => m.Contact.City)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Contact.City, new { @class = "cw-field-large", placeholder = "Site Contact City", title = "Site Contact City" })%>
				<%: Html.ValidationMessageFor(m => m.Contact.City)%>
			</div>
			<div>&nbsp;</div>
			<div class="six_column section">
				<div class="two column">
					<div><%: Html.LabelFor(m => m.Contact.StateRegion)%></div>
					<div><%: Html.TextBoxFor(m => m.Contact.StateRegion, new { @class = "cw-field-small", placeholder = "State/Region", title = "Site Contact State/Region" })%>
					<%: Html.ValidationMessageFor(m => m.Contact.StateRegion)%></div>
				</div>
				<div class="four column">
					<div><%: Html.LabelFor(m => m.Contact.PostalCode)%></div>
					<div><%: Html.TextBoxFor(m => m.Contact.PostalCode, new { @class = "cw-field-small", placeholder = "Zip/PostalCode", title = "Site Contact Zip/PostalCode" })%>
					<%: Html.ValidationMessageFor(m => m.Contact.PostalCode)%></div>
				</div>
			</div>
			<div>
				<%: Html.LabelFor(m => m.Contact.Country)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Contact.Country, new { @class = "cw-field-large", placeholder = "Country", title = "Site Contact Country" })%>
				<%: Html.ValidationMessageFor(m => m.Contact.Country)%>
			</div>

		</div>
		<div class="three column">
			<div>&nbsp;</div>
						
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Contact.Phone1)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Contact.Phone1, new { @class = "cw-field-large", placeholder = "Site Contact Phone", title = "Site Contact Phone 1" })%>
				<%: Html.ValidationMessageFor(m => m.Contact.Phone1)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Contact.Phone2)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Contact.Phone2, new { @class = "cw-field-large", title = "Site Contact Phone 2" })%>
				<%: Html.ValidationMessageFor(m => m.Contact.Phone2)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Contact.Fax)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Contact.Fax, new { @class = "cw-field-large", placeholder = "Site Contact Fax", title = "Site Contact Fax" })%>
				<%: Html.ValidationMessageFor(m => m.Contact.Fax)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Contact.Email)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Contact.Email, new { @class = "cw-field-large", placeholder = "Site Contact E-mail", title = "Site Contact E-mail" })%>
				<%: Html.ValidationMessageFor(m => m.Contact.Email)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Contact.AdminEmail)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Contact.AdminEmail, new { @class = "cw-field-large", placeholder = "Site Administrative E-mail", title = "Site Administrative E-mail" })%>
				<%: Html.ValidationMessageFor(m => m.Contact.AdminEmail)%>
			</div>
		</div>
	</div>
	</div>