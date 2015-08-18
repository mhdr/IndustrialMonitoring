CREATE TABLE [dbo].[UsersServicesPermissions]
(
	[UserServicePermissionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [ServiceId] INT NOT NULL
)
