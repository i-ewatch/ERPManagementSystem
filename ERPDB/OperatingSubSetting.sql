CREATE TABLE [dbo].[OperatingSubSetting]
(
	[OperatingFlag] INT NOT NULL, 
    [OperatingNumber] NVARCHAR(12) NOT NULL, 
    [OperatingNo] INT NOT NULL DEFAULT 1, 
    [ProductNumber] NVARCHAR(24) NOT NULL DEFAULT '', 
    [ProductName] NVARCHAR(24) NOT NULL DEFAULT '', 
    [ProductUnit] NVARCHAR(10) NULL, 
    [ProductQty] FLOAT NOT NULL DEFAULT 0, 
    [ProductPrice] FLOAT NOT NULL DEFAULT 0, 
    [ProductTotal] FLOAT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_[OperatingSubSetting] PRIMARY KEY ([OperatingFlag], [OperatingNo], [OperatingNumber]) 
)
