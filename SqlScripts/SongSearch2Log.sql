/****** Object:  Table [dbo].[ContentActionEvents]    Script Date: 08/15/2010 07:33:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentActionEvents]') AND type in (N'U'))
DROP TABLE [dbo].[ContentActionEvents]
GO
/****** Object:  Table [dbo].[SearchEvents]    Script Date: 08/15/2010 07:33:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchEvents]') AND type in (N'U'))
DROP TABLE [dbo].[SearchEvents]
GO
/****** Object:  Table [dbo].[UserActionEvents]    Script Date: 08/15/2010 07:33:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserActionEvents]') AND type in (N'U'))
DROP TABLE [dbo].[UserActionEvents]
GO
/****** Object:  Table [dbo].[UserActionEvents]    Script Date: 08/15/2010 07:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserActionEvents]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserActionEvents](
	[UserActionEventId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserActionEventDate] [datetime] NOT NULL,
	[SessionId] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserId] [int] NOT NULL,
	[UserActionId] [int] NOT NULL,
 CONSTRAINT [PK_UserActionEvents] PRIMARY KEY CLUSTERED 
(
	[UserActionEventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[UserActionEvents] ON
INSERT [dbo].[UserActionEvents] ([UserActionEventId], [UserActionEventDate], [SessionId], [UserId], [UserActionId]) VALUES (1, CAST(0x00009DD3007A2281 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 4)
INSERT [dbo].[UserActionEvents] ([UserActionEventId], [UserActionEventDate], [SessionId], [UserId], [UserActionId]) VALUES (2, CAST(0x00009DD3007ACC9E AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 3)
INSERT [dbo].[UserActionEvents] ([UserActionEventId], [UserActionEventDate], [SessionId], [UserId], [UserActionId]) VALUES (3, CAST(0x00009DD3007B42D9 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 2)
INSERT [dbo].[UserActionEvents] ([UserActionEventId], [UserActionEventDate], [SessionId], [UserId], [UserActionId]) VALUES (4, CAST(0x00009DD3007B5FAB AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 4)
INSERT [dbo].[UserActionEvents] ([UserActionEventId], [UserActionEventDate], [SessionId], [UserId], [UserActionId]) VALUES (5, CAST(0x00009DD3007BA93C AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 4)
INSERT [dbo].[UserActionEvents] ([UserActionEventId], [UserActionEventDate], [SessionId], [UserId], [UserActionId]) VALUES (6, CAST(0x00009DD3007BC800 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 5)
SET IDENTITY_INSERT [dbo].[UserActionEvents] OFF
/****** Object:  Table [dbo].[SearchEvents]    Script Date: 08/15/2010 07:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchEvents]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SearchEvents](
	[SearchEventId] [bigint] IDENTITY(1,1) NOT NULL,
	[SearchEventDate] [datetime] NOT NULL,
	[SessionId] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserId] [int] NOT NULL,
	[ResultCount] [int] NOT NULL,
	[QueryString] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_SearchEvents] PRIMARY KEY CLUSTERED 
(
	[SearchEventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[ContentActionEvents]    Script Date: 08/15/2010 07:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentActionEvents]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentActionEvents](
	[ContentActionEventId] [bigint] IDENTITY(1,1) NOT NULL,
	[ContentActionEventDate] [datetime] NOT NULL,
	[SessionId] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserId] [int] NOT NULL,
	[ContentActionId] [int] NOT NULL,
	[ContentId] [int] NOT NULL,
 CONSTRAINT [PK_ContentActionEvents] PRIMARY KEY CLUSTERED 
(
	[ContentActionEventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[ContentActionEvents] ON
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (1, CAST(0x00009DD3007A3647 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 14, 5165)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (2, CAST(0x00009DD3007A3647 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 14, 2160)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (3, CAST(0x00009DD3007A3E43 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 10, 27714)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (4, CAST(0x00009DD3007A4716 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 21, 27714)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (5, CAST(0x00009DD3007A4950 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 11, 27714)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (6, CAST(0x00009DD3007A5336 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 15, 286)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (7, CAST(0x00009DD3007A86A8 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 13, 27400)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (8, CAST(0x00009DD3007A86A8 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 13, 27397)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (9, CAST(0x00009DD3007A86A8 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 13, 27471)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (10, CAST(0x00009DD3007A9262 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 22, 32108)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (11, CAST(0x00009DD3007AB4E6 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 16, 286)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (12, CAST(0x00009DD3007ABFFB AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 17, 286)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (13, CAST(0x00009DD3007BBBE7 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 14, 27400)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (14, CAST(0x00009DD3007BBBE7 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 14, 27397)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (15, CAST(0x00009DD3007BBBE7 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 14, 27471)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (16, CAST(0x00009DD3007C332D AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 10, 4748)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (17, CAST(0x00009DD3007C35E8 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 10, 27130)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (18, CAST(0x00009DD3007C37D7 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 10, 2787)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (19, CAST(0x00009DD3007C3A40 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 10, 27330)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (20, CAST(0x00009DD3007C3CE5 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 13, 4748)
INSERT [dbo].[ContentActionEvents] ([ContentActionEventId], [ContentActionEventDate], [SessionId], [UserId], [ContentActionId], [ContentId]) VALUES (21, CAST(0x00009DD3007C3CE5 AS DateTime), N'x5lvl4fylp5hvatsjxr5stb4', 2, 13, 27130)
SET IDENTITY_INSERT [dbo].[ContentActionEvents] OFF
