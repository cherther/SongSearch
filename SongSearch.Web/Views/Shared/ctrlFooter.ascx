<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ViewModel>" %>
<div class="six_column section">
		<div class="one column" style="font-size: 1em">&copy; <%:DateTime.Now.Year %>&nbsp;</div>
		<div class="five column">
			<span class="cw-small-cap"><%: Html.SiteProfile().CompanyName %></span>
			<div style="font-style: normal; font-size: 0.7em;"><%: Html.SiteProfileTagLine() %></div>        
	</div>
</div>
