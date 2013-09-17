CREATE TABLE [dbo].[Notifications]
(
	[NotificationId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NOT NULL, 
	[NotificationTitle] NVARCHAR(MAX) NOT NULL, 
    [NotificationDescription] NVARCHAR(MAX) NULL, 
	[HighLimit] FLOAT NULL, 
    [LowLimit] FLOAT NULL, 
    [AutoAcknowledge] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_Notifications_Items] FOREIGN KEY (ItemId) REFERENCES Items(ItemId), 
    
)
