CREATE TABLE [dbo].[NotificationBot]
(
	[NotificationBotId] INT NOT NULL PRIMARY KEY, 
	[NotificationId] INT NOT NULL, 
    [NotificationLogId] INT NOT NULL, 
    [RegisterTime] DATETIME2 NOT NULL, 
	[Delay] INT NOT NULL DEFAULT 0, 
	[SentTime] DATETIME2 NULL, 
	[IsSent] BIT NOT NULL DEFAULT 0, 
	[IsCompleted] BIT NOT NULL DEFAULT 0, 
    [TimeStamp] TIMESTAMP NOT NULL, 
    CONSTRAINT [FK_NotificationBot_NotificationItemsLog] FOREIGN KEY ([NotificationLogId]) REFERENCES [NotificationItemsLog]([NotificationLogId]), 
    CONSTRAINT [FK_NotificationBot_NotificationItems] FOREIGN KEY ([NotificationId]) REFERENCES [NotificationItems]([NotificationId])
)
