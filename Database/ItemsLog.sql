CREATE TABLE [dbo].[ItemsLog]
(
	[ItemLogId] INT NOT NULL PRIMARY KEY, 
    [ItemId] INT NOT NULL, 
    [Value] NVARCHAR(50) NOT NULL, 
    [Time] DATETIME2 NOT NULL, 
    [TimeStamp] TIMESTAMP NOT NULL, 
    CONSTRAINT [FK_ItemsLog_Items] FOREIGN KEY ([ItemId]) REFERENCES [Items]([ItemId])
)
