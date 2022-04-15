CREATE TABLE [dbo].[PickingMainSetting]
(
	[PickingFlag] INT NOT NULL, 
    [PickingNumber] NVARCHAR(12) NOT NULL, 
    [PickingDate] DATE NOT NULL DEFAULT getdate(), 
    [PickingCustomerNumber] NVARCHAR(6) NOT NULL DEFAULT '', 
    [ProjectNumber] [nvarchar](12) NULL,
    [PickingTax] INT NOT NULL DEFAULT 0, 
    [PickingInvoiceNo] NVARCHAR(10) NULL, 
    [PickingEmployeeNumber] NVARCHAR(6) NOT NULL DEFAULT '', 
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
)

GO

CREATE INDEX [IX_PickingMainSetting_ProjectNumber] ON [dbo].[PickingMainSetting] ([ProjectNumber])