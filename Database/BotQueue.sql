CREATE TABLE [dbo].[BotQueue]
(
	[BotQueueId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[QueueDirection] INT NOT NULL,
    [ChatId] INT NOT NULL, 
    [MessageText] NVARCHAR(MAX) NULL, 
    [IsCompleted] BIT NOT NULL DEFAULT 0, 
	[Date] DATETIME2 NOT NULL, 
)
