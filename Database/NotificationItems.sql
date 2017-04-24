CREATE TABLE [dbo].[NotificationItems]
(
	[NotificationId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NOT NULL, 
    [NotificationType] INT NOT NULL, 
	[Low] FLOAT NULL,
	[High] FLOAT NULL, 
	[NotificationMsg] NVARCHAR(MAX) NULL, 
    [Priority] INT NULL, 
    [DelayForSendingNotificationInTelegram] INT NULL, 
    [DisableSendingNotificationInTelegram] BIT NULL DEFAULT 0, 
    [DisableNotification] BIT NOT NULL DEFAULT 0, 
    [SendAsEvent] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_NotificationItems_Items] FOREIGN KEY (ItemId) REFERENCES Items(ItemId) 
)
