CREATE TABLE [dbo].[ZoneHotelMapping]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [ZoneID] INT NULL, 
    [HotelID] varchar(100) NULL, 
    [LastMofifyTime] DATETIME NULL
 
)
