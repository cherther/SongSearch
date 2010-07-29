<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.Data.UserQuotas>" %>
<div class="cw-outl-orange cw-padded cw-rounded-corners">
<table id="cw-user-quotas">
<%
	var perc = Model.NumberOfSongs.Allowed > 0 ?
	  String.Format("{0}%", ((decimal)Model.NumberOfSongs.Used / (decimal)Model.NumberOfSongs.Allowed) * 100) :
	  "0%";
	
	var title = Model.NumberOfSongs.Allowed > 0 ? 
		String.Format("{0} of {1} Songs ({2})", Model.NumberOfSongs.Used, Model.NumberOfSongs.Allowed, perc) :
		"Unlimited Songs";
%>
<tr>
	<td width="20%">
	Songs
	</td>
	<td>
	<div title="<%: title %>" class="cw-usage-box">
		<span class="cw-usage-bar" style="width:<%: perc %>"><%: perc %></span> 
	</div>
	</td>
</tr>
<%
	perc = Model.NumberOfInvitedUsers.Allowed > 0 ?
	  String.Format("{0}%", ((decimal)Model.NumberOfInvitedUsers.Used / (decimal)Model.NumberOfInvitedUsers.Allowed) * 100):
	  "0%";
	title = Model.NumberOfInvitedUsers.Allowed > 0 ?
	  String.Format("{0} of {1} Users ({2})", Model.NumberOfInvitedUsers.Used, Model.NumberOfInvitedUsers.Allowed, perc) :
	  "Unlimited Users";
%>
<tr>
	<td>
	Users
	</td>
	<td>
	<div title="<%: title %>" class="cw-usage-box">
		<span class="cw-usage-bar" style="width:<%: perc %>"><%: perc %></span> 
	</div>
	</td>
</tr>
<%perc = Model.NumberOfCatalogAdmins.Allowed > 0 ? 
	  String.Format("{0}%", ((decimal)Model.NumberOfCatalogAdmins.Used / (decimal)Model.NumberOfCatalogAdmins.Allowed) * 100) :
	  "0%";
  title = Model.NumberOfCatalogAdmins.Allowed > 0 ?
	String.Format("{0} of {1} Catalog Admins ({2})", Model.NumberOfCatalogAdmins.Used, Model.NumberOfCatalogAdmins.Allowed, perc) :
	"Unlimited Users";	
%>
<tr>
	<td>
	Catalog Admins
	</td>
	<td>
	<div title="<%: title %>" class="cw-usage-box">
		<span class="cw-usage-bar" style="width:<%: perc %>"><%: perc %></span> 
	</div>
	</td>
</tr>
</table>
</div>