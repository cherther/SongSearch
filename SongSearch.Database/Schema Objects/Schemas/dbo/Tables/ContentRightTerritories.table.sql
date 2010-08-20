CREATE TABLE [dbo].[ContentRightTerritories](
	[ContentRightId] [int] NOT NULL,
	[TerritoryId] [int] NOT NULL,
 CONSTRAINT [PK_ItemsRightsTerritories] PRIMARY KEY CLUSTERED 
(
	[ContentRightId] ASC,
	[TerritoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_ContentRightTerritories_ContentRights] FOREIGN KEY([ContentRightId])
REFERENCES [dbo].[ContentRights] ([ContentRightId])
ON DELETE CASCADE
NOT FOR REPLICATION ,
 CONSTRAINT [FK_ContentRightTerritories_Territories] FOREIGN KEY([TerritoryId])
REFERENCES [dbo].[Territories] ([TerritoryId])
ON DELETE CASCADE
)


