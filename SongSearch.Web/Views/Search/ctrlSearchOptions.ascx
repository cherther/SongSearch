<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.SearchViewModel>" %>
<ul id="search-menu" class="cw-list-searchoptions">
    <%using (Html.BeginForm("Results", "Search", FormMethod.Get, new { id = "searchForm"})) { %>
        <li>
            <button id="submit-top" type="submit" title="Search" class="cw-button cw-simple cw-blue">
            <span class="b-search">Search</span>
            </button>
            <a href="#" class="reset" title="Reset the search form to start over">New</a>
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
                <%: Html.TextBox(String.Format("f[{0}].V", i), value.First(), new { @class = !String.IsNullOrWhiteSpace(value.First()) ? "cw-input-highlight" : "" })%>
                <%break;%>
                <%} %>
                <%case SearchTypes.Join: {%>
                <%: Html.TextBox(String.Format("f[{0}].V", i), value.First())%>
                <%break;%>
                <%} %>
                <%case SearchTypes.Range: {%>
                <%
                    var value1 = value.First();
                    var value2 = value.Last();      
                %>
                <%: Html.TextBox(String.Format("f[{0}].V[0]", i), value1, new { size = 5, rel = "cw-first", @class = !String.IsNullOrWhiteSpace(value1) ? "cw-input-highlight" : "" })%>&nbsp;to&nbsp;
                <%: Html.TextBox(String.Format("f[{0}].V[1]", i), value2, new { size = 5, rel = "cw-second", @class = !String.IsNullOrWhiteSpace(value2) ? "cw-input-highlight" : "" })%>
                <%break;%>
                <%} %>
                <%case SearchTypes.HasValue: {%>
                <%: Html.CheckBox(String.Format("f[{0}].V", i))%>
                <%break;%>
                <%} %>
                <%case SearchTypes.IsTrue: {%>
                <%: Html.CheckBox(String.Format("f[{0}].V", i))%>
                <%break;%>
                <%} %>
                <%case SearchTypes.Tag: {%>
                <%
                    var tagType = (TagType)item.PropertyId;
                    var tags = Model.SearchTags[tagType];
                    var selectedTags = value != null && value.All(v => !String.IsNullOrWhiteSpace(v)) ? value.Select(v => int.Parse(v)).ToArray() : null;
                    var model = new TagCloudViewModel() { Tags = tags, TagType = tagType, InitialTagNumber = 5, SelectedTags = selectedTags };
                %>    
                <% Html.RenderPartial("ctrlTagCloud", model); %>
                <%: Html.Hidden(String.Format("f[{0}].V", i)) %>
                <%break;%>
                <%} %>
            <%}%>
            </div>
        </li>
        <%} %>
       
        <li>
            <div>&nbsp;</div>
            <a href="#" id="more-options-link">Show More Search Options</a>
            <div>&nbsp;</div>
         </li>
        <li>
            <button id="submit-bottom" type="submit" title="Search" class="cw-button cw-simple cw-blue">
            <span class="b-search">Search</span>
            </button>
            <a href="#" class="reset" title="Reset the search form to start over">New</a>
        </li>
    <%
        
   } %>
</ul>

