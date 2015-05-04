CREATE TABLE [dbo].[Offer](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[ratePlanId] [int] NULL,
	[OfferCode] [int] NULL,
	[OfferDescription] [nvarchar](500) NULL,
	[NightsRequired] [int] NULL,
	[NightsDiscounted] [int] NULL,
	[DiscountPattern] [varchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
)
GO