CREATE TABLE [dbo].[ProductItem2Setting]
(
	[DepartmentNumber] NVARCHAR(2) NOT NULL DEFAULT 00 , 
    [Item1Number] NVARCHAR(2) NOT NULL DEFAULT 00, 
    [Item2Number] NVARCHAR(2) NOT NULL DEFAULT 00, 
    [Item2Name] NVARCHAR(20) NOT NULL DEFAULT '', 
    PRIMARY KEY ([DepartmentNumber], [Item2Number], [Item1Number])
)
