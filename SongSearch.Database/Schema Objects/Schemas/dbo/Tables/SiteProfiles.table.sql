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
 CONSTRAINT [PK_SiteProfiles] PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)


