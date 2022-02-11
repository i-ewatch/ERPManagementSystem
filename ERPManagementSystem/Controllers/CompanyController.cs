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
    public class CompanyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        private string CompanyLog { get; set; }
        public CompanyController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = _configuration["WorkPath"]+ "廠商";
            SqlDB = _configuration["SqlDB"];
            CompanyLog = _configuration["CompanyLog"];
        }
        /// <summary>
        /// 查詢全部廠商資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<CompanySetting> GetCompany()
        {
            List<CompanySetting> companySettings = new List<CompanySetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CompanyLog}";
                    companySettings = connection.Query<CompanySetting>(sql).ToList();
                }
                return companySettings;
            }
            catch (Exception)
            {
                return companySettings;
            }
        }
        /// <summary>
        /// 廠商編碼查詢
        /// </summary>
        /// <param name="CompanyNumber"> 廠商編碼</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/CompanyNumber/{CompanyNumber}")]
        public List<CompanySetting> GetCompanyNumber(string CompanyNumber)
        {
            List<CompanySetting> companySettings = new List<CompanySetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CompanyLog} WHERE CompanyNumber = N'{CompanyNumber}'";
                    companySettings = connection.Query<CompanySetting>(sql).ToList();
                }
                return companySettings;
            }
            catch (Exception)
            {
                return companySettings;
            }
        }
        /// <summary>
        /// 廠商資訊新增
        /// </summary>
        /// <param name="companySetting">廠商資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/InserterCompany")]
        public IActionResult InserterCompany(CompanySetting companySetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"INSERT INTO {CompanyLog}(CompanyNumber,CompanyName,CompanyShortName,UniformNumbers,Phone,Fax,ContactName,ContactEmail,ContactPhone,CheckoutType,Remark) VALUES " +
                        $"(N'{companySetting.CompanyNumber}',N'{companySetting.CompanyName}',N'{companySetting.CompanyShortName}',N'{companySetting.UniformNumbers}',N'{companySetting.Phone}',N'{companySetting.Fax}',N'{companySetting.ContactName}',N'{companySetting.ContactEmail}'" +
                        $",N'{companySetting.ContactPhone}',{companySetting.CheckoutType},N'{companySetting.Remark}')";
                    DateIndex = connection.Execute(sql);
                }
                if (DateIndex > 0)
                {
                    return Ok($"{companySetting.CompanyName}資訊，上傳成功!");
                }
                else
                {
                    return BadRequest($"{companySetting.CompanyName}資訊，上傳失敗");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{companySetting.CompanyName}資訊，上傳失敗");
            }
        }
        /// <summary>
        /// 廠商資訊更新
        /// </summary>
        /// <param name="companySetting">廠商資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/UpdateCompany")]
        public IActionResult UpdateCompany(CompanySetting companySetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"UPDATE {CompanyLog} SET CompanyName = N'{companySetting.CompanyName}',CompanyShortName = N'{companySetting.CompanyShortName}',UniformNumbers = N'{companySetting.UniformNumbers}',Phone = N'{companySetting.Phone}',Fax = N'{companySetting.Fax}'," +
                        $"ContactName = N'{companySetting.ContactName}',ContactEmail = N'{companySetting.ContactEmail}',ContactPhone = N'{companySetting.ContactPhone}',CheckoutType = {companySetting.CheckoutType},Remark = N'{companySetting.Remark}'" +
                        $" WHERE CompanyNumber = N'{companySetting.CompanyNumber}'";
                    DateIndex = connection.Execute(sql);
                }
                if (DateIndex > 0)
                {
                    return Ok($"{companySetting.CompanyName}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{companySetting.CompanyName}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{companySetting.CompanyName}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 廠商附件檔案更新
        /// </summary>
        /// <param name="CompanyNumber">廠商編碼</param>
        /// <param name="CompanyName">廠商名稱</param>
        /// <param name="AttachmentFile">附件檔案</param>
        /// <param name="FileExtension">副檔名</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/UpdateCompanyAttachmentFile")]
        public IActionResult PostCompanyAttachmentFile(string CompanyNumber, string CompanyName, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(CompanyNumber) && !string.IsNullOrEmpty(CompanyName))
                {
                    WorkPath += $"\\{CompanyNumber}{CompanyName}";
                    if (!Directory.Exists(WorkPath))
                    {
                        Directory.CreateDirectory($"{WorkPath}");
                    }
                    WorkPath += $"\\{AttachmentFile.FileName}";
                    using (var stream = new FileStream(WorkPath, FileMode.Create))
                    {
                        AttachmentFile.CopyToAsync(stream);
                    }
                    return Ok($"{CompanyName}檔案上傳成功");
                }
                else
                {
                    return BadRequest($"{CompanyName}檔案上傳失敗");
                }
                #region 資料庫存檔
                //int DateIndex = 0;
                //byte[] attachmentFile = new byte[AttachmentFile.Length];
                //AttachmentFile.OpenReadStream().Read(attachmentFile, 0, Convert.ToInt32(AttachmentFile.Length));
                //using (IDbConnection connection = new SqlConnection(SqlDB))
                //{
                //    string sql = $"UPDATE {CompanyLog} SET AttachmentFile = @file,FileExtension = N'{FileExtension}' WHERE CompanyNumber = N'{CompanyNumber}' AND CompanyName = N'{CompanyName}'";
                //    DateIndex = connection.Execute(sql, new { file = attachmentFile });
                //}
                //if (DateIndex > 0)
                //{
                //    return Ok($"{CompanyName}檔案上傳成功");
                //}
                //else
                //{
                //    return BadRequest($"{CompanyName}檔案上傳失敗");
                //}
                #endregion
            }
            catch (Exception)
            {
                return BadRequest($"{CompanyName}檔案上傳失敗");
            }
        }
    }
}
