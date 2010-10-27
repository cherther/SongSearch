<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ViewModel>" %>
<style>
#shadow-container {
	position: relative;
	left: 3px;
	top: 3px;
	margin-right: 3px;
	margin-bottom: 3px;
	
}
 
#shadow-container .shadow2,
#shadow-container .shadow3,
#shadow-container .container {
	position: relative;
	left: -1px;
	top: -1px;
}
 
	#shadow-container .shadow1 {
		background: #F1F0F1;
	}
 
	#shadow-container .shadow2 {
		background: #DBDADB;
	}
 
	#shadow-container .shadow3 {
		background: #B8B6B8;
	}
 
	#shadow-container .container {
		background: #ffffff;
		border: 1px solid #848284;
		padding: 5px;
	}
	</style>
	<% if (Html.SiteProfile().HasProfileLogo) {%>
	<div style="text-align: center; vertical-align:bottom;">
		<div id="shadow-container">
			<div class="shadow1 cw-rounded-corners">
				<div class="shadow2 cw-rounded-corners">
					<div class="shadow3 cw-rounded-corners">
						<div class="container cw-rounded-corners">
			<a href="/" class="cw-logo">
			<img src="<%: Html.SiteProfile().SiteProfileLogoUrl() %>" height="70" 
			title="<%: Html.SiteProfile().CompanyName %>" alt="<%: Html.SiteProfile().CompanyName %>" 
			style="padding: 0px;"/>
			<div style="font-style: normal; font-size: 0.8em;"><%: Html.SiteProfileTagLine() %></div>
			</a>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<%} else { %>
	<h1 style="display:inline">
		<a href="/" class="cw-logo">
			<%: Html.SiteProfile().CompanyName%>
			</a>
	</h1>
	<%} %>
	</a>
<% if (!Html.SiteProfile().HasProfileLogo) {%>
<span style="padding-left: 5px; display:inline; font-weight: bold;color:Gray" class="text-medium cw-small-cap">Beta</span>
	<h3 class="cw-logo-sub">Your professional music licensing resource</h3>
<%}%>
