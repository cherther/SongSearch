CREATE TABLE [dbo].[Catalogs](
	[CatalogId] [int] IDENTITY(1,1) NOT NULL,
	[CatalogName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedByUserId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Catalogs] PRIMARY KEY CLUSTERED 
(
	[CatalogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_Catalogs_Creators] FOREIGN KEY([CreatedByUserId])
REFERENCES [dbo].[Users] ([UserId])
)


