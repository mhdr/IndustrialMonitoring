/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Random1',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Random1',5,2,4,1000,1,1);


INSERT INTO Tabs (Tabs.TabName) VALUES ('TabTasisat');