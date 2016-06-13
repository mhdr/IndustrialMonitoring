CREATE TABLE [dbo].[Session]
(
	[SessionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [SessionKey] NVARCHAR(MAX) NOT NULL, 
    [IsValid] BIT NOT NULL, 
    [ValidUntil] DATETIME2 NULL, 
    CONSTRAINT [FK_Session_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId])
)

GO
