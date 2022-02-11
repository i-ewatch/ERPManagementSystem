using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class CustomerSetting
    {
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string UniformNumbers { get; set; }
        public string RemittanceAccount { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public int CheckoutType { get; set; }
        //public byte[] AttachmentFile { get; set; }
        //public string FileExtension { get; set; }
    }
}
