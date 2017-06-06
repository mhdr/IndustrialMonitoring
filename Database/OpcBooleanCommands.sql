CREATE TABLE [dbo].[OpcBooleanCommands]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Location] NVARCHAR(MAX) NOT NULL, 
    [CommandValue] BIT NOT NULL, 
    [Interval] INT NOT NULL
)
