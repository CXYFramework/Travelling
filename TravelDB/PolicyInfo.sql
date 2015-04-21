CREATE TABLE [dbo].[PolicyInfo]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [HotelId] varchar(100) NULL, 
    [CheckIn] NVARCHAR(200) NULL, 
    [CheckOut] NVARCHAR(200) NULL, 
    [LastModifyTime] DATETIME NULL
  

)
