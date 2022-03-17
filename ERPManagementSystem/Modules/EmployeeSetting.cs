using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class EmployeeSetting
    {
        /// <summary>
        /// 編號
        /// </summary>
        [StringLength(6, ErrorMessage = "字串最多6個字")]
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(10, ErrorMessage = "字串最多10個字")]
        public string EmployeeName { get; set; }
        /// <summary>
        /// 手機
        /// </summary>
        [StringLength(11, ErrorMessage = "字串最多11個字")]
        public string Phone { get; set; }
        /// <summary>
        /// 戶籍地址
        /// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string Address { get; set; }
        /// <summary>
        /// 權限
        /// </summary>
        [Range(0, 1, ErrorMessage = "0=一般使用者，1=管理員")]
        public int Token { get; set; }
        /// <summary>
        /// 帳號
        /// </summary>
        [StringLength(20,ErrorMessage ="字串最多20個字")]
        public string AccountNo { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        [StringLength(20, ErrorMessage = "字串最多20個字")]
        public string PassWord { get; set; }
    }
}
