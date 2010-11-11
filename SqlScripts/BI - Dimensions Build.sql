
truncate table dbo.fct_LogEvents;

insert into dbo.fct_LogEvents
select
	ActionEventId
	, ActionEventDate
	, ISNULL(SessionId, -1) As SessionId
	, ISNULL(UserId, -1) As UserId
	, ISNULL(ActionId, -1) As ActionId
	, ISNULL(ContentId, -1) As ContentId
	, ISNULL(QueryString, '(N/A)') As QueryString
	, ISNULL(ResultCount, 0) As ResultCount
	, LogEventCnt As LogEvents
from dbo.vwLogEvents e
;

truncate table dbo.dim_Contents;

insert into dbo.dim_Contents
select
	c.ContentId
	, c.CreatedByUserId
	, c.CreatedOn
	, c.LastUpdatedByUserId
	, c.LastUpdatedOn 
	, c.CatalogId
	, cat.CatalogName
	, cat.CreatedByUserId As CatatalogCreatedByUserId
	, cat.CreatedOn As CatalogCreatedOn
	, c.IsControlledAllIn
	, c.Title
	, c.Artist
	, c.Writers
	, c.Pop
	, c.Country
	, c.ReleaseYear
	, c.RecordLabel
from
	dbo.Contents c inner join dbo.Catalogs cat
	on c.CatalogId = cat.CatalogId
	;

truncate table dbo.dim_Users;
	
insert into dbo.dim_Users
select
	u.UserId
	, u.UserName
	, u.FirstName
	, u.LastName
	, u.ParentUserId
	, u.PlanUserId
	, u.RoleId
	, r.RoleName
	, u.SiteProfileId
	, sp.ProfileName
	, u.PricingPlanId
	, pp.PricingPlanName
	, u.RegisteredOn
from
	dbo.Users u inner join dbo.Roles r 
	on u.RoleId = r.RoleId
	inner join dbo.PricingPlans pp
	on u.PricingPlanId = pp.PricingPlanId
	inner join dbo.SiteProfiles sp
	on u.SiteProfileId = sp.ProfileId
	;