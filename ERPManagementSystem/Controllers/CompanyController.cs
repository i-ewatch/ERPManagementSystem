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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

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
            WorkPath = "廠商";
            SqlDB = _configuration["SqlDB"];
            CompanyLog = _configuration["CompanyLog"];
        }
        /// <summary>
        /// 查詢全部廠商資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<CompanySetting>> GetCompany()
        {
            List<CompanySetting> companySettings = new List<CompanySetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {CompanyLog}";
                        companySettings = connection.Query<CompanySetting>(sql).ToList();
                    }
                });
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
        public async Task<List<CompanySetting>> GetCompanyNumber(string CompanyNumber)
        {
            List<CompanySetting> companySettings = new List<CompanySetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {CompanyLog} WHERE CompanyNumber = @CompanyNumber";
                        companySettings = connection.Query<CompanySetting>(sql, new { CompanyNumber = CompanyNumber }).ToList();
                    }
                });
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
        public async Task<IActionResult> InserterCompany(CompanySetting companySetting)
        {
            try
            {
                List<CompanySetting> companySettings = new List<CompanySetting>();
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {CompanyLog} WHERE CompanyNumber = @CompanyNumber";
                        companySettings = connection.Query<CompanySetting>(sql, new { CompanyNumber = companySetting.CompanyNumber }).ToList();
                    }
                });
                if (companySettings.Count == 0)
                {
                    int DateIndex = 0;
                    await Task.Run(() =>
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"INSERT INTO {CompanyLog}(CompanyNumber,CompanyName,CompanyShortName,UniformNumbers,Phone,Fax,RemittanceAccount,ContactName,ContactEmail,ContactPhone,CheckoutType,Remark) VALUES " +
                            $"(@CompanyNumber,@CompanyName,@CompanyShortName,@UniformNumbers,@Phone,@Fax,@RemittanceAccount,@ContactName,@ContactEmail" +
                            $",@ContactPhone,@CheckoutType,@Remark)";
                            DateIndex = connection.Execute(sql, new
                            {
                                CompanyNumber = companySetting.CompanyNumber,
                                CompanyName = companySetting.CompanyName,
                                CompanyShortName = companySetting.CompanyShortName,
                                UniformNumbers = companySetting.UniformNumbers,
                                Phone = companySetting.Phone,
                                Fax = companySetting.Fax,
                                RemittanceAccount = companySetting.RemittanceAccount,
                                ContactName = companySetting.ContactName,
                                ContactEmail = companySetting.ContactEmail,
                                ContactPhone = companySetting.ContactPhone,
                                CheckoutType = companySetting.CheckoutType,
                                Remark = companySetting.Remark
                            });
                        }
                    });
                    if (DateIndex > 0)
                    {
                        return Ok($"{companySetting.CompanyName}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{companySetting.CompanyName}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{companySetting.CompanyName}資訊，編碼已存在");
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
        [HttpPut]
        public async Task<IActionResult> UpdateCompany(CompanySetting companySetting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE {CompanyLog} SET CompanyName = @CompanyName,CompanyShortName = @CompanyShortName,UniformNumbers = @UniformNumbers,Phone = @Phone,Fax = @Fax,RemittanceAccount = @RemittanceAccount," +
                            $"ContactName = @ContactName,ContactEmail = @ContactEmail,ContactPhone = @ContactPhone,CheckoutType = @CheckoutType,Remark = @Remark" +
                            $" WHERE CompanyNumber = @CompanyNumber";
                        DateIndex = connection.Execute(sql, new
                        {
                            CompanyNumber = companySetting.CompanyNumber,
                            CompanyName = companySetting.CompanyName,
                            CompanyShortName = companySetting.CompanyShortName,
                            UniformNumbers = companySetting.UniformNumbers,
                            Phone = companySetting.Phone,
                            Fax = companySetting.Fax,
                            RemittanceAccount = companySetting.RemittanceAccount,
                            ContactName = companySetting.ContactName,
                            ContactEmail = companySetting.ContactEmail,
                            ContactPhone = companySetting.ContactPhone,
                            CheckoutType = companySetting.CheckoutType,
                            Remark = companySetting.Remark
                        });
                    }
                });
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
        /// 廠商資訊刪除
        /// </summary>
        /// <param name="companySetting">廠商資訊物件</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCompany(CompanySetting companySetting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"DELETE FROM {CompanyLog} WHERE CompanyNumber = @CompanyNumber";
                        DateIndex = connection.Execute(sql, new { CompanyNumber = companySetting.CompanyNumber });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{companySetting.CompanyName}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{companySetting.CompanyName}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{companySetting.CompanyName}資訊，刪除失敗");
            }
        }
        /// <summary>
        /// 廠商附件檔案更新
        /// </summary>
        /// <param name="CompanyNumber">廠商編碼</param>
        /// <param name="CompanyName">廠商名稱</param>
        /// <param name="AttachmentFile">附件檔案</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/CompanyAttachmentFile")]
        public async Task<IActionResult> PostCompanyAttachmentFile(string CompanyNumber, string CompanyName, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(CompanyNumber) && !string.IsNullOrEmpty(CompanyName))
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{CompanyNumber}";
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
                            string sql = $"UPDATE {CompanyLog} SET FileName = @FileName  WHERE CompanyNumber = @CompanyNumber AND CompanyName = @CompanyName";
                            connection.Execute(sql, new { FileName = AttachmentFile.FileName, CompanyNumber = CompanyNumber, CompanyName = CompanyName });
                        }
                    });
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
        /// <summary>
        /// 廠商附件檔案下載
        /// </summary>
        /// <param name="CompanyNumber">廠商編碼</param>
        /// <param name="CompanyName">廠商名稱</param>
        /// <param name="AttachmentFile">附件檔案名稱</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/CompanyAttachmentFile")]
        public async Task<IActionResult> GetCompanyAttachmentFile(string CompanyNumber, string CompanyName, string AttachmentFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(AttachmentFile) && !string.IsNullOrEmpty(CompanyNumber) && !string.IsNullOrEmpty(CompanyName))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{CompanyNumber}\\{AttachmentFile}";
                    if (System.IO.File.Exists(WorkPath))
                    {
                        var memoryStream = new MemoryStream();
                        await Task.Run(() =>
                        {
                            FileStream stream = new FileStream(WorkPath, FileMode.Open);
                            stream.CopyTo(memoryStream);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            stream.Close();
                        });
                        return new FileStreamResult(memoryStream, $"application/{FileExtension}") { FileDownloadName = AttachmentFile };
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest($"{CompanyName}檔案下載失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{CompanyName}檔案下載失敗");
            }
        }
    }
}
