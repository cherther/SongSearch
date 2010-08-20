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


