
CREATE TABLE [dbo].[OfferRules]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[OfferId] [int] NULL,
	[StartTime] [varchar](50) NULL,
	[EndTime] [varchar](50) NULL,
	[RestrictionType] [varchar](20) NULL,
	[RestrictionDateCode] [int] NULL,
	[LastModifyTime] [datetime] NULL,
)
GO