CREATE TABLE [dbo].[Items]
(
	[ItemId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemName] NVARCHAR(50) NOT NULL, 
    [ItemType] INT NOT NULL, 
    [Location] NVARCHAR(MAX) NOT NULL, 
    [SaveInItemsLogTimeInterval] INT NOT NULL DEFAULT 5, 
    [SaveInItemsLogLastesTimeInterval] INT NOT NULL DEFAULT 60, 
    [ShowInUITimeInterval] INT NOT NULL DEFAULT 5, 
    [ScanCycle] INT NOT NULL DEFAULT 100, 
    [SaveInItemsLogWhen] INT NOT NULL DEFAULT 1, 
    [SaveInItemsLogLastWhen] INT NOT NULL DEFAULT 1, 
    [DefinationType] INT NULL DEFAULT 1
)
