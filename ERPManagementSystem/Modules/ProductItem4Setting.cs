using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class ProductItem4Setting
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
        /// 項目3編號
        /// </summary>
        [StringLength(2, ErrorMessage = "字串最多2個字")]
        public string Item3Number { get; set; }
        /// <summary>
        /// 項目4編號
        /// </summary>
        [StringLength(2, ErrorMessage = "字串最多2個字")]
        public string Item4Number { get; set; }
        /// <summary>
        /// 項目4名稱
        /// </summary>
        [StringLength(20, ErrorMessage = "字串最多20個字")]
        public string Item4Name { get; set; }
    }
}
