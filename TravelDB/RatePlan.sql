CREATE TABLE [dbo].[RatePlan]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [RatePlanCode] INT NULL, 
    [RatePlanCategory] NCHAR(10) NULL, 
    [IsCommissionable] BIT NULL, 
    [RateReturn] BIT NULL, 
    [MarketCode] NCHAR(10) NULL, 
    [LastModifyTime] DATETIME NULL

)
