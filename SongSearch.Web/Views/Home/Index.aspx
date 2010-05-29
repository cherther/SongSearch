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
		<div>5/29/2010</div>
        <ul class="cw-vertical">
            <li>Media Player Testing: Flash based (Soundmanager 2)</li>
            <li>Song Detail starts Full Version in media player when not in cart page</li>
			<li>Volume, Timer and Song Progress not yet implemented</li>
        </ul>
		<div>&nbsp;</div>
        <div>5/28/2010</div>
        <ul class="cw-vertical">
            <li><%: Html.ActionLink("User Management", "Index", "UserManagement") %> feature complete<br /> 
			(Delete User & Take Ownership features are working, but for testing reasons are intentionally not actually deleting users or changing ownership)</li>
            <li>Added search breadcrumb, showing what user searched for</li>
            <li>Added error notification bar to top of page</li>
			<li>Added user notification bar to top of page, auto fades after 2 secs <br />
				Shows if items are in cart (when logging in)<br />
				Shows if carts are zipped by not downloaded <br />
			</li>
        </ul>
        <div>&nbsp;</div>
		<div>5/25/2010</div>
        <ul class="cw-vertical">
            <li>Downloads now working</li>
            <li>'Add to Cart' working</li>
            <li><%: Html.ActionLink("Cart", "Index", "Cart") %> zipping, downloading, archiving etc all working</li>
        </ul>
        <div>&nbsp;</div>
        <div>5/21/2010</div>
        <ul class="cw-vertical">
            <li>Caching added</li>
            <li>Autocomplete on all text search fields</li>
            <li>All search fields now working</li>
            <li>Additional tag choices are no hidable/showable</li>
        </ul>
        <div>&nbsp;</div>
		<div>5/18/2010</div>
        <ul class="cw-vertical">
            <li>Account management implemented (Change Password etc)</li>
        </ul>
        <div>&nbsp;</div>
    </div>
</div>
</asp:Content>
