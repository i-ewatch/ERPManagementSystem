CREATE TABLE [dbo].[CompanySetting]
(
	[CompanyNumber] NVARCHAR(6) NOT NULL , 
    [CompanyName] NVARCHAR(50) NOT NULL, 
    [CompanyShortName] NVARCHAR(10) NULL, 
    [UniformNumbers] NVARCHAR(8) NOT NULL, 
    [Phone] NVARCHAR(20) NOT NULL,
    [Fax] NVARCHAR(11) NOT NULL,
    [RemittanceAccount] NVARCHAR(50) NOT NULL, 
    [ContactName] NVARCHAR(10) NOT NULL,
    [ContactEmail] NVARCHAR(50) NOT NULL,
    [ContactPhone] NVARCHAR(11) NOT NULL,
    [CheckoutType] INT NOT NULL,
    [Remark] NVARCHAR(50) NULL DEFAULT '',
    [AttachmentFile] VARBINARY(MAX) NULL, 
    [FileName] NVARCHAR(50) NULL, 
    PRIMARY KEY ([CompanyNumber]),
)