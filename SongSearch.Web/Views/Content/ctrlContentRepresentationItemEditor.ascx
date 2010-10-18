<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentRepresentationItemViewModel>" %>
<%	
	var modelId = Model.ModelId;
	var contentRep = Model.ContentRepresentation;

	var itemName = String.Format("representation[{0}].", modelId);
	var itemId = String.Format("r_{0}_", modelId);
	var repShare = contentRep.RepresentationShare.ToString("P2");
%>
	<tr>
		<td class="cw-content-field">
			<%: Html.Hidden(String.Concat(itemName, "ContentId"), contentRep.ContentId)%>
			<%: Html.Hidden(String.Concat(itemName, "ContentRepresentationId"), contentRep.ContentRepresentationId)%>
			<%: Html.DropDownList(String.Concat(itemName, "RightsTypeId"), 
				new SelectList(ModelEnums.GetRightsTypes(), 
					(RightsTypes)contentRep.RightsTypeId), 
								new { title = "Select a rights type from the list" })%>
		</td>
		<td class="cw-content-field text-right">
			<%: Html.TextBox(String.Concat(itemName, "RepresentationShare"), repShare, new { @class = "cw-field-small", title = "Enter a percentage, e.g. 33.33% = 33.33" })%>
		</td>
		<td>
			<%//if (contentRep != null && contentRep.ContentRepresentationId > 0) { %>
			<%: Html.Hidden(String.Concat(itemName, "ModelAction"), (int)ModelAction.Update, new { @class = "cw-model-action" })%>
			<a href="#" class="cw-delete-right-link" title="Delete"><img src="../../public/images/icons/silk/delete.png" alt="Delete" /></a>
			<%//} else { %>
			<%//: Html.Hidden(String.Concat(itemName, "ModelAction"), (int)ModelAction.Add, new { @class = "cw-model-action" })%>
			<%//} %>
		</td>
		
	
	</tr>
	<tr>
		<td colspan="3">
		<%		
		var model = new TagCloudViewModel<SongSearch.Web.Data.Territory>() {
			EditMode = EditModes.Editing,
			Tags = Model.Territories,
			SelectedTags = contentRep != null ? contentRep.Territories.Select(x => x.TerritoryId).ToArray() : null,
			NumberTagsInRow = 7,
			TagClass = "cw-tagbox-label",
			TagIdTemplate = String.Concat(itemId, "t_{0}"),
			TagNameTemplate = String.Concat(itemName, "Territories[{0}]")
			};              
		%>    
		Territories:
		<%: Html.Partial(MVC.Shared.Views.ctrlTerritoryCloud, model) %>
		</td>
	</tr>