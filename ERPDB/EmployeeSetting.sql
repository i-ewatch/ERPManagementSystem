CREATE TABLE [dbo].[EmployeeSetting]
(
	[EmployeeNumber] NVARCHAR(6) NOT NULL PRIMARY KEY, 
    [EmployeeName] NVARCHAR(10) NULL, 
    [Phone] NVARCHAR(11) NULL, 
    [Address] NVARCHAR(50) NULL, 
    [Token] INT NOT NULL DEFAULT 0
)