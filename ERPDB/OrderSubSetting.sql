CREATE TABLE [dbo].[OrderSubSetting]
(
	[OrderNumber] NVARCHAR(12) NOT NULL , 
    [OrderNo] INT NOT NULL, 
    [ProductName] NVARCHAR(100) NOT NULL DEFAULT '', 
    [ProductUnit] NVARCHAR(10) NULL, 
    [ProductQty] FLOAT NOT NULL DEFAULT 0, 
    [ProductPrice] FLOAT NOT NULL DEFAULT 0, 
    [ProductTotal] FLOAT NOT NULL DEFAULT 0,
    PRIMARY KEY ([OrderNumber], [OrderNo])
)
