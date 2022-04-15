CREATE TABLE [dbo].[ProjectSetting]
(
	[ProjectNumber] NVARCHAR(12) NOT NULL PRIMARY KEY, 
    [ProjectName] NVARCHAR(20) NULL, 
    [Remark] NVARCHAR(100) NULL, 
    [FileName] NVARCHAR(50) NULL, 
    [ProjectIncome] FLOAT NULL, 
    [ProjectCost] FLOAT NULL, 
    [ProjectProfit] FLOAT NULL, 
    [ProjectBonusCommission] FLOAT NULL, 
    [PostingDate] DATE NULL, 
    [ProfitSharingDate] DATE NULL
)
