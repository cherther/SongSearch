<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.SearchViewModel>" %>
<ul id="search-menu">
    <%using (Html.BeginForm("Results", "Search", FormMethod.Get, new { id = "searchForm"})) { %>
        <li>
            <button id="submit-top" type="submit" title="Search" class="cw-button cw-simple cw-blue">
            <span class="b-search">Search</span>
            </button>
            <a href="#" class="reset" title="Reset the search form to start over">New</a>
        </li>
        <%
          var searchMenuProperties = Model.SearchMenuProperties;
          var searchFields = Model.SearchFields;
        %>
        <%foreach (var item in searchMenuProperties) { %>
        <%
              var i = searchMenuProperties.IndexOf(item); 
              var searchType = (SearchTypes) item.SearchTypeId;
              var value = searchFields != null && searchFields.Count > 0 ? searchFields[i].V : new string[] {"",""};
              %>
        <li>
            <label><%: item.PropertyName %></label>
            <%: Html.Hidden(String.Format("s[{0}].P", i), item.PropertyId)%>
            <%: Html.Hidden(String.Format("s[{0}].T", i), item.SearchTypeId)%>

            <%switch (searchType) {%>
                <%case SearchTypes.Contains: {%>
                <%: Html.TextBox(String.Format("s[{0}].V", i), value[0])%>
                <%break;%>
                <%} %>
                <%case SearchTypes.Range: {%>
                <%: Html.TextBox(String.Format("s[{0}].V", i), value[0])%>&nbsp;to&nbsp;
                <%: Html.TextBox(String.Format("s[{0}].V", i), value[1])%>
                <%break;%>
                <%} %>
                <%case SearchTypes.HasValue: {%>
                <%: Html.CheckBox(String.Format("s[{0}].V", i))%>
                <%break;%>
                <%} %>
                <%case SearchTypes.Tag: {%>
                <%: Html.TextBox(String.Format("s[{0}].V", i))%>
                <%break;%>
                <%} %>
            <%}%>
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

