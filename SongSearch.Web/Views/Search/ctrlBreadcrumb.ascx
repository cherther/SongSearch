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
		var vals = searchField.V.Where(v => !String.IsNullOrWhiteSpace(v)).ToArray();		
	   
	%>
		<a href="#" class="cw-tag-box cw-button cw-simple cw-small">

		<%switch (searchType) {%>
			<%case SearchTypes.Contains: {%>
			<%: prop.DisplayName%>:
			<%foreach (var v in vals) {%>
				'<%:v%>'<%: Array.IndexOf(vals, v) < vals.Count()-1 ? " + " : "" %>
			<%} %>
			<%break;%>
			<%} %>
<%--			<%case SearchTypes.Join: {%>
			<%goto case (SearchTypes.Contains);	%>	
			<%}%>
--%>			<%case SearchTypes.HasValue: {%>
				<%:prop.DisplayName%>&nbsp;
			<%break;%>
			<%} %>
			<%case SearchTypes.Range: {%>
			<%: prop.ShortName%>:
			<%for(var i=0; i<searchField.V.Count(); i++){%>
				<% var v = searchField.V[i]; %>
				<%if (!String.IsNullOrWhiteSpace(v)) { %>
				<%: i == 0 ? "" : " < "%><%:v%>
				<%} %>
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
					<%: tag.TagName %><%: Array.IndexOf(tagIds, tagId) < tagIds.Count() - 1 ? " + " : ""%>
				<%} %>
			<%break;%>
			<%} %>
			<%case SearchTypes.TagText: {%>
			<% var tags = vals.SplitTagNames(','); %>
			<%: prop.DisplayName%>:
			<%foreach (var v in tags) {%>
				'<%:v%>'
				<%: Array.IndexOf(tags, v) < tags.Count() - 1 ? " + " : ""%>
			<%} %>
			<%break;%>
			<%} %>
		<%} %>

		</a>&nbsp;
	<%} %>
<%} %>