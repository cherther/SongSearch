TRUNCATE TABLE [dbo].[ContentMediaStatus]
;

INSERT INTO [SongSearch2].[dbo].[ContentMediaStatus]
           ([ContentId]
           ,[MediaVersion]
           ,[MediaDate]
           ,[IsRemote]
           ,[MediaType]
           ,[MediaBitRate]
           ,[MediaSize]
           ,[MediaLength])
SELECT
	[ContentId], 1, ISNULL([MediaDate], GETDATE()), IsMediaOnRemoteServer, [MediaType],[MediaBitRate],[MediaSize],[MediaLength]
FROM dbo.Contents
WHERE HasMediaPreviewVersion = 1
;
INSERT INTO [SongSearch2].[dbo].[ContentMediaStatus]
           ([ContentId]
           ,[MediaVersion]
           ,[MediaDate]
           ,[IsRemote]
           ,[MediaType]
           ,[MediaBitRate]
           ,[MediaSize]
           ,[MediaLength])
SELECT
	[ContentId], 2, ISNULL([MediaDate], GETDATE()), IsMediaOnRemoteServer, [MediaType],[MediaBitRate],[MediaSize],[MediaLength]
FROM dbo.Contents
WHERE HasMediaFullVersion = 1