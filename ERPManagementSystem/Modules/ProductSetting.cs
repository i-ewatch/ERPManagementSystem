using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class ProductSetting
    {
        /// <summary>
        /// 廠商編號
        /// </summary>
        public string ProductCompanyNumber { get; set; }
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProductNumber { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 產品型號
        /// </summary>
        public string ProductModel { get; set; }
        /// <summary>
        /// 產品種類
        /// </summary>
        public int ProductType { get; set; }
        /// <summary>
        /// 產品類別編號
        /// </summary>
        public string ProductCategory { get; set; }
        /// <summary>
        /// FootPrint
        /// </summary>
        public string FootPrint { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 說明
        /// </summary>
        public string Explanation { get; set; }
        ///// <summary>
        ///// 附件檔案
        ///// </summary>
        //public byte[] AttachmentFile { get; set; }
        ///// <summary>
        ///// 附件名稱
        ///// </summary>
        public string FileName { get; set; }
    }
}
