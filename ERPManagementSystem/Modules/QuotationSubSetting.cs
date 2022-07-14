using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class QuotationSubSetting
    {
        /// <summary>
        /// 報價單號
        /// </summary>
        public string QuotationNumber { get; set; }
        /// <summary>
        /// 報價明細序號
        /// </summary>
        public int QuotationNo { get; set; }
        /// <summary>
        /// 報價單明細次序號
        /// </summary>
        public int QuotationSubNo { get; set; }
        /// <summary>
        /// 報價單明細末序號
        /// </summary>
        public int QuotationThrNo { get; set; }
        /// <summary>
        /// 項次旗標
        /// <para>0 = 大項 </para>
        /// <para>1 = 中項</para>
        /// <para>2 = 小項</para>
        /// </summary>
        public int LineFlag { get; set; }
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
