CREATE TABLE [dbo].[SiteProfilesContacts](
	[SiteProfileId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
 CONSTRAINT [PK_SiteProfilesContacts] PRIMARY KEY CLUSTERED 
(
	[SiteProfileId] ASC,
	[ContactId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_SiteProfilesContacts_Contacts] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contacts] ([ContactId])
ON DELETE CASCADE,
 CONSTRAINT [FK_SiteProfilesContacts_SiteProfiles] FOREIGN KEY([SiteProfileId])
REFERENCES [dbo].[SiteProfiles] ([ProfileId])
ON DELETE CASCADE
)


