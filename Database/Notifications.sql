CREATE TABLE [dbo].[Notifications]
(
	[NotificationId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NOT NULL, 
	[NotificationTitle] NVARCHAR(MAX) NOT NULL, 
    [NotificationDescription] NVARCHAR(MAX) NULL, 
	[HighLimit] FLOAT NULL, 
    [LowLimit] FLOAT NULL,
	[NotificationPriority] INT NOT NULL DEFAULT 0,  
    [AutoAcknowledge] BIT NOT NULL DEFAULT 1, 
    [IsSilent] BIT NOT NULL DEFAULT 0, 
    [AutoUnmute] BIT NOT NULL DEFAULT 1, 
    [UnmuteDate] DATETIME2 NULL, 
    CONSTRAINT [FK_Notifications_Items] FOREIGN KEY (ItemId) REFERENCES Items(ItemId), 
    
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'0 : not auto',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Notifications',
    @level2type = N'COLUMN',
    @level2name = N'AutoUnmute'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'1 , 2 , 3 , 4 , 5',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Notifications',
    @level2type = N'COLUMN',
    @level2name = N'NotificationPriority'