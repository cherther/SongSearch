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


