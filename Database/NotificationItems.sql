CREATE TABLE [dbo].[NotificationItems]
(
	[NotificationId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NOT NULL, 
    [Location] NVARCHAR(MAX) NOT NULL,
	[NotificationMsg] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_NotificationItems_Items] FOREIGN KEY (ItemId) REFERENCES Items(ItemId) 
)
