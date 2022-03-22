using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class CustomerDirectorySetting
    {
        /// <summary>
        /// 客戶編號
        /// </summary>
        [StringLength(6, ErrorMessage = "字串最多6個字")]
        public string DirectoryCustomer { get; set; }
        /// <summary>
        /// 編號
        /// </summary>
        [StringLength(6, ErrorMessage = "字串最多6個字")]
        public string DirectoryNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(10, ErrorMessage = "字串最多10個字")]
        public string DirectoryName { get; set; }
        /// <summary>
        /// 職稱
        /// </summary>
        [StringLength(11, ErrorMessage = "字串最多11個字")]
        public string JobTitle { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        [StringLength(20, ErrorMessage = "字串最多20個字")]
        public string Phone { get; set; }
        /// <summary>
        /// 手機
        /// </summary>
        [StringLength(11, ErrorMessage = "字串最多11個字")]
        public string MobilePhone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string Email { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(250, ErrorMessage = "字串最多250個字")]
        public string Remark { get; set; }
        /// <summary>
        /// 附件檔案
        /// </summary>
        //public byte[] AttachmentFile { get; set; }
        /// <summary>
        /// 附件名稱
        /// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string FileName { get; set; }
    }
}
