CREATE TABLE [dbo].[TextItem]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	HotelId varchar(100),
	Category int,
	DescriptionText nvarchar(1000),
	LastModityTime datetime
)
