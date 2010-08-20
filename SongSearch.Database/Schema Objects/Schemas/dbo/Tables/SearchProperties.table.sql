CREATE TABLE [dbo].[SearchProperties](
	[PropertyId] [int] NOT NULL,
	[PropertyName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DisplayName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ShortName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SearchGroup] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SearchTypeId] [smallint] NULL,
	[SearchPredicate] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PropertyType] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LookupName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ListName] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AccessLevel] [smallint] NOT NULL,
	[IsListable] [bit] NOT NULL,
	[IsCacheable] [bit] NOT NULL,
	[IsIndexable] [bit] NOT NULL,
	[IncludeInSearchMenu] [bit] NOT NULL,
 CONSTRAINT [PK_SearchProperties] PRIMARY KEY CLUSTERED 
(
	[PropertyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)


