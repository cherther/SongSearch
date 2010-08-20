CREATE TABLE [dbo].[Invitations](
	[InvitationId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Invitations_InvitationID]  DEFAULT (newid()),
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


