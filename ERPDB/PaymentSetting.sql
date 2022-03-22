CREATE TABLE [dbo].[PaymentSetting]
(
	[PaymentNumber] NVARCHAR(12) NOT NULL PRIMARY KEY, 
    [PaymentDate] DATE NOT NULL, 
    [PaymentInvoiceNo] NVARCHAR(10) NULL, 
    [PaymentItemNo] NVARCHAR(24) NOT NULL, 
    [PaymentUse] NVARCHAR(100) NOT NULL, 
    [EmployeeNumber] NVARCHAR(6) NOT NULL, 
    [PaymentAmount] FLOAT NOT NULL, 
    [PaymentMethod] INT NOT NULL, 
    [Remark] NVARCHAR(250) NULL, 
    [TransferDate] DATE NULL, 
    [FileName] NVARCHAR(50) NULL
)
