using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class SalesSubSetting
    {
        /// <summary>
        /// 銷貨旗標 
        /// </summary>
        [Range(1, 2, ErrorMessage = "3=銷貨，4=銷貨退回")]
        public int SalesFlag { get; set; }
        /// <summary>
        /// 銷貨單號
        /// </summary>
        public string SalesNumber { get; set; }
        /// <summary>
        /// 銷貨明細序號
        /// </summary>
        public int PurchaseNo { get; set; }
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
