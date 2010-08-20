CREATE TABLE [dbo].[Contents](
	[ContentId] [int] IDENTITY(1,1) NOT NULL,
	[CatalogId] [int] NOT NULL,
	[CreatedByUserId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastUpdatedByUserId] [int] NOT NULL,
	[LastUpdatedOn] [datetime] NOT NULL,
	[IsControlledAllIn] [bit] NOT NULL,
	[Title] [nvarchar](300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Artist] [nvarchar](300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Writers] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Pop] [int] NULL,
	[Country] [int] NULL,
	[ReleaseYear] [int] NULL,
	[RecordLabel] [nvarchar](300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Lyrics] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LyricsIndex] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Notes] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Keywords] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SimilarSongs] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LicensingNotes] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SoundsLike] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Instruments] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Contents] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_Contents_Catalogs] FOREIGN KEY([CatalogId])
REFERENCES [dbo].[Catalogs] ([CatalogId])
ON DELETE CASCADE
NOT FOR REPLICATION 
)


