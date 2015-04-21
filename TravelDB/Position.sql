CREATE TABLE [dbo].[Position]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [HotelID] varchar(100) NULL, 
    [Latitude] DECIMAL(10, 5) NULL, 
    [Longitude] DECIMAL(10, 5) NULL, 
    [PositionTypeCode] INT NULL, 
    [LastMofifyTime] DATETIME NULL
	
)
