using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class CustomerSetting
    {
        /// <summary>
        /// 編號
        /// </summary>
        public string CustomerNumber { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 統一編號
        /// </summary>
        public string UniformNumbers { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 傳真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 匯款帳號
        /// </summary>
        public string RemittanceAccount { get; set; }
        /// <summary>
        /// 會計聯絡人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 會計聯絡人Email
        /// </summary>
        public string ContactEmail { get; set; }
        /// <summary>
        /// 會計聯絡人電話
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 結帳方式
        /// </summary>
        public int CheckoutType { get; set; }
        /// <summary>
        /// 附件檔案
        /// </summary>
        //public byte[] AttachmentFile { get; set; } = null;
        /// <summary>
        /// 附件名稱
        /// </summary>
        public string FileName { get; set; }
    }
}
