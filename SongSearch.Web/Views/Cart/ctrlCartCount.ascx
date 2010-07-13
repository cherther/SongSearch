<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ViewModel>" %>
<%var cartText = String.Concat("Song Cart", Model.MyActiveCartCount > 0 ? String.Format(" ({0})", Model.MyActiveCartCount) : ""); %>
<%var cartTitle = String.Format("There {0} {1} {2} in your cart", "is".Pluralize(Model.MyActiveCartCount), Model.MyActiveCartCount, "song".Pluralize(Model.MyActiveCartCount)); %>
<%= Html.ActionLink(cartText, MVC.Cart.Index(), new { @class = "cw-rounded-corners-top", title = cartTitle })%>