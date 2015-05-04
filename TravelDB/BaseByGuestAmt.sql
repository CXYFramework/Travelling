CREATE TABLE [dbo].[BaseByGuestAmt]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [RateId] INT NULL, 
    [AmountBeforeTax] DECIMAL(18, 4) NULL, 
    [CurrencyCode] NVARCHAR(10) NULL, 
    [NumberOfGuests] INT NULL, 
    [ListPrice] DECIMAL(18, 4) NULL, 
   
	OtherCurrency decimal(18,4),
	OtherCurrencyCode NVARCHAR(10) NULL, 
	 [LastModifyTime] DATETIME NULL
   
)
