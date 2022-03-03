using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class ProductCategorySetting
    {
        /// <summary>
        /// 產品類別編號
        /// </summary>
        [StringLength(6, ErrorMessage = "字串最多6個字")]
        public string CategoryNumber { get; set; }
        /// <summary>
        /// 產品類別名稱
        /// </summary>
        [StringLength(20, ErrorMessage = "字串最多20個字")]
        public string CategoryName { get; set; }
    }
}
