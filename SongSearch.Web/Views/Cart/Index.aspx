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


        var compressedCarts = Model.MyCarts.Where(c => c.CartStatus == (int)CartStatusCodes.Compressed).OrderByDescending(c => c.LastUpdatedOn).ToList();
        var downloadedCarts = Model.MyCarts.Where(c => c.CartStatus == (int)CartStatusCodes.Downloaded).OrderByDescending(c => c.LastUpdatedOn).ToList();
        
        
    %>
    <%if (activeCartCount  > 0) {%>
    <p>Your song cart contains <strong><%=activeCartCount%></strong> item(s).</p>
    <% using (Html.BeginForm("Zip", "Cart", FormMethod.Post))
	    { %>
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
        <% Html.RenderPartial("ctrlCartContentsTable", contentListViewModel); %>

        <%}%>

    <%} else {%>
     <p>&nbsp;</p><p>Your Song Cart is empty.</p>
    <p>&nbsp;</p>
    <p>To put something in your Song Cart, start by <%: Html.ActionLink("searching", "", "Search",null, null) %> our database. 
    When you find a song you like, click the <em>Add to Cart</em> button.</p>
    <% }%>

    </div>
    <div>&nbsp;</div>
    <hr />
    <div>&nbsp;</div>
    <div class="cw-outl cw-carts">
		  <h3>(2) Zipped Song Carts</h3>
          <%if (compressedCarts != null && compressedCarts.Count() > 0) { %>
          <p>Your zipped up song carts are listed below. To save them to your computer, click the 'Save' button next to the file you want to download.</p>
		  <p>Zipped carts will be shown here for 7 days after you have requested them, then they will move to the 'Zipped Song Carts Archive' section below.</p>
          <% ViewData["CartHeaders"] = new string[] { "Date", "Zip File", "# Songs", "Size", "Status", "Save" }; %>
          <% ViewData["CartContentHeaders"] = new string[] { "Title", "Artist", "Year" }; %>
          <% Html.RenderPartial("ctrlCartTable", compressedCarts); %>
          <%} else {%>
          <p>You have no zip files waiting to be downloaded. </p>
          <%} %>
    </div>
    <div>&nbsp;</div>
    <hr />
    <div>&nbsp;</div>
    <div class="cw-outl cw-carts">
		 
          <%if (downloadedCarts != null && downloadedCarts.Count() > 0) { %>
		  <h3>(3) Zipped Song Carts Archive</h3>
          <p>Previously downloaded zip files appear below.</p>
		  <p>To save them to your computer, click the 'Save' button next to the file you want to download, or 'Delete' the ones you no longer need.</p>
		  <p>Previously downloaded files will be stored up to 7 days after you have first downloaded them.</p>
          <% ViewData["CartHeaders"] = new string[] { "Date", "Zip File", "# Songs", "Size", "Status", "Save", "Delete" }; %>
          <% ViewData["CartContentHeaders"] = new string[] { "Title", "Artist", "Year" }; %>
          <% Html.RenderPartial("ctrlCartTable", downloadedCarts); %>
          <%} %>

    </div>
</div>
</asp:Content>

