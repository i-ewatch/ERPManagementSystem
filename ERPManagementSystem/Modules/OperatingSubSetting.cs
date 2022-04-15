using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class OperatingSubSetting
    {
        /// <summary>
        /// 營運旗標 
        /// </summary>
        [Range(7, 8, ErrorMessage = "7=營運，8=營運退出")]
        public int OperatingFlag { get; set; }
        /// <summary>
        /// 營運單號
        /// </summary>
        public string OperatingNumber { get; set; }
        /// <summary>
        /// 營運明細序號
        /// </summary>
        public int OperatingNo { get; set; }
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProductNumber { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 產品單位
        /// </summary>
        public string ProductUnit { get; set; }
        /// <summary>
        /// 產品數量
        /// </summary>
        public double ProductQty { get; set; }
        /// <summary>
        /// 產品單價
        /// </summary>
        public double ProductPrice { get; set; }
        /// <summary>
        /// 產品小計
        /// </summary>
        public double ProductTotal { get; set; }
    }
}
