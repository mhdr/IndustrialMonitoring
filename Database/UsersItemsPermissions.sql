CREATE TABLE [dbo].[UsersItemsPermissions]
(
	[PermissionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [ItemId] INT NOT NULL, 
    [ReceiveDelayAlarmInTelegram] BIT NULL DEFAULT 0, 
    CONSTRAINT [FK_UsersItemsPermissions_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]), 
    CONSTRAINT [FK_UsersItemsPermissions_Items] FOREIGN KEY ([ItemId]) REFERENCES [Items]([ItemId])
)
