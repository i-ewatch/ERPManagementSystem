CREATE TABLE [dbo].[ProductCategorySetting]
(
	[CategoryNumber] NVARCHAR(6) NOT NULL , 
    [CategoryName] NCHAR(20) NOT NULL, 
    CONSTRAINT [PK_ProductCategorySetting] PRIMARY KEY ([CategoryNumber])
)