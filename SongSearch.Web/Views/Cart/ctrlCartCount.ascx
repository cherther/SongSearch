<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ViewModel>" %>
<%var cartText = String.Concat("Song Cart", Model.MyActiveCartCount > 0 ? String.Format(" ({0})", Model.MyActiveCartCount) : ""); %>
<%= Html.ActionLink(cartText, "Index", "Cart")%>