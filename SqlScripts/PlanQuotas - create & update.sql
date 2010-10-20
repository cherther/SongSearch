ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_PlanBalances];

truncate table dbo.PlanBalances;

insert into dbo.PlanBalances
(PricingPlanId, LastUpdatedOn, LastUpdatedByUserId, NumberOfSongs, NumberOfInvitedUsers, NumberOfCatalogAdmins)
values (6,GETDATE(), 2, 0,0,0);

insert into dbo.PlanBalances
(PricingPlanId, LastUpdatedOn, LastUpdatedByUserId, NumberOfSongs, NumberOfInvitedUsers, NumberOfCatalogAdmins)
select 
u.PricingPlanId, --u.username, 
GETDATE() LastUpdatedOn, u.UserId LastUpdatedByUserId, 0 NumberOfSongs, 0 NumberOfInvitedUsers, 0 NumberOfCatalogAdmins
from dbo.Users u
where u.PricingPlanId > 0 and u.PricingPlanId <> 6
order by u.UserId
;

update dbo.Users
set PlanBalanceId = p.PlanBalanceId --select *
from dbo.PlanBalances p inner join dbo.Users u on p.LastUpdatedByUserId = u.UserId
where u.PricingPlanId <> 6
;
update dbo.Users
set PlanBalanceId = p.PlanBalanceId --select *
from dbo.PlanBalances p inner join dbo.Users u on p.LastUpdatedByUserId = u.ParentUserId
where u.PricingPlanId = 0
;
update dbo.Users
set PlanBalanceId = uo.PlanBalanceId --select *
from dbo.users ui inner join (select u.UserId, pu.UserId ParentUserId
	, pu.PlanBalanceId
	, pu.PricingPlanId 
	from dbo.Users u inner join dbo.Users pu on u.ParentUserId = pu.UserId
where pu.PricingPlanId = 0) uo on ui.UserId = uo.UserId
;
update dbo.Users
set PlanBalanceId = p.PlanBalanceId --select *
from dbo.PlanBalances p inner join dbo.Users u on p.PricingPlanId = u.PricingPlanId
where u.PricingPlanId = 6

;

--drop table #planusers
;
select 
u.userid, u.PlanBalanceId
into #planusers
from dbo.Users u
where u.PlanUserId = u.UserId and u.PlanBalanceId > 1
order by u.UserId;
;
update dbo.PlanBalances
set 
--select 	
	NumberOfInvitedUsers = u.NumberOfInvitedUsers
from dbo.PlanBalances pq, (
select COUNT(*) as NumberOfInvitedUsers
From dbo.Users) u
where pq.PlanBalanceId = 1;

update dbo.PlanBalances
set 
--select pu.PlanBalanceId,
	NumberOfInvitedUsers = pu.NumberOfInvitedUsers
from dbo.PlanBalances pq 
inner join
(select pu.PlanBalanceId, COUNT(*) as NumberOfInvitedUsers from #planusers pu inner join dbo.Users u
on pu.UserId = u.ParentUserId or pu.UserId = u.UserId
group by pu.PlanBalanceId
) pu on pu.PlanBalanceId = pq.PlanBalanceId
;
drop table #planusers
;
update dbo.PlanBalances
set 
--select 	
	NumberOfCatalogAdmins = u.NumberOfCatalogAdmins
from dbo.PlanBalances pq, (
select COUNT(*) as NumberOfCatalogAdmins
From dbo.Users where RoleId < 4) u
where pq.PlanBalanceId = 1;

update dbo.PlanBalances
set 
--select 	
	NumberOfCatalogAdmins = pu.NumberOfCatalogAdmins
from dbo.PlanBalances pq 
inner join
(select pu.PlanBalanceId, COUNT(*) as NumberOfCatalogAdmins from dbo.Users pu
where pu.RoleId < 4
group by pu.PlanBalanceId
) pu on pu.PlanBalanceId = pq.PlanBalanceId
;

update dbo.PlanBalances
set 
--select pq.PlanBalanceId,	
	NumberOfSongs = pc.NumberOfSongs
from dbo.PlanBalances pq,
(
	select
	COUNT(DISTINCT c.ContentId) as NumberOfSongs
	from
	dbo.Contents c 
) pc 
where pq.PlanBalanceId = 1;

update dbo.PlanBalances
set 
--select pq.PlanBalanceId,	
	NumberOfSongs = pc.NumberOfSongs
from dbo.PlanBalances pq 
inner join
(
	select
	u.PlanBalanceId,
	COUNT(DISTINCT c.ContentId) as NumberOfSongs
	from
	dbo.Users u 
	inner join dbo.Catalogs cat on u.UserId = cat.CreatedByUserId
	--inner join dbo.UserCatalogRoles uc on u.UserId = uc.UserId
	inner join dbo.Contents c on cat.CatalogId = c.CatalogId
	--where 
	--uc.RoleId <= 2
	group by u.PlanBalanceId
) pc on pq.PlanBalanceId = pc.PlanBalanceId
where pq.PlanBalanceId > 1;


ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_PlanBalances] FOREIGN KEY([PlanBalanceId])
REFERENCES [dbo].[PlanBalances] ([PlanBalanceId])
ON DELETE CASCADE
;
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_PlanBalances]
;
