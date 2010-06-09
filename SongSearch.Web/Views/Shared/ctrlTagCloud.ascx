<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagCloudViewModel<SongSearch.Web.Data.Tag>>" %>
<%
	var tagCount = Model.InitialTagNumber > 0 ? Model.InitialTagNumber : Model.Tags.Count();
	
	//if we're taking a top 5 etc, we'll sort the rest by alpha
	var tags = Model.InitialTagNumber > 0 ? 
		(Model.TagCountSeed == 0 ? Model.Tags.Take(tagCount) : Model.Tags.Skip(tagCount).OrderBy(t => t.TagName).AsEnumerable())
		: Model.Tags;
	var hasMoreLinks = Model.TagCountSeed == 0 && Model.InitialTagNumber > 0 && Model.Tags.Count > Model.InitialTagNumber;
	var isEditing = Model.EditMode == EditModes.Editing;
	var tagDivClass = Model.InitialTagNumber > 0 && Model.TagCountSeed > 0 ? "cw-more-tags cw-optional" : "";
	
	%>
<div class="<%= tagDivClass %>">
<%
	foreach (var tag in tags)
	{
		var tagName = tag.TagName;

		var tagId = isEditing ? String.Format(Model.TagIdTemplate, Model.TagCountSeed) : String.Format("{0}-{1}", Model.TagIdTemplate, tag.TagId);
		var isSelected = Model.SelectedTags != null && Model.SelectedTags.Contains(tag.TagId);
		var tagClass = String.Concat(Model.TagClass, isEditing ? "-edit" : "", " cw-button cw-simple cw-small ", isSelected ? " cw-blue" : "");
		
	 %>
	 <%if (isEditing) { %>
	 <label for="<%= tagId %>" class="<%: tagClass %>"><%: tagName%></label>
	 <%: Html.CheckBox(tagId, isSelected, new { id = tagId, value = tag.TagId, @class = "cw-tagbox-checkbox" })%>
	 <%} else { %>
	 <a id="<%= tagId%>" class="<%=tagClass %>"> <%=tagName%></a>
	 <%} %>
	<%
		Model.TagCountSeed++;
	} %>
</div>
<%if (hasMoreLinks){ %>
<% Html.RenderPartial(MVC.Shared.Views.ctrlTagCloud, Model); %>
<%}%>