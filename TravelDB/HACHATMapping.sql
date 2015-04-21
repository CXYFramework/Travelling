CREATE TABLE [dbo].[HACHATMapping]
(
	[Id] INT NOT NULL identity(1,1) PRIMARY KEY, 
    [HACID] INT NULL, 
    [HATID] INT NULL, 
	HotelID varchar(100) ,
    [LastModifyTine] DATETIME NULL
  

)
