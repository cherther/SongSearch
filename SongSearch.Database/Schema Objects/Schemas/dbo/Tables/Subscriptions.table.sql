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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_Subscriptions_PricingPlans] FOREIGN KEY([PricingPlanId])
REFERENCES [dbo].[PricingPlans] ([PricingPlanId]),
 CONSTRAINT [FK_Subscriptions_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
)


