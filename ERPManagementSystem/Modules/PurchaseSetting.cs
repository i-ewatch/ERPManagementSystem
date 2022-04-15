using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class PurchaseSetting
    {
        /// <summary>
        /// 進貨旗標 
        /// </summary>
        [Range(1, 2, ErrorMessage = "1=進貨，2=進貨退出")]
        public int PurchaseFlag { get; set; }
        /// <summary>
        /// 進貨單號
        /// </summary>
        public string PurchaseNumber { get; set; }
        /// <summary>
        /// 專案代碼
        /// </summary>
        [StringLength(12, ErrorMessage = "字串最多12個字")]
        public string ProjectNumber { get; set; }
        /// <summary>
        /// 進貨日期
        /// </summary>
        public System.DateTime PurchaseDate { get; set; }
        /// <summary>
        /// 廠商編號
        /// </summary>
        public string PurchaseCompanyNumber { get; set; }
        /// <summary>
        /// 稅別
        /// </summary>
        [Range(0, 4, ErrorMessage = "0.應稅，1.零稅，2.免稅，3.二聯")]
        public int PurchaseTax { get; set; }
        /// <summary>
        /// 發票號碼
        /// </summary>
        public string PurchaseInvoiceNo { get; set; }
        /// <summary>
        /// 員工號碼
        /// </summary>
        public string PurchaseEmployeeNumber { get; set; }
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
        /// 過帳日期
        /// </summary>
        public DateTime? PostingDate { get; set; }
        public List<PurchaseSubSetting> PurchaseSub { get; set; }
    }
}
