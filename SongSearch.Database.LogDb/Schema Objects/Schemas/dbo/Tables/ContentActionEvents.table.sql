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


