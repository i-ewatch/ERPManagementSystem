using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class PickingSubSetting
    {
        /// <summary>
        /// 領料旗標 
        /// </summary>
        /// [Range(5, 6, ErrorMessage = "5.領料，6.領料退回")]
        public int PickingFlag { get; set; }
        /// <summary>
        /// 領料單號
        /// </summary>
        public string PickingNumber { get; set; }
        /// <summary>
        /// 領料明細序號
        /// </summary>
        public int PickingNo { get; set; }
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
        /// <summary>
        /// 產品單項成本
        /// </summary>
        public double Cost { get; set; }
        /// <summary>
        /// 成本小計
        /// </summary>
        public double CostTotal { get; set; }
    }
}
