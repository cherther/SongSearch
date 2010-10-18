<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentViewModel>" %>
<%
	var contentRepShares = Model.Content.ContentRepresentations;
	var territories = Model.Territories;
				
	 %>
	<table class="cw-tbl-content-representation" style="width: 95%">
	<tr>
		<th>
			Rights Type
		</th>
		<th class="text-right" style="text-align: right">
			Representation Share %
		</th>
		<th>Territories</th>
	</tr>
	<%var r = 0; %>
	<%if (contentRepShares != null && contentRepShares.Count() > 0) { %>
		<%foreach (var contentRep in contentRepShares) { %>
		<%	
			var itemName = String.Format("representation[{0}].", r);
			var itemId = String.Format("r_{0}_", r);
			r++;

			var repShare = contentRep.RepresentationShare.ToString("P2");
		%>
		<tr>
			<td class="cw-content-field" style="width:25%"><%: (RightsTypes)contentRep.RightsTypeId%></td>
			<td class="cw-content-field text-right" style="width:25%"><%: repShare%></td>
			<td class="cw-content-field">
			<%    
			var selectedTerritories = contentRep.Territories.Select(x => x.TerritoryId).ToArray();			
			var model = new TagCloudViewModel<SongSearch.Web.Data.Territory>() {
				EditMode = EditModes.Viewing,
				Tags = contentRep.Territories.ToList(),
				SelectedTags = selectedTerritories,
				NumberTagsInRow = 7,
				TagClass = "cw-tagbox-label",
				TagIdTemplate = String.Concat(itemId, "t_{0}"),
				TagNameTemplate = String.Concat(itemName, "Territories[{0}]")
				};              
				%>
				<% Html.RenderPartial(MVC.Shared.Views.ctrlTerritoryCloud, model); %>
				<%//} %>
			</td>
		</tr>
		<%} %>
	<%} %>
</table>
