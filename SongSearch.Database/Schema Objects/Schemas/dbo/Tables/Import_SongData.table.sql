CREATE TABLE [dbo].[Import_SongData](
	[SongID] [int] NOT NULL,
	[Title] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Artist] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RecordLabel] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ReleaseYear] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Catalog] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Notes] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Splits] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MasterPerc] [float] NULL,
	[CompPerc] [float] NULL,
	[AllIn] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsAllIn] [bit] NULL,
	[Instrumentation] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SoundsLike] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Keywords] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Restictions] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Styles] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SimilarSongs] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)


