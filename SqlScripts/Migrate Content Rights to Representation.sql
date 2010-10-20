ALTER TABLE [dbo].[ContentRepresentationTerritories] 
DROP CONSTRAINT [FK_ContentRepresentationTerritories_ContentRepresentation]

truncate table dbo.ContentRepresentation
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

truncate table dbo.ContentRepresentationTerritories
insert into dbo.ContentRepresentationTerritories
(ContentRepresentationId, TerritoryId)
select 
	ContentRightId ContentRepresentationId
	, TerritoryId
from dbo.ContentRightTerritories cr

ALTER TABLE [dbo].[ContentRepresentationTerritories]  
WITH CHECK ADD  CONSTRAINT [FK_ContentRepresentationTerritories_ContentRepresentation] 
FOREIGN KEY([ContentRepresentationId])
REFERENCES [dbo].[ContentRepresentation] ([ContentRepresentationId])
ON DELETE CASCADE

ALTER TABLE [dbo].[ContentRepresentationTerritories] 
CHECK CONSTRAINT [FK_ContentRepresentationTerritories_ContentRepresentation]


