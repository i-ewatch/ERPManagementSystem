using Dapper;
using ERPManagementSystem.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ERPManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        private string CustomerLog { get; set; }
        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "客戶";
            SqlDB = _configuration["SqlDB"];
            CustomerLog = _configuration["CustomerLog"];
        }
        /// <summary>
        /// 查詢全部客戶資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<CustomerSetting> GetCustomer()
        {
            List<CustomerSetting> customerSettings = new List<CustomerSetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CustomerLog}";
                    customerSettings = connection.Query<CustomerSetting>(sql).ToList();
                }
                return customerSettings;
            }
            catch (Exception)
            {
                return customerSettings;
            }
        }
        /// <summary>
        /// 客戶編碼查詢
        /// </summary>
        /// <param name="CompanyNumber">客戶編碼</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/CustomerNumber/{CustomerNumber}")]
        public List<CustomerSetting> GetCustomerNumber(string CustomerNumber)
        {
            List<CustomerSetting> customerSettings = new List<CustomerSetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CustomerLog} WHERE CustomerNumber = @CustomerNumber";
                    customerSettings = connection.Query<CustomerSetting>(sql, new { CustomerNumber = CustomerNumber }).ToList();
                }
                return customerSettings;
            }
            catch (Exception)
            {
                return customerSettings;
            }
        }
        /// <summary>
        /// 客戶資訊新增
        /// </summary>
        /// <param name="customerSetting">客戶資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InserterCustomer(CustomerSetting customerSetting)
        {
            try
            {
                List<CustomerSetting> customerSettings = new List<CustomerSetting>();
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CustomerLog} WHERE CustomerNumber = @CustomerNumber";
                    customerSettings = connection.Query<CustomerSetting>(sql, new { CustomerNumber = customerSetting.CustomerNumber }).ToList();
                }
                if (customerSettings.Count == 0)
                {
                    int DateIndex = 0;
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"INSERT INTO {CustomerLog} (CustomerNumber,CustomerName,UniformNumbers,Phone,Fax,RemittanceAccount,ContactName,ContactEmail,ContactPhone,CheckoutType) VALUES " +
                            $"(@CustomerNumber,@CustomerName,@UniformNumbers,@Phone,@Fax,@RemittanceAccount,@ContactName,@ContactEmail" +
                            $",@ContactPhone,@CheckoutType)";
                        DateIndex = connection.Execute(sql, new
                        {
                            CustomerNumber = customerSetting.CustomerNumber,
                            CustomerName = customerSetting.CustomerName,
                            UniformNumbers = customerSetting.UniformNumbers,
                            Phone = customerSetting.Phone,
                            Fax = customerSetting.Fax,
                            RemittanceAccount = customerSetting.RemittanceAccount,
                            ContactName = customerSetting.ContactName,
                            ContactEmail = customerSetting.ContactEmail,
                            ContactPhone = customerSetting.ContactPhone,
                            CheckoutType = customerSetting.CheckoutType
                        });
                    }
                    if (DateIndex > 0)
                    {
                        return Ok($"{customerSetting.CustomerName}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{customerSetting.CustomerName}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{customerSetting.CustomerName}資訊，上傳失敗");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{customerSetting.CustomerName}資訊，編碼已存在");
            }
        }
        /// <summary>
        /// 客戶資訊更新
        /// </summary>
        /// <param name="customerSetting">客戶資訊物件</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateCustomer(CustomerSetting customerSetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"UPDATE {CustomerLog} SET CustomerName = @CustomerName,UniformNumbers = @UniformNumbers,Phone = @Phone,Fax = @Fax,RemittanceAccount = @RemittanceAccount," +
                        $"ContactName = @ContactName,ContactEmail = @ContactEmail,ContactPhone = @ContactPhone,CheckoutType = @CheckoutType" +
                        $" WHERE CustomerNumber = @CustomerNumber";
                    DateIndex = connection.Execute(sql, new
                    {
                        CustomerNumber = customerSetting.CustomerNumber,
                        CustomerName = customerSetting.CustomerName,
                        UniformNumbers = customerSetting.UniformNumbers,
                        Phone = customerSetting.Phone,
                        Fax = customerSetting.Fax,
                        RemittanceAccount = customerSetting.RemittanceAccount,
                        ContactName = customerSetting.ContactName,
                        ContactEmail = customerSetting.ContactEmail,
                        ContactPhone = customerSetting.ContactPhone,
                        CheckoutType = customerSetting.CheckoutType
                    });
                }
                if (DateIndex > 0)
                {
                    return Ok($"{customerSetting.CustomerName}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{customerSetting.CustomerName}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{customerSetting.CustomerName}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 客戶資訊刪除
        /// </summary>
        /// <param name="customerSetting">客戶資訊物件</param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteCustomer(CustomerSetting customerSetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"DELETE FROM {CustomerLog} WHERE CustomerNumber = @CustomerNumber";
                    DateIndex = connection.Execute(sql, new
                    {
                        CustomerNumber = customerSetting.CustomerNumber
                    });
                }
                if (DateIndex > 0)
                {
                    return Ok($"{customerSetting.CustomerName}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{customerSetting.CustomerName}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{customerSetting.CustomerName}資訊，刪除失敗");
            }
        }
        /// <summary>
        /// 客戶附件檔案更新
        /// </summary>
        /// <param name="CustomerNumber">客戶編碼</param>
        /// <param name="CustomerName">客戶名稱</param>
        /// <param name="AttachmentFile">附件檔案</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/CustomerAttachmentFile")]
        public IActionResult PostCompanyAttachmentFile(string CustomerNumber, string CustomerName, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(CustomerNumber) && !string.IsNullOrEmpty(CustomerName))
                {
                    WorkPath += $"\\{CustomerNumber}";
                    if (!Directory.Exists(WorkPath))
                    {
                        Directory.CreateDirectory($"{WorkPath}");
                    }
                    WorkPath += $"\\{AttachmentFile.FileName}";
                    using (var stream = new FileStream(WorkPath, FileMode.Create))
                    {
                        var fs = new BinaryReader(AttachmentFile.OpenReadStream());
                        int filelong = Convert.ToInt32(AttachmentFile.Length);
                        var bytes = new byte[filelong];
                        fs.Read(bytes, 0, filelong);
                        stream.Write(bytes, 0, filelong);
                        fs.Close();
                        stream.Flush();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE {CustomerLog} SET FileName = @FileName WHERE CustomerNumber = @CustomerNumber AND CustomerName = @CustomerName";
                        connection.Execute(sql, new { FileName = AttachmentFile.FileName, CustomerNumber = CustomerNumber, CustomerName = CustomerName });
                    }
                    return Ok($"{CustomerName}檔案上傳成功");
                }
                else
                {
                    return BadRequest($"{CustomerName}檔案上傳失敗");
                }
                #region 資料庫存檔
                //int DateIndex = 0;
                //byte[] attachmentFile = new byte[AttachmentFile.Length];
                //AttachmentFile.OpenReadStream().Read(attachmentFile, 0, Convert.ToInt32(AttachmentFile.Length));
                //using (IDbConnection connection = new SqlConnection(SqlDB))
                //{
                //    string sql = $"UPDATE {CustomerLog} SET AttachmentFile = @file,FileExtension = N'{FileExtension}' WHERE CustomerNumber = N'{CustomerNumber}' AND CustomerName = N'{CustomerName}'";
                //    DateIndex = connection.Execute(sql, new { file = attachmentFile });
                //}
                //if (DateIndex > 0)
                //{
                //    return Ok($"{CustomerName}檔案上傳成功");
                //}
                //else
                //{
                //    return BadRequest($"{CustomerName}檔案上傳失敗");
                //}
                #endregion
            }
            catch (Exception)
            {
                return BadRequest($"{CustomerName}檔案上傳失敗");
            }
        }
        /// <summary>
        /// 客戶附件檔案更新
        /// </summary>
        /// <param name="CustomerNumber">客戶編碼</param>
        /// <param name="CustomerName">客戶名稱</param>
        /// <param name="AttachmentFile">附件檔案</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/CustomerAttachmentFile")]
        public IActionResult GetCompanyAttachmentFile(string CustomerNumber, string CustomerName, string AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(CustomerNumber) && !string.IsNullOrEmpty(CustomerName))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{CustomerNumber}\\{AttachmentFile}";
                    if (System.IO.File.Exists(WorkPath))
                    {
                        var memoryStream = new MemoryStream();
                        FileStream stream = new FileStream(WorkPath, FileMode.Open);
                        stream.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        stream.Close();
                        return new FileStreamResult(memoryStream, $"application/{FileExtension}") { FileDownloadName = AttachmentFile };
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest($"{CustomerName}檔案下載失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{CustomerName}檔案下載失敗");
            }
        }
    }
}
