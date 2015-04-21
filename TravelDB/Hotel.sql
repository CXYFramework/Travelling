CREATE TABLE [dbo].[Hotel]
(
	[Id] varchar(100) NOT NULL  PRIMARY KEY, 
    [HotelCode] INT NULL, 
    [BrandCode] INT NULL, 
    [HotelCityCode] INT NULL, 
    [HotelName] NVARCHAR(50) NULL, 
    [AreaID] INT NULL, 
    [WhenBuilt] DATETIME NULL, 
    [LastUpdated] DATETIME NULL, 
    [LastMofifyTime] DATETIME NULL, 
    [Timestamp] TIMESTAMP NULL
)
