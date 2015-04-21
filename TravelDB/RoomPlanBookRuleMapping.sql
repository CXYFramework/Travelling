CREATE TABLE [dbo].[RoomPlanBookRuleMapping]
(
	[Id] INT NOT NULL PRIMARY KEY  identity(1,1),  
    [BookRuleId] INT NULL, 
    [RatePlanId] INT NULL, 
    [Parameters] NVARCHAR(200) NULL, 
    [LastModifyTime] DATETIME NULL
  

)
