CREATE TABLE [dbo].[ProductSetting]
(
	[ProductCompanyNumber] NVARCHAR(6) NOT NULL , 
    [ProductNumber] NVARCHAR(24) NOT NULL, 
    [ProductName] NVARCHAR(24) NOT NULL, 
    [ProductModel] NVARCHAR(24) NOT NULL, 
    [ProductType] INT NOT NULL, 
    [ProductCategory] NVARCHAR(6) NULL, 
    [FootPrint] NVARCHAR(50) NOT NULL, 
    [Remark] NVARCHAR(50) NULL, 
    [Explanation] NVARCHAR(50) NULL, 
    [AttachmentFile] VARBINARY(MAX) NULL, 
    [FileExtension] NVARCHAR(5) NULL, 
    PRIMARY KEY ([ProductNumber])
)
