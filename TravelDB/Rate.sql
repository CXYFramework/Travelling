CREATE TABLE [dbo].[Rate]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [RatePlanId] INT NULL, 
    [NumberOfUnits] INT NULL, 
    [Start] DATETIME NULL, 
    [End] DATETIME NULL, 
    [IsInstantConfirm] BIT NULL, 
    [Status] VARCHAR(50) NULL, 
	IsBreakfast bit,
	BreakfastNumber int,
    [LastModifyTime] DATETIME NULL

)
