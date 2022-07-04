CREATE TABLE [dbo].[OrderMainSetting]
(
	[OrderNumber] NVARCHAR(12) NOT NULL PRIMARY KEY, 
    [OrderDate] DATETIME NOT NULL, 
    [OrderCompanyNumber] NVARCHAR(6) NOT NULL,
    [OrderDirectoryNumber] NVARCHAR(6) NOT NULL,
    [Address]  NVARCHAR(50) NULL DEFAULT '',
    [ProjectNumber] NVARCHAR(12) NULL DEFAULT '', 
    [OrderEmployeeNumber] NVARCHAR(6) NOT NULL, 
    [Remark] NVARCHAR(250) NULL DEFAULT '',
    [Total] FLOAT NOT NULL DEFAULT 0, 
    [Tax] FLOAT NOT NULL DEFAULT 0, 
    [TotalTax] FLOAT NOT NULL DEFAULT 0, 
    [TotalQty] FLOAT NOT NULL DEFAULT 0, 
    [FileName] NVARCHAR(50) NULL, 
    [OrderNote] NVARCHAR(50) NULL, 
    [InvalidFlag] BIT NOT NULL DEFAULT 0
)
