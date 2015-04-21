CREATE TABLE [dbo].[RateExtensions]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
	RateId int ,
    [StartPeriod] DATETIME NULL, 
    [EndPeriod] DATETIME NULL, 
    [ProgramName] NVARCHAR(100) NULL, 
    [Amount] DECIMAL(18, 4) NULL, 
    [CurrencyCode] NVARCHAR(10) NULL, 
    [Code] INT NULL, 
    [DescriptionText] NVARCHAR(500) NULL, 
    [LastModifyTime] DATETIME NULL
  
)
