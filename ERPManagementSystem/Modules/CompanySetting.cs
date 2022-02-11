using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Modules
{
    public class CompanySetting
    {
        public string CompanyNumber { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; } = null;
        public string UniformNumbers { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public int CheckoutType { get; set; }
        public string Remark { get; set; } = null;
        public byte[] AttachmentFile { get; set; } = null;
        public string FileExtension { get; set; } = null;
    }
}
