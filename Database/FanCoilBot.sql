CREATE TABLE [dbo].[FanCoilBot]
(
	[BotProcessId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ChatId] INT NULL, 
    [UserId] INT NOT NULL, 
    [Token] NVARCHAR(MAX) NOT NULL, 
    [IsAuthorized] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_FanCoilBot_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId])
)
