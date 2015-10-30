CREATE TABLE [dbo].[UsersItemsPermissions]
(
	[PermissionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [ItemId] INT NOT NULL, 
    [ReceiveDelayedNotificationInTelegram] BIT NULL, 
    CONSTRAINT [FK_UsersItemsPermissions_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]), 
    CONSTRAINT [FK_UsersItemsPermissions_Items] FOREIGN KEY ([ItemId]) REFERENCES [Items]([ItemId])
)
