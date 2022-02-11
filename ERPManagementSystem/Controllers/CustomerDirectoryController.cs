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
using System.Threading.Tasks;

namespace ERPManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerDirectoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        private string CustomerDirectoryLog { get; set; }
        public CustomerDirectoryController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "客戶";
            SqlDB = _configuration["SqlDB"];
            CustomerDirectoryLog = _configuration["CustomerDirectoryLog"];
        }
        /// <summary>
        /// 查詢全部客戶通訊錄資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<CustomerDirectorySetting> GetCompany()
        {
            List<CustomerDirectorySetting> customerDirectorySettings = new List<CustomerDirectorySetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CustomerDirectoryLog}";
                    customerDirectorySettings = connection.Query<CustomerDirectorySetting>(sql).ToList();
                }
                return customerDirectorySettings;
            }
            catch (Exception)
            {
                return customerDirectorySettings;
            }
        }
        /// <summary>
        /// 客戶通訊錄編碼查詢
        /// </summary>
        /// <param name="CustomerDirectoryNumber"> 客戶通訊錄編碼</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/CustomerDirectoryNumber/{CustomerDirectoryNumber}")]
        public List<CustomerDirectorySetting> GetCompanyDirectoryNumber(string CustomerDirectoryNumber)
        {
            List<CustomerDirectorySetting> customerDirectorySettings = new List<CustomerDirectorySetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CustomerDirectoryLog} WHERE DirectoryNumber = N'{CustomerDirectoryNumber}'";
                    customerDirectorySettings = connection.Query<CustomerDirectorySetting>(sql).ToList();
                }
                return customerDirectorySettings;
            }
            catch (Exception)
            {
                return customerDirectorySettings;
            }
        }
        /// <summary>
        /// 客戶編碼查詢
        /// </summary>
        /// <param name="DirectoryCustomerNumber"> 客戶編碼</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/DirectoryCustomer/{DirectoryCompanyNumber}")]
        public List<CustomerDirectorySetting> GetCompanyNumber(string DirectoryCustomerNumber)
        {
            List<CustomerDirectorySetting> companyDirectorySettings = new List<CustomerDirectorySetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CustomerDirectoryLog} WHERE DirectoryCustomer = N'{DirectoryCustomerNumber}'";
                    companyDirectorySettings = connection.Query<CustomerDirectorySetting>(sql).ToList();
                }
                return companyDirectorySettings;
            }
            catch (Exception)
            {
                return companyDirectorySettings;
            }
        }
        /// <summary>
        /// 客戶通訊錄資訊新增
        /// </summary>
        /// <param name="customerDirectorySetting">客戶通訊錄資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/InserterCustomerDirectory")]
        public IActionResult InserterCustomerDirectory(CustomerDirectorySetting customerDirectorySetting)
        {
            try
            {
                int DateIndex = 0;
                List<CustomerDirectorySetting> customerDirectorySettings = new List<CustomerDirectorySetting>();
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CustomerDirectoryLog} WHERE DirectoryNumber = N'{customerDirectorySetting.DirectoryNumber}'";
                    customerDirectorySettings = connection.Query<CustomerDirectorySetting>(sql).ToList();
                }
                if (customerDirectorySettings.Count == 0)
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"INSERT INTO {CustomerDirectoryLog}(DirectoryCustomer,DirectoryNumber,DirectoryName,JobTitle,Phone,MobilePhone,Email,Remark) VALUES " +
                            $"(N'{customerDirectorySetting.DirectoryCustomer}',N'{customerDirectorySetting.DirectoryNumber}',N'{customerDirectorySetting.DirectoryName}',N'{customerDirectorySetting.JobTitle}',N'{customerDirectorySetting.Phone}',N'{customerDirectorySetting.MobilePhone}',N'{customerDirectorySetting.Email}',N'{customerDirectorySetting.Remark}')";
                        DateIndex = connection.Execute(sql);
                    }
                    if (DateIndex > 0)
                    {
                        return Ok($"{customerDirectorySetting.DirectoryName}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{customerDirectorySetting.DirectoryName}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{customerDirectorySetting.DirectoryName}資訊，已存在編碼");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{customerDirectorySetting.DirectoryName}資訊，上傳失敗");
            }
        }
        /// <summary>
        /// 客戶通訊錄資訊更新
        /// </summary>
        /// <param name="customerDirectorySetting">客戶通訊錄資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/UpdateCustomerDirectory")]
        public IActionResult UpdateCustomerDirectory(CustomerDirectorySetting customerDirectorySetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"UPDATE {CustomerDirectoryLog} SET DirectoryCustomer = N'{customerDirectorySetting.DirectoryCustomer}',DirectoryName = N'{customerDirectorySetting.DirectoryName}',JobTitle = N'{customerDirectorySetting.JobTitle}',Phone = N'{customerDirectorySetting.Phone}',MobilePhone = N'{customerDirectorySetting.MobilePhone}',Email = N'{customerDirectorySetting.Email}',Remark = N'{customerDirectorySetting.Remark}'" +
                        $" WHERE DirectoryNumber = N'{customerDirectorySetting.DirectoryNumber}'";
                    DateIndex = connection.Execute(sql);
                }
                if (DateIndex > 0)
                {
                    return Ok($"{customerDirectorySetting.DirectoryName}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{customerDirectorySetting.DirectoryName}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{customerDirectorySetting.DirectoryName}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 客戶通訊錄附件檔案更新
        /// </summary>
        /// <param name="DirectoryCustomer">客戶編碼</param>
        /// <param name="DirectoryNumber">客戶通訊錄編碼</param>
        /// <param name="AttachmentFile">附件檔案</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/UpdateCustomerDirectoryAttachmentFile")]
        public IActionResult PostCustomerDirectoryAttachmentFile(string DirectoryCustomer, string DirectoryNumber, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(DirectoryCustomer) && !string.IsNullOrEmpty(DirectoryNumber))
                {
                    WorkPath += $"\\{DirectoryCustomer}";
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
                        string sql = $"UPDATE {CustomerDirectoryLog} SET FileName = N'{AttachmentFile.FileName}' WHERE DirectoryCustomer = N'{DirectoryCustomer}' AND DirectoryNumber = N'{DirectoryNumber}'";
                        connection.Execute(sql);
                    }
                    return Ok($"{DirectoryNumber}檔案上傳成功");
                }
                else
                {
                    return BadRequest($"{DirectoryNumber}檔案上傳失敗");
                }
                #region 資料庫存檔
                //int DateIndex = 0;
                //byte[] attachmentFile = new byte[AttachmentFile.Length];
                //AttachmentFile.OpenReadStream().Read(attachmentFile, 0, Convert.ToInt32(AttachmentFile.Length));
                //using (IDbConnection connection = new SqlConnection(SqlDB))
                //{
                //    string sql = $"UPDATE {CustomerDirectoryLog} SET AttachmentFile = @file,FileExtension = N'{FileExtension}' WHERE DirectoryCustomer = N'{DirectoryCustomer}' AND DirectoryNumber = N'{DirectoryNumber}'";
                //    DateIndex = connection.Execute(sql, new { file = attachmentFile });
                //}
                //if (DateIndex > 0)
                //{
                //    return Ok($"{DirectoryNumber}檔案上傳成功");
                //}
                //else
                //{
                //    return BadRequest($"{DirectoryNumber}檔案上傳失敗");
                //}
                #endregion
            }
            catch (Exception)
            {
                return BadRequest($"{DirectoryNumber}檔案上傳失敗");
            }
        }
    }
}
