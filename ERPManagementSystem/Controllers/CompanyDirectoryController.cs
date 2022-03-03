using Dapper;
using ERPManagementSystem.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
                    string sql = $"SELECT * FROM  {CompanyDirectoryLog} WHERE DirectoryNumber = @CompanyDirectoryNumber";
                    companyDirectorySettings = connection.Query<CompanyDirectorySetting>(sql, new { CompanyDirectoryNumber = CompanyDirectoryNumber }).ToList();
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
                    string sql = $"SELECT * FROM  {CompanyDirectoryLog} WHERE DirectoryCompany = @DirectoryCompanyNumber";
                    companyDirectorySettings = connection.Query<CompanyDirectorySetting>(sql, new { DirectoryCompanyNumber = DirectoryCompanyNumber }).ToList();
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
        public IActionResult InserterCompanyDirectory(CompanyDirectorySetting companyDirectorySetting)
        {
            try
            {
                int DateIndex = 0;
                List<CompanyDirectorySetting> companyDirectorySettings = new List<CompanyDirectorySetting>();
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {CompanyDirectoryLog} WHERE DirectoryCompany = @DirectoryCompany AND DirectoryNumber = @DirectoryNumber";
                    companyDirectorySettings = connection.Query<CompanyDirectorySetting>(sql, new { DirectoryCompany = companyDirectorySetting.DirectoryCompany, DirectoryNumber = companyDirectorySetting.DirectoryNumber }).ToList();
                }
                if (companyDirectorySettings.Count == 0)
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"INSERT INTO {CompanyDirectoryLog} (DirectoryCompany,DirectoryNumber,DirectoryName,JobTitle,Phone,MobilePhone,Email,Remark) VALUES " +
                            $"(@DirectoryCompany,@DirectoryNumber,@DirectoryName,@JobTitle,@Phone,@MobilePhone,@Email,@Remark)";
                        DateIndex = connection.Execute(sql, new
                        {
                            DirectoryCompany = companyDirectorySetting.DirectoryCompany,
                            DirectoryNumber = companyDirectorySetting.DirectoryNumber,
                            DirectoryName = companyDirectorySetting.DirectoryName,
                            JobTitle = companyDirectorySetting.JobTitle,
                            Phone = companyDirectorySetting.Phone,
                            MobilePhone = companyDirectorySetting.MobilePhone,
                            Email = companyDirectorySetting.Email,
                            Remark = companyDirectorySetting.Remark
                        });
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
        [HttpPut]
        public IActionResult UpdateCompanyDirectory(CompanyDirectorySetting companyDirectorySetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"UPDATE {CompanyDirectoryLog} SET DirectoryName = @DirectoryName,JobTitle = @JobTitle,Phone = @Phone,MobilePhone = @MobilePhone,Email = @Email,Remark = @Remark" +
                        $" WHERE DirectoryCompany = @DirectoryCompany AND DirectoryNumber = @DirectoryNumber";
                    DateIndex = connection.Execute(sql, new
                    {
                        DirectoryCompany = companyDirectorySetting.DirectoryCompany,
                        DirectoryNumber = companyDirectorySetting.DirectoryNumber,
                        DirectoryName = companyDirectorySetting.DirectoryName,
                        JobTitle = companyDirectorySetting.JobTitle,
                        Phone = companyDirectorySetting.Phone,
                        MobilePhone = companyDirectorySetting.MobilePhone,
                        Email = companyDirectorySetting.Email,
                        Remark = companyDirectorySetting.Remark,
                    });
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
        /// 廠商通訊錄資訊刪除
        /// </summary>
        /// <param name="companyDirectorySetting">廠商通訊錄資訊物件</param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteCompanyDirectory(CompanyDirectorySetting companyDirectorySetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql =  $"DELETE FROM {CompanyDirectoryLog} WHERE DirectoryCompany = @DirectoryCompany AND DirectoryNumber = @DirectoryNumber";
                    DateIndex = connection.Execute(sql, new
                    {
                        DirectoryCompany = companyDirectorySetting.DirectoryCompany,
                        DirectoryNumber = companyDirectorySetting.DirectoryNumber
                    });
                }
                if (DateIndex > 0)
                {
                    return Ok($"{companyDirectorySetting.DirectoryName}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{companyDirectorySetting.DirectoryName}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{companyDirectorySetting.DirectoryName}資訊，刪除失敗");
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
        [Route("/api/CompanyDirectoryAttachmentFile")]
        public IActionResult PostCompanyDirectoryAttachmentFile(string DirectoryCompany, string DirectoryNumber,IFormFile AttachmentFile)
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
                        string sql = $"UPDATE {CompanyDirectoryLog} SET FileName = @FileName WHERE DirectoryCompany = @DirectoryCompany AND DirectoryNumber = @DirectoryNumber";
                        connection.Execute(sql, new { FileName = AttachmentFile.FileName, DirectoryCompany = DirectoryCompany, DirectoryNumber = DirectoryNumber });
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
        /// <summary>
        /// 廠商附件檔案下載
        /// </summary>
        /// <param name="DirectoryCompany">廠商編碼</param>
        /// <param name="DirectoryNumber">廠商通訊錄編碼</param>
        /// <param name="AttachmentFile">附件檔案名稱</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/CompanyDirectoryAttachmentFile")]
        public IActionResult GetCompanyDirectoryAttachmentFile(string DirectoryCompany, string DirectoryNumber, string AttachmentFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(AttachmentFile) && !string.IsNullOrEmpty(DirectoryCompany) && !string.IsNullOrEmpty(DirectoryNumber))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{DirectoryCompany}\\{AttachmentFile}";
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
                    return BadRequest($"{DirectoryNumber}檔案下載失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{DirectoryNumber}檔案下載失敗");
            }
        }
    }
}
