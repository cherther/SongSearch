<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentListViewModel>" %>
<%
	var results = Model.List;
	var request = Model.RequestUrl;
	var printUrl = request.Replace("Results?", "Print?");
	var sortUrl = Model.HeaderSortUrl;
	var props = Model.SearchMenuProperties;
	var tableHeader = Model.ListHeaders;
	var rightCols = new string[]{ "Pop", "Country", "ReleaseYear" };
	var addAllToCartLink = Url.Action("AddMultiple", "Cart");
	
	var isPrint = Model.ViewMode == ViewModes.Print;
 %>
	
	<table width="100%" class="cw-tbl-search-results">
		<%if (!isPrint) { %>
		<tr>
		<td colspan="2" style="text-align: left; white-space: nowrap; padding-left: 1px">
			<a href="<%: addAllToCartLink %>" id="cw-add-all-to-cart" title="Add all selected songs to your cart" class="cw-button cw-simple cw-small cw-blue">
				<span class="b-cart">Add</span>
			</a>&nbsp;
		</td>
		<td colspan="<%: tableHeader.Count()-1%>" align="right">
			<a href="<%:printUrl%>" class="cw-button cw-simple cw-small cw-blue" title="Print these search results" target="_new">
				<span class="b-print">Print</span>
			</a>
		</td>
		</tr>
		<%} %>
		<tr>
		<%if (!isPrint) { %>
			<th style="text-align: left; white-space: nowrap; padding-left: 5px">
			<input type="checkbox" id="cw-select-all-items-check" title="Click to select all songs shown" />
			</th>
		<%} %>
		<%foreach(var col in tableHeader){ %>
		<% var classId = rightCols.Contains(col) ? "text-right" : ""; %>
			<th class="<%: classId %>">
				<%
					var prop = props.Where(p => p.PropertyName.Equals(col)).SingleOrDefault();
					if (prop != null){
						var sp = prop.PropertyId;
						var ord = Model.SortPropertyId.GetValueOrDefault().Equals(sp) ? (int)Model.SortType.Flip() : (int)SortType.Ascending;
						var colUrl = String.Format("{0}&s={1}&o={2}", sortUrl, sp, ord);
					%>
					<a href="<%:colUrl%>"  title="Sort by <%: prop.DisplayName%>"><%: prop.ShortName%></a>
					<%} else { %>
					<%: col%>
					<%}%>
			</th>
			<%} %>
		</tr>

	<% foreach (var item in results) { %>
		<% 
			var mediaUrl = item.HasMediaFullVersion ? item.ContentMedia.FullVersion().MediaUrl() : "";
		
			//var mediaUrl = Url.SiteRoot();
			//mediaUrl = item.HasMediaFullVersion ? 
			//    String.Concat(mediaUrl, Url.Action(MVC.Media.Stream(item.ContentId, MediaVersion.Full))) : "";

			var titleLength = 30;
			var title = (!String.IsNullOrWhiteSpace(item.Title) ?
					item.Title.Length > titleLength ? String.Concat(item.Title.Substring(0, titleLength), "...")
					: item.Title
					: "(N/A)").ToUpper();

			var artistLength = 15;
			var artist = (!String.IsNullOrWhiteSpace(item.Artist) ?
					item.Artist.Length > artistLength ? String.Concat(item.Artist.Substring(0, artistLength), "...")
					: item.Artist
					: "(N/A)").ToUpper();
		    //var artistUrl = String.Concat("/Artist/", Url.Encode(item.Artist), "*");
			var artistUrl = String.Concat("/Search/Results?f[0].P=2&f[0].T=1&f[0].V=", Url.Encode(item.Artist));
	
			var cartState = item.IsInMyActiveCart ? @"checked=checked disabled=disabled" : @"class=add-to-cart-checkbox";
		%>
		<tr class="cw-tbl-data">
			<%if (!isPrint) { %>
			<td width="5%">
			<input type="checkbox" id="<%: item.ContentId.ToString() %>" <%: cartState %> />
			</td>
			<%} %>
<%--			style="white-space: nowrap"--%>
			<td width="40%">
				<%: !isPrint ? Html.ActionLink(title, MVC.Content.Detail(item.ContentId), new { @class = "cw-content-detail-link", title = "Show/hide song details", rel = mediaUrl }) : MvcHtmlString.Create(title) %>
			</td>
			<td width="30%">
				<%: artist %>
				<% if (!isPrint) {%>
				&nbsp;<a href="<%: artistUrl%>" title="Show more songs by <%: artist%>"><img src="/public/images/icons/arrow.gif" alt="right-arrow"/></a>
				<%} %>
			</td>
			<td width="10%" class="text-right">
				<%: item.Pop %>
			</td>
			<td width="10%" class="text-right">
				<%: item.Country %>
			</td>
			<td width="5%" class="text-right">
				<%: item.ReleaseYear %>
			</td>
		   
		</tr>
	
	<% } %>

	</table>