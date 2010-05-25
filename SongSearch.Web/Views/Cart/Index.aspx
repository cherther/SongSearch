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
        var content = activeCart != null ? new PagedList<SongSearch.Web.Data.Content>(activeCart.Contents.AsQueryable(), 0, 0) : null;
        var activeCartCount = content != null && content.Count > 0 ? content.Count() : 0;
        
        var contentListViewModel = new ContentListViewModel() {
            List = content,
            ListHeaders = Model.CartContentHeaders//,
            //HeaderSortUrl = Model.HeaderSortUrl,
            //SearchMenuProperties = Model.SearchMenuProperties,
            //RequestUrl = Model.RequestUrl,
            //SortPropertyId = Model.SortPropertyId,
            //SortType = Model.SortType,
        
        };
    %>
    <p>Your song cart contains <strong><%=activeCartCount%></strong> item(s).</p>
    <% using (Html.BeginForm("Zip", "Cart", FormMethod.Post))
	    { %>
        <%if (activeCartCount  > 0) {%>
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
    <%}%>

    </div>
</div>
</asp:Content>

