<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.Data.Contact>" %>

<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
	<%: Html.HiddenFor(m => m.ContactId)%>
	
	<div class="six_column section">
	
		<div class="three column">
			<div>&nbsp;</div>

			<div>
				<%: Html.LabelFor(m => m.ContactName)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.ContactName, new { @class = "cw-field-large", placeholder = "Site Contact Name", title = "Site Contact Name" })%>
				<%: Html.ValidationMessageFor(m => m.ContactName)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.CompanyName)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.CompanyName, new { @class = "cw-field-large", placeholder = "Site Company Name", title = "Site Company Name" })%>
				<%: Html.ValidationMessageFor(m => m.CompanyName)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Address1)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Address1, new { @class = "cw-field-large", placeholder = "Site Contact Address", title = "Site Contact Address" })%>
				<%: Html.ValidationMessageFor(m => m.Address1)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Address2)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Address2, new { @class = "cw-field-large", title = "Site Contact Address (cont'd)" })%>
				<%: Html.ValidationMessageFor(m => m.Address2)%>
			</div>
			<div>
				<%: Html.LabelFor(m => m.City)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.City, new { @class = "cw-field-large", placeholder = "Site Contact City", title = "Site Contact City" })%>
				<%: Html.ValidationMessageFor(m => m.City)%>
			</div>
			<div>&nbsp;</div>
			<div class="six_column section">
				<div class="two column">
					<div><%: Html.LabelFor(m => m.StateRegion)%></div>
					<div><%: Html.TextBoxFor(m => m.StateRegion, new { @class = "cw-field-small", placeholder = "State/Region", title = "Site Contact State/Region" })%>
					<%: Html.ValidationMessageFor(m => m.StateRegion)%></div>
				</div>
				<div class="four column">
					<div><%: Html.LabelFor(m => m.PostalCode)%></div>
					<div><%: Html.TextBoxFor(m => m.PostalCode, new { @class = "cw-field-small", placeholder = "Zip/PostalCode", title = "Site Contact Zip/PostalCode" })%>
					<%: Html.ValidationMessageFor(m => m.PostalCode)%></div>
				</div>
			</div>
			<div>
				<%: Html.LabelFor(m => m.Country)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Country, new { @class = "cw-field-large", placeholder = "Country", title = "Site Contact Country" })%>
				<%: Html.ValidationMessageFor(m => m.Country)%>
			</div>

		</div>
		<div class="three column">
			<div>&nbsp;</div>
						
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Phone1)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Phone1, new { @class = "cw-field-large", placeholder = "Site Contact Phone", title = "Site Contact Phone 1" })%>
				<%: Html.ValidationMessageFor(m => m.Phone1)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Phone2)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Phone2, new { @class = "cw-field-large", title = "Site Contact Phone 2" })%>
				<%: Html.ValidationMessageFor(m => m.Phone2)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Fax)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Fax, new { @class = "cw-field-large", placeholder = "Site Contact Fax", title = "Site Contact Fax" })%>
				<%: Html.ValidationMessageFor(m => m.Fax)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.Email)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Email, new { @class = "cw-field-large", placeholder = "Site Contact E-mail", title = "Site Contact E-mail" })%>
				<%: Html.ValidationMessageFor(m => m.Email)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.LabelFor(m => m.AdminEmail)%>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.AdminEmail, new { @class = "cw-field-large", placeholder = "Site Administrative E-mail", title = "Site Administrative E-mail" })%>
				<%: Html.ValidationMessageFor(m => m.AdminEmail)%>
			</div>
		</div>
	</div>
	</div>