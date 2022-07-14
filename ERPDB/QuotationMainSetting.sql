CREATE TABLE [dbo].[QuotationMainSetting]
(
	[QuotationNumber] NVARCHAR(12) NOT NULL PRIMARY KEY, 
    [QuotationDate] DATETIME NOT NULL, 
    [QuotationCustomerNumber] NVARCHAR(6) NOT NULL,
    [QuotationDirectoryNumber] NVARCHAR(6) NOT NULL,
    [Address]  NVARCHAR(50) NULL DEFAULT '',
    [ProjectNumber] NVARCHAR(12) NULL DEFAULT '', 
    [QuotationEmployeeNumber] NVARCHAR(6) NOT NULL, 
    [Remark] NVARCHAR(250) NULL DEFAULT '',
    [QuotationTax] INT NULL DEFAULT 0,
    [Total] FLOAT NOT NULL DEFAULT 0, 
    [Tax] FLOAT NOT NULL DEFAULT 0, 
    [TotalTax] FLOAT NOT NULL DEFAULT 0, 
    [TotalQty] FLOAT NOT NULL DEFAULT 0, 
    [FileName] NVARCHAR(50) NULL, 
    [QuotationNote] NVARCHAR(50) NULL, 
    [InvalidFlag] BIT NOT NULL DEFAULT 0
)
