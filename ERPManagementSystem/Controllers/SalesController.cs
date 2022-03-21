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
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERPManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        public SalesController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "銷貨";
            SqlDB = _configuration["SqlDB"];
        }

        /// <summary>
        /// 查詢全部【銷貨】和【銷貨退出】父子資料
        /// </summary>
        /// <returns></returns>
        // GET: api/<SalesController>
        [HttpGet]
        public async Task<List<SalesSetting>> GetSales()
        {
            List<SalesSetting> result = new List<SalesSetting>();
            List<SalesSubSetting> Sub = new List<SalesSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  SalesMainSetting order by SalesFlag,SalesNumber ";
                        result = connection.Query<SalesSetting>(sql).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  SalesSubSetting order by SalesFlag,SalesNumber,SalesNo ";
                        Sub = connection.Query<SalesSubSetting>(sql).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].SalesSub = (from S in Sub
                                                  where S.SalesFlag == result[i].SalesFlag && S.SalesNumber == result[i].SalesNumber
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
        /// 查詢全部【單】【銷貨】或【銷貨退出】銷貨父子資料
        /// <param name="SalesFlag">銷貨旗標 </param>
        /// </summary>
        /// <returns></returns>
        [HttpGet("{SalesFlag}")]
        public async Task<List<SalesSetting>> GetSales(int SalesFlag)
        {
            List<SalesSetting> result = new List<SalesSetting>();
            List<SalesSubSetting> Sub = new List<SalesSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  SalesMainSetting " +
                                        $"where SalesFlag=@SalesFlag " +
                                        $"order by SalesFlag,SalesNumber ";
                        result = connection.Query<SalesSetting>(sql, new { SalesFlag = SalesFlag }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  SalesSubSetting " +
                                        $"where SalesFlag=@SalesFlag " +
                                        $"order by SalesFlag,SalesNumber,SalesNo ";
                        Sub = connection.Query<SalesSubSetting>(sql, new { SalesFlag = SalesFlag }).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].SalesSub = (from S in Sub
                                                  where S.SalesFlag == result[i].SalesFlag && S.SalesNumber == result[i].SalesNumber
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

        /// 查詢單筆【銷貨】或【銷貨退出】父子資料
        /// </summary>
        /// <param name="SalesFlag">銷貨旗標 </param>
        /// <param name="SalesNumber">銷貨單號</param>
        /// <returns></returns>
        [HttpGet("{SalesFlag}/{SalesNumber}")]
        public async Task<List<SalesSetting>> GetSales(int SalesFlag, string SalesNumber)
        {
            List<SalesSetting> result = new List<SalesSetting>();
            List<SalesSubSetting> Sub = new List<SalesSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  SalesMainSetting " +
                                        $"where SalesFlag=@SalesFlag and SalesNumber=@SalesNumber " +
                                        $"order by SalesFlag,SalesNumber ";
                        result = connection.Query<SalesSetting>(sql, new { SalesFlag = SalesFlag, SalesNumber = SalesNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  SalesSubSetting " +
                                        $"where SalesFlag=@SalesFlag and SalesNumber=@SalesNumber " +
                                        $"order by SalesFlag,SalesNumber,SalesNo ";
                        Sub = connection.Query<SalesSubSetting>(sql, new { SalesFlag = SalesFlag, SalesNumber = SalesNumber }).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].SalesSub = (from S in Sub
                                                  where S.SalesFlag == result[i].SalesFlag && S.SalesNumber == result[i].SalesNumber
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
        /// 查詢全部【單】【銷貨】或【銷貨退出】進貨父資料
        /// </summary>
        /// <param name="SalesNumber">銷貨單號(YYYYmm)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Sales/SalesNumber")]
        public async Task<List<SalesMainSetting>> GetSales(string SalesNumber)
        {
            List<SalesMainSetting> salesMains = new List<SalesMainSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  SalesMainSetting " +
                                        $"where SalesNumber >= @SalesNumberStart AND  SalesNumber <= @SalesNumberEnd " +
                                        $"order by SalesFlag,SalesNumber ";
                        salesMains = connection.Query<SalesMainSetting>(sql, new { SalesNumberStart = SalesNumber + "010000", SalesNumberEnd = SalesNumber + "319999" }).ToList();
                    }
                });
                return salesMains;
            }
            catch (Exception)
            {
                return salesMains;
            }
        }
        /// <summary>
        /// 查詢全部未過帳【單】【銷貨】或【銷貨退出】進貨父資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Sales/SalesPosting")]
        public async Task<List<SalesMainSetting>> GetSalesPosting()
        {
            List<SalesMainSetting> salesMains = new List<SalesMainSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  SalesMainSetting " +
                                        $"where Posting = 0 " +
                                        $"order by SalesFlag,SalesNumber ";
                        salesMains = connection.Query<SalesMainSetting>(sql).ToList();
                    }
                });
                return salesMains;
            }
            catch (Exception)
            {
                return salesMains;
            }
        }
        /// <summary>
        /// 銷貨父子新增
        /// </summary>
        /// <param name="value"></param>
        // POST api/<SalesMainController>
        [HttpPost]
        public async Task<List<SalesSetting>> PostSales([FromBody] SalesSetting salesSetting)
        {
            List<SalesSetting> result = new List<SalesSetting>();
            List<SalesMainSetting> CheckSalesNumber = new List<SalesMainSetting>();
            string ResultSalesNumber;
            try
            {
                await Task.Run(() =>
                {
                    //salesMainSetting.SalesFlag = 1;       // 1.銷貨，2.銷貨退出
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                    {
                        string sql = $"SELECT top 1 * FROM  SalesMainSetting " +
                                        $"where SalesFlag=@SalesFlag and SalesNumber like @SalesNumber " +
                                        $"order by SalesNumber desc ";
                        CheckSalesNumber = connection.Query<SalesMainSetting>(sql, new { SalesFlag = salesSetting.SalesFlag, SalesNumber = salesSetting.SalesDate.ToString("yyyyMMdd") + "%" }).ToList();
                    }
                    if (CheckSalesNumber.Count == 0)
                    {
                        ResultSalesNumber = salesSetting.SalesDate.ToString("yyyyMMdd") + "0001";     // 當天無進貨，由 0001開始
                    }
                    else
                    {
                        ResultSalesNumber = salesSetting.SalesDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckSalesNumber[0].SalesNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆進貨單號+1
                    }
                    salesSetting.SalesNumber = ResultSalesNumber;
                    double mTotal = 0;
                    for (int i = 0; i < salesSetting.SalesSub.Count(); i++)
                    {
                        salesSetting.SalesSub[i].SalesFlag = salesSetting.SalesFlag;
                        salesSetting.SalesSub[i].SalesNumber = salesSetting.SalesNumber;
                        mTotal += Convert.ToDouble(salesSetting.SalesSub[i].ProductTotal);
                    }
                    salesSetting.Total = mTotal;

                    if (salesSetting.SalesTax == 0)
                    {
                        salesSetting.Tax = Math.Round(mTotal * 0.05, 0);
                    }
                    salesSetting.TotalTax = mTotal + salesSetting.Tax;
                    //using (var ts = new TransactionScope())
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增父
                        {
                            string sql = $"INSERT INTO SalesMainSetting(SalesFlag , SalesNumber , SalesDate , SalesCustomerNumber , SalesTax , SalesInvoiceNo , SalesEmployeeNumber , Remark , Total , Tax , TotalTax , Posting , FileName ) " +
                                            $"Values(@SalesFlag , @SalesNumber , @SalesDate , @SalesCustomerNumber , @SalesTax , @SalesInvoiceNo , @SalesEmployeeNumber , @Remark , @Total , @Tax , @TotalTax , @Posting , @FileName)";
                            connection.Execute(sql, salesSetting);
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                        {
                            string sql = $"INSERT INTO SalesSubSetting(SalesFlag , SalesNumber , SalesNo , ProductNumber , ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal ) " +
                                            $"Values(@SalesFlag , @SalesNumber , @SalesNo , @ProductNumber , @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal)";
                            connection.Execute(sql, salesSetting.SalesSub);
                        }
                        //ts.Complete();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢父
                    {
                        string sql = $"SELECT * FROM  SalesMainSetting where SalesFlag=@SalesFlag and SalesNumber=@SalesNumber ";
                        result = connection.Query<SalesSetting>(sql, new { SalesFlag = salesSetting.SalesFlag, SalesNumber = ResultSalesNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢子
                    {
                        string sql = $"SELECT * FROM  SalesSubSetting " +
                                        $"where SalesFlag=@SalesFlag and SalesNumber=@SalesNumber " +
                                        $"order by SalesFlag , SalesNumber , SalesNo ";
                        result[0].SalesSub = connection.Query<SalesSubSetting>(sql, new { SalesFlag = salesSetting.SalesFlag, SalesNumber = ResultSalesNumber }).ToList();
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
        /// 銷貨父更新
        /// </summary>
        // PUT api/<SalesController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateSales(SalesSetting salesSetting)
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
                            string sql = $"UPDATE SalesMainSetting SET " +
                                        $"SalesDate = @SalesDate , SalesCustomerNumber = @SalesCustomerNumber , SalesTax = @SalesTax , SalesInvoiceNo= @SalesInvoiceNo , " +
                                        $"SalesEmployeeNumber = @SalesEmployeeNumber , Remark = @Remark , Total  = @Total , Tax = @Tax , TotalTax = @TotalTax , Posting = @Posting , FileName = @FileName " +
                                        "Where SalesFlag=@SalesFlag and SalesNumber=@SalesNumber ";
                            DateMainIndex = connection.Execute(sql, salesSetting);
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete SalesSubSetting Where SalesFlag=@SalesFlag and SalesNumber=@SalesNumber ";
                                connection.Execute(DeleteSql, new { SalesFlag = salesSetting.SalesFlag, SalesNumber = salesSetting.SalesNumber });
                            }
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                            {
                                string sql = $"INSERT INTO SalesSubSetting(SalesFlag , SalesNumber , SalesNo , ProductNumber , ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal ) " +
                                                $"Values(@SalesFlag , @SalesNumber , @SalesNo , @ProductNumber , @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal)";
                                connection.Execute(sql, salesSetting.SalesSub);
                            }
                        });
                        return Ok($"{salesSetting.SalesNumber}資訊，更新成功!");
                    }
                    else
                    {
                        return BadRequest($"{salesSetting.SalesNumber}資訊，更新父資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{salesSetting.SalesNumber}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 更新父資料
        /// </summary>
        /// <param name="salesMainSetting"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/Sales/UpdateSalesMain")]
        public async Task<IActionResult> UpdateSalesMain(SalesMainSetting salesMainSetting)
        {
            try
            {
                int DateMainIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE SalesMainSetting SET " +
                                    $" Posting = @Posting " +
                                    "Where SalesFlag=@SalesFlag and SalesNumber=@SalesNumber ";
                        DateMainIndex = connection.Execute(sql, salesMainSetting);
                    }
                });
                if (DateMainIndex > 0)
                {
                    return Ok($"{salesMainSetting.SalesNumber}資訊，更新父資料成功!");
                }
                else
                {
                    return BadRequest($"{salesMainSetting.SalesNumber}資訊，更新父資料失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{salesMainSetting.SalesNumber}資訊，更新父資料失敗");
            }
        }
        // DELETE api/<SalesController>/5
        [HttpDelete("{SalesFlag}/{SalesNumber}")]
        public async Task<IActionResult> DeleteSales(int SalesFlag, string SalesNumber)
        {
            try
            {
                int DateMainIndex = 0;
                //using (var ts = new TransactionScope())
                {
                    await Task.Run(() =>
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string DeleteSql = "delete SalesMainSetting Where SalesFlag=@SalesFlag and SalesNumber=@SalesNumber ";
                            DateMainIndex = connection.Execute(DeleteSql, new { SalesFlag = SalesFlag, SalesNumber = SalesNumber });
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete SalesSubSetting Where SalesFlag=@SalesFlag and SalesNumber=@SalesNumber ";
                                DateMainIndex = connection.Execute(DeleteSql, new { SalesFlag = SalesFlag, SalesNumber = SalesNumber });
                            }
                        });
                        return Ok($"{SalesNumber}資訊，刪除成功!");
                    }
                    else
                    {
                        return BadRequest($"{SalesNumber}資訊，刪除資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{SalesNumber}資訊，更新失敗");
            }
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="SalesFlag"></param>
        /// <param name="SalesCustomerNumber"></param>
        /// <param name="SalesDate"></param>
        /// <param name="SalesNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/SalesAttachmentFile")]
        public async Task<IActionResult> PostSalesAttachmenFile(int SalesFlag, string SalesCustomerNumber, DateTime SalesDate, string SalesNumber, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null)
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{SalesCustomerNumber}";
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
                        List<SalesMainSetting> CheckSalesNumber = new List<SalesMainSetting>();
                        string ResultSalesNumber;
                        if (SalesNumber == null)
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                            {
                                string sql = $"SELECT top 1 * FROM  SalesMainSetting " +
                                                $"where SalesFlag=@SalesFlag and SalesNumber like @SalesNumber " +
                                                $"order by SalesNumber desc ";
                                CheckSalesNumber = connection.Query<SalesMainSetting>(sql, new { SalesFlag = SalesFlag, SalesNumber = SalesDate.ToString("yyyyMMdd") + "%" }).ToList();
                            }
                            if (CheckSalesNumber.Count == 0)
                            {
                                ResultSalesNumber = SalesDate.ToString("yyyyMMdd") + "0001";     // 當天無進貨，由 0001開始
                            }
                            else
                            {
                                ResultSalesNumber = SalesDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckSalesNumber[0].SalesNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆進貨單號+1
                            }
                            SalesNumber = ResultSalesNumber;
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"UPDATE SalesMainSetting SET FileName = @FileName  WHERE SalesFlag=@SalesFlag and SalesNumber=@SalesNumber";
                            connection.Execute(sql, new { FileName = AttachmentFile.FileName, SalesFlag = SalesFlag, SalesNumber = SalesNumber });
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
        /// <param name="SalesCustomerNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/SalesAttachmentFile")]
        public async Task<IActionResult> GetSalesAttachmenFile(string SalesCustomerNumber, string AttachmentFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(SalesCustomerNumber) && !string.IsNullOrEmpty(AttachmentFile))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{SalesCustomerNumber}\\{AttachmentFile}";
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
