truncate table dbo.fct_SearchEvents;

insert into dbo.fct_SearchEvents
select
	convert(datetime, convert(varchar, f.SearchEventDate, 101)) as EventDate
	, isnull(f.SessionId, '(N/A)') as SessionId
	, isnull(f.UserId, -1) as UserId
	, isnull(f.SearchActionId , -1) as EventActionId
	, f.ResultCount
	, LEFT(f.QueryString, 100) as QueryString
	, count(*) as SearchEventCnt
from
	SongSearchBI_ODS.dbo.SearchEvents f
group by	
	convert(datetime, convert(varchar, f.SearchEventDate, 101))
	, isnull(f.SessionId, '(N/A)')
	, isnull(f.UserId, -1)
	, isnull(f.SearchActionId , -1)
	, f.ResultCount
	, LEFT(f.QueryString, 100)
;

truncate table 	dbo.fct_UserEvents
insert into
	dbo.fct_UserEvents
select
	convert(datetime, convert(varchar, f.UserActionEventDate, 101)) as EventDate
	, isnull(f.SessionId, '(N/A)') as SessionId
	, isnull(f.UserId, -1) as UserId
	, isnull(f.UserActionId , -1) as EventActionId
	, count(*) As UserEventCnt
from
	SongSearchBI_ODS.dbo.UserActionEvents f
group by
	convert(datetime, convert(varchar, f.UserActionEventDate, 101))
	, isnull(f.SessionId, '(N/A)')
	, isnull(f.UserId, -1)
	, isnull(f.UserActionId , -1)
;
truncate table 	dbo.fct_ContentEvents
insert into
	dbo.fct_ContentEvents
select
	convert(datetime, convert(varchar, f.ContentActionEventDate, 101)) as EventDate
	, isnull(f.SessionId, '(N/A)') as SessionId
	, isnull(f.UserId, -1) as UserId
	, isnull(f.ContentActionId , -1) as EventActionId
	, isnull(f.ContentId, -1) as ContentId
	, count(*) As ContentEventCnt
from
	SongSearchBI_ODS.dbo.ContentActionEvents f
group by
	convert(datetime, convert(varchar, f.ContentActionEventDate, 101))
	, isnull(f.SessionId, '(N/A)')
	, isnull(f.UserId, -1)
	, isnull(f.ContentActionId , -1)
	, isnull(f.ContentId, -1)