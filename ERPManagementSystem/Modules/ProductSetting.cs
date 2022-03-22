using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class ProductSetting
    {
        /// <summary>
        /// 廠商編號
        /// </summary>
        [StringLength(6, ErrorMessage = "字串最多6個字")]
        public string ProductCompanyNumber { get; set; }
        /// <summary>
        /// 產品編號
        /// </summary>
        [StringLength(24, ErrorMessage = "字串最多24個字")]
        public string ProductNumber { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        [StringLength(24, ErrorMessage = "字串最多24個字")]
        public string ProductName { get; set; }
        /// <summary>
        /// 產品型號
        /// </summary>
        [StringLength(24, ErrorMessage = "字串最多24個字")]
        public string ProductModel { get; set; }
        /// <summary>
        /// 產品種類
        /// </summary>
        [Range(0, 1, ErrorMessage = "0=設備，1=零件")]
        public int ProductType { get; set; }
        /// <summary>
        /// 產品類別編號
        /// </summary>
        [StringLength(6, ErrorMessage = "字串最多6個字")]
        public string ProductCategory { get; set; }
        /// <summary>
        /// FootPrint
        /// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string FootPrint { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(250, ErrorMessage = "字串最多250個字")]
        public string Remark { get; set; }
        /// <summary>
        /// 說明
        /// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string Explanation { get; set; }
        ///// <summary>
        ///// 附件檔案
        ///// </summary>
        //public byte[] AttachmentFile { get; set; }
        ///// <summary>
        ///// 附件名稱
        ///// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string FileName { get; set; }
    }
}
