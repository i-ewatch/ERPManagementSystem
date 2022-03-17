using System;
using System.ComponentModel.DataAnnotations;

namespace ERPManagementSystem.Modules
{
    public class PaymentSetting
    {
        /// <summary>
        /// 編號
        /// </summary>
        [StringLength(12, ErrorMessage = "字串最多12個字")]
        public string PaymentNumber { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime PaymentDate { get; set; }
        /// <summary>
        /// 發票號碼
        /// </summary>
        [StringLength(10, ErrorMessage = "字串最多10個字")]
        public string PaymentInvoiceNo { get; set; }
        /// <summary>
        /// 品項代碼
        /// </summary>
        [StringLength(24, ErrorMessage = "字串最多24個字")]
        public string PaymentItemNo { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        [StringLength(100, ErrorMessage = "字串最多100個字")]
        public string PaymentUse { get; set; }
        /// <summary>
        /// 申請人
        /// </summary>
        [StringLength(6, ErrorMessage = "字串最多6個字")]
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// 金額
        /// </summary>
        public double PaymentAmount { get; set; }
        /// <summary>
        /// 請款方式
        /// </summary>
        [Range(0, 1, ErrorMessage = "0=隨薪資轉帳，1=零用金請款")]
        public int PaymentMethod { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(100, ErrorMessage = "字串最多100個字")]
        public string Remark { get; set; }
        /// <summary>
        /// 匯款收款日期
        /// </summary>
        public DateTime? TransferDate { get; set; }
        /// <summary>
        /// 附件檔名
        /// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string FileName { get; set; }
    }
}
