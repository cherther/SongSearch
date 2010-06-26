<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentListViewModel>" %>
<%
	var results = Model.List;
	var request = Model.RequestUrl;
	var printUrl = request.Replace("Results?", "Print?");
	var sortUrl = Model.HeaderSortUrl;
	var props = Model.SearchMenuProperties;
	var tableHeader = Model.ListHeaders;
	var tableHeaderCount = tableHeader.Count();
	var headerActions = Model.GridActions.Where(a => a != GridAction.ShowDetails);
	tableHeaderCount += headerActions.Count();
	var rightCols = new string[]{ "Pop", "Country", "ReleaseYear" };
	var addAllToCartLink = Url.Action("AddMultiple", "Cart");

	var deleteContentLink = "";
	
	var isPrint = Model.ViewMode == ViewModes.Print;
 %>
	
	<table width="100%" class="cw-tbl-search-results">
		<%if (!isPrint && Model.GridActions.Count() > 0) { %>
		<tr>
		<td colspan="<%: tableHeaderCount-1%>" style="text-align: left; white-space: nowrap; padding-left: 1px">
			<%if (headerActions.Contains(GridAction.AddToCart)) { %>
			<a href="<%: addAllToCartLink %>" id="cw-add-all-to-cart" class="cw-button cw-simple cw-small cw-blue">
				<span class="b-cart">Add</span>
			</a>&nbsp;
			<%} %>
			<%if (headerActions.Contains(GridAction.RemoveFromCart)) { %>
			<button type="submit" id="cw-remove-all-from-cart" class="cw-button cw-simple cw-small cw-red" value="Remove" />
			<%} %>&nbsp;
			<%if (headerActions.Contains(GridAction.Delete)) { %>
			<a href="<%: deleteContentLink %>" id="cw-delete-content" class="cw-button cw-simple cw-small cw-red">
				<span class="b-delete">Delete</span>
			</a>&nbsp;
			<%} %>
	
		</td>
		<td align="right">
			<a href="<%=printUrl%>" class="cw-button cw-simple cw-small cw-blue" target="_new">
				<span class="b-print">Print</span>
			</a>
		</td>
		</tr>
		<%} %>
		<tr>
		<%if (!isPrint && (headerActions.Count() > 0)) { %>
			<th style="text-align: left; white-space: nowrap; padding-left: 5px">
			<input type="checkbox" id="cw-select-all-items-check" />
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
					<a href="<%=colUrl%>"><%: prop.ShortName%></a>
					<%} else { %>
					<%: col%>
					<%}%>
			</th>
			<%} %>
		</tr>

	<% foreach (var item in results) { %>
		<% 
			var mediaUrl = Url.SiteRoot();
			mediaUrl = item.HasMediaFullVersion ? 
				String.Concat(mediaUrl, Url.Action(MVC.Media.Stream(item.ContentId, MediaVersion.FullSong))) : "";

			var titleLength = 45;
			var title = (!String.IsNullOrWhiteSpace(item.Title) ?
					item.Title.Length > titleLength ? String.Concat(item.Title.Substring(0, titleLength), "...")
					: item.Title
					: "(N/A)").ToUpper();

			var artistLength = 35;
			var artist = (!String.IsNullOrWhiteSpace(item.Artist) ?
					item.Artist.Length > artistLength ? String.Concat(item.Artist.Substring(0, artistLength), "...")
					: item.Artist
					: "(N/A)").ToUpper();
		    //var artistUrl = String.Concat("/Artist/", Url.Encode(item.Artist), "*");
			var artistUrl = String.Concat("/Search/Results?f[0].P=2&f[0].T=1&f[0].V=", Url.Encode(item.Artist));
	
			var cartState = item.IsInMyActiveCart ? @"checked=checked disabled=disabled" : @"class=add-to-cart-checkbox";
		%>
		<tr class="cw-tbl-data">
			<%if (!isPrint && (headerActions.Count() > 0)) { %>
			<td>
			<%if (headerActions.Contains(GridAction.AddToCart)) { %>
			<input type="checkbox" id="<%: item.ContentId.ToString() %>" <%: cartState %> />
			<%} %>
			<%if (headerActions.Contains(GridAction.RemoveFromCart)) { %>
			<input type="checkbox" id="<%: item.ContentId.ToString() %>" name="<%: item.ContentId.ToString() %>" value="<%: item.ContentId %>" class="remove-from-cart-checkbox"/>
			<%} %>
			</td>
			<%} %>
			<td width="40%" style="white-space: nowrap">
				<%: !isPrint ? Html.ActionLink(title, MVC.Content.Detail(item.ContentId), new { @class = "cw-content-detail-link", rel = mediaUrl }) : MvcHtmlString.Create(title) %>
			</td>
			<td width="30%" style="white-space: nowrap">
				<%: artist %>
				<% if (!isPrint) {%>
				&nbsp;<a href="<%: artistUrl%>" title="More by this Artist"><img src="/public/images/icons/arrow.gif" alt="right-arrow"/></a>
				<%} %>
			</td>
			<td class="text-right">
				<%: item.Pop %>
			</td>
			<td class="text-right">
				<%: item.Country %>
			</td>
			<td class="text-right">
				<%: item.ReleaseYear %>
			</td>
		   
		</tr>
	
	<% } %>

	</table>