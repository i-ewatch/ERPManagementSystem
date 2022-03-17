CREATE TABLE [dbo].[EmployeeSetting]
(
	[EmployeeNumber] NVARCHAR(6) NOT NULL PRIMARY KEY, 
    [EmployeeName] NVARCHAR(10) NOT NULL, 
    [Phone] NVARCHAR(11) NOT NULL, 
    [Address] NVARCHAR(50) NOT NULL,
    [Token] INT NOT NULL DEFAULT 0,
    [AccountNo] NVARCHAR(20) NOT NULL DEFAULT "", 
    [PassWord] NVARCHAR(20) NOT NULL DEFAULT ""
)