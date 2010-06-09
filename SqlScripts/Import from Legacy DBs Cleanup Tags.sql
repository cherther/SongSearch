delete from dbo.Tags where TagTypeId = 13

update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, 'Earth, Wind', 'Earth Wind')
where SoundsLike like '%Earth, Wind%'

update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, 'Earth, Wind', 'Earth Wind')
where SoundsLike like '%Earth, Wind%'

update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, ', & Fire', ' & Fire')
where SoundsLike like '%, & Fire%'

update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, 'Crosby, Stills', 'Crosby Stills')
where SoundsLike like '%Crosby, Stills%'

update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, 'Nash,', 'Nash')
where SoundsLike like '%Nash,%'

update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, ', Nash', ' Nash')
where SoundsLike like '%, Nash%'

update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, 'CSN', 'Crosby Stills & Nash')
where SoundsLike like '%CSN%'

update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, ', & Nash', ' & Nash')
where SoundsLike like '%, & Nash%'

update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, '07', 'Zero 7')
where SoundsLike like '%07%'


update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, 'Led Zepplin', 'Led Zeppelin')
where SoundsLike like '%Led Zepplin%'

update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, 'Macy Grey', 'Macy Gray')
where SoundsLike like '%Macy Grey%'

update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, 'Macy Gray', 'Macy Gray')
where SoundsLike like '%Macy Gray%'


update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, 'Macy Gray', 'Macy Gray')
where SoundsLike like '%Macy Gray%'


update dbo.Import_SongData
set SoundsLike = Replace(SoundsLike, 'Matchbox 20', 'Matchbox 20')
where SoundsLike like '%Matchbox 20%'



delete from dbo.Tags where TagTypeId = 14


update dbo.Import_SongData
set Instrumentation = Replace(Instrumentation, 'Piano', 'Piano')
where Instrumentation like '%Piano%'

update dbo.Import_SongData
set Instrumentation = Replace(Instrumentation, 'Saxophone', 'Sax')
where Instrumentation like '%Saxophone%'

update dbo.Import_SongData
set Instrumentation = Replace(Instrumentation, 'Sax', 'Saxophone')
where Instrumentation like '%Sax%'
