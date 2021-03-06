/****** Object:  Table [dbo].[PricingPlans]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PricingPlans]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PricingPlans](
	[PricingPlanId] [int] NOT NULL,
	[PricingPlanName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PromoMessage] [nvarchar](150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsPromo] [bit] NOT NULL,
	[IsFeatured] [bit] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[ShowOnSite] [bit] NOT NULL,
	[PlanCharge] [decimal](28, 4) NULL,
	[PlanRecurrance] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NumberOfSongs] [int] NULL,
	[NumberOfInvitedUsers] [int] NULL,
	[NumberOfCatalogAdmins] [int] NULL,
	[CustomContactUs] [bit] NOT NULL,
	[CustomSiteProfile] [bit] NOT NULL,
 CONSTRAINT [PK_PricingPlans] PRIMARY KEY CLUSTERED 
(
	[PricingPlanId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[PlanBalances]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlanBalances]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PlanBalances](
	[PlanBalanceId] [int] IDENTITY(1,1) NOT NULL,
	[PricingPlanId] [int] NOT NULL,
	[LastUpdatedOn] [datetime] NOT NULL,
	[LastUpdatedByUserId] [int] NOT NULL,
	[NumberOfSongs] [int] NOT NULL,
	[NumberOfInvitedUsers] [int] NOT NULL,
	[NumberOfCatalogAdmins] [int] NOT NULL,
 CONSTRAINT [PK_PlanQuotas] PRIMARY KEY CLUSTERED 
(
	[PlanBalanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] NOT NULL,
	[RoleName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12/07/2010 09:56:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Password] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FirstName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LastName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ParentUserId] [int] NULL,
	[PlanUserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[SiteProfileId] [int] NOT NULL,
	[PricingPlanId] [int] NOT NULL,
	[PlanBalanceId] [int] NOT NULL,
	[InvitationId] [uniqueidentifier] NULL,
	[RegisteredOn] [datetime] NOT NULL,
	[Signature] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ShowDebugInfo] [bit] NULL,
	[AppendSignatureToTitle] [bit] NOT NULL,
	[HasAgreedToPrivacyPolicy] [bit] NOT NULL,
	[HasAllowedCommunication] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'CLNX_Users_UserName')
CREATE CLUSTERED INDEX [CLNX_Users_UserName] ON [dbo].[Users] 
(
	[UserName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'NX_Users_ParentUserId')
CREATE NONCLUSTERED INDEX [NX_Users_ParentUserId] ON [dbo].[Users] 
(
	[ParentUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'NX_Users_Password')
CREATE NONCLUSTERED INDEX [NX_Users_Password] ON [dbo].[Users] 
(
	[Password] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[Catalogs]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Catalogs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Catalogs](
	[CatalogId] [int] IDENTITY(1,1) NOT NULL,
	[CatalogName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedByUserId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Catalogs] PRIMARY KEY CLUSTERED 
(
	[CatalogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Catalogs]') AND name = N'NX_Catalogs_CatalogName')
CREATE NONCLUSTERED INDEX [NX_Catalogs_CatalogName] ON [dbo].[Catalogs] 
(
	[CatalogName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Catalogs]') AND name = N'NX_Catalogs_CreatedByUserId')
CREATE NONCLUSTERED INDEX [NX_Catalogs_CreatedByUserId] ON [dbo].[Catalogs] 
(
	[CreatedByUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[Contents]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contents]') AND type in (N'U'))
BEGIN
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Contents]') AND name = N'NX_Contents_Artist')
CREATE NONCLUSTERED INDEX [NX_Contents_Artist] ON [dbo].[Contents] 
(
	[Artist] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Contents]') AND name = N'NX_Contents_RecordLabel')
CREATE NONCLUSTERED INDEX [NX_Contents_RecordLabel] ON [dbo].[Contents] 
(
	[RecordLabel] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Contents]') AND name = N'NX_Contents_ReleaseYear')
CREATE NONCLUSTERED INDEX [NX_Contents_ReleaseYear] ON [dbo].[Contents] 
(
	[RecordLabel] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Contents]') AND name = N'NX_Contents_Title')
CREATE NONCLUSTERED INDEX [NX_Contents_Title] ON [dbo].[Contents] 
(
	[Title] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[ContentRights]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentRights]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentRights](
	[ContentRightId] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [int] NOT NULL,
	[RightsTypeId] [int] NOT NULL,
	[RightsHolderName] [nvarchar](300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[RightsHolderShare] [decimal](28, 4) NOT NULL,
	[CreatedByUserId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ItemsRights] PRIMARY KEY NONCLUSTERED 
(
	[ContentRightId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ContentRights]') AND name = N'CLNX_ContentRights_ContentId')
CREATE CLUSTERED INDEX [CLNX_ContentRights_ContentId] ON [dbo].[ContentRights] 
(
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ContentRights]') AND name = N'NX_ContentRights_RightsHolderName')
CREATE NONCLUSTERED INDEX [NX_ContentRights_RightsHolderName] ON [dbo].[ContentRights] 
(
	[RightsHolderName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[ContentRepresentation]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentRepresentation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentRepresentation](
	[ContentRepresentationId] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [int] NOT NULL,
	[RightsTypeId] [int] NOT NULL,
	[RepresentationShare] [decimal](28, 4) NOT NULL,
	[CreatedByUserId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ContentRepresentation] PRIMARY KEY CLUSTERED 
(
	[ContentRepresentationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[ContentMedia]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentMedia]') AND type in (N'U'))
BEGIN
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
END
GO
/****** Object:  Table [dbo].[Carts]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Carts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Carts](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CartStatus] [smallint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastUpdatedOn] [datetime] NOT NULL,
	[NumberItems] [int] NULL,
	[CompressedSize] [decimal](28, 4) NULL,
	[ArchiveName] [varchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsLastProcessed] [bit] NULL,
 CONSTRAINT [PK_Carts] PRIMARY KEY CLUSTERED 
(
	[CartId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Carts]') AND name = N'NX_Carts_UserId')
CREATE NONCLUSTERED INDEX [NX_Carts_UserId] ON [dbo].[Carts] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[CartContents]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CartContents]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CartContents](
	[CartId] [int] NOT NULL,
	[ContentId] [int] NOT NULL,
 CONSTRAINT [PK_CartItems] PRIMARY KEY CLUSTERED 
(
	[CartId] ASC,
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[SearchEvents]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchEvents]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SearchEvents](
	[SearchEventId] [bigint] IDENTITY(1,1) NOT NULL,
	[SearchEventDate] [datetime] NOT NULL,
	[SessionId] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserId] [int] NOT NULL,
	[SearchActionId] [int] NOT NULL,
	[ResultCount] [int] NOT NULL,
	[QueryString] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_SearchEvents] PRIMARY KEY CLUSTERED 
(
	[SearchEventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Invitations]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Invitations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Invitations](
	[InvitationId] [uniqueidentifier] NOT NULL,
	[InvitationEmailAddress] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ExpirationDate] [datetime] NOT NULL,
	[InvitationStatus] [smallint] NOT NULL,
	[InvitedByUserId] [int] NOT NULL,
	[IsPlanInvitation] [bit] NOT NULL,
 CONSTRAINT [PK_Invitations] PRIMARY KEY CLUSTERED 
(
	[InvitationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Invitations]') AND name = N'NX_Invitations_InvitedByUserId')
CREATE NONCLUSTERED INDEX [NX_Invitations_InvitedByUserId] ON [dbo].[Invitations] 
(
	[InvitedByUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[ContentActionEvents]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentActionEvents]') AND type in (N'U'))
BEGIN
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
END
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contacts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Contacts](
	[ContactId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ContactTypeId] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
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
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[UserActionEvents]    Script Date: 12/07/2010 09:56:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserActionEvents]') AND type in (N'U'))
BEGIN
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
END
GO
/****** Object:  View [dbo].[vwLogEvents]    Script Date: 12/07/2010 09:56:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwLogEvents]'))
EXEC dbo.sp_executesql @statement = N'

CREATE view [dbo].[vwLogEvents]
as
select
	''U'' + CONVERT(varchar, uae.UserActionEventId) As ActionEventId, 
	CONVERT(datetime, CONVERT(varchar, uae.UserActionEventDate, 101)) As ActionEventDate, 
	uae.SessionId, 
	uae.UserId, 
	uae.UserActionId As ActionId,
	NULL As ContentId,
	NULL As ResultCount,
	NULL As QueryString,
	1 As LogEventCnt
from
	dbo.UserActionEvents uae
union all
select
	''C'' + CONVERT(varchar, cae.ContentActionEventId) As ActionEventId, 
	CONVERT(datetime, CONVERT(varchar, cae.ContentActionEventDate, 101)) As ActionEventDate, 
	cae.SessionId, 
	cae.UserId, 
	cae.ContentActionId As ActionId, 
	cae.ContentId,
	NULL As ResultCount,
	NULL As QueryString,
	1 As LogEventCnt
from
	dbo.ContentActionEvents cae
union all
select
	''S'' + CONVERT(varchar, se.SearchEventId) As ActionEventId, 
	CONVERT(datetime, CONVERT(varchar, se.SearchEventDate, 101)) As ActionEventDate, 
	se.SessionId, 
	se.UserId, 
	se.SearchActionId As ActionId, 
	NULL As ContentId,
	se.ResultCount As ResultCount,
	se.QueryString As QueryString,
	1 As LogEventCnt
from
	dbo.SearchEvents se

'
GO
/****** Object:  Table [dbo].[SiteProfiles]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiteProfiles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SiteProfiles](
	[ProfileId] [int] IDENTITY(1,1) NOT NULL,
	[ProfileName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CompanyName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContactAddress] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContactAddress2] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ContactCity] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContactStateRegion] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContactPostalCode] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContactCountry] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContactPhone] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContactFax] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ContactEmail] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AdminEmail] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedByUserId] [int] NOT NULL,
	[HasProfileLogo] [bit] NOT NULL,
 CONSTRAINT [PK_SiteProfiles] PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[SiteProfilesContacts]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiteProfilesContacts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SiteProfilesContacts](
	[SiteProfileId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
 CONSTRAINT [PK_SiteProfilesContacts] PRIMARY KEY CLUSTERED 
(
	[SiteProfileId] ASC,
	[ContactId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[SearchProperties]    Script Date: 12/07/2010 09:56:43 ******/
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
/****** Object:  Table [dbo].[Territories]    Script Date: 12/07/2010 09:56:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Territories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Territories](
	[TerritoryId] [int] IDENTITY(1,1) NOT NULL,
	[TerritoryCode] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[TerritoryName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_Territories] PRIMARY KEY CLUSTERED 
(
	[TerritoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[ContentRightTerritories]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentRightTerritories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentRightTerritories](
	[ContentRightId] [int] NOT NULL,
	[TerritoryId] [int] NOT NULL,
 CONSTRAINT [PK_ItemsRightsTerritories] PRIMARY KEY CLUSTERED 
(
	[ContentRightId] ASC,
	[TerritoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[ContentRepresentationTerritories]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentRepresentationTerritories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentRepresentationTerritories](
	[ContentRepresentationId] [int] NOT NULL,
	[TerritoryId] [int] NOT NULL,
 CONSTRAINT [PK_ContentRepresentationTerritories] PRIMARY KEY CLUSTERED 
(
	[ContentRepresentationId] ASC,
	[TerritoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tags](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[TagTypeId] [int] NOT NULL,
	[CreatedByUserId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Tags]') AND name = N'NX_Tags_CreatedByUserId')
CREATE NONCLUSTERED INDEX [NX_Tags_CreatedByUserId] ON [dbo].[Tags] 
(
	[CreatedByUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[ContentTags]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentTags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentTags](
	[ContentId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
 CONSTRAINT [PK_ContentTags] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Import_SongData]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Import_SongData]') AND type in (N'U'))
BEGIN
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
END
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_LogError]    Script Date: 12/07/2010 09:56:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ELMAH_LogError]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[ELMAH_LogError]
(
    @ErrorId UNIQUEIDENTIFIER,
    @Application NVARCHAR(60),
    @Host NVARCHAR(30),
    @Type NVARCHAR(100),
    @Source NVARCHAR(60),
    @Message NVARCHAR(500),
    @User NVARCHAR(50),
    @AllXml NTEXT,
    @StatusCode INT,
    @TimeUtc DATETIME
)
AS

    SET NOCOUNT ON

    INSERT
    INTO
        [ELMAH_Error]
        (
            [ErrorId],
            [Application],
            [Host],
            [Type],
            [Source],
            [Message],
            [User],
            [AllXml],
            [StatusCode],
            [TimeUtc]
        )
    VALUES
        (
            @ErrorId,
            @Application,
            @Host,
            @Type,
            @Source,
            @Message,
            @User,
            @AllXml,
            @StatusCode,
            @TimeUtc
        )

' 
END
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorXml]    Script Date: 12/07/2010 09:56:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ELMAH_GetErrorXml]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml]
(
    @Application NVARCHAR(60),
    @ErrorId UNIQUEIDENTIFIER
)
AS

    SET NOCOUNT ON

    SELECT 
        [AllXml]
    FROM 
        [ELMAH_Error]
    WHERE
        [ErrorId] = @ErrorId
    AND
        [Application] = @Application

' 
END
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorsXml]    Script Date: 12/07/2010 09:56:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ELMAH_GetErrorsXml]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml]
(
    @Application NVARCHAR(60),
    @PageIndex INT = 0,
    @PageSize INT = 15,
    @TotalCount INT OUTPUT
)
AS 

    SET NOCOUNT ON

    DECLARE @FirstTimeUTC DATETIME
    DECLARE @FirstSequence INT
    DECLARE @StartRow INT
    DECLARE @StartRowIndex INT

    SELECT 
        @TotalCount = COUNT(1) 
    FROM 
        [ELMAH_Error]
    WHERE 
        [Application] = @Application

    -- Get the ID of the first error for the requested page

    SET @StartRowIndex = @PageIndex * @PageSize + 1

    IF @StartRowIndex <= @TotalCount
    BEGIN

        SET ROWCOUNT @StartRowIndex

        SELECT  
            @FirstTimeUTC = [TimeUtc],
            @FirstSequence = [Sequence]
        FROM 
            [ELMAH_Error]
        WHERE   
            [Application] = @Application
        ORDER BY 
            [TimeUtc] DESC, 
            [Sequence] DESC

    END
    ELSE
    BEGIN

        SET @PageSize = 0

    END

    -- Now set the row count to the requested page size and get
    -- all records below it for the pertaining application.

    SET ROWCOUNT @PageSize

    SELECT 
        errorId     = [ErrorId], 
        application = [Application],
        host        = [Host], 
        type        = [Type],
        source      = [Source],
        message     = [Message],
        [user]      = [User],
        statusCode  = [StatusCode], 
        time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + ''Z''
    FROM 
        [ELMAH_Error] error
    WHERE
        [Application] = @Application
    AND
        [TimeUtc] <= @FirstTimeUTC
    AND 
        [Sequence] <= @FirstSequence
    ORDER BY
        [TimeUtc] DESC, 
        [Sequence] DESC
    FOR
        XML AUTO

' 
END
GO
/****** Object:  Table [dbo].[UserCatalogRoles]    Script Date: 12/07/2010 09:56:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserCatalogRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserCatalogRoles](
	[UserId] [int] NOT NULL,
	[CatalogId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_UserCatalogRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[CatalogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Subscriptions]    Script Date: 12/07/2010 09:56:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Subscriptions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Subscriptions](
	[SubscriptionId] [int] IDENTITY(1,1) NOT NULL,
	[PricingPlanId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[SubscriptionStartDate] [datetime] NOT NULL,
	[SubscriptionEndDate] [datetime] NULL,
	[PlanCharge] [decimal](28, 4) NOT NULL,
 CONSTRAINT [PK_Subscriptions] PRIMARY KEY CLUSTERED 
(
	[SubscriptionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Default [DF_Contacts_ContactTypeId]    Script Date: 12/07/2010 09:56:43 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Contacts_ContactTypeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contacts]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Contacts_ContactTypeId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Contacts] ADD  CONSTRAINT [DF_Contacts_ContactTypeId]  DEFAULT ((1)) FOR [ContactTypeId]
END


End
GO
/****** Object:  Default [DF_Contacts_IsDefault]    Script Date: 12/07/2010 09:56:43 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Contacts_IsDefault]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contacts]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Contacts_IsDefault]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Contacts] ADD  CONSTRAINT [DF_Contacts_IsDefault]  DEFAULT ((1)) FOR [IsDefault]
END


End
GO
/****** Object:  Default [DF_Invitations_InvitationID]    Script Date: 12/07/2010 09:56:43 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Invitations_InvitationID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Invitations]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Invitations_InvitationID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Invitations] ADD  CONSTRAINT [DF_Invitations_InvitationID]  DEFAULT (newid()) FOR [InvitationId]
END


End
GO
/****** Object:  Default [DF_SearchEvents_SearchActionId]    Script Date: 12/07/2010 09:56:43 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_SearchEvents_SearchActionId]') AND parent_object_id = OBJECT_ID(N'[dbo].[SearchEvents]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SearchEvents_SearchActionId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SearchEvents] ADD  CONSTRAINT [DF_SearchEvents_SearchActionId]  DEFAULT ((100)) FOR [SearchActionId]
END


End
GO
/****** Object:  Default [DF_SiteProfiles_HasProfileLogo]    Script Date: 12/07/2010 09:56:43 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_SiteProfiles_HasProfileLogo]') AND parent_object_id = OBJECT_ID(N'[dbo].[SiteProfiles]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SiteProfiles_HasProfileLogo]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SiteProfiles] ADD  CONSTRAINT [DF_SiteProfiles_HasProfileLogo]  DEFAULT ((0)) FOR [HasProfileLogo]
END


End
GO
/****** Object:  Default [DF_Tags_CreatedByUserId]    Script Date: 12/07/2010 09:56:43 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Tags_CreatedByUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tags]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Tags_CreatedByUserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Tags] ADD  CONSTRAINT [DF_Tags_CreatedByUserId]  DEFAULT ((0)) FOR [CreatedByUserId]
END


End
GO
/****** Object:  Default [DF_Tags_CreatedOn]    Script Date: 12/07/2010 09:56:43 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Tags_CreatedOn]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tags]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Tags_CreatedOn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Tags] ADD  CONSTRAINT [DF_Tags_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
END


End
GO
/****** Object:  Default [DF_Users_SiteProfileId]    Script Date: 12/07/2010 09:56:44 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Users_SiteProfileId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_SiteProfileId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_SiteProfileId]  DEFAULT ((1)) FOR [SiteProfileId]
END


End
GO
/****** Object:  Default [DF_Users_PlanBalanceId]    Script Date: 12/07/2010 09:56:44 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Users_PlanBalanceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_PlanBalanceId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_PlanBalanceId]  DEFAULT ((1)) FOR [PlanBalanceId]
END


End
GO
/****** Object:  ForeignKey [FK_CartContents_Contents]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CartContents_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[CartContents]'))
ALTER TABLE [dbo].[CartContents]  WITH NOCHECK ADD  CONSTRAINT [FK_CartContents_Contents] FOREIGN KEY([ContentId])
REFERENCES [dbo].[Contents] ([ContentId])
ON DELETE CASCADE
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CartContents_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[CartContents]'))
ALTER TABLE [dbo].[CartContents] CHECK CONSTRAINT [FK_CartContents_Contents]
GO
/****** Object:  ForeignKey [FK_CartItems_Carts]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CartItems_Carts]') AND parent_object_id = OBJECT_ID(N'[dbo].[CartContents]'))
ALTER TABLE [dbo].[CartContents]  WITH NOCHECK ADD  CONSTRAINT [FK_CartItems_Carts] FOREIGN KEY([CartId])
REFERENCES [dbo].[Carts] ([CartId])
ON DELETE CASCADE
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CartItems_Carts]') AND parent_object_id = OBJECT_ID(N'[dbo].[CartContents]'))
ALTER TABLE [dbo].[CartContents] CHECK CONSTRAINT [FK_CartItems_Carts]
GO
/****** Object:  ForeignKey [FK_Carts_Users]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Carts_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Carts]'))
ALTER TABLE [dbo].[Carts]  WITH NOCHECK ADD  CONSTRAINT [FK_Carts_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Carts_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Carts]'))
ALTER TABLE [dbo].[Carts] CHECK CONSTRAINT [FK_Carts_Users]
GO
/****** Object:  ForeignKey [FK_Catalogs_Creators]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Catalogs_Creators]') AND parent_object_id = OBJECT_ID(N'[dbo].[Catalogs]'))
ALTER TABLE [dbo].[Catalogs]  WITH CHECK ADD  CONSTRAINT [FK_Catalogs_Creators] FOREIGN KEY([CreatedByUserId])
REFERENCES [dbo].[Users] ([UserId])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Catalogs_Creators]') AND parent_object_id = OBJECT_ID(N'[dbo].[Catalogs]'))
ALTER TABLE [dbo].[Catalogs] CHECK CONSTRAINT [FK_Catalogs_Creators]
GO
/****** Object:  ForeignKey [FK_Contacts_Users]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Contacts_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contacts]'))
ALTER TABLE [dbo].[Contacts]  WITH CHECK ADD  CONSTRAINT [FK_Contacts_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Contacts_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contacts]'))
ALTER TABLE [dbo].[Contacts] CHECK CONSTRAINT [FK_Contacts_Users]
GO
/****** Object:  ForeignKey [FK_ContentActionEvents_Contents]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentActionEvents_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentActionEvents]'))
ALTER TABLE [dbo].[ContentActionEvents]  WITH CHECK ADD  CONSTRAINT [FK_ContentActionEvents_Contents] FOREIGN KEY([ContentId])
REFERENCES [dbo].[Contents] ([ContentId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentActionEvents_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentActionEvents]'))
ALTER TABLE [dbo].[ContentActionEvents] CHECK CONSTRAINT [FK_ContentActionEvents_Contents]
GO
/****** Object:  ForeignKey [FK_ContentActionEvents_Users]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentActionEvents_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentActionEvents]'))
ALTER TABLE [dbo].[ContentActionEvents]  WITH CHECK ADD  CONSTRAINT [FK_ContentActionEvents_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentActionEvents_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentActionEvents]'))
ALTER TABLE [dbo].[ContentActionEvents] CHECK CONSTRAINT [FK_ContentActionEvents_Users]
GO
/****** Object:  ForeignKey [FK_ContentMedia_Contents]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentMedia_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentMedia]'))
ALTER TABLE [dbo].[ContentMedia]  WITH CHECK ADD  CONSTRAINT [FK_ContentMedia_Contents] FOREIGN KEY([ContentId])
REFERENCES [dbo].[Contents] ([ContentId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentMedia_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentMedia]'))
ALTER TABLE [dbo].[ContentMedia] CHECK CONSTRAINT [FK_ContentMedia_Contents]
GO
/****** Object:  ForeignKey [FK_ContentRepresentation_Contents]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRepresentation_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRepresentation]'))
ALTER TABLE [dbo].[ContentRepresentation]  WITH CHECK ADD  CONSTRAINT [FK_ContentRepresentation_Contents] FOREIGN KEY([ContentId])
REFERENCES [dbo].[Contents] ([ContentId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRepresentation_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRepresentation]'))
ALTER TABLE [dbo].[ContentRepresentation] CHECK CONSTRAINT [FK_ContentRepresentation_Contents]
GO
/****** Object:  ForeignKey [FK_ContentRepresentationTerritories_ContentRepresentation]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRepresentationTerritories_ContentRepresentation]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRepresentationTerritories]'))
ALTER TABLE [dbo].[ContentRepresentationTerritories]  WITH CHECK ADD  CONSTRAINT [FK_ContentRepresentationTerritories_ContentRepresentation] FOREIGN KEY([ContentRepresentationId])
REFERENCES [dbo].[ContentRepresentation] ([ContentRepresentationId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRepresentationTerritories_ContentRepresentation]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRepresentationTerritories]'))
ALTER TABLE [dbo].[ContentRepresentationTerritories] CHECK CONSTRAINT [FK_ContentRepresentationTerritories_ContentRepresentation]
GO
/****** Object:  ForeignKey [FK_ContentRepresentationTerritories_Territories]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRepresentationTerritories_Territories]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRepresentationTerritories]'))
ALTER TABLE [dbo].[ContentRepresentationTerritories]  WITH CHECK ADD  CONSTRAINT [FK_ContentRepresentationTerritories_Territories] FOREIGN KEY([TerritoryId])
REFERENCES [dbo].[Territories] ([TerritoryId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRepresentationTerritories_Territories]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRepresentationTerritories]'))
ALTER TABLE [dbo].[ContentRepresentationTerritories] CHECK CONSTRAINT [FK_ContentRepresentationTerritories_Territories]
GO
/****** Object:  ForeignKey [FK_ContentRights_Contents]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRights_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRights]'))
ALTER TABLE [dbo].[ContentRights]  WITH NOCHECK ADD  CONSTRAINT [FK_ContentRights_Contents] FOREIGN KEY([ContentId])
REFERENCES [dbo].[Contents] ([ContentId])
ON DELETE CASCADE
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRights_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRights]'))
ALTER TABLE [dbo].[ContentRights] CHECK CONSTRAINT [FK_ContentRights_Contents]
GO
/****** Object:  ForeignKey [FK_ContentRightTerritories_ContentRights]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRightTerritories_ContentRights]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRightTerritories]'))
ALTER TABLE [dbo].[ContentRightTerritories]  WITH NOCHECK ADD  CONSTRAINT [FK_ContentRightTerritories_ContentRights] FOREIGN KEY([ContentRightId])
REFERENCES [dbo].[ContentRights] ([ContentRightId])
ON DELETE CASCADE
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRightTerritories_ContentRights]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRightTerritories]'))
ALTER TABLE [dbo].[ContentRightTerritories] CHECK CONSTRAINT [FK_ContentRightTerritories_ContentRights]
GO
/****** Object:  ForeignKey [FK_ContentRightTerritories_Territories]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRightTerritories_Territories]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRightTerritories]'))
ALTER TABLE [dbo].[ContentRightTerritories]  WITH CHECK ADD  CONSTRAINT [FK_ContentRightTerritories_Territories] FOREIGN KEY([TerritoryId])
REFERENCES [dbo].[Territories] ([TerritoryId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentRightTerritories_Territories]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentRightTerritories]'))
ALTER TABLE [dbo].[ContentRightTerritories] CHECK CONSTRAINT [FK_ContentRightTerritories_Territories]
GO
/****** Object:  ForeignKey [FK_Contents_Catalogs]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Contents_Catalogs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contents]'))
ALTER TABLE [dbo].[Contents]  WITH NOCHECK ADD  CONSTRAINT [FK_Contents_Catalogs] FOREIGN KEY([CatalogId])
REFERENCES [dbo].[Catalogs] ([CatalogId])
ON DELETE CASCADE
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Contents_Catalogs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contents]'))
ALTER TABLE [dbo].[Contents] CHECK CONSTRAINT [FK_Contents_Catalogs]
GO
/****** Object:  ForeignKey [FK_ContentTags_Contents]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentTags_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentTags]'))
ALTER TABLE [dbo].[ContentTags]  WITH NOCHECK ADD  CONSTRAINT [FK_ContentTags_Contents] FOREIGN KEY([ContentId])
REFERENCES [dbo].[Contents] ([ContentId])
ON DELETE CASCADE
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentTags_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentTags]'))
ALTER TABLE [dbo].[ContentTags] CHECK CONSTRAINT [FK_ContentTags_Contents]
GO
/****** Object:  ForeignKey [FK_ContentTags_Tags]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentTags_Tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentTags]'))
ALTER TABLE [dbo].[ContentTags]  WITH NOCHECK ADD  CONSTRAINT [FK_ContentTags_Tags] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([TagId])
ON DELETE CASCADE
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContentTags_Tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContentTags]'))
ALTER TABLE [dbo].[ContentTags] CHECK CONSTRAINT [FK_ContentTags_Tags]
GO
/****** Object:  ForeignKey [FK_Invitations_Users]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Invitations_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Invitations]'))
ALTER TABLE [dbo].[Invitations]  WITH CHECK ADD  CONSTRAINT [FK_Invitations_Users] FOREIGN KEY([InvitedByUserId])
REFERENCES [dbo].[Users] ([UserId])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Invitations_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Invitations]'))
ALTER TABLE [dbo].[Invitations] CHECK CONSTRAINT [FK_Invitations_Users]
GO
/****** Object:  ForeignKey [FK_PlanBalances_PricingPlans]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PlanBalances_PricingPlans]') AND parent_object_id = OBJECT_ID(N'[dbo].[PlanBalances]'))
ALTER TABLE [dbo].[PlanBalances]  WITH CHECK ADD  CONSTRAINT [FK_PlanBalances_PricingPlans] FOREIGN KEY([PricingPlanId])
REFERENCES [dbo].[PricingPlans] ([PricingPlanId])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PlanBalances_PricingPlans]') AND parent_object_id = OBJECT_ID(N'[dbo].[PlanBalances]'))
ALTER TABLE [dbo].[PlanBalances] CHECK CONSTRAINT [FK_PlanBalances_PricingPlans]
GO
/****** Object:  ForeignKey [FK_SearchEvents_Users]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SearchEvents_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[SearchEvents]'))
ALTER TABLE [dbo].[SearchEvents]  WITH CHECK ADD  CONSTRAINT [FK_SearchEvents_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SearchEvents_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[SearchEvents]'))
ALTER TABLE [dbo].[SearchEvents] CHECK CONSTRAINT [FK_SearchEvents_Users]
GO
/****** Object:  ForeignKey [FK_SiteProfilesContacts_Contacts]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SiteProfilesContacts_Contacts]') AND parent_object_id = OBJECT_ID(N'[dbo].[SiteProfilesContacts]'))
ALTER TABLE [dbo].[SiteProfilesContacts]  WITH CHECK ADD  CONSTRAINT [FK_SiteProfilesContacts_Contacts] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contacts] ([ContactId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SiteProfilesContacts_Contacts]') AND parent_object_id = OBJECT_ID(N'[dbo].[SiteProfilesContacts]'))
ALTER TABLE [dbo].[SiteProfilesContacts] CHECK CONSTRAINT [FK_SiteProfilesContacts_Contacts]
GO
/****** Object:  ForeignKey [FK_SiteProfilesContacts_SiteProfiles]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SiteProfilesContacts_SiteProfiles]') AND parent_object_id = OBJECT_ID(N'[dbo].[SiteProfilesContacts]'))
ALTER TABLE [dbo].[SiteProfilesContacts]  WITH CHECK ADD  CONSTRAINT [FK_SiteProfilesContacts_SiteProfiles] FOREIGN KEY([SiteProfileId])
REFERENCES [dbo].[SiteProfiles] ([ProfileId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SiteProfilesContacts_SiteProfiles]') AND parent_object_id = OBJECT_ID(N'[dbo].[SiteProfilesContacts]'))
ALTER TABLE [dbo].[SiteProfilesContacts] CHECK CONSTRAINT [FK_SiteProfilesContacts_SiteProfiles]
GO
/****** Object:  ForeignKey [FK_Subscriptions_PricingPlans]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Subscriptions_PricingPlans]') AND parent_object_id = OBJECT_ID(N'[dbo].[Subscriptions]'))
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_Subscriptions_PricingPlans] FOREIGN KEY([PricingPlanId])
REFERENCES [dbo].[PricingPlans] ([PricingPlanId])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Subscriptions_PricingPlans]') AND parent_object_id = OBJECT_ID(N'[dbo].[Subscriptions]'))
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_Subscriptions_PricingPlans]
GO
/****** Object:  ForeignKey [FK_Subscriptions_Users]    Script Date: 12/07/2010 09:56:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Subscriptions_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Subscriptions]'))
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_Subscriptions_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Subscriptions_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Subscriptions]'))
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_Subscriptions_Users]
GO
/****** Object:  ForeignKey [FK_UserActionEvents_Users]    Script Date: 12/07/2010 09:56:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserActionEvents_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserActionEvents]'))
ALTER TABLE [dbo].[UserActionEvents]  WITH CHECK ADD  CONSTRAINT [FK_UserActionEvents_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserActionEvents_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserActionEvents]'))
ALTER TABLE [dbo].[UserActionEvents] CHECK CONSTRAINT [FK_UserActionEvents_Users]
GO
/****** Object:  ForeignKey [FK_UsersCatalogs_Catalogs]    Script Date: 12/07/2010 09:56:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersCatalogs_Catalogs]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCatalogRoles]'))
ALTER TABLE [dbo].[UserCatalogRoles]  WITH NOCHECK ADD  CONSTRAINT [FK_UsersCatalogs_Catalogs] FOREIGN KEY([CatalogId])
REFERENCES [dbo].[Catalogs] ([CatalogId])
ON DELETE CASCADE
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersCatalogs_Catalogs]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCatalogRoles]'))
ALTER TABLE [dbo].[UserCatalogRoles] CHECK CONSTRAINT [FK_UsersCatalogs_Catalogs]
GO
/****** Object:  ForeignKey [FK_UsersCatalogs_Roles]    Script Date: 12/07/2010 09:56:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersCatalogs_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCatalogRoles]'))
ALTER TABLE [dbo].[UserCatalogRoles]  WITH NOCHECK ADD  CONSTRAINT [FK_UsersCatalogs_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersCatalogs_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCatalogRoles]'))
ALTER TABLE [dbo].[UserCatalogRoles] CHECK CONSTRAINT [FK_UsersCatalogs_Roles]
GO
/****** Object:  ForeignKey [FK_UsersCatalogs_Users]    Script Date: 12/07/2010 09:56:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersCatalogs_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCatalogRoles]'))
ALTER TABLE [dbo].[UserCatalogRoles]  WITH NOCHECK ADD  CONSTRAINT [FK_UsersCatalogs_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersCatalogs_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCatalogRoles]'))
ALTER TABLE [dbo].[UserCatalogRoles] CHECK CONSTRAINT [FK_UsersCatalogs_Users]
GO
/****** Object:  ForeignKey [FK_Users_Invitations]    Script Date: 12/07/2010 09:56:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Invitations]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [FK_Users_Invitations] FOREIGN KEY([InvitationId])
REFERENCES [dbo].[Invitations] ([InvitationId])
ON DELETE CASCADE
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Invitations]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Invitations]
GO
/****** Object:  ForeignKey [FK_Users_PlanBalances]    Script Date: 12/07/2010 09:56:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_PlanBalances]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_PlanBalances] FOREIGN KEY([PlanBalanceId])
REFERENCES [dbo].[PlanBalances] ([PlanBalanceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_PlanBalances]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_PlanBalances]
GO
/****** Object:  ForeignKey [FK_Users_PlanUsers]    Script Date: 12/07/2010 09:56:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_PlanUsers]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_PlanUsers] FOREIGN KEY([PlanUserId])
REFERENCES [dbo].[Users] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_PlanUsers]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_PlanUsers]
GO
/****** Object:  ForeignKey [FK_Users_PricingPlans]    Script Date: 12/07/2010 09:56:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_PricingPlans]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_PricingPlans] FOREIGN KEY([PricingPlanId])
REFERENCES [dbo].[PricingPlans] ([PricingPlanId])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_PricingPlans]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_PricingPlans]
GO
/****** Object:  ForeignKey [FK_Users_Roles]    Script Date: 12/07/2010 09:56:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
/****** Object:  ForeignKey [FK_Users_Users]    Script Date: 12/07/2010 09:56:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users] FOREIGN KEY([ParentUserId])
REFERENCES [dbo].[Users] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users]
GO
