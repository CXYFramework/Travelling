CREATE TABLE [dbo].[GuestRoom]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [Name] NVARCHAR(100) NULL, 
    [HotelID] varchar(100) NULL, 
    [StandardOccupancy] INT NULL, 
    [Size] NVARCHAR(100) NULL, 
    [RoomTypeCode] INT NULL, 
    [Floor] nvarchar(100) NULL, 
    [BedTypeCode] INT NULL, 
    [Quantity] INT NULL, 
    [FeatureDescription] NVARCHAR(200) NULL, 
	InvBlockCode int,
	NonSmoking int,
	RoomSize NVARCHAR(100),
	HasWindow int,
    [LastMofifyTime] DATETIME NULL
)
