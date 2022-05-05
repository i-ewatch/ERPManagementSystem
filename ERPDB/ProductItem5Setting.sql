CREATE TABLE [dbo].[ProductItem5Setting]
(
	[DepartmentNumber] NVARCHAR(2) NOT NULL DEFAULT 00 , 
    [Item1Number] NVARCHAR(2) NOT NULL DEFAULT 00, 
    [Item2Number] NVARCHAR(2) NOT NULL DEFAULT 00, 
    [Item3Number] NVARCHAR(2) NOT NULL DEFAULT 00, 
    [Item4Number] NVARCHAR(2) NOT NULL DEFAULT 00, 
    [Item5Number] NVARCHAR(4) NOT NULL DEFAULT 00, 
    [Item5Name] NVARCHAR(20) NOT NULL DEFAULT '', 
    PRIMARY KEY ([DepartmentNumber], [Item5Number], [Item1Number], [Item2Number], [Item3Number], [Item4Number])
)
