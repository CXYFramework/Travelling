CREATE TABLE [dbo].[RMARoomMapping]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [RoomID] INT NULL, 
    [RMAID] INT NULL, 
    [LastMofifyTime] DATETIME NULL
  
)
