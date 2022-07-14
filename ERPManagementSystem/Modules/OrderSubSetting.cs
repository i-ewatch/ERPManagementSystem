namespace ERPManagementSystem.Modules
{
    public class OrderSubSetting
    {
        /// <summary>
        /// 訂單單號
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 訂單明細序號
        /// </summary>
        public int OrderNo { get; set; }
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
