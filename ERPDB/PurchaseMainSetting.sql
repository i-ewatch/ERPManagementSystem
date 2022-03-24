CREATE TABLE [dbo].[PurchaseMainSetting]
(
	[PurchaseFlag] INT NOT NULL , 
    [PurchaseNumber] NVARCHAR(12) NOT NULL, 
    [PurchaseDate] DATE NOT NULL DEFAULT getdate(), 
    [PurchaseCompanyNumber] NVARCHAR(6) NOT NULL, 
    [PurchaseTax] INT NOT NULL DEFAULT 0, 
    [PurchaseInvoiceNo] NVARCHAR(10) NULL, 
    [PurchaseEmployeeNumber] NVARCHAR(6) NOT NULL, 
    [Remark] NVARCHAR(250) NULL, 
    [Total] FLOAT NOT NULL DEFAULT 0, 
    [Tax] FLOAT NOT NULL DEFAULT 0, 
    [TotalTax] FLOAT NOT NULL DEFAULT 0, 
    [Posting] INT NOT NULL DEFAULT 0, 
    [FileName] NVARCHAR(50) NULL, 
    [PostingDate] DATE NULL , 
    CONSTRAINT [PK_PurchaseMainSetting] PRIMARY KEY ([PurchaseFlag],[PurchaseNumber])
)
