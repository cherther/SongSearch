<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagCloudViewModel<SongSearch.Web.Data.Territory>>" %>
<%
	
    var tags = Model.Tags;
    var hasMoreLinks = Model.InitialTagNumber > 0 && tags.Count > Model.InitialTagNumber;
	var isEditing = Model.EditMode == EditModes.Editing;
%>
<%
    var tagCount = 0;
    foreach (var tag in tags)//.Take(initialTagNumber))
    {
        if (hasMoreLinks && tagCount == Model.InitialTagNumber)
        { %>
            <div><a class="cw-small cw-tags-more-link" href="#">more <%: Model.TagTypeName.ToString().ToLower()%> choices...</a>
            <div class="cw-more-tags cw-optional">
          <%
        }
        
        var tagName = tag.TerritoryName;
        var tagId = isEditing? String.Format(Model.TagIdTemplate, tagCount) : String.Format("{0}-{1}", Model.TagIdTemplate, tag.TerritoryId);
		var isSelected = Model.SelectedTags != null && Model.SelectedTags.Contains(tag.TerritoryId);
		var tagClass = String.Concat(Model.TagClass, isEditing ? "-edit" : "", " cw-button cw-simple cw-small ", isSelected ? " cw-blue" : "");
        
     %>
	 <%if (isEditing) { %>
	 <label for="<%= tagId %>" class="<%: tagClass %>"><%: tagName%></label>
	 <%: Html.CheckBox(tagId, isSelected, new { id = tagId, value = tag.TerritoryId, @class = "cw-tagbox-checkbox" })%>
	 <%} else { %>
     <a id="<%= tagId%>" class="cw-search-tag cw-tag-box cw-button cw-simple cw-small <%=tagClass %>"> <%=tagName%></a>
	 <%} %>
    <%
        tagCount++;
    } %>
    <%if (hasMoreLinks)
      { %>
        </div></div>
    <%}%>    
