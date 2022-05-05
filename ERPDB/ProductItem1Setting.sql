CREATE TABLE [dbo].[ProductItem1Setting]
(
	[DepartmentNumber] NVARCHAR(2) NOT NULL DEFAULT 00 , 
    [Item1Number] NVARCHAR(2) NOT NULL DEFAULT 00, 
    [Item1Name] NVARCHAR(20) NOT NULL DEFAULT '', 
    PRIMARY KEY ([DepartmentNumber], [Item1Number])
)
