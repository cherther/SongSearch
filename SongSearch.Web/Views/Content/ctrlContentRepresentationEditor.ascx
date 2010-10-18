<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentViewModel>" %>
<%
	var contentRepShares = Model.Content.ContentRepresentations;
	var territories = Model.Territories;
				
	 %>
<table class="cw-tbl-content-representation">
	<tr>
		<th>
			Rights Type
		</th>
		<th class="text-right" style="text-align: right">
			Representation Share %
		</th>
		<th>
		</th>
	</tr>
	<%var r = 0; %>
	<%if (contentRepShares != null && contentRepShares.Count() > 0) { %>
		<%foreach (var contentRep in contentRepShares) { %>
		<%	
			var itemViewModel = new ContentRepresentationItemViewModel() {
				ModelId = r,
				ContentRepresentation = contentRep,
				EditMode = Model.EditMode,
				Territories = Model.Territories
			};
		%>								
		
		<%: Html.Partial(MVC.Content.Views.ctrlContentRepresentationItemEditor, itemViewModel)%>
		<%	r++; %>    
		<%} %>
	<%} %>
	<%--<%	
		var newItemViewModel = new ContentRepresentationItemViewModel() {
			ModelId = r,
			ContentRepresentation = new SongSearch.Web.Data.ContentRepresentation() { ContentId = Model.Content != null ? Model.Content.ContentId : 0 },
			EditMode = Model.EditMode,
			Territories = Model.Territories
		};
		
		
	%>								
	<%: Html.Partial(MVC.Content.Views.ctrlContentRepresentationItemEditor, newItemViewModel)%>
	<%	r++; %>   	--%>
	<tr id="cw-add-rep-row">
		<td colspan="3">
			<%: Html.Hidden("modelId", r)%>
			<a href="<%: Url.Action(MVC.Content.AddNewRepresentation()) %>" class="cw-add-rep-link cw-button cw-simple cw-small" title="Add New"><span class="b-add">Add New</span></a>
		</td>
	</tr>
</table>
