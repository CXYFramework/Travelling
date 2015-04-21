CREATE TABLE [dbo].[HotelAwardMapping]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [AwardID] INT NULL, 
    [HotelID] varchar(100) NULL, 
    [LastMofifyTime] DATETIME NULL, 
	Rating decimal(10,4)
  

)
