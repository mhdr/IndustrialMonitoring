CREATE TABLE [dbo].[TabsItems]
(
	[TabItemId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TabId] INT NOT NULL, 
    [ItemId] INT NOT NULL, 
    CONSTRAINT [FK_TabsItems_Tabs] FOREIGN KEY ([TabId]) REFERENCES [Tabs]([TabId]), 
    CONSTRAINT [FK_TabsItems_Items] FOREIGN KEY ([ItemId]) REFERENCES [Items]([ItemId])
)
