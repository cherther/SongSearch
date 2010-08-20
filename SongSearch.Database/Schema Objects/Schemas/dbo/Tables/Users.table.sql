CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Password] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FirstName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LastName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ParentUserId] [int] NULL,
	[PlanUserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[SiteProfileId] [int] NOT NULL CONSTRAINT [DF_Users_SiteProfileId]  DEFAULT ((1)),
	[PricingPlanId] [int] NOT NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_Users_Invitations] FOREIGN KEY([InvitationId])
REFERENCES [dbo].[Invitations] ([InvitationId])
ON DELETE CASCADE
NOT FOR REPLICATION ,
 CONSTRAINT [FK_Users_PlanUsers] FOREIGN KEY([PlanUserId])
REFERENCES [dbo].[Users] ([UserId]),
 CONSTRAINT [FK_Users_PricingPlans] FOREIGN KEY([PricingPlanId])
REFERENCES [dbo].[PricingPlans] ([PricingPlanId]),
 CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
NOT FOR REPLICATION ,
 CONSTRAINT [FK_Users_Users] FOREIGN KEY([ParentUserId])
REFERENCES [dbo].[Users] ([UserId])
)


