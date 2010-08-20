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


