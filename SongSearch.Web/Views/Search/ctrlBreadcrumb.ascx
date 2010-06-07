<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.SearchViewModel>" %>
<%
	var searchFields = Model.SearchFields;
	var properties = Model.SearchMenuProperties;
	
%>
<%if (searchFields.Count > 0){ %>
	You searched for:
	<%foreach(var searchField in searchFields){ %>
	<%
		var prop = properties.Where(p => p.PropertyId == searchField.P).Single();
		var searchType = (SearchTypes) prop.SearchTypeId;
	
	%>
		<a href="#" class="cw-tag-box cw-button cw-simple cw-small">

		<%switch (searchType) {%>
			<%case SearchTypes.Contains: {%>
			<%: prop.DisplayName%>:
			<%foreach (var v in searchField.V) {%>
				<%:v%>&nbsp;
			<%} %>
			<%break;%>
			<%} %>
			<%case SearchTypes.Join: {%>
			<%goto case (SearchTypes.Contains);	%>	
			<%}%>
			<%case SearchTypes.HasValue: {%>
				<%:prop.DisplayName%>&nbsp;
			<%break;%>
			<%} %>
			<%case SearchTypes.Range: {%>
			<%foreach (var v in searchField.V) {%>
				<%:v%> - 
			<%} %>
			<%break;%>
			<%} %>
			<%case SearchTypes.IsTrue: {%>
			<%:prop.DisplayName%>&nbsp;
			<%break;%>
			<%} %>
			<%case SearchTypes.Tag: {%>
			<% 
				var tagType = (TagType)prop.PropertyId;
				var tags = Model.SearchTags[tagType];
				var tagIds = searchField.V.First().SplitTags(';');
			%>
			<%:tagType%>: 
				<%foreach (var tagId in tagIds) { %>
					<%var tag = tags.Where(t => t.TagId == tagId).Single(); %>
					<%: tag.TagName %>&nbsp;
				<%} %>
			<%break;%>
			<%} %>
		<%} %>

		</a>&nbsp;
	<%} %>
<%} %>