CREATE TABLE [dbo].[SalesSubSetting]
(
	[SalesFlag] INT NOT NULL, 
    [SalesNumber] NVARCHAR(12) NOT NULL, 
    [PurchaseNo] INT NOT NULL DEFAULT 1, 
    [ProductNumber] NVARCHAR(24) NOT NULL DEFAULT '', 
    [ProductName] NVARCHAR(24) NOT NULL DEFAULT '', 
    [ProductUnit] NVARCHAR(10) NULL, 
    [ProductQty] FLOAT NOT NULL DEFAULT 0, 
    [ProductPrice] FLOAT NOT NULL DEFAULT 0, 
    [ProductTotal] FLOAT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_SalesSubSetting] PRIMARY KEY ([SalesFlag], [PurchaseNo], [SalesNumber]) 
)
