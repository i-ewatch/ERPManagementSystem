CREATE TABLE [dbo].[ProjectEmployeeSetting]
(
	[ProjectNumber] NVARCHAR(12) NOT NULL , 
    [EmployeeNumber] NVARCHAR(6) NOT NULL, 
    [BonusRatio] FLOAT NOT NULL DEFAULT 0, 
    PRIMARY KEY ([ProjectNumber], [EmployeeNumber])
)
