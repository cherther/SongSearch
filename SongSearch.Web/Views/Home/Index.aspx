<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-fill-lite cw-padded cw-rounded-corners-bottom">
	<h2>Welcome</h2>
	<div>&nbsp;</div>
<%--	<div class="nine_column section">
		<div class="six column">
--%>
		<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
			<h3>What's New?</h3>
			<div>&nbsp;</div>
			<div>Welcome to <%: Html.SiteProfile().CompanyName %>. 
			We have recently launched a private beta version of the site. If you'd like to participate, please <%: Html.ActionLink("contact us", MVC.Home.Contact()) %> for an invitation.
			</div>
			
		</div>
<%--		<div>&nbsp;</div>--%>
			<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
				<h3>Artists on <%: Html.SiteProfile().CompanyName %></h3>
				<div style="line-height: 1em">Aerosmith, Frank Sinatra, Snoop Dogg, Sarah Vaughan, Lil Wayne, Jimi Hendrix, Ray Charles, Norah Jones, Charlie Parker, Donovan, Christina Aguilera, Bing Crosby, Ziggy Marley, Quincy Jones, Busta Rhyme and many others.
				</div>			
			</div>
		<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
			<h3>Who Uses <%: Html.SiteProfile().CompanyName %>?</h3>
				<div>&nbsp;</div>
				<div id="who-uses-tabs">
					<ul>
						<li><a href="#tab-1">Music Supervisors</a></li>
						<li><a href="#tab-2">Artists & Songwriters</a></li>
						<li><a href="#tab-3">Record Labels & Publishers</a></li>
					</ul>			
					<div id="tab-1">
						You will find thousands of hand-picked classic and contemporary songs in all styles, years and moods available for easy preview, download and licensing utilizing this state-of-the-art online song repository.
					</div>
					<div id="tab-2">
					If invited into the network you can securely upload, self-manage and give permissions to whomever you want to preview, download and license your music. Your songs will gain exposure to many of the leading executives responsible for placing songs in film, television, advertising, games and new media. <%: Html.SiteProfile().CompanyName %> will not ask for or retain any of your rights.
					</div>
					<div id="tab-3">
					<%: Html.SiteProfile().CompanyName %> provides music catalogs with an affordable, self-branded solution for creating an instant Web presence.  You can upload, self-manage and give permissions to clients around the world with this intuitive, secure and completely scalable system.  Benefit from the latest features in search functionality by subscribing to <%: Html.SiteProfile().CompanyName %> now (<%: Html.ActionLink("Contact us for an invitation code", MVC.Home.Contact()) %>)
					</div>
				</div>
			</div>

		<%--</div>

		<div class="one column">&nbsp;</div>
		--%>
<%--		<div class="two column" style="text-align: left">
--%>
<%--		</div>
	</div>--%>
<%--	<div>&nbsp;</div>
--%>
	<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
		<h3>Which Companies Have Licensed Music from <%: Html.SiteProfile().CompanyName %>?</h3>
		<div>&nbsp;</div>
		<div class="cw-outl-thick cw-fill-lite cw-padded cw-rounded-corners">
		<%
			var imgFolder = new System.IO.DirectoryInfo(Server.MapPath("~/public/images/logos/"));
			var images = imgFolder.GetFiles();
		%>
		<%
		for(int i=0; i < images.Length; i++) {					
			var imgUrl = String.Concat("/public/images/logos/", images[i].Name);					
		%>
			<%if (i == 0 || i % 6 == 0){ %>
			<div class="six_column section">
			<%} %>
				<div class="one column cw-outl-thick cw-fill-white cw-rounded-corners" style="margin:1px;text-align: center; vertical-align: middle">
					<img src="<%: imgUrl %>" width="70" alt="logo" />
				</div>
			<%if ((i+1) % 6 == 0){ %>
			</div>			
			<%} %>
		<%} %>
		</div>			
	</div>
	<div>&nbsp;</div>

	<div>&nbsp;</div>
	<div class="cw-outl-thick cw-fill cw-padded cw-rounded-corners">
	Latest Version: <% = SongSearch.Web.VersionInfo.BuildVersion()%> - <% = SongSearch.Web.VersionInfo.BuildTime()%>
	</div>

</div>		
<script type="text/javascript">
	$(function () {
		$('#who-uses-tabs').tabs();
	});
</script>
</asp:Content>
