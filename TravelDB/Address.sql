CREATE TABLE [dbo].[Address]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [HotelID] varchar(100) NULL, 
    [AddressLine] NVARCHAR(200) NULL, 
    [CityName] NVARCHAR(50) NULL, 
    [PostalCode] VARCHAR(20) NULL, 
    [LastModifyTime] DATETIME NULL
)
