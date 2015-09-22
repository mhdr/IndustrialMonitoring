CREATE TABLE [dbo].[RelatedNotifications]
(
	[RelationId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NotificationIdParent] INT NOT NULL, 
    [NotificationIdChild] INT NOT NULL, 
    CONSTRAINT [FK_RelatedNotifications_NotificationItems] FOREIGN KEY ([NotificationIdParent]) REFERENCES [NotificationItems]([NotificationId]), 
    CONSTRAINT [FK_RelatedNotifications_NotificationItems2] FOREIGN KEY ([NotificationIdChild]) REFERENCES [NotificationItems]([NotificationId])
)
