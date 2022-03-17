CREATE TABLE [dbo].[SalesMainSetting]
(
	[SalesFlag] INT NOT NULL, 
    [SalesNumber] NVARCHAR(12) NOT NULL, 
    [SalesDate] DATE NOT NULL DEFAULT getdate(), 
    [SalesCustomerNumber] NVARCHAR(6) NOT NULL DEFAULT '', 
    [SalesTax] INT NOT NULL DEFAULT 0, 
    [SalesInvoiceNo] NVARCHAR(10) NULL, 
    [SalesEmployeeNumber] NVARCHAR(6) NOT NULL DEFAULT '', 
    [Remark] NVARCHAR(100) NULL, 
    [Total] FLOAT NOT NULL DEFAULT 0, 
    [Tax] FLOAT NOT NULL DEFAULT 0, 
    [TotalTax] FLOAT NOT NULL DEFAULT 0, 
    [Posting] INT NOT NULL DEFAULT 0, 
    [FileName] NVARCHAR(50) NULL, 
    CONSTRAINT [PK_SaleMainSetting] PRIMARY KEY ([SalesFlag], [SalesNumber]) 
)
