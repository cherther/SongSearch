-- All In
update dbo.Import_SongData
set IsAllIn = 1
where lower(AllIn) = 'x'

update dbo.Import_SongData
set IsAllIn = 0
where AllIn is null or lower(AllIn) <> 'x'

update dbo.Contents
set IsControlledAllIn = i.IsAllIn
from dbo.Contents c inner join dbo.Import_SongData i on c.ContentId = i.SongID

-- Keywords
update dbo.Contents
set Keywords = i.Keywords
from dbo.Contents c inner join dbo.Import_SongData i on c.ContentId = i.SongID

-- SimilarSongs
update dbo.Contents
set SimilarSongs = i.SimilarSongs
from dbo.Contents c inner join dbo.Import_SongData i on c.ContentId = i.SongID

-- SimilarSongs
update dbo.Contents
set LicensingNotes = i.Restictions
from dbo.Contents c inner join dbo.Import_SongData i on c.ContentId = i.SongID

-- Content Rights
truncate table dbo.ContentRightTerritories
delete from dbo.ContentRights


insert into dbo.ContentRights
(ContentId, RightsTypeId, RightsHolderName, RightsHolderShare)
select
sd.SongID as ContentId,
1 As RightsTypeId,
'FORD MUSIC SERVICES' AS RightsHolderName,
CONVERT(decimal(28,4), sd.MasterPerc) AS RightsHolderShare
from
dbo.Import_SongData sd
where MasterPerc is not null


insert into dbo.ContentRights
(ContentId, RightsTypeId, RightsHolderName, RightsHolderShare)
select
sd.SongID as ContentId,
2 As RightsTypeId,
'FORD MUSIC SERVICES' AS RightsHolderName,
CONVERT(decimal(28,4), sd.CompPerc) AS RightsHolderShare
from
dbo.Import_SongData sd
where CompPerc is not null


insert into dbo.ContentRightTerritories
(ContentRightId, TerritoryId)
select
cr.ContentRightId, tr.TerritoryId
from dbo.ContentRights cr,
dbo.Territories tr
where tr.TerritoryId in (1,7)



select
i.SoundsLike
from dbo.Import_SongData i
where i.SoundsLike is not null

