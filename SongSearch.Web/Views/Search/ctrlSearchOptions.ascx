<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.SearchViewModel>" %>
<div class="cw-outl-thick cw-fill-dark cw-rounded-corners">
<ul id="search-menu" class="cw-list-searchoptions">
	<%using (Html.BeginForm(MVC.Search.Results(), FormMethod.Get, new { id = "searchForm"})) { %>
		<li>
			<button id="submit-top" type="submit" title="Click to search" class="cw-button cw-simple cw-blue">
			<span class="b-search">Search</span>
			</button>
			&nbsp;<a href="#" class="cw-reset-form" title="Start over">Clear</a>
			<div>&nbsp;</div>
		</li>
		<%
		  var searchMenuProperties = Model.SearchMenuProperties;
		  var searchFields = Model.SearchFields;
		  //var lastGroup = "";
		  //var i = 0;
		%>
		<%foreach (var prop in searchMenuProperties) { %>
		<%
			var i = searchMenuProperties.IndexOf(prop);
		
			var item = prop;
			//var i = searchMenuProperties.IndexOf(item); 
			var isGroup = item.SearchGroup != null;		
			//var group = item.SearchGroup ?? "";
						  
			var searchType = (SearchTypes) item.SearchTypeId;
			var searchField = searchFields != null ? searchFields.SingleOrDefault(f => f.P == item.PropertyId) : null;
			var value = searchField != null ? searchField.V : new string[] { "", "" };
			var valueClass = ""; 
			  
			  %>
			<li>
			<%if (!isGroup && (searchType == SearchTypes.Contains || searchType == SearchTypes.Range || searchType == SearchTypes.TagText)) { %>
			<label><%: item.DisplayName%></label>
			<%} %>
			<input type="hidden" name="<%: String.Format("f[{0}].P", i)%>" value="<%: item.PropertyId %>" />
			<input type="hidden" name="<%: String.Format("f[{0}].T", i)%>" value="<%: item.SearchTypeId %>" />
			<div>
			<%switch (searchType) {%>
				<%case SearchTypes.Contains: {%>
				<%
					 valueClass = item.IsCacheable ? "cw-autocomplete" : ""; // String.Concat("cw-autocomplete-", item.PropertyCode.ToLower());
					 valueClass = String.Concat(valueClass, " ", "cw-form-value", !String.IsNullOrWhiteSpace(value.First()) ? " cw-input-highlight" : "");
					
				%>
				<input type="text" name="<%: String.Format("f[{0}].V", i)%>" value="<%: value.First() %>" class="<%: valueClass %>" alt="<%: item.PropertyName.ToLower()%>" />

				<%break;%>
				<%} %>
				<%--<%case SearchTypes.Join: {%>
				<%  var valueClass = item.IsCacheable ? "cw-autocomplete" : ""; // String.Concat("cw-autocomplete-", item.PropertyCode.ToLower());
					valueClass = String.Concat(valueClass, " ", "cw-form-value", !String.IsNullOrWhiteSpace(value.First()) ? " cw-input-highlight" : "");
				%>
				<%: Html.TextBox(String.Format("f[{0}].V", i), value.First(), new { @class = valueClass, alt = item.PropertyName.ToLower() })%>
				<%break;%>
				<%} %>--%>
				<%case SearchTypes.Range: {%>
				<%
					var value1 = value.First();
					var value2 = value.Last();
					var valueClass1 = !String.IsNullOrWhiteSpace(value1) ? "cw-form-value cw-input-highlight" : "cw-form-value";
					var valueClass2 = !String.IsNullOrWhiteSpace(value2) ? "cw-form-value cw-input-highlight" : "cw-form-value";   
				%>
				<input type="text" name="<%: String.Format("f[{0}].V[0]", i)%>" value="<%: value1 %>" size = "5" class="<%: valueClass1 %>" alt="<%: item.PropertyName.ToLower()%>" title="Enter a four digit year for an exact search" />&nbsp;to&nbsp;
				<input type="text" name="<%: String.Format("f[{0}].V[1]", i)%>" value="<%: value2 %>" size = "5" class="<%: valueClass2 %>" alt="<%: item.PropertyName.ToLower()%>" title="Enter a four digit year for an 'ending' search or a second four digit year for a range search" />
				<%break;%>
				<%} %>
				<%case SearchTypes.HasValue: {%>
				<% var buttonId = String.Format("button-{0}", item.PropertyId); %>
				<%: Html.CheckBox(String.Format("f[{0}].V", i), new { id = buttonId, @class = "cw-form-value cw-form-value-button" })%>
				<label for="<%: buttonId %>">
				<%: item.DisplayName%>
				</label>
				<%break;%>
				<%} %>
				<%case SearchTypes.IsTrue: {%>
				<% var buttonId = String.Format("button-{0}", item.PropertyId); %>
				<%: Html.CheckBox(String.Format("f[{0}].V", i), new { id = buttonId, @class = "cw-form-value cw-form-value-button" })%>
				<label for="<%: buttonId %>">
				<%: item.DisplayName%>
				</label>
				<%break;%>
				<%} %>
				<%case SearchTypes.Tag: {%>
				<%
					var tagType = (TagType)item.PropertyId;
					var tags = Model.SearchTags[tagType];
					var selectedTagValues = value.First() != null ? value.First().Split(';') : null;
					var selectedTags = selectedTagValues.Where(v => !String.IsNullOrWhiteSpace(v)).Select(v => int.Parse(v)).ToArray();
					var model = new TagCloudViewModel<SongSearch.Web.Data.Tag>() { 
						Tags = tags, 
						TagTypeName = item.PropertyName, 
						InitialTagNumber = 5, 
						NumberTagsInRow = 3,
						SelectedTags = selectedTags,
						TagClass = "cw-tagbox-search", 
						TagIdTemplate = String.Concat(String.Format("f_{0}__V-", i), "{0}"),
						TagNameTemplate = String.Format("f_{0}__V", i)
					};
		  
				%>
				<div>
				<label>
				<%: item.DisplayName%>
				</label>
				<%if (tags.Count > model.InitialTagNumber) {  %><a class="cw-small cw-tags-more-link" href="#" title="Show more <%: item.DisplayName%> choices">more...</a><%} %>
				<% Html.RenderPartial(MVC.Shared.Views.ctrlTagCloud, model); %>
				</div>
				<%: Html.Hidden(String.Format("f[{0}].V", i), "", new { @class = "cw-form-value" })%>
				<%break;%>
				<%} %>
				<%case SearchTypes.TagText: {%>
				<%
					 valueClass = item.IsCacheable ? "cw-autocomplete" : ""; // String.Concat("cw-autocomplete-", item.PropertyCode.ToLower());
					 valueClass = String.Concat(valueClass, " ", "cw-form-value", !String.IsNullOrWhiteSpace(value.First()) ? " cw-input-highlight" : "");
				%>
				<%: Html.TextBox(String.Format("f[{0}].V", i), value.First(), new { @class = valueClass, alt = item.PropertyName.ToLower() })%>
				<%break;%>
				<%} %>
			<%}%>
			</div>
		</li>
		<% //i++; %>
		<%} %>
	   
		<%--<li>
			<div>&nbsp;</div>
			<a href="#" id="more-options-link">Show More Search Options</a>
			<div>&nbsp;</div>
		 </li>--%>
		<li>
			<button id="submit-bottom" type="submit" title="Click to search" class="cw-button cw-simple cw-blue">
			<span class="b-search">Search</span>
			</button>
			&nbsp;<a href="#" class="cw-reset-form" title="Start over">Clear</a>
		</li>
	<%
		
   } %>
</ul>
</div>
