CREATE TABLE [dbo].[ThreadGroup]
(
	[ThreadGroupId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ThreadGroupName] NVARCHAR(MAX) NOT NULL, 
    [IntervalBetweenItems] INT NOT NULL, 
    [IntervalBetweenCycle] INT NOT NULL
)
