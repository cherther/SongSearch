<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagCloudViewModel>" %>
<%
    var tags = Model.Tags;
    var tagTypeId = (int)Model.TagType;
    var hasMoreLinks = Model.InitialTagNumber > 0 && tags.Count > Model.InitialTagNumber;
%>
<%
    var tagCount = 0;
    foreach (var tag in tags)//.Take(initialTagNumber))
    {
        if (hasMoreLinks && tagCount == Model.InitialTagNumber)
        { %>
            <div><a class="cw-small cw-tags-more-link" href="#">more <%: Model.TagTypeName.ToString().ToLower()%> choices...</a>
            <div class="cw-more-tags cw-optional">
          <%
        }
        
        var tagName = tag.TagName;
        var tagId = String.Format("{0}-{1}", Model.TagIdTemplate, tag.TagId);

        var tagClass = String.Concat(Model.TagClass, Model.SelectedTags != null && Model.SelectedTags.Contains(tag.TagId) ? " cw-blue" : "");
        
     %>
     <a id="<%= tagId%>" class="cw-search-tag cw-tag-box cw-button cw-simple cw-small <%=tagClass %>"> <%=tagName%></a>    
    <%
        tagCount++;
    } %>
    <%if (hasMoreLinks)
      { %>
        </div></div>
    <%}%>    
