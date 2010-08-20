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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_ContentRights_Contents] FOREIGN KEY([ContentId])
REFERENCES [dbo].[Contents] ([ContentId])
ON DELETE CASCADE
NOT FOR REPLICATION 
)


