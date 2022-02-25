using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class EmployeeSetting
    {
        /// <summary>
        /// 編號
        /// </summary>
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 手機
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 戶籍地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 權限
        /// </summary>
        public int Token { get; set; }
    }
}
