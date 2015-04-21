CREATE TABLE [dbo].[RoomExtension]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [RoomID] INT NULL, 
    [FacilityName] NVARCHAR(200) NULL, 
	[FaciliTypeName] NVARCHAR(200) NULL, 
	[IsAllAvailable] int,
    [LastMofifyTime] DATETIME NULL
	
  


)
