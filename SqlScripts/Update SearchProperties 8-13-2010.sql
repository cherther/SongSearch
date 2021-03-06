/****** Object:  Table [dbo].[Contacts]    Script Date: 08/13/2010 08:18:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contacts]') AND type in (N'U'))
DROP TABLE [dbo].[Contacts]
GO
/****** Object:  Table [dbo].[SearchProperties]    Script Date: 08/13/2010 08:18:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchProperties]') AND type in (N'U'))
DROP TABLE [dbo].[SearchProperties]
GO
/****** Object:  Default [DF_Contacts_IsDefault]    Script Date: 08/13/2010 08:18:15 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Contacts_IsDefault]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contacts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Contacts_IsDefault]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Contacts] DROP CONSTRAINT [DF_Contacts_IsDefault]
END


End
GO
/****** Object:  Table [dbo].[SearchProperties]    Script Date: 08/13/2010 08:18:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchProperties]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SearchProperties](
	[PropertyId] [int] NOT NULL,
	[PropertyName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DisplayName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ShortName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SearchGroup] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SearchMenuOrder] [int] NOT NULL,
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
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SearchProperties]') AND name = N'NX_SearchProperties_PropertyName')
CREATE NONCLUSTERED INDEX [NX_SearchProperties_PropertyName] ON [dbo].[SearchProperties] 
(
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (1, N'Title', N'Title', N'Title', NULL, 1, 1, N'like ''{0}''', N'Overview', NULL, N'Title', 4, 1, 0, 1, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (2, N'Artist', N'Artist', N'Artist', NULL, 2, 1, N'like ''{0}''', N'Overview', NULL, N'Artist', 4, 1, 1, 1, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (3, N'Pop', N'Billboard Pop Chart', N'Pop', N'Charts', 3, 2, N'is not null', N'Overview', NULL, N'Pop', 4, 1, 0, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (4, N'Country', N'Billboard Country Chart', N'Country', N'Charts', 4, 2, N'is not null', N'Overview', NULL, N'Country', 4, 1, 0, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (5, N'ReleaseYear', N'Release Year', N'Year', NULL, 6, 3, N'between {0} and {1}', N'Overview', NULL, N'Year', 4, 1, 0, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (6, N'RecordLabel', N'Record Label', N'Label', NULL, 14, 1, N'like ''{0}''', N'Overview', NULL, N'Label', 4, 1, 1, 1, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (7, N'Lyrics', N'Lyrics', N'Lyrics', NULL, 5, 1, N'like ''{0}''', N'Lyrics', NULL, NULL, 4, 0, 0, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (8, N'Tempo', N'Tempo', N'Tempo', NULL, 7, 4, NULL, N'Tag', N'ContentTags', N'Tempo', 4, 0, 0, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (9, N'Gender', N'Gender', N'Gender', NULL, 11, 4, NULL, N'Tag', N'ContentTags', N'Gender', 4, 0, 0, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (10, N'Mood', N'Mood', N'Mood', NULL, 9, 4, NULL, N'Tag', N'ContentTags', NULL, 4, 0, 1, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (11, N'Style', N'Style', N'Style', NULL, 5, 4, NULL, N'Tag', N'ContentTags', NULL, 4, 0, 1, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (12, N'Writers', N'Writers', N'Writers', NULL, 15, 1, N'like ''{0}''', N'Overview', NULL, N'Writer', 4, 0, 1, 1, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (13, N'SoundsLike', N'Sounds Like', N'Sounds Like', NULL, 8, 1, NULL, N'Tag', N'ContentTags', NULL, 4, 0, 0, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (14, N'Instruments', N'Featured Instruments', N'Instruments', NULL, 12, 1, NULL, N'Tag', N'ContentTags', NULL, 4, 0, 0, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (15, N'Language', N'Language', N'Language', NULL, 0, 4, NULL, N'Tag', N'ContentTags', N'Language', 4, 0, 0, 0, 0)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (16, N'Catalog.CatalogName', N'Catalog', N'Catalog', NULL, 16, 1, N'like ''{0}''', N'Overview', N'Catalog', N'Catalog', 2, 1, 1, 1, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (17, N'IsControlledAllIn', N'100% All-in Master & Comp.', N'All-in', NULL, 13, 6, N'= 1', N'Licensing', NULL, N'All-In', 3, 0, 0, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (18, N'LicensingNotes', N'Licensing Restrictions', N'Licensing', NULL, 0, 1, N'like ''{0}''', N'Licensing', NULL, NULL, 3, 0, 0, 0, 0)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (19, N'Notes', N'Notes', N'Notes', NULL, 17, 1, N'like ''{0}''', N'Overview', NULL, N'Notes', 1, 0, 0, 0, 1)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (20, N'Brand', N'Brand', N'Brand', NULL, 0, 4, NULL, N'Tag', N'ContentTags', N'Brand', 4, 0, 0, 0, 0)
INSERT [dbo].[SearchProperties] ([PropertyId], [PropertyName], [DisplayName], [ShortName], [SearchGroup], [SearchMenuOrder], [SearchTypeId], [SearchPredicate], [PropertyType], [LookupName], [ListName], [AccessLevel], [IsListable], [IsCacheable], [IsIndexable], [IncludeInSearchMenu]) VALUES (21, N'Keywords', N'Keywords', N'Keywords', NULL, 10, 1, N'like ''{0}''', N'Overview', NULL, NULL, 4, 0, 0, 0, 1)
/****** Object:  Table [dbo].[Contacts]    Script Date: 08/13/2010 08:18:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contacts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Contacts](
	[ContactId] [int] IDENTITY(1,1) NOT NULL,
	[ContactName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CompanyName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Address1] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Address2] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[City] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[StateRegion] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PostalCode] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Country] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Phone1] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Phone2] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Fax] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Email] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AdminEmail] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsDefault] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedByUserId] [int] NOT NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[Contacts] ON
INSERT [dbo].[Contacts] ([ContactId], [ContactName], [CompanyName], [Address1], [Address2], [City], [StateRegion], [PostalCode], [Country], [Phone1], [Phone2], [Fax], [Email], [AdminEmail], [IsDefault], [CreatedOn], [CreatedByUserId]) VALUES (1, N'Art Ford', N'WorldSongNet.com', NULL, NULL, NULL, NULL, NULL, NULL, N'(323) 939-2955', NULL, N'(323) 939-2951', N'artfordmusic@yahoo.com', N'worldsongnet@yahoo.com,info@codewerks.de', 1, CAST(0x00009DB300000000 AS DateTime), 1)
INSERT [dbo].[Contacts] ([ContactId], [ContactName], [CompanyName], [Address1], [Address2], [City], [StateRegion], [PostalCode], [Country], [Phone1], [Phone2], [Fax], [Email], [AdminEmail], [IsDefault], [CreatedOn], [CreatedByUserId]) VALUES (2, N'Claus Herther', N'Tuneworks', N'5850 W 3rd Street', N'Suite 274', N'Los Angeles', N'CA', N'90036', N'USA', N'(323) 982-9006', N'(323) 982-9006', N'(323) 982-9006', N'claus.herther@gmail.com', N'info@codewerks.de', 1, CAST(0x00009DB900000000 AS DateTime), 2)
INSERT [dbo].[Contacts] ([ContactId], [ContactName], [CompanyName], [Address1], [Address2], [City], [StateRegion], [PostalCode], [Country], [Phone1], [Phone2], [Fax], [Email], [AdminEmail], [IsDefault], [CreatedOn], [CreatedByUserId]) VALUES (7, N'P. Luger', N'PlugsWorldwide.com', NULL, NULL, NULL, NULL, NULL, NULL, N'1-800-MYSONGS', NULL, NULL, N'claus.herther@gmail.com', N'claus.herther@gmail.com', 1, CAST(0x00009DB900AC755C AS DateTime), 5)
INSERT [dbo].[Contacts] ([ContactId], [ContactName], [CompanyName], [Address1], [Address2], [City], [StateRegion], [PostalCode], [Country], [Phone1], [Phone2], [Fax], [Email], [AdminEmail], [IsDefault], [CreatedOn], [CreatedByUserId]) VALUES (8, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'claus@codewerks.de', NULL, 1, CAST(0x00009DBB009E12FE AS DateTime), 47)
INSERT [dbo].[Contacts] ([ContactId], [ContactName], [CompanyName], [Address1], [Address2], [City], [StateRegion], [PostalCode], [Country], [Phone1], [Phone2], [Fax], [Email], [AdminEmail], [IsDefault], [CreatedOn], [CreatedByUserId]) VALUES (9, N'Jody Friedman', N'HD MUSIC NOW', N'26185 Hillsford Place', NULL, N'Lake Forest', N'CA', N'92630', N'USA', N'(949) 916-8368', N'(678) 849-7444', N'(949) 916-8141', N'jody@hdmusicnow.com', N'jody@hdmusicnow.com', 1, CAST(0x00009DBC0166AD02 AS DateTime), 48)
SET IDENTITY_INSERT [dbo].[Contacts] OFF
/****** Object:  Default [DF_Contacts_IsDefault]    Script Date: 08/13/2010 08:18:15 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Contacts_IsDefault]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contacts]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Contacts_IsDefault]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Contacts] ADD  CONSTRAINT [DF_Contacts_IsDefault]  DEFAULT ((1)) FOR [IsDefault]
END


End
GO
