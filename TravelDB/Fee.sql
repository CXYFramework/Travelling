CREATE TABLE [dbo].[Fee]
(
	[Id] INT NOT NULL primary key identity(1,1), 
    [RateId] INT NOT NULL, 
   
    [Code] INT NULL, 
    [Amount] DECIMAL(18, 4) NULL, 
    [CurrencyCode] NVARCHAR(10) NULL, 
    [ChargeUnit] INT NULL, 
    [DescriptionText] NVARCHAR(100) NULL, 
    [LastModifyTime] DATETIME NULL
)
