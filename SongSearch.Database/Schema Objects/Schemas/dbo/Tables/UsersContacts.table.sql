CREATE TABLE [dbo].[UsersContacts](
	[UserId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
 CONSTRAINT [PK_UsersContacts] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ContactId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_UsersContacts_Contacts] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contacts] ([ContactId])
ON DELETE CASCADE,
 CONSTRAINT [FK_UsersContacts_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
)


