CREATE TABLE [dbo].[OperatingMainSetting]
(
	[OperatingFlag] INT NOT NULL , 
    [OperatingNumber] NVARCHAR(12) NOT NULL, 
    [OperatingDate] DATE NOT NULL DEFAULT getdate(), 
    [OperatingCompanyNumber] NVARCHAR(6) NOT NULL, 
    [ProjectNumber] [nvarchar](12) NULL,
    [OperatingTax] INT NOT NULL DEFAULT 0, 
    [OperatingInvoiceNo] NVARCHAR(10) NULL, 
    [OperatingEmployeeNumber] NVARCHAR(6) NOT NULL, 
    [Remark] NVARCHAR(250) NULL, 
    [Total] FLOAT NOT NULL DEFAULT 0, 
    [Tax] FLOAT NOT NULL DEFAULT 0, 
    [TotalTax] FLOAT NOT NULL DEFAULT 0, 
    [Posting] INT NOT NULL DEFAULT 0, 
    [FileName] NVARCHAR(50) NULL, 
    [PostingDate] DATE NULL , 
    CONSTRAINT [PK_OperatingMainSetting] PRIMARY KEY ([OperatingFlag],[OperatingNumber])
)
