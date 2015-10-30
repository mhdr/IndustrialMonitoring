CREATE TABLE [dbo].[NotificationItems]
(
	[NotificationId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NOT NULL, 
    [NotificationType] INT NOT NULL, 
	[Low] FLOAT NULL,
	[High] FLOAT NULL, 
	[NotificationMsg] NVARCHAR(MAX) NULL, 
    [Priority] INT NULL, 
    [NumberOfSecondsInReceivingDelayedAlarmInTelegram] INT NULL DEFAULT 120, 
    CONSTRAINT [FK_NotificationItems_Items] FOREIGN KEY (ItemId) REFERENCES Items(ItemId) 
)
