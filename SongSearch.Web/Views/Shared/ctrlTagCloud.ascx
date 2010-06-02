<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagCloudViewModel<SongSearch.Web.Data.Tag>>" %>
<%
    var tags = Model.Tags;
    var hasMoreLinks = Model.InitialTagNumber > 0 && tags.Count > Model.InitialTagNumber;
	var isEditing = Model.EditMode == EditModes.Editing;
%>
<%
    foreach (var tag in tags)//.Take(initialTagNumber))
    {
		if (hasMoreLinks && Model.TagCountSeed == Model.InitialTagNumber)
        { %>
            <div><a class="cw-small cw-tags-more-link" href="#">more <%: Model.TagTypeName.ToString().ToLower()%> choices...</a>
            <div class="cw-more-tags cw-optional">
          <%
        }
        
        var tagName = tag.TagName;

		var tagId = isEditing ? String.Format(Model.TagIdTemplate, Model.TagCountSeed) : String.Format("{0}-{1}", Model.TagIdTemplate, tag.TagId);
		var isSelected = Model.SelectedTags != null && Model.SelectedTags.Contains(tag.TagId);
		var tagClass = String.Concat(Model.TagClass, isSelected ? " cw-blue" : "");
        
     %>
	 <%if (isEditing) { %>
	 <label for="<%= tagId %>"><%: tagName%></label>
	 <%: Html.CheckBox(tagId, isSelected, new { value = tag.TagId })%>
	 <%} else { %>
     <a id="<%= tagId%>" class="cw-search-tag cw-tag-box cw-button cw-simple cw-small <%=tagClass %>"> <%=tagName%></a>
	 <%} %>
    <%
		Model.TagCountSeed++;
    } %>
    <%if (hasMoreLinks)
      { %>
        </div></div>
    <%}%>    
