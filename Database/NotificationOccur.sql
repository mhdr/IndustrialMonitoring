CREATE TABLE [dbo].[NotificationOccur]
(
	[OccurId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NotificationId] INT NOT NULL, 
    [DateOccured] DATETIME2 NOT NULL,  
	[IsDismissed] BIT NOT NULL DEFAULT 0,
	[DateDismissed] DATETIME2 NULL, 
    [IsAcknowledged] BIT NOT NULL DEFAULT 0,
	[DateAcknowledged] DATETIME2 NULL, 
    [UserIdAcknowledged] INT NULL, 
    CONSTRAINT [FK_NotificationOccur_Users] FOREIGN KEY (UserIdAcknowledged) REFERENCES Users(UserId),
)
