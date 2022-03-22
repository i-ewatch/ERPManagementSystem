CREATE TABLE [dbo].[CompanyDirectorySetting]
(
	[DirectoryCompany] NVARCHAR(6) NOT NULL , 
    [DirectoryNumber] NVARCHAR(6) NOT NULL, 
    [DirectoryName] NVARCHAR(10) NOT NULL, 
    [JobTitle] NVARCHAR(11) NULL, 
    [Phone] NVARCHAR(20) NULL, 
    [MobilePhone] NVARCHAR(10) NULL, 
    [Email] NVARCHAR(650) NULL, 
    [Remark] NVARCHAR(250) NULL, 
    [AttachmentFile] VARBINARY(MAX) NULL, 
    [FileName] NVARCHAR(50) NULL, 
    PRIMARY KEY ([DirectoryCompany], [DirectoryNumber])
)
