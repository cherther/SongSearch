declare @profileName varchar(50)
declare @profileCompany varchar(50)
declare @owner varchar(255)

set @profileName = 'WorldsEnd'
set @profileCompany = 'Worlds End'
set @owner = 'colin@worldsend.com'

declare @createProfile bit = 0;
declare @createProfileContact bit = 1;

if (@createProfile = 1)
begin
	insert into SongSearch2.dbo.SiteProfiles
		   (ProfileName
		   ,CompanyName
		   ,ContactAddress
		   ,ContactAddress2
		   ,ContactCity
		   ,ContactStateRegion
		   ,ContactPostalCode
		   ,ContactCountry
		   ,ContactPhone
		   ,ContactFax
		   ,ContactEmail
		   ,AdminEmail
		   ,CreatedOn
		   ,CreatedByUserId
		   ,HasProfileLogo)
	values
		(@profileName,
		@profileCompany, '', '', 'Los Angeles', 'CA', '', 'USA'
		, '(323) 555-5555', null, @owner, @owner, GETDATE(), 1, 1)
end
declare @ownerId int;
declare @planBalanceId int;

select @ownerId = u.UserId, @planBalanceId = u.PlanBalanceId
from Users u where u.UserName = @owner

declare @profileId int
select @profileId = ProfileId from SiteProfiles where ProfileName = @profileName

if (@createProfileContact = 1)
begin
	declare @contactId int

	insert into dbo.Contacts
	(
		ContactName, CompanyName, Address1, Address2, City, StateRegion, PostalCode, Country, Phone1, Phone2, Fax, Email, AdminEmail, IsDefault, CreatedOn, CreatedByUserId, ContactTypeId
	)
	values
	(
		@profileCompany, @profileCompany, '','','','','','','','','',@owner, @owner, 1, GETDATE(), @ownerId, 1	
	)

	select @contactId = ContactId from Contacts where ContactName = @profileCompany
	
	insert into SongSearch2.dbo.SiteProfilesContacts
		(SiteProfileId, ContactId)
	values
		(@profileId, @contactId)
end


update Users
set SiteProfileId = @profileId
, PricingPlanId = 5
where UserId = @ownerId

update Subscriptions
set PricingPlanId = 5
where UserId = @ownerId

update PlanBalances
set PricingPlanId = 5
where PlanBalanceId = @planBalanceId 