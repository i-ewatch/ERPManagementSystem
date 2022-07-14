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
    public class QuotationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        public QuotationController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "報價單";
            SqlDB = _configuration["SqlDB"];
        }
        /// <summary>
        /// 查詢單筆報價單父子資料
        /// </summary>
        /// <param name="QuotationNumber">進貨單號</param>
        /// <returns></returns>
        [HttpGet("{QuotationNumber}")]
        public async Task<List<QuotationSetting>> GetQuotation(string QuotationNumber)
        {
            List<QuotationSetting> result = new List<QuotationSetting>();
            List<QuotationSubSetting> Sub = new List<QuotationSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  QuotationMainSetting " +
                                        $"where QuotationNumber=@QuotationNumber " +
                                        $"order by QuotationNumber ";
                        result = connection.Query<QuotationSetting>(sql, new { QuotationNumber = QuotationNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  QuotationSubSetting " +
                                        $"where  QuotationNumber=@QuotationNumber " +
                                        $"order by QuotationNumber ";
                        Sub = connection.Query<QuotationSubSetting>(sql, new { QuotationNumber = QuotationNumber }).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].QuotationSub = (from S in Sub
                                                      where S.QuotationNumber == result[i].QuotationNumber
                                                      select S).ToList();
                        }
                    }
                });
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        /// <summary>
        /// 查詢全部年份報價單父資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("Year")]
        public async Task<List<QuotationMainSetting>> GetQuotation_Year(string Year)
        {
            List<QuotationMainSetting> QuotationMains = new List<QuotationMainSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  QuotationMainSetting " +
                                        $"where LEFT(QuotationNumber,4) = @Year " +
                                        $"order by QuotationNumber ";
                        QuotationMains = connection.Query<QuotationMainSetting>(sql, new { Year }).ToList();

                        string customer = "SELECT * FROM CustomerSetting";
                        var Customer = connection.Query<CustomerSetting>(customer).ToList();
                        string customerdirectory = "SELECT * FROM CustomerDirectorySetting";
                        var CustomerDirectory = connection.Query<CustomerDirectorySetting>(customerdirectory).ToList();
                        string project = "SELECT * FROM ProjectSetting";
                        var Project = connection.Query<ProjectSetting>(project).ToList();
                        string employee = "SELECT * FROM EmployeeSetting";
                        var Employee = connection.Query<EmployeeSetting>(employee).ToList();
                        foreach (var item in QuotationMains)
                        {
                            var customerdata = Customer.SingleOrDefault(g => g.CustomerNumber == item.QuotationCustomerNumber);
                            var customerdirectorydata = CustomerDirectory.SingleOrDefault(g => g.DirectoryCustomer == item.QuotationCustomerNumber & g.DirectoryNumber == item.QuotationDirectoryNumber);
                            var projectdata = Project.SingleOrDefault(g => g.ProjectNumber == item.ProjectNumber);
                            var employeedata = Employee.SingleOrDefault(g => g.EmployeeNumber == item.QuotationEmployeeNumber);
                            if (customerdata != null)
                            {
                                item.QuotationCustomerNumber = customerdata.CustomerName;
                            }
                            if (customerdirectorydata != null)
                            {
                                item.QuotationDirectoryNumber = customerdirectorydata.DirectoryName;
                            }
                            if (projectdata != null)
                            {
                                item.ProjectNumber = projectdata.ProjectName;
                            }
                            if (employeedata != null)
                            {
                                item.QuotationEmployeeNumber = employeedata.EmployeeName;
                            }
                        }
                    }
                });
                return QuotationMains;
            }
            catch (Exception)
            {
                return QuotationMains;
            }
        }
        /// <summary>
        /// 報價單父子新增
        /// </summary>
        /// <param name="value"></param>
        // POST api/<OrderMainController>
        [HttpPost]
        public async Task<List<QuotationSetting>> PostQuotation([FromBody] QuotationSetting quotationSetting)
        {
            List<QuotationSetting> result = new List<QuotationSetting>();
            List<QuotationMainSetting> CheckQuotationNumber = new List<QuotationMainSetting>();
            string ResultQuotationNumber;
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                    {
                        string sql = $"SELECT top 1 * FROM  QuotationMainSetting " +
                                        $"where QuotationNumber like @QuotationNumber " +
                                        $"order by QuotationNumber desc ";
                        CheckQuotationNumber = connection.Query<QuotationMainSetting>(sql, new { QuotationNumber = quotationSetting.QuotationDate.ToString("yyyyMMdd") + "%" }).ToList();
                    }
                    if (CheckQuotationNumber.Count == 0)
                    {
                        ResultQuotationNumber = quotationSetting.QuotationDate.ToString("yyyyMMdd") + "0001";     // 當天無報價，由 0001開始
                    }
                    else
                    {
                        ResultQuotationNumber = quotationSetting.QuotationDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckQuotationNumber[0].QuotationNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆報價單號+1
                    }
                    quotationSetting.QuotationNumber = ResultQuotationNumber;
                    double mTotal = 0;
                    for (int i = 0; i < quotationSetting.QuotationSub.Count(); i++)
                    {
                        quotationSetting.QuotationSub[i].QuotationNumber = quotationSetting.QuotationNumber;
                        mTotal += Convert.ToDouble(quotationSetting.QuotationSub[i].ProductTotal);
                    }
                    quotationSetting.Total = mTotal;

                    if (quotationSetting.QuotationTax == 0)
                    {
                        quotationSetting.Tax = Math.Round(mTotal * 0.05, 0);
                    }
                    quotationSetting.TotalTax = mTotal + quotationSetting.Tax;
                    //using (var ts = new TransactionScope())
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增父
                        {
                            string sql = $"INSERT INTO QuotationMainSetting(QuotationNumber , ProjectNumber , QuotationDate , QuotationCustomerNumber , QuotationDirectoryNumber ,Address , QuotationEmployeeNumber , Remark ,QuotationTax , Total , Tax , TotalTax , TotalQty , FileName , QuotationNote,InvalidFlag ) " +
                                            $"Values( @QuotationNumber, @ProjectNumber , @QuotationDate , @QuotationCustomerNumber ,@QuotationDirectoryNumber ,@Address , @QuotationEmployeeNumber , @Remark ,@QuotationTax, @Total , @Tax , @TotalTax , @TotalQty , @FileName , @QuotationNote,@InvalidFlag)";
                            connection.Execute(sql, quotationSetting);
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                        {
                            string sql = $"INSERT INTO QuotationSubSetting( QuotationNumber , QuotationNo,QuotationSubNo ,QuotationThrNo ,  ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal,LineFlag ) " +
                                            $"Values( @QuotationNumber , @QuotationNo,@QuotationSubNo ,@QuotationThrNo , @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal,@LineFlag)";
                            connection.Execute(sql, quotationSetting.QuotationSub);
                        }
                        //ts.Complete();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢父
                    {
                        string sql = $"SELECT * FROM  QuotationMainSetting where QuotationNumber=@QuotationNumber ";
                        result = connection.Query<QuotationSetting>(sql, new { QuotationNumber = ResultQuotationNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢子
                    {
                        string sql = $"SELECT * FROM  QuotationSubSetting " +
                                        $"where QuotationNumber=@QuotationNumber " +
                                        $"order by  QuotationNumber , QuotationNo ";
                        result[0].QuotationSub = connection.Query<QuotationSubSetting>(sql, new { QuotationNumber = ResultQuotationNumber }).ToList();
                    }
                });
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        /// <summary>
        /// 報價單父子更新
        /// </summary>
        // PUT api/<QuotationController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateQuotation(QuotationSetting quotationSetting)
        {
            try
            {
                int DateMainIndex = 0;
                int DateSubIndex = 0;
                //using (var ts = new TransactionScope())
                {
                    await Task.Run(() =>
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"UPDATE QuotationMainSetting SET " +
                                        $"QuotationDate = @QuotationDate , QuotationCustomerNumber = @QuotationCustomerNumber , ProjectNumber = @ProjectNumber ," +
                                        $"QuotationEmployeeNumber = @QuotationEmployeeNumber , Remark = @Remark , QuotationTax = @QuotationTax,Total  = @Total , Tax = @Tax , TotalTax = @TotalTax , TotalQty = @TotalQty , FileName = @FileName , QuotationNote = @QuotationNote ,InvalidFlag = @InvalidFlag " +
                                        "Where  QuotationNumber=@QuotationNumber ";
                            DateMainIndex = connection.Execute(sql, quotationSetting);
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete QuotationSubSetting Where QuotationNumber=@QuotationNumber ";
                                connection.Execute(DeleteSql, new { QuotationNumber = quotationSetting.QuotationNumber });
                            }
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                            {
                                string sql = $"INSERT INTO QuotationSubSetting( QuotationNumber , QuotationNo ,QuotationSubNo ,QuotationThrNo ,  ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal,LineFlag ) " +
                                                $"Values( @QuotationNumber , @QuotationNo ,@QuotationSubNo ,@QuotationThrNo,  @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal,@LineFlag)";
                                connection.Execute(sql, quotationSetting.QuotationSub);
                            }
                        });
                        return Ok($"{quotationSetting.QuotationNumber}資訊，更新成功!");
                    }
                    else
                    {
                        return BadRequest($"{quotationSetting.QuotationNumber}資訊，更新父資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{quotationSetting.QuotationNumber}資訊，更新失敗");
            }
        }
        // DELETE api/<QuotationController>/5
        [HttpDelete("{QuotationNumber}")]
        public async Task<IActionResult> DeleteQuotation(string QuotationNumber)
        {
            try
            {
                int DateMainIndex = 0;
                //using (var ts = new TransactionScope())
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{QuotationNumber}";
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
                            string DeleteSql = "delete QuotationMainSetting Where  QuotationNumber=@QuotationNumber ";
                            DateMainIndex = connection.Execute(DeleteSql, new { QuotationNumber = QuotationNumber });
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete QuotationSubSetting Where  QuotationNumber=@QuotationNumber ";
                                DateMainIndex = connection.Execute(DeleteSql, new { QuotationNumber = QuotationNumber });
                            }
                        });
                        return Ok($"{QuotationNumber}資訊，刪除成功!");
                    }
                    else
                    {
                        return BadRequest($"{QuotationNumber}資訊，刪除資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{QuotationNumber}資訊，刪除失敗");
            }
        }
        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="QuotationDate"></param>
        /// <param name="QuotationNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/QuotationAttachmentFile")]
        public async Task<IActionResult> PostQuotationAttachmenFile(DateTime QuotationDate, string QuotationNumber, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null)
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{QuotationNumber}";
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
                        List<QuotationMainSetting> CheckQuotationNumber = new List<QuotationMainSetting>();
                        string ResultQuotationNumber;
                        if (QuotationNumber == null)
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                            {
                                string sql = $"SELECT top 1 * FROM  QuotationMainSetting " +
                                                $"where QuotationNumber like @QuotationNumber " +
                                                $"order by QuotationNumber desc ";
                                CheckQuotationNumber = connection.Query<QuotationMainSetting>(sql, new { QuotationNumber = QuotationDate.ToString("yyyyMMdd") + "%" }).ToList();
                            }
                            if (CheckQuotationNumber.Count == 0)
                            {
                                ResultQuotationNumber = QuotationDate.ToString("yyyyMMdd") + "0001";     // 當天無報價，由 0001開始
                            }
                            else
                            {
                                ResultQuotationNumber = QuotationDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckQuotationNumber[0].QuotationNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆報價單號+1
                            }
                            QuotationNumber = ResultQuotationNumber;
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"UPDATE QuotationMainSetting SET FileName = @FileName  WHERE  and QuotationNumber=@QuotationNumber";
                            connection.Execute(sql, new { FileName = AttachmentFile.FileName, QuotationNumber = QuotationNumber });
                        }
                    });
                    return Ok($"{AttachmentFile.FileName} 檔案上傳成功");
                }
                else
                {
                    return BadRequest($"{AttachmentFile.FileName} 檔案上傳失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{AttachmentFile.FileName} 檔案上傳失敗");
            }
        }
        /// <summary>
        /// 下載檔案
        /// </summary>
        /// <param name="QuotationNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/QuotationAttachmentFile")]
        public async Task<IActionResult> GetQuotationAttachmenFile(string QuotationNumber, string AttachmentFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(QuotationNumber) && !string.IsNullOrEmpty(AttachmentFile))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{QuotationNumber}\\{AttachmentFile}";
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
                    return BadRequest($"{AttachmentFile} 檔案下載失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{AttachmentFile} 檔案下載失敗");
            }
        }
    }
}
