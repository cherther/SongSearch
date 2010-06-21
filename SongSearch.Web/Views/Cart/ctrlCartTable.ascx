<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<SongSearch.Web.Data.Cart>>" %>
<table class="cw-tbl-carts">
<thead>
<tr>
<% 
	var tableHeader = ViewData["CartHeaders"] as string[];
	var dontshow = new string[] { "Save", "Delete" };
	%>
<th>&nbsp;</th>
<%foreach(var col in tableHeader){ %>
<th><%: !dontshow.Contains(col) ? col : "" %></th>
<%} %> 
</tr>
</thead>
<tbody>
<%
var carts = Model.OrderBy(m => m.CartStatus).ToList();
var cartToHighlight = int.Parse(ViewData["CartToHighlight"] != null ? ViewData["CartToHighlight"].ToString() : "0");
	
foreach (var cart in carts)
{
	var cartClass = cart.CartStatus == (int)CartStatusCodes.Downloaded ? 
		" cw-tbl-carts-archived" 
		: 
			(cart.CartStatus == (int)CartStatusCodes.Processing ? 
			" cw-tbl-carts-processing"	
			: null
		);

	cartClass = cartClass ?? (cart.CartId == cartToHighlight ? "cw-tbl-carts-processing" : "");
	%>                         
	<tr class="cw-tbl-carts-main <%: cartClass %>">
		<td><div id="s-<%: cart.CartId%>" class="cw-carts-contents cw-carts-contents-show">&nbsp;</div></td>
		<td>
			<%: cart.LastUpdatedOn.ToShortDateString()%>
		</td>
		<td>
			<%: cart.ArchiveName%>
		</td>
		<td>
			<%: cart.NumberItems%>
		</td>
		<td>
			<%: cart.CompressedSize.ToFileSizeDescription()%>
		</td>
		<td>
			<%: ((CartStatusCodes)cart.CartStatus)%>
		</td>
		<%if (tableHeader.Contains("Save")) { %><td>
		<%if ((CartStatusCodes)cart.CartStatus != CartStatusCodes.Processing) {%>
		<% using (Html.BeginForm(MVC.Cart.Download(cart.CartId), FormMethod.Post)) { %>
		<%: Html.AntiForgeryToken()%>
			<button type="submit" class="cw-button cw-simple cw-small cw-blue">Download</button>
<%--	        <span class="b-save">Download</span></button>
--%>	    <%}%>
		<%} %>
		</td>
		<%} %>
		<%if (tableHeader.Contains("Delete")) { %>
		<td>
		<% using (Html.BeginForm(MVC.Cart.Delete(cart.CartId), FormMethod.Post)) {%>
		<%: Html.AntiForgeryToken() %>
			<button type="submit" class="cw-button cw-simple cw-small cw-blue">
			<span class="b-delete"><%: (CartStatusCodes)cart.CartStatus == CartStatusCodes.Processing ? "Cancel" : "Delete" %></span></button>
		<%}%>
		</td>
		<%} %>
	</tr>
	<tr id="c-<%: cart.CartId%>" class="cw-tbl-carts-details">
		<td></td>
		<td colspan="<%= tableHeader.Count() %>">
		<%
			var cartContentHeaders = ViewData["CartContentHeaders"] as string[]; 
			var cartContent = new PagedList<SongSearch.Web.Data.Content>(cart.Contents.AsQueryable(), 0, 0);
			var contentListViewModel = new ContentListViewModel() { List = cartContent, ListHeaders = cartContentHeaders, ShowDetails = cart.CartStatus == (int)CartStatusCodes.Active };
		%>
			
		<% Html.RenderPartial(MVC.Cart.Views.ctrlCartContentsTable, contentListViewModel); %>  
		</td>
	</tr>
<%
}
%>
</tbody>
</table>