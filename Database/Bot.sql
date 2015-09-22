CREATE TABLE [dbo].[Bot]
(
	[BotProcessId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ChatId] INT NULL, 
    [UserId] INT NOT NULL, 
    [Token] NVARCHAR(MAX) NOT NULL, 
    [IsAuthorized] BIT NOT NULL DEFAULT 0, 
    [ReceiveAlarms] BIT NULL DEFAULT 1, 
    CONSTRAINT [FK_Bot_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId])
)
