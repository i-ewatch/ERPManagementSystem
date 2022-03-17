using System.ComponentModel.DataAnnotations;

namespace ERPManagementSystem.Modules
{
    public class PaymentItemSetting
    {
        /// <summary>
        /// 品項代碼
        /// </summary>
        [StringLength(24, ErrorMessage = "字串最多24個字")]
        public string PaymentItemNo { get; set; }
        /// <summary>
        /// 品項名稱
        /// </summary>
        [StringLength(50, ErrorMessage = "字串最多50個字")]
        public string PaymentItemName { get; set; }
    }
}
