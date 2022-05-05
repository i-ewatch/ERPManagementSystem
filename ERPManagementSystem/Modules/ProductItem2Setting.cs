using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class ProductItem2Setting
    {
        /// <summary>
        /// 部門編號
        /// </summary>
        [StringLength(2, ErrorMessage = "字串最多2個字")]
        public string DepartmentNumber { get; set; }
        /// <summary>
        /// 項目1編號
        /// </summary>
        [StringLength(2, ErrorMessage = "字串最多2個字")]
        public string Item1Number { get; set; }
        /// <summary>
        /// 項目2編號
        /// </summary>
        [StringLength(2, ErrorMessage = "字串最多2個字")]
        public string Item2Number { get; set; }
        /// <summary>
        /// 項目2名稱
        /// </summary>
        [StringLength(20, ErrorMessage = "字串最多20個字")]
        public string Item2Name { get; set; }
    }
}
