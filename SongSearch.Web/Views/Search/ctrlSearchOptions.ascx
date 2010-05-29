﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.SearchViewModel>" %>
<div class="cw-outl">
<ul id="search-menu" class="cw-list-searchoptions">
    <%using (Html.BeginForm("Results", "Search", FormMethod.Get, new { id = "searchForm"})) { %>
        <li>
            <button id="submit-top" type="submit" title="Search" class="cw-button cw-simple cw-blue">
            <span class="b-search">Search</span>
            </button>
            &nbsp;<a href="#" class="cw-reset-form" title="Reset the search form to start over">Clear</a>
        </li>
        <div>&nbsp;</div>
        <%
          var searchMenuProperties = Model.SearchMenuProperties;
          var searchFields = Model.SearchFields;
        %>
        <%foreach (var item in searchMenuProperties) { %>
        <%
              var i = searchMenuProperties.IndexOf(item); 
              var searchType = (SearchTypes) item.SearchTypeId;
              var searchField = searchFields != null ? searchFields.SingleOrDefault(f => f.P == item.PropertyId) : null;
              var value = searchField != null ? searchField.V : new string[] { "", "" };
              %>
        <li>
            <label><%: item.PropertyName %></label>
            <%: Html.Hidden(String.Format("f[{0}].P", i), item.PropertyId)%>
            <%: Html.Hidden(String.Format("f[{0}].T", i), item.SearchTypeId)%>
            <div>
            <%switch (searchType) {%>
                <%case SearchTypes.Contains: {%>
                <%
                     var valueClass = item.IsCacheable ? "cw-autocomplete" : ""; // String.Concat("cw-autocomplete-", item.PropertyCode.ToLower());
                     valueClass = String.Concat(valueClass, " ", "cw-form-value", !String.IsNullOrWhiteSpace(value.First()) ? " cw-input-highlight" : "");
                %>
                <%: Html.TextBox(String.Format("f[{0}].V", i), value.First(), new { @class = valueClass, alt = item.PropertyCode.ToLower() })%>
                <%break;%>
                <%} %>
                <%case SearchTypes.Join: {%>
                <%  var valueClass = item.IsCacheable ? "cw-autocomplete" : ""; // String.Concat("cw-autocomplete-", item.PropertyCode.ToLower());
                    valueClass = String.Concat(valueClass, " ", "cw-form-value", !String.IsNullOrWhiteSpace(value.First()) ? " cw-input-highlight" : "");
                %>
                <%: Html.TextBox(String.Format("f[{0}].V", i), value.First(), new { @class = valueClass, alt = item.PropertyCode.ToLower() })%>
                <%break;%>
                <%} %>
                <%case SearchTypes.Range: {%>
                <%
                    var value1 = value.First();
                    var value2 = value.Last();      
                %>
                <%: Html.TextBox(String.Format("f[{0}].V[0]", i), value1, new { size = 5, @class = !String.IsNullOrWhiteSpace(value1) ? "cw-form-value cw-input-highlight" : "cw-form-value" })%>&nbsp;to&nbsp;
                <%: Html.TextBox(String.Format("f[{0}].V[1]", i), value2, new { size = 5, @class = !String.IsNullOrWhiteSpace(value2) ? "cw-form-value cw-input-highlight" : "cw-form-value" })%>
                <%break;%>
                <%} %>
                <%case SearchTypes.HasValue: {%>
                <%: Html.CheckBox(String.Format("f[{0}].V", i), new { @class = "cw-form-value" })%>
                <%break;%>
                <%} %>
                <%case SearchTypes.IsTrue: {%>
                <%: Html.CheckBox(String.Format("f[{0}].V", i), new { @class = "cw-form-value" })%>
                <%break;%>
                <%} %>
                <%case SearchTypes.Tag: {%>
                <%
                    var tagType = (TagType)item.PropertyId;
                    var tags = Model.SearchTags[tagType];
                    var selectedTagValues = value.First() != null ? value.First().Split(';') : null;
                    var selectedTags = selectedTagValues.Where(v => !String.IsNullOrWhiteSpace(v)).Select(v => int.Parse(v)).ToArray();
                    var model = new TagCloudViewModel() { Tags = tags, TagType = tagType, TagTypeName = item.PropertyName, InitialTagNumber = 5, SelectedTags = selectedTags, TagClass = "cw-tagbox-search", TagIdTemplate = String.Format("f_{0}__V", i) };
                %>    
                <% Html.RenderPartial("ctrlTagCloud", model); %>
                <%: Html.Hidden(String.Format("f[{0}].V", i), "", new { @class = "cw-form-value" })%>
                <%break;%>
                <%} %>
            <%}%>
            </div>
        </li>
        <%} %>
       
        <%--<li>
            <div>&nbsp;</div>
            <a href="#" id="more-options-link">Show More Search Options</a>
            <div>&nbsp;</div>
         </li>--%>
        <li>
            <button id="submit-bottom" type="submit" title="Search" class="cw-button cw-simple cw-blue">
            <span class="b-search">Search</span>
            </button>
            &nbsp;<a href="#" class="cw-reset-form" title="Reset the search form to start over">Clear</a>
        </li>
    <%
        
   } %>
</ul>
</div>
