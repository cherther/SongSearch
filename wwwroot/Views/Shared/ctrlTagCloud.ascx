<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagCloudViewModel<SongSearch.Web.Data.Tag>>" %>
<%
	var tagCount = Model.InitialTagNumber > 0 ? Model.InitialTagNumber : Model.Tags.Count();
	var rowSize = Model.NumberTagsInRow > 0 ? Model.NumberTagsInRow : 5;
	
	//if we're taking a top 5 etc, we'll sort the rest by alpha
	var tags = Model.InitialTagNumber > 0 ? 
		(Model.TagCountSeed == 0 ? Model.Tags.Take(tagCount) : Model.Tags.Skip(tagCount).OrderBy(t => t.TagName).AsEnumerable())
		: Model.Tags;
	var hasMoreLinks = Model.TagCountSeed == 0 && Model.InitialTagNumber > 0 && Model.Tags.Count > Model.InitialTagNumber;
	var isEditing = Model.EditMode == EditModes.Editing;
	var tagDivClass = Model.InitialTagNumber > 0 && Model.TagCountSeed > 0 ? "cw-more-tags cw-optional" : "";
	
	%>
<div class="<%: tagDivClass %>">
<%
	var i = 1;
	var rowIsComplete = true;
	foreach (var tag in tags)
	{
		var tagDisplay = tag.TagName;
		var tagId = isEditing ? String.Format(Model.TagIdTemplate, Model.TagCountSeed) : String.Format(Model.TagIdTemplate, tag.TagId);
		var tagName = isEditing ? String.Format(Model.TagNameTemplate, Model.TagCountSeed) : String.Format(Model.TagNameTemplate, tag.TagId); 
		// : String.Format("{0}-{1}", Model.TagNameTemplate, tag.TerritoryId);
		var isSelected = Model.SelectedTags != null && Model.SelectedTags.Contains(tag.TagId);
		var tagClass = String.Concat(Model.TagClass, isEditing ? "-edit" : "", " cw-button cw-simple cw-small ", isSelected ? " cw-blue" : "");

		
	 %>
	 <%if (rowIsComplete){//(i == 1) || (i - 1 % 6 == 0)) { %>
		<div>
	<%} %>
	<%rowIsComplete = false;%>
	 <%if (isEditing) { %>
		<%var tagLocatorClass = String.Concat("tag_", tag.TagId); %>
		<label for="<%: tagId %>" class="<%: tagClass %> <%: tagLocatorClass %>"><%: tagDisplay%></label>
		<%: Html.CheckBox(tagName, isSelected, new { id = tagId, value = tag.TagId, @class = String.Concat("cw-tagbox-checkbox ", tagLocatorClass) })%>
		<%if (tag.CreatedByUserId == Model.ActiveUserId) { %>
		<%var deleteUrl = Url.Action(MVC.Content.DeleteTag(tag.TagId)); %>
		<a href="<%: deleteUrl %>" class="cw-delete-tag-link" rel="<%: tagLocatorClass %>" title="Delete this tag"><img src="../../public/images/icons/silk/delete.png" alt="Delete" /></a>
		<%} %>
	 <%} else { %>
	 <a id="<%: tagId%>" class="<%:tagClass %>" rev="<%: Model.TagNameTemplate %>" rel="<%: tag.TagId %>"> <%: tagDisplay%></a>
	 <%} %>
	 <%if (i % rowSize == 0) { %>
		<% rowIsComplete = true; %>
		</div>
	<%} %>
	<%
		i++;
		Model.TagCountSeed++;
	} %>
	<%if (!rowIsComplete) { %>
	</div>
	<%} %>
	
</div>
<%if (hasMoreLinks){ %>
<% Html.RenderPartial(MVC.Shared.Views.ctrlTagCloud, Model); %>
<%}%>