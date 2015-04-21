CREATE TABLE [dbo].[CancelPenalty]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [RateId] INT NULL, 
    [Start] DATETIME NULL, 
    [End] DATETIME NULL, 
    [AmountPercent] DECIMAL(18, 4) NULL, 
    [CurrencyCode] NVARCHAR(10) NULL, 
    [OtherCurrencyAmount] DECIMAL(18, 4) NULL, 
    [OtherCurrencyCode] NVARCHAR(10) NULL, 
    [LastModifyTime] DATETIME NULL
)
