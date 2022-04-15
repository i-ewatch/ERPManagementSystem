CREATE TABLE [dbo].[SalesMainSetting]
(
	[SalesFlag] INT NOT NULL, 
    [SalesNumber] NVARCHAR(12) NOT NULL, 
    [SalesDate] DATE NOT NULL DEFAULT getdate(), 
    [SalesCustomerNumber] NVARCHAR(6) NOT NULL DEFAULT '', 
    [ProjectNumber] [nvarchar](12) NULL,
    [SalesTax] INT NOT NULL DEFAULT 0, 
    [SalesInvoiceNo] NVARCHAR(10) NULL, 
    [SalesEmployeeNumber] NVARCHAR(6) NOT NULL DEFAULT '', 
    [Remark] NVARCHAR(250) NULL, 
    [Total] FLOAT NOT NULL DEFAULT 0, 
    [Tax] FLOAT NOT NULL DEFAULT 0, 
    [TotalTax] FLOAT NOT NULL DEFAULT 0, 
    [Posting] INT NOT NULL DEFAULT 0, 
    [FileName] NVARCHAR(50) NULL, 
    [TakeACut] INT NOT NULL DEFAULT 0, 
    [Cost] FLOAT NOT NULL DEFAULT 0, 
    [ProfitSharing] FLOAT NOT NULL DEFAULT 0, 
    [PostingDate] DATE NULL , 
    [ProfitSharingDate] DATE NULL, 
    CONSTRAINT [PK_SaleMainSetting] PRIMARY KEY ([SalesFlag], [SalesNumber]) 
)


GO

CREATE INDEX [IX_SalesMainSetting_ProjectNumber] ON [dbo].[SalesMainSetting] ([ProjectNumber])
