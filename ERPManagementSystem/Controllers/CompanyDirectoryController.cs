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
    public class CompanyDirectoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        private string CompanyDirectoryLog { get; set; }
        public CompanyDirectoryController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "廠商";
            SqlDB = _configuration["SqlDB"];
            CompanyDirectoryLog = _configuration["CompanyDirectoryLog"];
        }
        /// <summary>
        /// 查詢全部廠商通訊錄資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<CompanyDirectorySetting> GetCompany()
        {
            List<CompanyDirectorySetting> companyDirectorySettings = new List<CompanyDirectorySetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CompanyDirectoryLog}";
                    companyDirectorySettings = connection.Query<CompanyDirectorySetting>(sql).ToList();
                }
                return companyDirectorySettings;
            }
            catch (Exception)
            {
                return companyDirectorySettings;
            }
        }
        /// <summary>
        /// 廠商通訊錄編碼查詢
        /// </summary>
        /// <param name="CompanyDirectoryNumber"> 廠商通訊錄編碼</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/CompanyDirectoryNumber/{CompanyDirectoryNumber}")]
        public List<CompanyDirectorySetting> GetCompanyDirectoryNumber(string CompanyDirectoryNumber)
        {
            List<CompanyDirectorySetting> companyDirectorySettings = new List<CompanyDirectorySetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CompanyDirectoryLog} WHERE DirectoryNumber = N'{CompanyDirectoryNumber}'";
                    companyDirectorySettings = connection.Query<CompanyDirectorySetting>(sql).ToList();
                }
                return companyDirectorySettings;
            }
            catch (Exception)
            {
                return companyDirectorySettings;
            }
        }
        /// <summary>
        /// 廠商編碼查詢
        /// </summary>
        /// <param name="DirectoryCompanyNumber"> 廠商編碼</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/DirectoryCompany/{DirectoryCompanyNumber}")]
        public List<CompanyDirectorySetting> GetCompanyNumber(string DirectoryCompanyNumber)
        {
            List<CompanyDirectorySetting> companyDirectorySettings = new List<CompanyDirectorySetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CompanyDirectoryLog} WHERE DirectoryCompany = N'{DirectoryCompanyNumber}'";
                    companyDirectorySettings = connection.Query<CompanyDirectorySetting>(sql).ToList();
                }
                return companyDirectorySettings;
            }
            catch (Exception)
            {
                return companyDirectorySettings;
            }
        }
        /// <summary>
        /// 廠商通訊錄資訊新增
        /// </summary>
        /// <param name="companyDirectorySetting">廠商通訊錄資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/InserterCompanyDirectory")]
        public IActionResult InserterCompanyDirectory(CompanyDirectorySetting companyDirectorySetting)
        {
            try
            {
                int DateIndex = 0;
                List<CompanyDirectorySetting> companyDirectorySettings = new List<CompanyDirectorySetting>();
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CompanyDirectoryLog} WHERE DirectoryNumber = N'{companyDirectorySetting.DirectoryNumber}'";
                    companyDirectorySettings = connection.Query<CompanyDirectorySetting>(sql).ToList();
                }
                if (companyDirectorySettings.Count == 0)
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"INSERT INTO {CompanyDirectoryLog}(DirectoryCompany,DirectoryNumber,DirectoryName,JobTitle,Phone,MobilePhone,Email,Remark) VALUES " +
                            $"(N'{companyDirectorySetting.DirectoryCompany}',N'{companyDirectorySetting.DirectoryNumber}',N'{companyDirectorySetting.DirectoryName}',N'{companyDirectorySetting.JobTitle}',N'{companyDirectorySetting.Phone}',N'{companyDirectorySetting.MobilePhone}',N'{companyDirectorySetting.Email}',N'{companyDirectorySetting.Remark}')";
                        DateIndex = connection.Execute(sql);
                    }
                    if (DateIndex > 0)
                    {
                        return Ok($"{companyDirectorySetting.DirectoryName}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{companyDirectorySetting.DirectoryName}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{companyDirectorySetting.DirectoryName}資訊，已存在編碼");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{companyDirectorySetting.DirectoryName}資訊，上傳失敗");
            }
        }
        /// <summary>
        /// 廠商通訊錄資訊更新
        /// </summary>
        /// <param name="companyDirectorySetting">廠商通訊錄資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/UpdateCompanyDirectory")]
        public IActionResult UpdateCompanyDirectory(CompanyDirectorySetting companyDirectorySetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"UPDATE {CompanyDirectoryLog} SET DirectoryCompany = N'{companyDirectorySetting.DirectoryCompany}',DirectoryName = N'{companyDirectorySetting.DirectoryName}',JobTitle = N'{companyDirectorySetting.JobTitle}',Phone = N'{companyDirectorySetting.Phone}',MobilePhone = N'{companyDirectorySetting.MobilePhone}',Email = N'{companyDirectorySetting.Email}',Remark = N'{companyDirectorySetting.Remark}'" +
                        $" WHERE DirectoryNumber = N'{companyDirectorySetting.DirectoryNumber}'";
                    DateIndex = connection.Execute(sql);
                }
                if (DateIndex > 0)
                {
                    return Ok($"{companyDirectorySetting.DirectoryName}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{companyDirectorySetting.DirectoryName}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{companyDirectorySetting.DirectoryName}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 廠商附件檔案更新
        /// </summary>
        /// <param name="DirectoryCompany">廠商編碼</param>
        /// <param name="DirectoryNumber">廠商通訊錄編碼</param>
        /// <param name="AttachmentFile">附件檔案</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/UpdateCompanyDirectoryAttachmentFile")]
        public IActionResult PostCompanyDirectoryAttachmentFile(string DirectoryCompany, string DirectoryNumber, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(DirectoryCompany) && !string.IsNullOrEmpty(DirectoryNumber))
                {
                    WorkPath += $"\\{DirectoryCompany}";
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
                        string sql = $"UPDATE {CompanyDirectoryLog} SET FileName = N'{AttachmentFile.FileName}' WHERE DirectoryCompany = N'{DirectoryCompany}' AND DirectoryNumber = N'{DirectoryNumber}'";
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
                //    string sql = $"UPDATE {CompanyDirectoryLog} SET AttachmentFile = @file,FileExtension = N'{FileExtension}' WHERE DirectoryCompany = N'{DirectoryCompany}' AND DirectoryNumber = N'{DirectoryNumber}'";
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
