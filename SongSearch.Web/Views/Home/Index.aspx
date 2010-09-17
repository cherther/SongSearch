<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-fill-lite cw-padded cw-rounded-corners-bottom">
	<h2>Welcome</h2>
	<div>
		<div>&nbsp;</div>
		<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
			<h3>What's New?</h3>
			<div>&nbsp;</div>
			<div>Welcome to <%: Model.SiteProfile.CompanyName %>.</div>
			<div>&nbsp;</div>
			<div>
			We have recently launched a private beta test of the site. If you'd like to participate, please <%: Html.ActionLink("contact us", MVC.Home.Contact()) %> for an invitation code.
			</div>
			<div>&nbsp;</div>

		</div>
		<%--<div>&nbsp;</div>
		<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
			<h3>Why Have Your Music on <%: Model.SiteProfile.CompanyName %>?</h3>
			<div>&nbsp;</div>
			<div>
			Your music will be considered for film & television use in major productions throughout the world.
			</div>
		</div>--%>
		<div>&nbsp;</div>
		<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
			<h3>Who Uses <%: Model.SiteProfile.CompanyName %>?</h3>
			<div>&nbsp;</div>
			<ul class="cw-bullet">
				<li>
				Music Supervisors, Editors, Directors, Producers, Ad Agencies, Game developers, New Media developers, music libraries, song placement consultants, film & television studios and anyone that needs a library system for their music.
				</li>
				<li>
				A large network of domestic and international film & television music placement professionals in the USA, England, Germany, Canada, Japan and France.
				</li>
				<li>
				YOU the recording artist/songwriter/producer that needs an advanced on-line “multi-criteria” search system to serve up your music to potential licensors.
				</li>
			</ul>
		</div>
		<div>&nbsp;</div>
		<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
			<h3>Which Companies Have Licensed Music from <%: Model.SiteProfile.CompanyName %>?</h3>
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
		<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
			<h3>Artists on <%: Model.SiteProfile.CompanyName %></h3>
			<div>&nbsp;</div>
			<div>Aerosmith, Frank Sinatra, Snoop Dogg, Sarah Vaughan, Lil Wayne, Jimi Hendrix, Ray Charles, Norah Jones, Charlie Parker, Donovan, Christina Aguilera, Bing Crosby, Ziggy Marley, Quincy Jones, Busta Rhyme and many others.
			</div>			
		</div>

		<div>&nbsp;</div>
		<div class="cw-outl-thick cw-fill cw-padded cw-rounded-corners">
		Latest Version: <% = SongSearch.Web.VersionInfo.BuildVersion()%> - <% = SongSearch.Web.VersionInfo.BuildTime()%>
		</div>

	</div>		
</div>
<script type="text/javascript">
	$(function () {
		//$("#logocarousel").carousel({ dispItems: 3 });
	});
</script>
</asp:Content>
