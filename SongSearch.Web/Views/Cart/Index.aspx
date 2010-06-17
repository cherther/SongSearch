<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.CartViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.PageTitle %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class = "cw-outl">
	<h2><%: Model.PageTitle %></h2>
	<div class="cw-outl cw-carts">
			  <h3>(1) Ready to Download</h3>
	<%
		
		
		var activeCart = Model.MyCarts.Where(c => c.CartStatus == (int)CartStatusCodes.Active).SingleOrDefault();

		var activeCartContent = activeCart != null ? new PagedList<SongSearch.Web.Data.Content>(activeCart.Contents.AsQueryable(), 0, 0) : null;
		var activeCartCount = activeCartContent != null && activeCartContent.Count > 0 ? activeCartContent.Count() : 0;
		
		var contentListViewModel = new ContentListViewModel() { List = activeCartContent, ListHeaders = Model.CartContentHeaders, ShowDetails = true };

		var processingCarts = Model.MyCarts.Where(c => c.CartStatus == (int)CartStatusCodes.Processing).OrderByDescending(c => c.LastUpdatedOn).ToList();
		
		var compressedCarts = Model.MyCarts.Where(c => c.CartStatus > (int)CartStatusCodes.Active && c.CartStatus < (int)CartStatusCodes.Processing).OrderByDescending(c => c.LastUpdatedOn).ToList();
		
		
	%>
	<%if (activeCartCount  > 0) {%>
	<p>Your song cart contains <strong><%=activeCartCount%></strong> item(s).</p>
	<% using (Html.BeginForm(MVC.Cart.Zip(), FormMethod.Post, new { id ="cw-cart-form" })) { %>
	<%= Html.AntiForgeryToken()%>
	<p>To create a single zip file containing all items, click the 'Zip My Song Cart' button below. </p>
	<div>&nbsp;</div>
		<div>
			<label>Zip file name (or leave blank for default):</label>
		</div>
		<div>
			<%=Html.TextBox("userArchiveName", "", new { size = 50 })%>
	   
				
			<button type="submit" class="cw-button cw-simple cw-blue">
			<span class="b-download2">Zip My Song Cart</span>
			</button>
		</div>
		<div>&nbsp;</div>
		<% Html.RenderPartial(MVC.Cart.Views.ctrlCartContentsTable, contentListViewModel); %>

		<%}%>

	<%} else {%>
	 <p>&nbsp;</p><p>Your Song Cart is empty.</p>
	<p>&nbsp;</p>
	<p>To put something in your Song Cart, start by <%: Html.ActionLink("searching", MVC.Search.Index()) %> our database. 
	When you find a song you like, click the <em>Add to Cart</em> button.</p>
	<% }%>

	</div>
	<%if (processingCarts != null && processingCarts.Count() > 0) { %>
	<div>&nbsp;</div>
	<hr />
	<div>&nbsp;</div>
	<div class="cw-outl cw-carts">
		  <h3>Processing</h3>
		  <p>We're currently compressing and creating a zip file with the song files in your active cart. Once this is complete, we'll move the finished zip file to the Zipped Song Carts section below.</p>
		  <p>&nbsp;</p>
		  <p>Please check back in a couple of minutes or <%: Html.ActionLink("refresh", MVC.Cart.Index()) %> this page.</p>
		  <% ViewData["CartHeaders"] = new string[] { "Date", "Zip File", "# Songs", "Size", "Status" }; %>
		  <% ViewData["CartContentHeaders"] = new string[] { "Title", "Artist", "Year" }; %>
		  <% Html.RenderPartial(MVC.Cart.Views.ctrlCartTable, processingCarts); %>
	</div>
	<%} %>


	<div>&nbsp;</div>
	<hr />
	<div>&nbsp;</div>
	<div class="cw-outl cw-carts">
		  <h3>(2) Zipped Song Carts</h3>
		  <%if (compressedCarts != null && compressedCarts.Count() > 0) { %>
		  <p>Your zipped up song carts are listed below. To save them to your computer, click the 'Download' button next to each file.</p>
		  <p>We'll keep zipped carts here for 14 days after you have requested them.</p>
		  <% ViewData["CartHeaders"] = new string[] { "Date", "Zip File", "# Songs", "Size", "Status", "Save", "Delete" }; %>
		  <% ViewData["CartContentHeaders"] = new string[] { "Title", "Artist", "Year" }; %>
		  <% ViewData["CartToHighlight"] = Model.CartToHighlight; %>
		  <% Html.RenderPartial(MVC.Cart.Views.ctrlCartTable, compressedCarts); %>
		  <%} else {%>
		  <p>You have no zip files waiting to be downloaded. </p>
		  <%} %>
	</div>
	
</div>
</asp:Content>

