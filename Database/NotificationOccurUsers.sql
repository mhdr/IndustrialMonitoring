CREATE TABLE [dbo].[NotificationOccurUsers]
(
	[OccurUserId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OccurId] INT NOT NULL,
    [DateAcknowledged] DATETIME2 NULL, 
    [ReasonOccured] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_NotificationOccurUsers_NotificationOccur] FOREIGN KEY (OccurId) REFERENCES NotificationOccur(OccurId), 
    
)
