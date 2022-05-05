using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class ProductDepartmentSetting
    {
        /// <summary>
        /// 部門編號
        /// </summary>
        [StringLength(2, ErrorMessage = "字串最多2個字")]
        public string DepartmentNumber { get; set; }
        /// <summary>
        /// 部門名稱
        /// </summary>
        [StringLength(20, ErrorMessage = "字串最多20個字")]
        public string DepartmentName { get; set; }
    }
}
