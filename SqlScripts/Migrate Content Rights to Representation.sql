set identity_insert dbo.ContentRepresentation on

insert into dbo.ContentRepresentation
(ContentRepresentationId, ContentId, RightsTypeId, RepresentationShare, CreatedByUserId, CreatedOn)
select 
	ContentRightId ContentRepresentationId
	, ContentId
	, RightsTypeId
	, RightsHolderShare RepresentationShare
	, CreatedByUserId
	, CreatedOn
from dbo.ContentRights cr

set identity_insert dbo.ContentRepresentation off

insert into dbo.ContentRepresentationTerritories
(ContentRepresentationId, TerritoryId)
select 
	ContentRightId ContentRepresentationId
	, TerritoryId
from dbo.ContentRightTerritories cr
