CREATE TABLE [dbo].[NotificationBot]
(
	[NotificationBotId] INT NOT NULL PRIMARY KEY, 
    [NotificationLogId] INT NOT NULL, 
    [SentTime] DATETIME2 NOT NULL, 
    [IsDelayed] BIT NOT NULL DEFAULT 0, 
    [TimeStamp] TIMESTAMP NOT NULL, 
    CONSTRAINT [FK_NotificationBot_NotificationItemsLog] FOREIGN KEY ([NotificationLogId]) REFERENCES [NotificationItemsLog]([NotificationLogId])
)
