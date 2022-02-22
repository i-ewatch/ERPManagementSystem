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
                    string sql = $"SELECT * FROM  {CustomerLog} WHERE CustomerNumber = N'{CustomerNumber}'";
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
                    string sql = $"SELECT * FROM  {CustomerLog} WHERE CustomerNumber = N'{customerSetting.CustomerNumber}'";
                    customerSettings = connection.Query<CustomerSetting>(sql).ToList();
                }
                if (customerSettings.Count == 0)
                {
                    int DateIndex = 0;
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"INSERT INTO {CustomerLog}(CustomerNumber,CustomerName,UniformNumbers,Phone,Fax,RemittanceAccount,ContactName,ContactEmail,ContactPhone,CheckoutType) VALUES " +
                            $"(N'{customerSetting.CustomerNumber}',N'{customerSetting.CustomerName}',N'{customerSetting.UniformNumbers}',N'{customerSetting.Phone}',N'{customerSetting.Fax}',N'{customerSetting.RemittanceAccount}',N'{customerSetting.ContactName}',N'{customerSetting.ContactEmail}'" +
                            $",N'{customerSetting.ContactPhone}',{customerSetting.CheckoutType})";
                        DateIndex = connection.Execute(sql);
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
                    string sql = $"UPDATE {CustomerLog} SET CustomerName = N'{customerSetting.CustomerName}',UniformNumbers = N'{customerSetting.UniformNumbers}',Phone = N'{customerSetting.Phone}',Fax = N'{customerSetting.Fax}',RemittanceAccount = N'{customerSetting.RemittanceAccount}'," +
                        $"ContactName = N'{customerSetting.ContactName}',ContactEmail = N'{customerSetting.ContactEmail}',ContactPhone = N'{customerSetting.ContactPhone}',CheckoutType = {customerSetting.CheckoutType}" +
                        $" WHERE CustomerNumber = N'{customerSetting.CustomerNumber}'";
                    DateIndex = connection.Execute(sql);
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
        /// 廠商附件檔案更新
        /// </summary>
        /// <param name="CustomerNumber">廠商編碼</param>
        /// <param name="CustomerName">廠商名稱</param>
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
                        AttachmentFile.CopyToAsync(stream);
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE {CustomerLog} SET FileName = N'{AttachmentFile.FileName}' WHERE CustomerNumber = N'{CustomerNumber}' AND CustomerName = N'{CustomerName}'";
                        connection.Execute(sql);
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
    }
}
