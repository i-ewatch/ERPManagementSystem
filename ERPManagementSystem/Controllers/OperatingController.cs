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
    public class OperatingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        public OperatingController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "營運";
            SqlDB = _configuration["SqlDB"];
        }

        /// <summary>
        /// 查詢全部【營運】和【營運退出】父子資料
        /// </summary>
        /// <returns></returns>
        // GET: api/<OperatingController>
        [HttpGet]
        public async Task<List<OperatingSetting>> GetOperating()
        {
            List<OperatingSetting> result = new List<OperatingSetting>();
            List<OperatingSubSetting> Sub = new List<OperatingSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  OperatingMainSetting order by OperatingFlag,OperatingNumber ";
                        result = connection.Query<OperatingSetting>(sql).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  OperatingSubSetting order by OperatingFlag,OperatingNumber,OperatingNo ";
                        Sub = connection.Query<OperatingSubSetting>(sql).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].OperatingSub = (from S in Sub
                                                     where S.OperatingFlag == result[i].OperatingFlag && S.OperatingNumber == result[i].OperatingNumber
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
        /// 查詢全部【單】【營運】或【營運退出】營運父子資料
        /// <param name="OperatingFlag">營運旗標 </param>
        /// </summary>
        /// <returns></returns>
        [HttpGet("{OperatingFlag}")]
        public async Task<List<OperatingSetting>> GetOperating(int OperatingFlag)
        {
            List<OperatingSetting> result = new List<OperatingSetting>();
            List<OperatingSubSetting> Sub = new List<OperatingSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  OperatingMainSetting " +
                                        $"where OperatingFlag=@OperatingFlag " +
                                        $"order by OperatingFlag,OperatingNumber ";
                        result = connection.Query<OperatingSetting>(sql, new { OperatingFlag = OperatingFlag }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  OperatingSubSetting " +
                                        $"where OperatingFlag=@OperatingFlag " +
                                        $"order by OperatingFlag,OperatingNumber,OperatingNo ";
                        Sub = connection.Query<OperatingSubSetting>(sql, new { OperatingFlag = OperatingFlag }).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].OperatingSub = (from S in Sub
                                                     where S.OperatingFlag == result[i].OperatingFlag && S.OperatingNumber == result[i].OperatingNumber
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
        /// 查詢單筆【營運】或【營運退出】父子資料
        /// </summary>
        /// <param name="OperatingFlag">營運旗標 </param>
        /// <param name="OperatingNumber">營運單號</param>
        /// <returns></returns>
        [HttpGet("{OperatingFlag}/{OperatingNumber}")]
        public async Task<List<OperatingSetting>> GetOperating(int OperatingFlag, string OperatingNumber)
        {
            List<OperatingSetting> result = new List<OperatingSetting>();
            List<OperatingSubSetting> Sub = new List<OperatingSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  OperatingMainSetting " +
                                        $"where OperatingFlag=@OperatingFlag and OperatingNumber=@OperatingNumber " +
                                        $"order by OperatingFlag,OperatingNumber ";
                        result = connection.Query<OperatingSetting>(sql, new { OperatingFlag = OperatingFlag, OperatingNumber = OperatingNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  OperatingSubSetting " +
                                        $"where OperatingFlag=@OperatingFlag and OperatingNumber=@OperatingNumber " +
                                        $"order by OperatingFlag,OperatingNumber,OperatingNo ";
                        Sub = connection.Query<OperatingSubSetting>(sql, new { OperatingFlag = OperatingFlag, OperatingNumber = OperatingNumber }).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].OperatingSub = (from S in Sub
                                                     where S.OperatingFlag == result[i].OperatingFlag && S.OperatingNumber == result[i].OperatingNumber
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
        /// 查詢全部【單】【營運】或【營運退出】營運父資料
        /// </summary>
        /// <param name="OperatingNumber">營運單號(YYYYmm)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Operating/OperatingNumber")]
        public async Task<List<OperatingMainSetting>> GetOperating(string OperatingNumber)
        {
            List<OperatingMainSetting> purchaseMains = new List<OperatingMainSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  OperatingMainSetting " +
                                        $"where OperatingNumber >= @OperatingNumberStart AND  OperatingNumber <= @OperatingNumberEnd " +
                                        $"order by OperatingFlag,OperatingNumber ";
                        purchaseMains = connection.Query<OperatingMainSetting>(sql, new { OperatingNumberStart = OperatingNumber + "010000", OperatingNumberEnd = OperatingNumber + "319999" }).ToList();
                    }
                });
                return purchaseMains;
            }
            catch (Exception)
            {
                return purchaseMains;
            }
        }
        /// <summary>
        /// 查詢全部未過帳【單】【營運】或【營運退出】營運父資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Operating/OperatingPosting")]
        public async Task<List<OperatingMainSetting>> GetOperatingPosting()
        {
            List<OperatingMainSetting> purchaseMains = new List<OperatingMainSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  OperatingMainSetting " +
                                        $"where  Posting = 0 " +
                                        $"order by OperatingFlag,OperatingNumber ";
                        purchaseMains = connection.Query<OperatingMainSetting>(sql).ToList();
                    }
                });
                return purchaseMains;
            }
            catch (Exception)
            {
                return purchaseMains;
            }
        }
        /// <summary>
        /// 營運父子新增
        /// </summary>
        /// <param name="value"></param>
        // POST api/<OperatingMainController>
        [HttpPost]
        public async Task<List<OperatingSetting>> PostOperating([FromBody] OperatingSetting purchaseSetting)
        {
            List<OperatingSetting> result = new List<OperatingSetting>();
            List<OperatingMainSetting> CheckOperatingNumber = new List<OperatingMainSetting>();
            string ResultOperatingNumber;
            try
            {
                await Task.Run(() =>
                {
                    //purchaseMainSetting.OperatingFlag = 1;       // 1.營運，2.營運退出
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                    {
                        string sql = $"SELECT top 1 * FROM  OperatingMainSetting " +
                                        $"where OperatingFlag=@OperatingFlag and OperatingNumber like @OperatingNumber " +
                                        $"order by OperatingNumber desc ";
                        CheckOperatingNumber = connection.Query<OperatingMainSetting>(sql, new { OperatingFlag = purchaseSetting.OperatingFlag, OperatingNumber = purchaseSetting.OperatingDate.ToString("yyyyMMdd") + "%" }).ToList();
                    }
                    if (CheckOperatingNumber.Count == 0)
                    {
                        ResultOperatingNumber = purchaseSetting.OperatingDate.ToString("yyyyMMdd") + "0001";     // 當天無營運，由 0001開始
                    }
                    else
                    {
                        ResultOperatingNumber = purchaseSetting.OperatingDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckOperatingNumber[0].OperatingNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆營運單號+1
                    }
                    purchaseSetting.OperatingNumber = ResultOperatingNumber;
                    double mTotal = 0;
                    for (int i = 0; i < purchaseSetting.OperatingSub.Count(); i++)
                    {
                        purchaseSetting.OperatingSub[i].OperatingFlag = purchaseSetting.OperatingFlag;
                        purchaseSetting.OperatingSub[i].OperatingNumber = purchaseSetting.OperatingNumber;
                        mTotal += Convert.ToDouble(purchaseSetting.OperatingSub[i].ProductTotal);
                    }
                    purchaseSetting.Total = mTotal;

                    if (purchaseSetting.OperatingTax == 0)
                    {
                        purchaseSetting.Tax = Math.Round(mTotal * 0.05, 0);
                    }
                    purchaseSetting.TotalTax = mTotal + purchaseSetting.Tax;
                    //using (var ts = new TransactionScope())
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增父
                        {
                            string sql = $"INSERT INTO OperatingMainSetting(OperatingFlag , OperatingNumber , ProjectNumber , OperatingDate , OperatingCompanyNumber , OperatingTax , OperatingInvoiceNo , OperatingEmployeeNumber , Remark , Total , Tax , TotalTax , Posting , FileName , PostingDate ) " +
                                            $"Values(@OperatingFlag , @OperatingNumber, @ProjectNumber , @OperatingDate , @OperatingCompanyNumber , @OperatingTax , @OperatingInvoiceNo , @OperatingEmployeeNumber , @Remark , @Total , @Tax , @TotalTax , @Posting , @FileName , @PostingDate)";
                            connection.Execute(sql, purchaseSetting);
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                        {
                            string sql = $"INSERT INTO OperatingSubSetting(OperatingFlag , OperatingNumber , OperatingNo , ProductNumber , ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal ) " +
                                            $"Values(@OperatingFlag , @OperatingNumber , @OperatingNo , @ProductNumber , @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal)";
                            connection.Execute(sql, purchaseSetting.OperatingSub);
                        }
                        //ts.Complete();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢父
                    {
                        string sql = $"SELECT * FROM  OperatingMainSetting where OperatingFlag=@OperatingFlag and OperatingNumber=@OperatingNumber ";
                        result = connection.Query<OperatingSetting>(sql, new { OperatingFlag = purchaseSetting.OperatingFlag, OperatingNumber = ResultOperatingNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢子
                    {
                        string sql = $"SELECT * FROM  OperatingSubSetting " +
                                        $"where OperatingFlag=@OperatingFlag and OperatingNumber=@OperatingNumber " +
                                        $"order by OperatingFlag , OperatingNumber , OperatingNo ";
                        result[0].OperatingSub = connection.Query<OperatingSubSetting>(sql, new { OperatingFlag = purchaseSetting.OperatingFlag, OperatingNumber = ResultOperatingNumber }).ToList();
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
        /// 營運父子更新
        /// </summary>
        // PUT api/<OperatingController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateOperating(OperatingSetting purchaseSetting)
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
                            string sql = $"UPDATE OperatingMainSetting SET " +
                                        $"OperatingDate = @OperatingDate , OperatingCompanyNumber = @OperatingCompanyNumber , ProjectNumber = @ProjectNumber , OperatingTax = @OperatingTax , OperatingInvoiceNo= @OperatingInvoiceNo , " +
                                        $"OperatingEmployeeNumber = @OperatingEmployeeNumber , Remark = @Remark , Total  = @Total , Tax = @Tax , TotalTax = @TotalTax , Posting = @Posting , FileName = @FileName , PostingDate = @PostingDate " +
                                        "Where OperatingFlag=@OperatingFlag and OperatingNumber=@OperatingNumber ";
                            DateMainIndex = connection.Execute(sql, purchaseSetting);
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete OperatingSubSetting Where OperatingFlag=@OperatingFlag and OperatingNumber=@OperatingNumber ";
                                connection.Execute(DeleteSql, new { OperatingFlag = purchaseSetting.OperatingFlag, OperatingNumber = purchaseSetting.OperatingNumber });
                            }
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                            {
                                string sql = $"INSERT INTO OperatingSubSetting(OperatingFlag , OperatingNumber , OperatingNo , ProductNumber , ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal ) " +
                                                $"Values(@OperatingFlag , @OperatingNumber , @OperatingNo , @ProductNumber , @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal)";
                                connection.Execute(sql, purchaseSetting.OperatingSub);
                            }
                        });
                        return Ok($"{purchaseSetting.OperatingNumber}資訊，更新成功!");
                    }
                    else
                    {
                        return BadRequest($"{purchaseSetting.OperatingNumber}資訊，更新父資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{purchaseSetting.OperatingNumber}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 營運父更新
        /// </summary>
        /// <param name="purchaseMainSetting"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/Operating/UpdateOperatingMain")]
        public async Task<IActionResult> UpdateOperatingMain(OperatingMainSetting purchaseMainSetting)
        {
            try
            {
                int DateMainIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE OperatingMainSetting SET " +
                                    "Posting = @Posting , PostingDate = @PostingDate " +
                                    "Where OperatingFlag=@OperatingFlag and OperatingNumber=@OperatingNumber ";
                        DateMainIndex = connection.Execute(sql, purchaseMainSetting);
                    }
                });
                if (DateMainIndex > 0)
                {
                    return Ok($"{purchaseMainSetting.OperatingNumber}資訊，更新父資料成功!");
                }
                else
                {
                    return BadRequest($"{purchaseMainSetting.OperatingNumber}資訊，更新父資料失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{purchaseMainSetting.OperatingNumber}資訊，更新父資料失敗");
            }
        }
        // DELETE api/<OperatingController>/5
        [HttpDelete("{OperatingFlag}/{OperatingNumber}")]
        public async Task<IActionResult> DeleteOperating(int OperatingFlag, string OperatingNumber)
        {
            try
            {
                int DateMainIndex = 0;
                //using (var ts = new TransactionScope())
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{OperatingNumber}";
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
                            string DeleteSql = "delete OperatingMainSetting Where OperatingFlag=@OperatingFlag and OperatingNumber=@OperatingNumber ";
                            DateMainIndex = connection.Execute(DeleteSql, new { OperatingFlag = OperatingFlag, OperatingNumber = OperatingNumber });
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete OperatingSubSetting Where OperatingFlag=@OperatingFlag and OperatingNumber=@OperatingNumber ";
                                DateMainIndex = connection.Execute(DeleteSql, new { OperatingFlag = OperatingFlag, OperatingNumber = OperatingNumber });
                            }
                        });
                        return Ok($"{OperatingNumber}資訊，刪除成功!");
                    }
                    else
                    {
                        return BadRequest($"{OperatingNumber}資訊，刪除資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{OperatingNumber}資訊，刪除失敗");
            }
        }
        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="OperatingFlag"></param>
        /// <param name="OperatingCompanyNumber"></param>
        /// <param name="OperatingDate"></param>
        /// <param name="OperatingNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/OperatingAttachmentFile")]
        public async Task<IActionResult> PostOperatingAttachmenFile(int OperatingFlag, DateTime OperatingDate, string OperatingNumber, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null)
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{OperatingNumber}";
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
                        List<OperatingMainSetting> CheckOperatingNumber = new List<OperatingMainSetting>();
                        string ResultOperatingNumber;
                        if (OperatingNumber == null)
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                            {
                                string sql = $"SELECT top 1 * FROM  OperatingMainSetting " +
                                                $"where OperatingFlag=@OperatingFlag and OperatingNumber like @OperatingNumber " +
                                                $"order by OperatingNumber desc ";
                                CheckOperatingNumber = connection.Query<OperatingMainSetting>(sql, new { OperatingFlag = OperatingFlag, OperatingNumber = OperatingDate.ToString("yyyyMMdd") + "%" }).ToList();
                            }
                            if (CheckOperatingNumber.Count == 0)
                            {
                                ResultOperatingNumber = OperatingDate.ToString("yyyyMMdd") + "0001";     // 當天無營運，由 0001開始
                            }
                            else
                            {
                                ResultOperatingNumber = OperatingDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckOperatingNumber[0].OperatingNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆營運單號+1
                            }
                            OperatingNumber = ResultOperatingNumber;
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"UPDATE OperatingMainSetting SET FileName = @FileName  WHERE OperatingFlag=@OperatingFlag and OperatingNumber=@OperatingNumber";
                            connection.Execute(sql, new { FileName = AttachmentFile.FileName, OperatingFlag = OperatingFlag, OperatingNumber = OperatingNumber });
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
        /// <param name="OperatingNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/OperatingAttachmentFile")]
        public async Task<IActionResult> GetOperatingAttachmenFile(string OperatingNumber, string AttachmentFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(OperatingNumber) && !string.IsNullOrEmpty(AttachmentFile))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{OperatingNumber}\\{AttachmentFile}";
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
