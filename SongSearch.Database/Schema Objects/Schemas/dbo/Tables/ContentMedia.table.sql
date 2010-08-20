CREATE TABLE [dbo].[ContentMedia](
	[ContentId] [int] NOT NULL,
	[MediaVersion] [int] NOT NULL,
	[MediaDate] [datetime] NOT NULL,
	[IsRemote] [bit] NOT NULL,
	[MediaType] [varchar](5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MediaBitRate] [int] NULL,
	[MediaSize] [bigint] NULL,
	[MediaLength] [bigint] NULL,
 CONSTRAINT [PK_ContentMedia] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC,
	[MediaVersion] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)


