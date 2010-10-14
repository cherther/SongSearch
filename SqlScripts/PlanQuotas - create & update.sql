truncate table dbo.PlanQuotas;
insert into dbo.PlanQuotas
(PricingPlanId, LastUpdatedOn, LastUpdatedByUserId, NumberOfSongs, NumberOfInvitedUsers, NumberOfCatalogAdmins)
values (6,GETDATE(), 2, 0,0,0);

insert into dbo.PlanQuotas
(PricingPlanId, LastUpdatedOn, LastUpdatedByUserId, NumberOfSongs, NumberOfInvitedUsers, NumberOfCatalogAdmins)
select 
u.PricingPlanId, u.RegisteredOn, u.UserId, 0, 0, 0
from dbo.Users u
where u.PricingPlanId > 0 and u.PricingPlanId <> 6
order by u.UserId
;

update dbo.Users
set PlanQuotaId = p.PlanQuotaId --select *
from dbo.PlanQuotas p inner join dbo.Users u on p.LastUpdatedByUserId = u.UserId
where u.PricingPlanId <> 6
;
update dbo.Users
set PlanQuotaId = p.PlanQuotaId --select *
from dbo.PlanQuotas p inner join dbo.Users u on p.LastUpdatedByUserId = u.ParentUserId
where u.PricingPlanId = 0
;
update dbo.Users
set PlanQuotaId = p.PlanQuotaId --select *
from dbo.PlanQuotas p inner join dbo.Users u on p.PricingPlanId = u.PricingPlanId
where u.PricingPlanId = 6

;

--drop table #planusers
;
select 
u.userid, u.PlanQuotaId
into #planusers
from dbo.Users u
where u.PlanUserId = u.UserId and u.PlanQuotaId > 1
order by u.UserId;
;
update dbo.PlanQuotas
set 
--select 	
	NumberOfInvitedUsers = u.NumberOfInvitedUsers
from dbo.PlanQuotas pq, (
select COUNT(*) as NumberOfInvitedUsers
From dbo.Users) u
where pq.PlanQuotaId = 1;

update dbo.PlanQuotas
set 
--select 	
	NumberOfInvitedUsers = pu.NumberOfInvitedUsers
from dbo.PlanQuotas pq 
inner join
(select pu.PlanQuotaId, COUNT(*) as NumberOfInvitedUsers from #planusers pu inner join dbo.Users u
on pu.UserId = u.ParentUserId or pu.UserId = u.UserId
group by pu.PlanQuotaId
) pu on pu.PlanQuotaId = pq.PlanQuotaId
;
drop table #planusers
;
update dbo.PlanQuotas
set 
--select 	
	NumberOfCatalogAdmins = u.NumberOfCatalogAdmins
from dbo.PlanQuotas pq, (
select COUNT(*) as NumberOfCatalogAdmins
From dbo.Users where RoleId < 4) u
where pq.PlanQuotaId = 1;

update dbo.PlanQuotas
set 
--select 	
	NumberOfCatalogAdmins = pu.NumberOfCatalogAdmins
from dbo.PlanQuotas pq 
inner join
(select pu.PlanQuotaId, COUNT(*) as NumberOfCatalogAdmins from dbo.Users pu
where pu.RoleId < 4
group by pu.PlanQuotaId
) pu on pu.PlanQuotaId = pq.PlanQuotaId
;

update dbo.PlanQuotas
set 
--select 	
	NumberOfSongs = pc.NumberOfSongs
from dbo.PlanQuotas pq 
inner join
(
	select
	u.PlanQuotaId,
	COUNT(DISTINCT c.ContentId) as NumberOfSongs
	from
	dbo.Users u 
	inner join dbo.UserCatalogRoles uc on u.UserId = uc.UserId
	inner join dbo.Contents c on uc.CatalogId = c.CatalogId
	where 
	uc.RoleId <= 2
	group by u.PlanQuotaId
) pc on pq.PlanQuotaId = pc.PlanQuotaId