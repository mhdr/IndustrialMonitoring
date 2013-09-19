CREATE TABLE [dbo].[NotificationsSilenceHistory]
(
	[SilenceId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NotificationId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
	[SilenceStatus] INT NOT NULL, 
    [ActionDate] DATETIME2 NOT NULL, 
    CONSTRAINT [FK_NotificationsSilenceHistory_Notifications] FOREIGN KEY (NotificationId) REFERENCES Notifications(NotificationId), 
    CONSTRAINT [FK_NotificationsSilenceHistory_Users] FOREIGN KEY (UserId) REFERENCES Users(UserId),
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'0 , 1',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NotificationsSilenceHistory',
    @level2type = N'COLUMN',
    @level2name = N'SilenceStatus'