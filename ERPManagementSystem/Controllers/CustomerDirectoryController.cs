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
        public async Task<List<CustomerDirectorySetting>> GetCompany()
        {
            List<CustomerDirectorySetting> customerDirectorySettings = new List<CustomerDirectorySetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {CustomerDirectoryLog}";
                        customerDirectorySettings = connection.Query<CustomerDirectorySetting>(sql).ToList();
                    }
                });
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
        public async Task<List<CustomerDirectorySetting>> GetCompanyDirectoryNumber(string CustomerDirectoryNumber)
        {
            List<CustomerDirectorySetting> customerDirectorySettings = new List<CustomerDirectorySetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {CustomerDirectoryLog} WHERE DirectoryNumber = @CustomerDirectoryNumber";
                        customerDirectorySettings = connection.Query<CustomerDirectorySetting>(sql, new { CustomerDirectoryNumber = CustomerDirectoryNumber }).ToList();
                    }
                });
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
        [Route("/api/DirectoryCustomer/{DirectoryCustomerNumber}")]
        public async Task<List<CustomerDirectorySetting>> GetCompanyNumber(string DirectoryCustomerNumber)
        {
            List<CustomerDirectorySetting> companyDirectorySettings = new List<CustomerDirectorySetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {CustomerDirectoryLog} WHERE DirectoryCustomer = @DirectoryCustomerNumber";
                        companyDirectorySettings = connection.Query<CustomerDirectorySetting>(sql, new { DirectoryCustomerNumber = DirectoryCustomerNumber }).ToList();
                    }
                });
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
        public async Task<IActionResult> InserterCustomerDirectory(CustomerDirectorySetting customerDirectorySetting)
        {
            try
            {
                int DateIndex = 0;
                List<CustomerDirectorySetting> customerDirectorySettings = new List<CustomerDirectorySetting>();
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {CustomerDirectoryLog} WHERE DirectoryCustomer = @DirectoryCustomer AND DirectoryNumber = @DirectoryNumber";
                        customerDirectorySettings = connection.Query<CustomerDirectorySetting>(sql, new { DirectoryCustomer = customerDirectorySetting.DirectoryCustomer, DirectoryNumber = customerDirectorySetting.DirectoryNumber }).ToList();
                    }
                });
                if (customerDirectorySettings.Count == 0)
                {
                    await Task.Run(() =>
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"INSERT INTO {CustomerDirectoryLog}(DirectoryCustomer,DirectoryNumber,DirectoryName,JobTitle,Phone,MobilePhone,Email,Remark) VALUES " +
                                $"(@DirectoryCustomer,@DirectoryNumber,@DirectoryName,@JobTitle,@Phone,@MobilePhone,@Email,@Remark)";
                            DateIndex = connection.Execute(sql, new
                            {
                                DirectoryCustomer = customerDirectorySetting.DirectoryCustomer,
                                DirectoryNumber = customerDirectorySetting.DirectoryNumber,
                                DirectoryName = customerDirectorySetting.DirectoryName,
                                JobTitle = customerDirectorySetting.JobTitle,
                                Phone = customerDirectorySetting.Phone,
                                MobilePhone = customerDirectorySetting.MobilePhone,
                                Email = customerDirectorySetting.Email,
                                Remark = customerDirectorySetting.Remark
                            });
                        }
                    });
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
        [HttpPut]
        public async Task<IActionResult> UpdateCustomerDirectory(CustomerDirectorySetting customerDirectorySetting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE {CustomerDirectoryLog} SET DirectoryCustomer = @DirectoryCustomer,DirectoryName = @DirectoryName,JobTitle = @JobTitle,Phone = @Phone,MobilePhone = @MobilePhone,Email = @Email,Remark = @Remark" +
                            $" WHERE DirectoryCustomer = @DirectoryCustomer AND  DirectoryNumber = @DirectoryNumber";
                        DateIndex = connection.Execute(sql, new
                        {
                            DirectoryCustomer = customerDirectorySetting.DirectoryCustomer,
                            DirectoryNumber = customerDirectorySetting.DirectoryNumber,
                            DirectoryName = customerDirectorySetting.DirectoryName,
                            JobTitle = customerDirectorySetting.JobTitle,
                            Phone = customerDirectorySetting.Phone,
                            MobilePhone = customerDirectorySetting.MobilePhone,
                            Email = customerDirectorySetting.Email,
                            Remark = customerDirectorySetting.Remark
                        });
                    }
                });
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
        /// 客戶通訊錄資訊刪除
        /// </summary>
        /// <param name="customerDirectorySetting">客戶通訊錄資訊物件</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerDirectory(CustomerDirectorySetting customerDirectorySetting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    WorkPath += $"\\{customerDirectorySetting.DirectoryCustomer}";
                    if (Directory.Exists(WorkPath))
                    {
                        foreach (string file in Directory.GetFileSystemEntries(WorkPath))
                        {
                            if (System.IO.File.Exists(file))
                            {
                                System.IO.File.Delete(file);
                            }
                        }
                        Directory.Delete(WorkPath);
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"DELETE FROM {CustomerDirectoryLog} WHERE DirectoryCustomer = @DirectoryCustomer AND  DirectoryNumber = @DirectoryNumber";
                        DateIndex = connection.Execute(sql, new
                        {
                            DirectoryCustomer = customerDirectorySetting.DirectoryCustomer,
                            DirectoryNumber = customerDirectorySetting.DirectoryNumber,
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{customerDirectorySetting.DirectoryName}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{customerDirectorySetting.DirectoryName}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{customerDirectorySetting.DirectoryName}資訊，刪除失敗");
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
        [Route("/api/CustomerDirectoryAttachmentFile")]
        public async Task<IActionResult> PostCustomerDirectoryAttachmentFile(string DirectoryCustomer, string DirectoryNumber, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(DirectoryCustomer) && !string.IsNullOrEmpty(DirectoryNumber))
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{DirectoryCustomer}";
                        if (!Directory.Exists(WorkPath))
                        {
                            Directory.CreateDirectory($"{WorkPath}");
                        }
                        else
                        {
                            foreach (string file in Directory.GetFileSystemEntries(WorkPath))
                            {
                                if (System.IO.File.Exists(file))
                                {
                                    System.IO.File.Delete(file);
                                }
                            }
                        }
                        WorkPath += $"\\{AttachmentFile.FileName}";
                        using (var stream = new FileStream(WorkPath, FileMode.Create, FileAccess.Write))
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
                            string sql = $"UPDATE {CustomerDirectoryLog} SET FileName = @FileName WHERE DirectoryCustomer = @DirectoryCustomer AND DirectoryNumber = @DirectoryNumber";
                            connection.Execute(sql, new { FileName = AttachmentFile.FileName, DirectoryCustomer = DirectoryCustomer, DirectoryNumber = DirectoryNumber });
                        }
                    });
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
        /// <summary>
        /// 客戶通訊錄附件檔案下載
        /// </summary>
        /// <param name="DirectoryCustomer">客戶編碼</param>
        /// <param name="DirectoryNumber">客戶通訊錄編碼</param>
        /// <param name="AttachmentFile">附件檔案</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/CustomerDirectoryAttachmentFile")]
        public async Task<IActionResult> GetCustomerDirectoryAttachmentFile(string DirectoryCustomer, string DirectoryNumber, string AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(DirectoryCustomer) && !string.IsNullOrEmpty(DirectoryNumber))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{DirectoryCustomer}\\{AttachmentFile}";
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
