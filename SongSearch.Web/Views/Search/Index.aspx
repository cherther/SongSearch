<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.SearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-fill-lite cw-rounded-corners-bottom">
	
	<div class="nine_column section">
	
		<div class="two column" style="min-width:220px!important;">
			<!--div id="cw-search-options-panel"-->
			<% Html.RenderPartial(MVC.Search.Views.ctrlSearchOptions); %>
			<!--/div-->
		</div>
		<div class="seven column">
			<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners cw-buffered-left" style="min-height:480px">
				<h2>Search Tips</h2>
				<div>&nbsp;</div>
				<h3>Enter Song Search criteria</h3>
					<ul class="cw-bullet">
						<li>
						To get a broader set of song search results, enter fewer (or no) search criteria and click <strong>Search</strong>.
						</li>
						<li>
						To narrow down the search (and get fewer, yet more refined results) simply enter the additional search criteria in the appropriate search fields and click <strong>Search</strong> again.   
						</li>
						<li>
						To clear all of the search criteria and start a new search, click on <strong>Clear</strong> next to the <strong>Search</strong> button.
						</li>
					</ul>
				<div>&nbsp;</div>
				<h3>
				Preview songs, get song info & download
				</h3>
				<ul class="cw-bullet">
					<li>
						To preview a full length song, click on the song <strong>title</strong> in the list of songs search results. 
					</li>
					<li>
						Click on one of the song tabs (e.g., Lyrics, Tags, etc.) to view additional song information, 
					</li>
					<li>Click the <strong>Download</strong> button to download the song as an mp3 file.
					</li>
					<li> 
						To add multiple songs to a Zip file for later use (e.g. to post on an FTP site), 
						click the <strong>Add To Cart</strong> button to add any song to your <strong>Song Cart</strong>.  
					</li>
					<li>
						In the <%: Html.ActionLink("Song Cart", MVC.Cart.Index()) %> section, you can zip up the contents of your Song Cart, with options to title the Zip file and rename individual songs.
						You can download the Zip file to your computer right away, or we'll hold on to it for you to download for 7 days.
					</li>
				</ul>
				<div>&nbsp;</div>
				<h3>
				Contact us to secure licensing 
				</h3>
				<ul class="cw-bullet">
					<li>
					The <%: Html.ActionLink("Contact Us", MVC.Home.Contact()) %> section of the site shows the licensing contact information for your account.  Call or email the contact entity for licensing information directly.
					</li>
				</ul>
			</div>
		</div>
	
	</div>

</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
</asp:Content>
