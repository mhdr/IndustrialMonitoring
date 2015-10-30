CREATE TABLE [dbo].[BotNotificationQueue]
(
	[BotQueueId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[NotificationId] INT NULL,
    [ChatId] INT NOT NULL,
    [MessageText] NVARCHAR(MAX) NULL, 
	[RegisterTime] DATETIME2 NOT NULL, 
	[Delay] INT NULL,
    [FinishTime] DATETIME2 NULL, 
	[IsSent] BIT NULL, 
	[IsCompleted] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_BotNotificationQueue_NotificationItems] FOREIGN KEY ([NotificationId]) REFERENCES [NotificationItems]([NotificationId]), 
)
