truncate table dbo.dim_Contents;
insert into dbo.dim_Contents
( ContentId, CreatedByUserId, CreatedOn, LastUpdatedByUserId, LastUpdatedOn, CatalogId, CatalogName, 
CatatalogCreatedByUserId, CatalogCreatedOn, IsControlledAllIn, Title, Artist, Writers, Pop, Country, ReleaseYear, RecordLabel)
select 
	-1 as ContentId
	, -1 as CreatedByUserId
	, '1/1/1900' as CreatedOn
	, -1 as LastUpdatedByUserId
	, '1/1/1900' as LastUpdatedOn
	, -1 as CatalogId
	, '(N/A)' as CatalogName
	, -1 as CatatalogCreatedByUserId
	, '1/1/1900' as CatatalogCreatedOn
	, 0 as IsControlledAllIn
	, '(N/A)' as Title
	, '(N/A)' as Artist
	, '(N/A)' as Writers
	, NULL as Pop
	, NULL as Country
	, NULL as ReleaseYear
	, '(N/A)' as RecordLabel	
;
	
insert into dbo.dim_Contents
select
	c.ContentId
	, isnull(c.CreatedByUserId, -1) as CreatedByUserId
	, c.CreatedOn
	, isnull(c.LastUpdatedByUserId, -1) as LastUpdatedByUserId
	, c.LastUpdatedOn 
	, isnull(c.CatalogId, -1) as CatalogId
	, isnull(cat.CatalogName, '(N/A)') as CatalogName
	, isnull(cat.CreatedByUserId, -1) as CatatalogCreatedByUserId
	, cat.CreatedOn as CatalogCreatedOn
	, c.IsControlledAllIn
	, isnull(c.Title, '(N/A)') as Title
	, isnull(c.Artist, '(N/A)') as Artist
	, isnull(c.Writers, '(N/A)') as Writers
	, c.Pop
	, c.Country
	, c.ReleaseYear
	, isnull(c.RecordLabel, '(N/A)') as RecordLabel
from
	SongSearchBI_ODS.dbo.Contents c inner join 
	SongSearchBI_ODS.dbo.Catalogs cat
		on c.CatalogId = cat.CatalogId
	;

truncate table dbo.dim_Users;
insert into dbo.dim_Users
select
	- 1 as UserId
	, '(N/A)' as UserName
	, '' as FirstName
	, '' as LastName
	, -1 as ParentUserId
	, -1 as PlanUserId
	, -1 as RoleId
	, '(N/A)' as RoleName
	, -1 as SiteProfileId
	, '(N/A)' as ProfileName
	, -1 as PricingPlanId
	, '(N/A)' as PricingPlanName
	, '1/1/1900' as RegisteredOn
	;
	
insert into dbo.dim_Users
select
	u.UserId
	, ISNULL(u.UserName, '(N/A)') as UserName
	, ISNULL(u.FirstName, '') as FirstName
	, ISNULL(u.LastName, '') as LastName
	, ISNULL(u.ParentUserId, -1) as ParentUserId
	, ISNULL(u.PlanUserId, -1) as PlanUserId
	, ISNULL(u.RoleId, -1) as RoleId
	, ISNULL(r.RoleName, '(N/A)') as RoleName
	, ISNULL(u.SiteProfileId, -1) as SiteProfileId
	, ISNULL(sp.ProfileName, '(N/A)') as ProfileName
	, ISNULL(u.PricingPlanId, -1) as PricingPlanId
	, ISNULL(pp.PricingPlanName, '(N/A)') as PricingPlanName
	, ISNULL(u.RegisteredOn, '1/1/1900') as RegisteredOn
from
	SongSearchBI_ODS.dbo.Users u inner join 
	SongSearchBI_ODS.dbo.Roles r 
		on u.RoleId = r.RoleId
	inner join SongSearchBI_ODS.dbo.PricingPlans pp
		on u.PricingPlanId = pp.PricingPlanId
	inner join SongSearchBI_ODS.dbo.SiteProfiles sp
		on u.SiteProfileId = sp.ProfileId
	;