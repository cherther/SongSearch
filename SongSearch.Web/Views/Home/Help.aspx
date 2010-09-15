<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Help
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="content" class="cw-outl cw-fill-lite cw-padded cw-rounded-corners-bottom">
	<h2><%: Model.PageTitle %></h2>
	<div>&nbsp;</div>
	<a href="#Feedback">Feedback</a> | <a href="#Requirements">Site Requirements</a> | <a href="#FAQ">FAQ</a>
	<div>&nbsp;</div>
	<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
		<h3><a name="Feedback">Feedback</a></h3>
		<div>&nbsp;</div>
		<p>
		We'd love your feedback! Please visit our <a href="#" onclick="UserVoice.Popin.show(uservoiceOptions); return false;">UserVoice forum</a> to let us know what you think.
		</p>
		<p>Or, you can <%: Html.ActionLink("contact us", MVC.Home.Contact()) %> directly with any questions and comments.</p>
		</p>
	</div>
	<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
		<h3><a name="Requirements">Site Requirements</a></h3>
		<div>&nbsp;</div>
		<div>
		<%: Model.SiteProfile.CompanyName %> is an interactive web application that takes advantage of a variety of standard and advanced plug-ins & browser features, such as:
		</div>
		<div>&nbsp;</div>
		<ul class="cw-bullet">
			<li>JavaScript - this enables some of the interactive features, such as playing song files on the page</li>
			<li>Cookies - they help us remember you when you come back</li>
			<li>Flash - we use this to play audio on the page</li>
			<li>Html5 - a new set of advanced web standards in newer browsers, we use it to upload song files and to make some of our forms work better</li>
		</ul>
		<div>&nbsp;</div>
		<div>Please make sure you have the <strong>latest version of your preferred browser</strong> installed (we like <a href="http://www.firefox.com" target="_blank">Firefox</a>, <a href="http://www.google.com/chrome" target="_blank">Chrome</a> and <a href="http://www.apple.com/safari" target="_blank">Safari</a>) and that you have <strong>not disabled or blocked JavaScript, Cookies, or Flash</strong> in your browser.
		<div>&nbsp;</div>
		<div>If you're not sure whether your browser meets these requirements, <a href="http://www.supportdetails.com" target="_blank">www.supportdetails.com</a> is a very cool site to check if you have JavaScript  enabled and that you have Flash installed.</div>
		<div>&nbsp;</div>
		Also, our music file downloads use the <strong>MP3 file format</strong> and are delivered in compressed <strong>Zip</strong> files, so you will need an MP3 compatible music player such as <a href="http://www.apple.com/iTunes" target="_blank">Apple's iTunes</a> and software to unzip Zip files, such as <a href="http://www.winzip.com/trialpay.htm" target="_blank">WinZip</a> (if you're using Windows.)
		</div>
	</div>
	<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
		<h3><a name="FAQ">FAQ (Frequently Asked Questions)</a></h3>
		<div>&nbsp;</div>
		<ul class="cw-bullet">
			<li><a href="#WhoUsesThisSite">Who uses this site?</a></li>
			<li><a href="#HowDoIGetAnInvitationCode">How do you get an invitation code?</a></li>
		</ul>
		<div>&nbsp;</div>
		<h5><a name="WhoUsesThisSite">Who uses this site?</a></h5>
		<p><%: Model.SiteProfile.CompanyName %> is used by a variety of professionals from the entertainment and advertising industries, such as song writers, music editors, music supervisors, record label executives, film and/or television producers and advertising executives.
		</p>
		<div>&nbsp;</div>
		<h5><a name="HowDoIGetAnInvitationCode">How do you get an invitation code?</a></h5>
		<p>Please <%: Html.ActionLink("contact us", MVC.Home.Contact()) %> for an invitation code.</p>
	</div>
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
</asp:Content>
