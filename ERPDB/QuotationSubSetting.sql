CREATE TABLE [dbo].[QuotationSubSetting]
(
	[QuotationNumber] NVARCHAR(12) NOT NULL , 
    [QuotationNo] INT NOT NULL, 
    [QuotationSubNo] INT NOT NULL DEFAULT 0,
    [QuotationThrNo] INT NOT NULL DEFAULT 0,
    [LineFlag] INT NOT NULL DEFAULT 0,
    [ProductName] NVARCHAR(100) NOT NULL DEFAULT '', 
    [ProductUnit] NVARCHAR(10) NULL, 
    [ProductQty] FLOAT NOT NULL DEFAULT 0, 
    [ProductPrice] FLOAT NOT NULL DEFAULT 0, 
    [ProductTotal] FLOAT NOT NULL DEFAULT 0,
    PRIMARY KEY ([QuotationNumber], [QuotationNo], [QuotationSubNo], [QuotationThrNo])
)
