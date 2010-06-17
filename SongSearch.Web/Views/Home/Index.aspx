<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded">
	<h2>Welcome</h2>
	<div>
		<div>&nbsp;</div>
		<h4>What's New</h4>
		<div>&nbsp;</div>
		<div>Latest Version: <% = SongSearch.Web.VersionInfo.BuildVersion()%></div>
		<div>&nbsp;</div>
		<div><strong>6/17/2010</strong></div>
		<ul class="cw-vertical cw-highlight">
			<li>Zipping carts now done offline</li>
			<ul class="cw-vertical">
				<li>Carts containing 10 or more songs are now being zipped in the backgound, so the page is not blocked while waiting</li>
				<li>When the zipping is complete, the user sees a message on the search and cart pages, also the cart page highlights the last completed zipped cart</li>
			</ul>
		</ul>
		<div>&nbsp;</div>
		<div>6/15/2010</div>
		<ul class="cw-vertical">
			<li>Instruments & SoundsLike now use textsearch</li>
			<ul class="cw-vertical">
				<li>Separate terms by comma, e.g. "Foo Fighters, Sum 41"</li>
			</ul>
		</ul>
		<div>&nbsp;</div>
		<div>6/14/2010</div>
		<ul class="cw-vertical">
			<li>Add to Cart option in search results via checkboxes</li>
			<li>Remove from Cart option in cart page via checkboxes</li>
			<li>Carts can have a max of 100 items ( = 1 search results page)</li>
			<li>User Management now simplies System access</li>
			<ul class="cw-vertical">
					<li>You can allow users to add new catalogs and users via a checkbox</li>
					<li>If the user is a SuperAdmin, or already has Admin access to at least one catalog this option is disabled, and the user can add new catalogs and users</li>
			</ul>
			<li>Search results are printable (by page)</li>
			
		</ul>
		<div>&nbsp;</div>
		<hr />
		<div>&nbsp;</div>
		<div>6/8/2010</div>
		<ul class="cw-vertical">
			<li>Catalog Management now working, catalog deletion disabled to avoid testing 'accidents'</li>
			<li>Mediaplayer now has Repeat button, repeats last played version</li>
			<li>Songs now searchable by Catalog name</li>
			<li>
			Song metadata uploaded, needs cleaning
			</li>
		</ul>
		<div>&nbsp;</div>
		<div></div>6/4/2010</div>
		<ul class="cw-vertical">
			<li>Song Edit now fully functional, no mp3 upload yet</li>
			<li>User Deletion and Take Ownership now enabled (was off for testing purposes)</li>
		</ul>
		<div>&nbsp;</div>
		<hr />
		<div>&nbsp;</div>
		<div>5/31/2010</div>
		<ul class="cw-vertical">
			<li>Lyrics search words highlighted in song detail</li>
			<li>Notes search words highlighted in song detail</li>
			<li>Playing another song now cancels any pending media player loads (cuts down bandwith use)</li>
			</ul>
		 <div>&nbsp;</div>
		<div>5/30/2010</div>
		<ul class="cw-vertical">
			<li>Volume control now working!</li>
			<ul class="cw-vertical">
					<li>Default is 60% of full volume</li>
					<li>No mute but can be dragged to 0%</li>
					<li>Remembers volume setting from song to song on the same page (i.e. as long as page is not refreshed, sorted etc)</li>
			</ul>
		</ul>
		<div>&nbsp;</div>
		<div>5/29/2010</div>
		<ul class="cw-vertical">
			<li>Media Player Beta:</li>
				<ul class="cw-vertical">
					<li>Pause/Start controls</li>
					<li>Shows current & total time and position</li>
					<li>Clicking on song in search results auto-starts full version</li>
					<li>Detects missing or blocked Flash plug-in and shows error message</li>
					<li>Volume control not yet implemented</li>
				</ul>
		</ul>
		<div>&nbsp;</div>
		<hr />
		<div>&nbsp;</div>
		<div>5/28/2010</div>
		<ul class="cw-vertical">
			<li><%: Html.ActionLink("User Management", MVC.UserManagement.Views.Index)%> feature complete<br /> 
			(Delete User & Take Ownership features are working, but for testing reasons are intentionally not actually deleting users or changing ownership)</li>
			<li>Added search breadcrumb, showing what user searched for</li>
			<li>Added error notification bar to top of page</li>
			<li>Added user notification bar to top of page, auto fades after 2 secs <br />
				Shows if items are in cart (when logging in)<br />
				Shows if carts are zipped by not downloaded <br />
			</li>
		</ul>
		<div>&nbsp;</div>
		<hr />
		<div>&nbsp;</div>
		<div>5/25/2010</div>
		<ul class="cw-vertical">
			<li>Downloads now working</li>
			<li>'Add to Cart' working</li>
			<li><%: Html.ActionLink("Cart", MVC.Cart.Views.Index)%> zipping, downloading, archiving etc all working</li>
		</ul>
		<div>&nbsp;</div>
		<hr />
		<div>&nbsp;</div>
		<div>5/21/2010</div>
		<ul class="cw-vertical">
			<li>Caching added</li>
			<li>Autocomplete on all text search fields</li>
			<li>All search fields now working</li>
			<li>Additional tag choices are no hidable/showable</li>
		</ul>
		<div>&nbsp;</div>
		<hr />
		<div>&nbsp;</div>
		<div>5/18/2010</div>
		<ul class="cw-vertical">
			<li>Account management implemented (Change Password etc)</li>
		</ul>
		<div>&nbsp;</div>
	</div>
</div>
</asp:Content>
