CREATE TABLE [dbo].[ProductItem3Setting]
(
	[DepartmentNumber] NVARCHAR(2) NOT NULL DEFAULT 00 , 
    [Item1Number] NVARCHAR(2) NOT NULL DEFAULT 00, 
    [Item2Number] NVARCHAR(2) NOT NULL DEFAULT 00, 
    [Item3Number] NVARCHAR(2) NOT NULL DEFAULT 00, 
    [Item3Name] NVARCHAR(20) NOT NULL DEFAULT '', 
    PRIMARY KEY ([DepartmentNumber], [Item3Number], [Item1Number], [Item2Number])
)
