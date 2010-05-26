﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<SongSearch.Web.Data.Cart>>" %>
<table class="cw-tbl-carts">
<thead>
<tr>
<% var tableHeader = ViewData["CartHeaders"] as string[]; %>
<th>&nbsp;</th>
<%foreach(var col in tableHeader){ %>
<th><%: col%></th>
<%} %> 
</tr>
</thead>
<tbody>
<%
foreach (var cart in Model)
{
	%>                         
	<tr class="cw-tbl-carts-main">
	    <td><div id="s-<%: cart.CartId%>" class="cw-tbl-carts-contents cw-tbl-carts-contents-show">&nbsp;</div></td>
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
	    <td>
	    <% using (Html.BeginForm("Download", "Cart", new { id = cart.CartId }, FormMethod.Post)) { %>
	        <button type="submit" class="cw-button cw-simple cw-small cw-blue">
	        <span class="b-save">Save</span></button>
	    <%}%>
	    </td>
        <%if (tableHeader.Contains("Delete")) { %>
        <td>
	    <% using (Html.BeginForm("Delete", "Cart", new { id = cart.CartId }, FormMethod.Post)) {%>
	        <button type="submit" class="cw-button cw-simple cw-small cw-blue">
	        <span class="b-delete">Delete</span></button>
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
			
	    <% Html.RenderPartial("ctrlCartContentsTable", contentListViewModel); %>  
	    </td>
	</tr>
<%
}
%>
</tbody>
</table>