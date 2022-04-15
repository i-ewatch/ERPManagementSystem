CREATE TABLE [dbo].[PickingSubSetting]
(
	[PickingFlag] INT NOT NULL, 
    [PickingNumber] NVARCHAR(12) NOT NULL, 
    [PickingNo] INT NOT NULL DEFAULT 1, 
    [ProductNumber] NVARCHAR(24) NOT NULL DEFAULT '', 
    [ProductName] NVARCHAR(24) NOT NULL DEFAULT '', 
    [ProductUnit] NVARCHAR(10) NULL, 
    [ProductQty] FLOAT NOT NULL DEFAULT 0, 
    [ProductPrice] FLOAT NOT NULL DEFAULT 0, 
    [ProductTotal] FLOAT NOT NULL DEFAULT 0, 
    [Cost] FLOAT NOT NULL DEFAULT 0, 
    [CostTotal] FLOAT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_PickingSubSetting] PRIMARY KEY ([PickingFlag], [PickingNo], [PickingNumber])
)
