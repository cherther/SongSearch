CREATE TABLE [dbo].[UserActionEvents](
	[UserActionEventId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserActionEventDate] [datetime] NOT NULL,
	[SessionId] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserId] [int] NOT NULL,
	[UserActionId] [int] NOT NULL,
 CONSTRAINT [PK_UserActionEvents] PRIMARY KEY CLUSTERED 
(
	[UserActionEventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
;

CREATE TABLE [dbo].[ContentActionEvents](
	[ContentActionEventId] [bigint] IDENTITY(1,1) NOT NULL,
	[ContentActionEventDate] [datetime] NOT NULL,
	[SessionId] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserId] [int] NOT NULL,
	[ContentActionId] [int] NOT NULL,
	[ContentId] [int] NOT NULL,
 CONSTRAINT [PK_ContentActionEvents] PRIMARY KEY CLUSTERED 
(
	[ContentActionEventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
;

CREATE TABLE [dbo].[SearchEvents](
	[SearchEventId] [bigint] IDENTITY(1,1) NOT NULL,
	[SearchEventDate] [datetime] NOT NULL,
	[SessionId] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserId] [int] NOT NULL,
	[ResultCount] [int] NOT NULL,
	[QueryString] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_SearchEvents] PRIMARY KEY CLUSTERED 
(
	[SearchEventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
;

ALTER TABLE dbo.SearchEvents ADD CONSTRAINT
	FK_SearchEvents_Users FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.Users
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
;

ALTER TABLE dbo.UserActionEvents ADD CONSTRAINT
	FK_UserActionEvents_Users FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.Users
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
;
ALTER TABLE dbo.ContentActionEvents ADD CONSTRAINT
	FK_ContentActionEvents_Contents FOREIGN KEY
	(
	ContentId
	) REFERENCES dbo.Contents
	(
	ContentId
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
;
ALTER TABLE dbo.ContentActionEvents ADD CONSTRAINT
	FK_ContentActionEvents_Users FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.Users
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
;

INSERT INTO dbo.UserActionEvents
(UserActionEventDate, SessionId, UserId, UserActionId)
SELECT UserActionEventDate, SessionId, UserId, UserActionId
FROM SongSearch2Log.dbo.UserActionEvents;
;

INSERT INTO dbo.ContentActionEvents
(ContentActionEventDate, SessionId, UserId, ContentActionId, ContentId)
SELECT ContentActionEventDate, SessionId, UserId, ContentActionId, ContentId
FROM SongSearch2Log.dbo.ContentActionEvents;

INSERT INTO dbo.SearchEvents
(SearchEventDate, SessionId, UserId, ResultCount, QueryString)
SELECT SearchEventDate, SessionId, UserId, ResultCount, QueryString
FROM SongSearch2Log.dbo.SearchEvents;