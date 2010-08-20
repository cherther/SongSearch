<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%: Html.TextBox("", ViewData.TemplateInfo.FormattedModelValue,
					 new { @class = "text-box single-line cw-autocomplete", alt = ViewData.ModelMetadata.PropertyName })%>