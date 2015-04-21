CREATE TABLE [dbo].[GuaranteePolicy]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
	RateId int,
    [GuaranteeCode] INT NULL, 
    [HoldTime] DATETIME NULL, 
    [LastModifyTime] DATETIME NULL
  

)
