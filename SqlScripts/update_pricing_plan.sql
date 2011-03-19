declare @username varchar(250)
declare @pricingplanname varchar(50)

------------------------------------------------------------------
-- Set these values
------------------------------------------------------------------

-- User Name: user's registered email (must be already registered, not part of someone else's plan)
set @username = 'jstone@radarmusic.com'

-- Pricing Plan: Options - Basic, Plus, Business, Pro 
set @pricingplanname = 'Plus' 
------------------------------------------------------------------

	
declare @pricingplanid int
select @pricingplanid = pricingplanid from pricingplans where pricingplanname = @pricingplanname

declare @userid int
declare @planuserid int
declare @planbalanceid int

select 
@userid = userid, 
@planuserid = planuserid, 
@planbalanceid = planbalanceid 
from users 
where username = @username

print 'UserID: ' + isnull(cast(@userid as varchar), 'is invalid')

if (@userid is not null and @userid = @planuserid)
begin
	-- update user table
	update users
	set pricingplanid = @pricingplanid
	where userid = @userid	
	
	-- update plan balance with new pricing plan
	update planbalances
	set pricingplanid = @pricingplanid, lastupdatedon = GETDATE()
	where planbalanceid = @planbalanceid	
	
	-- update current subsciption with new pricing plan
	update subscriptions
	set pricingplanid = @pricingplanid
	where userid = @userid and subscriptionenddate is null
end

;
--select * from users;
--select * from planbalances;
--select * from pricingplans;
--select * from subscriptions;
