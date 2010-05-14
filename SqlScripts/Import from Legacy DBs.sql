delete dbo.Contents




set identity_insert dbo.contents on

insert into dbo.Contents
( ContentId, CatalogId, CreatedByUserId, CreatedOn, LastUpdatedByUserId, LastUpdatedOn, IsControlledAllIn, HasMediaPreviewVersion, HasMediaFullVersion, Title, Artist, Writers, PopCharts, CountryCharts, ReleaseYear, RecordLabel, Lyrics, Notes)
select distinct
  SongID as ContentId
, c2.CatalogId
, 1 as CreatedByUserId
, ISNULL(LastUpdatedOn, GETDATE()) as CreatedOn
, 1 as LastUpdatedByUserId
, ISNULL(LastUpdatedOn, GETDATE()) LastUpdatedOn
, 0 as IsControlledAllIn
, HasPreview as HasMediaPreviewVersion
, HasFullSongFile as HasMediaFullVersion
, Title
, Artists as Artist
, Writers
, PopCharts
, CountryCharts
, ReleaseYear
, l.RecordLabelName as RecordLabel
, Lyrics
, Notes
from
	SongSearch.dbo.Songs s left outer join 
	SongSearch.dbo.RecordLabels l on s.RecordLabelID = l.RecordLabelID left outer  join
	SongSearch.dbo.Catalogs c on s.CatalogID = c.CatalogID inner  join
	SongSearch2.dbo.Catalogs c2 on UPPER(c.CatalogName) = UPPER(c2.CatalogName)
	
set identity_insert dbo.contents off


set identity_insert dbo.ContentRights on
insert into dbo.ContentRights
(ContentRightId, ContentId, RightsTypeId, RightsHolderId, RightsHolderName, RightsHolderShare)
select
ContentRightId, c.ContentId, RightsTypeId, cr.RightsHolderId, r.RightsHolderName, RightsHolderShare
from FordMusic.dbo.ContentRights cr inner join FordMusic.dbo.RightsHolders r on cr.RightsHolderId = r.RightsHolderId
inner join dbo.Contents c on cr.ContentId = c.ContentId
set identity_insert dbo.ContentRights off

insert into dbo.ContentRightTerritories
(ContentRightId, TerritoryId)
select c.ContentRightId, TerritoryId
from FordMusic.dbo.ContentRightsTerritories cr
inner join dbo.ContentRights c on cr.ContentRightId = c.ContentRightId


insert into dbo.ContentTags
(ContentId, TagId)
select ct.ContentId, TagId
from FordMusic.dbo.ContentTags ct
inner join dbo.Contents c on ct.ContentId = c.ContentId

