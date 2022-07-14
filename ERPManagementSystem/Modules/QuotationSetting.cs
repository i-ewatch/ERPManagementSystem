using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class QuotationSetting
    {
        /// <summary>
        /// 報價單號
        /// </summary>
        [StringLength(12, ErrorMessage = "字串最多12個字")]
        public string QuotationNumber { get; set; }
        /// <summary>
        /// 專案代碼
        /// </summary>
        [StringLength(12, ErrorMessage = "字串最多12個字")]
        public string ProjectNumber { get; set; }
        /// <summary>
        /// 報價日期
        /// </summary>
        public System.DateTime QuotationDate { get; set; }
        /// <summary>
        /// 客戶編號
        /// </summary>
        public string QuotationCustomerNumber { get; set; }
        /// <summary>
        /// 廠商人員編號
        /// </summary>
        public string QuotationDirectoryNumber { get; set; }
        /// <summary>
        /// 送貨地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 稅別
        /// </summary>
        [Range(0, 4, ErrorMessage = "0.應稅，1.零稅，2.免稅，3.二聯")]
        public int QuotationTax { get; set; }
        /// <summary>
        /// 員工號碼
        /// </summary>
        public string QuotationEmployeeNumber { get; set; }
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
        /// 數量合計
        /// </summary>
        public double TotalQty { get; set; }
        /// <summary>
        /// 附件檔名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 訂購單需知
        /// 內容呈現 : 0,1,2,13
        /// 0=出貨/訂單單
        /// 1=發票
        /// 2=回郵信封
        /// 11=貨到現金付款(T/T)
        /// 12=預付現金(T/T)
        /// 13=月結60天
        /// 14=月結30天
        /// </summary>
        public string QuotationNote { get; set; }
        /// <summary>
        /// 作廢旗標
        /// </summary>
        public bool InvalidFlag { get; set; }

        public List<QuotationSubSetting> QuotationSub { get; set; }
    }
}
