<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-fill-lite cw-padded cw-rounded-corners-bottom">
	<h2>Welcome</h2>
	<div>
		<div>&nbsp;</div>
		<div class="cw-outl cw-fill-white cw-padded cw-rounded-corners">
			<h3>What's New?</h3>
			<div>&nbsp;</div>
			<div>Welcome to <%: Model.SiteProfile.CompanyName %>.</div>
			<div>&nbsp;</div>
			<div>
			We have recently launched a private beta test of the site. If you'd like to participate, please <%: Html.ActionLink("contact us", MVC.Home.Contact()) %> for an invitation code.
			</div>
			<div>&nbsp;</div>

		</div>
		<div>&nbsp;</div>
		<div class="cw-outl cw-fill-white cw-padded cw-rounded-corners">
			<h3>Why Have Your Music on <%: Model.SiteProfile.CompanyName %>?</h3>
			<div>&nbsp;</div>
			<div>Your music will be previewed and considered for productions by many of the most notable companies and individuals responsible for placing songs in film & television throughout the world..
			</div>
		</div>
		<div>&nbsp;</div>
		<div class="cw-outl cw-fill-white cw-padded cw-rounded-corners">
			<h3>Who Uses <%: Model.SiteProfile.CompanyName %>?</h3>
			<div>&nbsp;</div>
			<ol>
				<li>
				Music Supervisors, producers, editors, game/new media developers and the film & television studios.
				</li>
				<li>
				A large network of domestic and international film & television music placement professionals in the USA, England, Germany, Canada, Japan and France.
				</li>
				<li>
				YOU the recording artist/songwriter/producer that needs an advanced on-line “multi-criteria” search system to serve up your music to potential licensors.
				</li>
			</ol>
		</div>
		<div>&nbsp;</div>
		<div class="cw-outl cw-fill-white cw-padded cw-rounded-corners">
			<h3>Which Companies Have Licensed Music from <%: Model.SiteProfile.CompanyName %>?</h3>
			<div>&nbsp;</div>
			<div>Your music will be previewed and considered for productions by many of the most notable companies and individuals responsible for placing songs in film & television throughout the world..
			</div>
		</div>

		<div>&nbsp;</div>
		<div class="cw-outl cw-fill cw-padded cw-rounded-corners">
		Latest Version: <% = SongSearch.Web.VersionInfo.BuildVersion()%> - <% = SongSearch.Web.VersionInfo.BuildTime()%>
		</div>

	</div>		
</div>
</asp:Content>
