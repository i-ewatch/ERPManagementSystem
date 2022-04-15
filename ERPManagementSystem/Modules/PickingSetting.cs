using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPManagementSystem.Modules
{
    public class PickingSetting
    {
        /// <summary>
        /// 領料旗標 
        /// </summary>
        [Range(5, 6, ErrorMessage = "5.領料，6.領料退回")]
        public int PickingFlag { get; set; }
        /// <summary>
        /// 領料單號
        /// </summary>
        public string PickingNumber { get; set; }
        /// <summary>
        /// 領料日期
        /// </summary>
        public DateTime PickingDate { get; set; }
        /// <summary>
        /// 客戶編號
        /// </summary>
        public string PickingCustomerNumber { get; set; }
        /// <summary>
        /// 專案代碼
        /// </summary>
        public string ProjectNumber { get; set; }
        /// <summary>
        /// 稅別
        /// </summary>
        public int PickingTax { get; set; }
        /// <summary>
        /// 發票號碼
        /// </summary>
        public string PickingInvoiceNo { get; set; }
        /// <summary>
        /// 員工號碼
        /// </summary>
        public string PickingEmployeeNumber { get; set; }
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
        public List<PickingSubSetting> PickingSub { get; set; }
    }
}
