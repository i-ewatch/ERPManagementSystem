using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class ProjectSetting
    {
        /// <summary>
        /// 專案代碼
        /// </summary>
        public string ProjectNumber { get; set; }
        /// <summary>
        /// 專案名稱
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 附件檔名
        /// </summary>
        public string FileName { get; set; } = null;
        /// <summary>
        /// 專案總收入
        /// </summary>
        public double ProjectIncome { get; set; }
        /// <summary>
        /// 專案總成本
        /// </summary>
        public double ProjectCost { get; set; }
        /// <summary>
        /// 專案穫利
        /// </summary>
        public double ProjectProfit { get; set; }
        /// <summary>
        /// 專案獎金提成
        /// </summary>
        public double ProjectBonusCommission { get; set; }
        /// <summary>
        /// 過帳日期
        /// </summary>
        public DateTime? PostingDate { get; set; }
        /// <summary>
        /// 分潤日期
        /// </summary>
        public DateTime? ProfitSharingDate { get; set; }
        public List<ProjectEmployeeSetting> ProjectEmployee { get; set; }
    }
}
