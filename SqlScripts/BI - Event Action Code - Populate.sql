truncate table dbo.dim_EventActions
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (-1, '(N/A)', '(N/A)', '(N/A)');

insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (1, 'Register', 'User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (2, 'Login', 'User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (3, 'Logout', 'User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (4, 'Session Started', 'User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (5, 'Update Profile', 'User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (6, 'Change Password', 'User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (7, 'Reset Password', 'User', 'User');
		
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (30, 'Sent Invite', 'Admin - User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (31, 'View User Detail', 'Admin - User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (32, 'Update User Role', 'Admin - User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (33, 'Toggle System Admin Access', 'Admin - User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (34, 'Update User Catalog Role', 'Admin - User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (35, 'Update All Catalogs', 'Admin - User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (36, 'Update All Users', 'Admin - Catalog', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (37, 'Delete User', 'Admin - User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (38, 'Take Ownership', 'Admin - User', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (39, 'View Catalog Detail', 'Admin - Catalog', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (40, 'Delete Catalog', 'Admin - Catalog', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (41, 'Upload Catalog', 'Admin - Catalog', 'User');

insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (50, 'View Cart', 'Cart', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (51, 'Compress Cart', 'Cart', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (52, 'Download Cart', 'Cart', 'User');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (53, 'Delete Cart', 'Cart', 'User');

insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (10, 'View Item Detail', 'View', 'Content');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (11, 'Print Item Detail', 'Print', 'Content');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (12, 'Print Item List', 'Print', 'Content');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (13, 'Add To Cart', 'Cart', 'Content');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (14, 'Remove From Cart', 'Cart', 'Content');

insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (20, 'Create New Content', 'Admin', 'Content');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (21, 'Update Content', 'Admin', 'Content');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (22, 'Delete Content', 'Admin', 'Content');

insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (100, 'Search', 'Result', 'Search');
insert into dbo.dim_EventActions (EventActionId, EventActionName, EventActionType, EventActionGroup) values (101, 'Print List', 'Result', 'Search');
