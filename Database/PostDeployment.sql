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

INSERT INTO Users(FirstName,LastName,UserName,Password) VALUES('Mahmood','Ramzani','mahmood','12345');
INSERT INTO Users(FirstName,LastName,UserName,Password) VALUES('Mohammad','Sarparast','mohammad','12345');
INSERT INTO Users(FirstName,LastName,UserName,Password) VALUES('Navid','Navaee','navid','12345');

INSERT INTO Tabs (Tabs.TabName) VALUES ('Tab1');
INSERT INTO Tabs (Tabs.TabName) VALUES ('Tab2');
INSERT INTO Tabs (Tabs.TabName) VALUES ('Tab3');

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Random1',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Random1',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(1,1);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,1);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(2,1);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(3,1);

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Random2',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Random2',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(2,1);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,2);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(2,2);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(3,2);

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Random3',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Random3',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(3,1);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,3);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(3,3);


INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Random4',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Random4',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(4,1);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,4);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(3,4);

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Sine1',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Sine1',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(5,2);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,5);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(3,5);

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Sine2',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Sine2',5,2,4,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(6,1);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,6);

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Sine3',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Sine3',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(7,3);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,7);

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Sine4',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Sine4',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(8,3);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,8);

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Ramp1',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Ramp1',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(9,1);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,9);

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Ramp2',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Ramp2',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(10,1);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,10);

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Ramp3',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Ramp3',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(11,2);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,11);

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('Ramp4',2,'\\localhost\Simulation\OPC\Simulation Examples\Functions\Ramp4',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(12,1);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,12);

INSERT INTO Items(ItemName,ItemType,Location,SaveInItemsLogTimeInterval,SaveInItemsLogLastesTimeInterval,ShowInUITimeInterval,ScanCycle,SaveInItemsLogWhen,SaveInItemsLogLastWhen)
VALUES('User3',1,'\\localhost\Simulation\OPC\Simulation Examples\Functions\User3',5,1,1,1000,1,1);
INSERT INTO TabsItems(ItemId,TabId) VALUES(13,2);
INSERT INTO UsersItemsPermissions(UserId,ItemId) VALUES(1,13);

INSERT INTO NotificationItems(ItemId,NotificationType,Low,NotificationMsg) 
VALUES(9,3,30,'Temperature is low');

INSERT INTO NotificationItems(ItemId,NotificationType,Low,NotificationMsg) 
VALUES(12,3,300,'Temperature is low');

INSERT INTO NotificationItems(ItemId,NotificationType,High,NotificationMsg) 
VALUES(12,1,800,'Temperature is high');

INSERT INTO NotificationItems(ItemId,NotificationType,Low,High,NotificationMsg) 
VALUES(2,2,300,800,'Temperature is out of range');