CREATE TABLE [dbo].[UsersServicesPermissions]
(
	[UserServicePermissionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [ServiceId] INT NOT NULL, 
    CONSTRAINT [FK_UsersServicesPermissions_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]), 
    CONSTRAINT [FK_UsersServicesPermissions_Services] FOREIGN KEY ([ServiceId]) REFERENCES [Services]([ServiceId])
)
