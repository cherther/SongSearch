<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ViewModel>" %>
<span class="text-small cw-small-cap">
<%: Html.ActionLink("Terms of Use", MVC.Home.TermsOfUse()) %>&nbsp;&nbsp;&nbsp;
<%: Html.ActionLink("Privacy Policy", MVC.Home.PrivacyPolicy()) %>
</span>