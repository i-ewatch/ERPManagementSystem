using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class SalesSetting
    {
        /// <summary>
        /// 銷貨旗標 
        /// </summary>
        [Range(3, 4, ErrorMessage = "3=銷貨，4=銷貨退回")]
        public int SalesFlag { get; set; }
        /// <summary>
        /// 銷貨單號
        /// </summary>
        public string SalesNumber { get; set; }
        /// <summary>
        /// 銷貨日期
        /// </summary>
        public System.DateTime SalesDate { get; set; }
        /// <summary>
        /// 客戶編號
        /// </summary>
        public string SalesCustomerNumber { get; set; }
        /// <summary>
        /// 專案代碼
        /// </summary>
        [StringLength(12, ErrorMessage = "字串最多12個字")]
        public string ProjectNumber { get; set; }
        /// <summary>
        /// 稅別
        /// </summary>
        [Range(0, 4, ErrorMessage = "0.應稅，1.零稅，2.免稅，3.二聯")]
        public int SalesTax { get; set; }
        /// <summary>
        /// 發票號碼
        /// </summary>
        public string SalesInvoiceNo { get; set; }
        /// <summary>
        /// 員工號碼
        /// </summary>
        public string SalesEmployeeNumber { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 合計
        /// </summary>
        public double Total { get; set; }
        /// <summary>
        /// 稅金
        /// </summary>
        public double Tax { get; set; }
        /// <summary>
        /// 稅後總計
        /// </summary>
        public double TotalTax { get; set; }
        /// <summary>
        /// 過帳
        /// </summary>
        public int Posting { get; set; }
        /// <summary>
        /// 附件檔名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 抽成
        /// </summary>
        public int TakeACut { get; set; }
        /// <summary>
        /// 成本
        /// </summary>
        public double Cost { get; set; }
        /// <summary>
        /// 分潤
        /// </summary>
        public double ProfitSharing { get; set; }
        /// <summary>
        /// 過帳日期
        /// </summary>
        public DateTime? PostingDate { get; set; }
        /// <summary>
        /// 分潤日期
        /// </summary>
        public DateTime? ProfitSharingDate { get; set; }
        public List<SalesSubSetting> SalesSub { get; set; }
    }
}
