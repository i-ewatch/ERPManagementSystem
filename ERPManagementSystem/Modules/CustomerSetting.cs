using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class CustomerSetting
    {
        /// <summary>
        /// 編號
        /// </summary>
        [StringLength(6, ErrorMessage = "字串最多6個字")]
        public string CustomerNumber { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string CustomerName { get; set; }
        /// <summary>
        /// 統一編號
        /// </summary>
        [StringLength(8, ErrorMessage = "字串最多8個字")]
        public string UniformNumbers { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        [StringLength(20, ErrorMessage = "字串最多20個字")]
        public string Phone { get; set; }
        /// <summary>
        /// 傳真
        /// </summary>
        [StringLength(11, ErrorMessage = "字串最多11個字")]
        public string Fax { get; set; }
        /// <summary>
        /// 匯款帳號
        /// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string RemittanceAccount { get; set; }
        /// <summary>
        /// 會計聯絡人姓名
        /// </summary>
        [StringLength(10, ErrorMessage = "字串最多10個字")]
        public string ContactName { get; set; }
        /// <summary>
        /// 會計聯絡人Email
        /// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string ContactEmail { get; set; }
        /// <summary>
        /// 會計聯絡人電話
        /// </summary>
        [StringLength(11, ErrorMessage = "字串最多11個字")]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 結帳方式
        /// </summary>
        [Range(0, 3, ErrorMessage = "0=現金，1=30天，2=60天，3=其他")]
        public int CheckoutType { get; set; }
        /// <summary>
        /// 附件檔案
        /// </summary>
        //public byte[] AttachmentFile { get; set; } = null;
        /// <summary>
        /// 附件名稱
        /// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string FileName { get; set; }
    }
}
