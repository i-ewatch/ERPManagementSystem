CREATE TABLE [dbo].[CustomerSetting]
(
	[CustomerNumber] NVARCHAR(6) NOT NULL , 
    [CustomerName] NVARCHAR(50) NOT NULL, 
    [UniformNumbers] NVARCHAR(8) NOT NULL, 
    [Phone] NVARCHAR(20) NOT NULL, 
    [Fax] NVARCHAR(11) NOT NULL, 
    [RemittanceAccount] NVARCHAR(50) NOT NULL, 
    [ContactName] NVARCHAR(10) NOT NULL, 
    [ContactEmail] NVARCHAR(50) NOT NULL, 
    [ContactPhone] NVARCHAR(11) NOT NULL, 
    [CheckoutType] INT NOT NULL, 
    [AttachmentFile] VARBINARY(MAX) NULL, 
    [FileExtension] NVARCHAR(5) NULL, 
    PRIMARY KEY ([CustomerNumber])
)