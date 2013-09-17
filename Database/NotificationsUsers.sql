CREATE TABLE [dbo].[NotificationsReceivers]
(
	[NotificationReceiverId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NotificationId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    CONSTRAINT [FK_NotificationsReceivers_Notifications] FOREIGN KEY (NotificationId) REFERENCES Notifications(NotificationId), 
    CONSTRAINT [FK_NotificationsReceivers_Users] FOREIGN KEY (UserId) REFERENCES Users(UserId)
)
