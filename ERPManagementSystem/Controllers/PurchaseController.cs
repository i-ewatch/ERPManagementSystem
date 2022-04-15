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
    public class PurchaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        public PurchaseController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "進貨";
            SqlDB = _configuration["SqlDB"];
        }

        /// <summary>
        /// 查詢全部【進貨】和【進貨退出】父子資料
        /// </summary>
        /// <returns></returns>
        // GET: api/<PurchaseController>
        [HttpGet]
        public async Task<List<PurchaseSetting>> GetPurchase()
        {
            List<PurchaseSetting> result = new List<PurchaseSetting>();
            List<PurchaseSubSetting> Sub = new List<PurchaseSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PurchaseMainSetting order by PurchaseFlag,PurchaseNumber ";
                        result = connection.Query<PurchaseSetting>(sql).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PurchaseSubSetting order by PurchaseFlag,PurchaseNumber,PurchaseNo ";
                        Sub = connection.Query<PurchaseSubSetting>(sql).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].PurchaseSub = (from S in Sub
                                                     where S.PurchaseFlag == result[i].PurchaseFlag && S.PurchaseNumber == result[i].PurchaseNumber
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
        /// 查詢全部【單】【進貨】或【進貨退出】進貨父子資料
        /// <param name="PurchaseFlag">進貨旗標 </param>
        /// </summary>
        /// <returns></returns>
        [HttpGet("{PurchaseFlag}")]
        public async Task<List<PurchaseSetting>> GetPurchase(int PurchaseFlag)
        {
            List<PurchaseSetting> result = new List<PurchaseSetting>();
            List<PurchaseSubSetting> Sub = new List<PurchaseSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PurchaseMainSetting " +
                                        $"where PurchaseFlag=@PurchaseFlag " +
                                        $"order by PurchaseFlag,PurchaseNumber ";
                        result = connection.Query<PurchaseSetting>(sql, new { PurchaseFlag = PurchaseFlag }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PurchaseSubSetting " +
                                        $"where PurchaseFlag=@PurchaseFlag " +
                                        $"order by PurchaseFlag,PurchaseNumber,PurchaseNo ";
                        Sub = connection.Query<PurchaseSubSetting>(sql, new { PurchaseFlag = PurchaseFlag }).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].PurchaseSub = (from S in Sub
                                                     where S.PurchaseFlag == result[i].PurchaseFlag && S.PurchaseNumber == result[i].PurchaseNumber
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
        /// 查詢單筆【進貨】或【進貨退出】父子資料
        /// </summary>
        /// <param name="PurchaseFlag">進貨旗標 </param>
        /// <param name="PurchaseNumber">進貨單號</param>
        /// <returns></returns>
        [HttpGet("{PurchaseFlag}/{PurchaseNumber}")]
        public async Task<List<PurchaseSetting>> GetPurchase(int PurchaseFlag, string PurchaseNumber)
        {
            List<PurchaseSetting> result = new List<PurchaseSetting>();
            List<PurchaseSubSetting> Sub = new List<PurchaseSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PurchaseMainSetting " +
                                        $"where PurchaseFlag=@PurchaseFlag and PurchaseNumber=@PurchaseNumber " +
                                        $"order by PurchaseFlag,PurchaseNumber ";
                        result = connection.Query<PurchaseSetting>(sql, new { PurchaseFlag = PurchaseFlag, PurchaseNumber = PurchaseNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PurchaseSubSetting " +
                                        $"where PurchaseFlag=@PurchaseFlag and PurchaseNumber=@PurchaseNumber " +
                                        $"order by PurchaseFlag,PurchaseNumber,PurchaseNo ";
                        Sub = connection.Query<PurchaseSubSetting>(sql, new { PurchaseFlag = PurchaseFlag, PurchaseNumber = PurchaseNumber }).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].PurchaseSub = (from S in Sub
                                                     where S.PurchaseFlag == result[i].PurchaseFlag && S.PurchaseNumber == result[i].PurchaseNumber
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
        /// 查詢全部【單】【進貨】或【進貨退出】進貨父資料
        /// </summary>
        /// <param name="PurchaseNumber">進貨單號(YYYYmm)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Purchase/PurchaseNumber")]
        public async Task<List<PurchaseMainSetting>> GetPurchase(string PurchaseNumber)
        {
            List<PurchaseMainSetting> purchaseMains = new List<PurchaseMainSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PurchaseMainSetting " +
                                        $"where PurchaseNumber >= @PurchaseNumberStart AND  PurchaseNumber <= @PurchaseNumberEnd " +
                                        $"order by PurchaseFlag,PurchaseNumber ";
                        purchaseMains = connection.Query<PurchaseMainSetting>(sql, new { PurchaseNumberStart = PurchaseNumber + "010000", PurchaseNumberEnd = PurchaseNumber + "319999" }).ToList();
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
        /// 查詢全部未過帳【單】【進貨】或【進貨退出】進貨父資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Purchase/PurchasePosting")]
        public async Task<List<PurchaseMainSetting>> GetPurchasePosting()
        {
            List<PurchaseMainSetting> purchaseMains = new List<PurchaseMainSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PurchaseMainSetting " +
                                        $"where  Posting = 0 " +
                                        $"order by PurchaseFlag,PurchaseNumber ";
                        purchaseMains = connection.Query<PurchaseMainSetting>(sql).ToList();
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
        /// 進貨父子新增
        /// </summary>
        /// <param name="value"></param>
        // POST api/<PurchaseMainController>
        [HttpPost]
        public async Task<List<PurchaseSetting>> PostPurchase([FromBody] PurchaseSetting purchaseSetting)
        {
            List<PurchaseSetting> result = new List<PurchaseSetting>();
            List<PurchaseMainSetting> CheckPurchaseNumber = new List<PurchaseMainSetting>();
            string ResultPurchaseNumber;
            try
            {
                await Task.Run(() =>
                {
                    //purchaseMainSetting.PurchaseFlag = 1;       // 1.進貨，2.進貨退出
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                    {
                        string sql = $"SELECT top 1 * FROM  PurchaseMainSetting " +
                                        $"where PurchaseFlag=@PurchaseFlag and PurchaseNumber like @PurchaseNumber " +
                                        $"order by PurchaseNumber desc ";
                        CheckPurchaseNumber = connection.Query<PurchaseMainSetting>(sql, new { PurchaseFlag = purchaseSetting.PurchaseFlag, PurchaseNumber = purchaseSetting.PurchaseDate.ToString("yyyyMMdd") + "%" }).ToList();
                    }
                    if (CheckPurchaseNumber.Count == 0)
                    {
                        ResultPurchaseNumber = purchaseSetting.PurchaseDate.ToString("yyyyMMdd") + "0001";     // 當天無進貨，由 0001開始
                    }
                    else
                    {
                        ResultPurchaseNumber = purchaseSetting.PurchaseDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckPurchaseNumber[0].PurchaseNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆進貨單號+1
                    }
                    purchaseSetting.PurchaseNumber = ResultPurchaseNumber;
                    double mTotal = 0;
                    for (int i = 0; i < purchaseSetting.PurchaseSub.Count(); i++)
                    {
                        purchaseSetting.PurchaseSub[i].PurchaseFlag = purchaseSetting.PurchaseFlag;
                        purchaseSetting.PurchaseSub[i].PurchaseNumber = purchaseSetting.PurchaseNumber;
                        mTotal += Convert.ToDouble(purchaseSetting.PurchaseSub[i].ProductTotal);
                    }
                    purchaseSetting.Total = mTotal;

                    if (purchaseSetting.PurchaseTax == 0)
                    {
                        purchaseSetting.Tax = Math.Round(mTotal * 0.05, 0);
                    }
                    purchaseSetting.TotalTax = mTotal + purchaseSetting.Tax;
                    //using (var ts = new TransactionScope())
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增父
                        {
                            string sql = $"INSERT INTO PurchaseMainSetting(PurchaseFlag , PurchaseNumber , ProjectNumber , PurchaseDate , PurchaseCompanyNumber , PurchaseTax , PurchaseInvoiceNo , PurchaseEmployeeNumber , Remark , Total , Tax , TotalTax , Posting , FileName , PostingDate ) " +
                                            $"Values(@PurchaseFlag , @PurchaseNumber, @ProjectNumber , @PurchaseDate , @PurchaseCompanyNumber , @PurchaseTax , @PurchaseInvoiceNo , @PurchaseEmployeeNumber , @Remark , @Total , @Tax , @TotalTax , @Posting , @FileName , @PostingDate)";
                            connection.Execute(sql, purchaseSetting);
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                        {
                            string sql = $"INSERT INTO PurchaseSubSetting(PurchaseFlag , PurchaseNumber , PurchaseNo , ProductNumber , ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal ) " +
                                            $"Values(@PurchaseFlag , @PurchaseNumber , @PurchaseNo , @ProductNumber , @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal)";
                            connection.Execute(sql, purchaseSetting.PurchaseSub);
                        }
                        //ts.Complete();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢父
                    {
                        string sql = $"SELECT * FROM  PurchaseMainSetting where PurchaseFlag=@PurchaseFlag and PurchaseNumber=@PurchaseNumber ";
                        result = connection.Query<PurchaseSetting>(sql, new { PurchaseFlag = purchaseSetting.PurchaseFlag, PurchaseNumber = ResultPurchaseNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢子
                    {
                        string sql = $"SELECT * FROM  PurchaseSubSetting " +
                                        $"where PurchaseFlag=@PurchaseFlag and PurchaseNumber=@PurchaseNumber " +
                                        $"order by PurchaseFlag , PurchaseNumber , PurchaseNo ";
                        result[0].PurchaseSub = connection.Query<PurchaseSubSetting>(sql, new { PurchaseFlag = purchaseSetting.PurchaseFlag, PurchaseNumber = ResultPurchaseNumber }).ToList();
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
        /// 進貨父子更新
        /// </summary>
        // PUT api/<PurchaseController>/5
        [HttpPut]
        public async Task<IActionResult> UpdatePurchase(PurchaseSetting purchaseSetting)
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
                            string sql = $"UPDATE PurchaseMainSetting SET " +
                                        $"PurchaseDate = @PurchaseDate , PurchaseCompanyNumber = @PurchaseCompanyNumber , ProjectNumber = @ProjectNumber , PurchaseTax = @PurchaseTax , PurchaseInvoiceNo= @PurchaseInvoiceNo , " +
                                        $"PurchaseEmployeeNumber = @PurchaseEmployeeNumber , Remark = @Remark , Total  = @Total , Tax = @Tax , TotalTax = @TotalTax , Posting = @Posting , FileName = @FileName , PostingDate = @PostingDate " +
                                        "Where PurchaseFlag=@PurchaseFlag and PurchaseNumber=@PurchaseNumber ";
                            DateMainIndex = connection.Execute(sql, purchaseSetting);
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete PurchaseSubSetting Where PurchaseFlag=@PurchaseFlag and PurchaseNumber=@PurchaseNumber ";
                                connection.Execute(DeleteSql, new { PurchaseFlag = purchaseSetting.PurchaseFlag, PurchaseNumber = purchaseSetting.PurchaseNumber });
                            }
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                            {
                                string sql = $"INSERT INTO PurchaseSubSetting(PurchaseFlag , PurchaseNumber , PurchaseNo , ProductNumber , ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal ) " +
                                                $"Values(@PurchaseFlag , @PurchaseNumber , @PurchaseNo , @ProductNumber , @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal)";
                                connection.Execute(sql, purchaseSetting.PurchaseSub);
                            }
                        });
                        return Ok($"{purchaseSetting.PurchaseNumber}資訊，更新成功!");
                    }
                    else
                    {
                        return BadRequest($"{purchaseSetting.PurchaseNumber}資訊，更新父資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{purchaseSetting.PurchaseNumber}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 進貨父更新
        /// </summary>
        /// <param name="purchaseMainSetting"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/Purchase/UpdatePurchaseMain")]
        public async Task<IActionResult> UpdatePurchaseMain(PurchaseMainSetting purchaseMainSetting)
        {
            try
            {
                int DateMainIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE PurchaseMainSetting SET " +
                                    "Posting = @Posting , PostingDate = @PostingDate " +
                                    "Where PurchaseFlag=@PurchaseFlag and PurchaseNumber=@PurchaseNumber ";
                        DateMainIndex = connection.Execute(sql, purchaseMainSetting);
                    }
                });
                if (DateMainIndex > 0)
                {
                    return Ok($"{purchaseMainSetting.PurchaseNumber}資訊，更新父資料成功!");
                }
                else
                {
                    return BadRequest($"{purchaseMainSetting.PurchaseNumber}資訊，更新父資料失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{purchaseMainSetting.PurchaseNumber}資訊，更新父資料失敗");
            }          
        }
        // DELETE api/<PurchaseController>/5
        [HttpDelete("{PurchaseFlag}/{PurchaseNumber}")]
        public async Task<IActionResult> DeletePurchase(int PurchaseFlag, string PurchaseNumber)
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
                            string DeleteSql = "delete PurchaseMainSetting Where PurchaseFlag=@PurchaseFlag and PurchaseNumber=@PurchaseNumber ";
                            DateMainIndex = connection.Execute(DeleteSql, new { PurchaseFlag = PurchaseFlag, PurchaseNumber = PurchaseNumber });
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete PurchaseSubSetting Where PurchaseFlag=@PurchaseFlag and PurchaseNumber=@PurchaseNumber ";
                                DateMainIndex = connection.Execute(DeleteSql, new { PurchaseFlag = PurchaseFlag, PurchaseNumber = PurchaseNumber });
                            }
                        });
                        return Ok($"{PurchaseNumber}資訊，刪除成功!");
                    }
                    else
                    {
                        return BadRequest($"{PurchaseNumber}資訊，刪除資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{PurchaseNumber}資訊，刪除失敗");
            }
        }
        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="PurchaseFlag"></param>
        /// <param name="PurchaseCompanyNumber"></param>
        /// <param name="PurchaseDate"></param>
        /// <param name="PurchaseNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/PurchaseAttachmentFile")]
        public async Task<IActionResult> PostPurchaseAttachmenFile(int PurchaseFlag,string PurchaseCompanyNumber, DateTime PurchaseDate,string PurchaseNumber, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null)
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{PurchaseCompanyNumber}";
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
                        List<PurchaseMainSetting> CheckPurchaseNumber = new List<PurchaseMainSetting>();
                        string ResultPurchaseNumber;
                        if (PurchaseNumber == null)
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                            {
                                string sql = $"SELECT top 1 * FROM  PurchaseMainSetting " +
                                                $"where PurchaseFlag=@PurchaseFlag and PurchaseNumber like @PurchaseNumber " +
                                                $"order by PurchaseNumber desc ";
                                CheckPurchaseNumber = connection.Query<PurchaseMainSetting>(sql, new { PurchaseFlag = PurchaseFlag, PurchaseNumber = PurchaseDate.ToString("yyyyMMdd") + "%" }).ToList();
                            }
                            if (CheckPurchaseNumber.Count == 0)
                            {
                                ResultPurchaseNumber = PurchaseDate.ToString("yyyyMMdd") + "0001";     // 當天無進貨，由 0001開始
                            }
                            else
                            {
                                ResultPurchaseNumber = PurchaseDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckPurchaseNumber[0].PurchaseNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆進貨單號+1
                            }
                            PurchaseNumber = ResultPurchaseNumber;
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"UPDATE PurchaseMainSetting SET FileName = @FileName  WHERE PurchaseFlag=@PurchaseFlag and PurchaseNumber=@PurchaseNumber";
                            connection.Execute(sql, new { FileName = AttachmentFile.FileName, PurchaseFlag = PurchaseFlag, PurchaseNumber = PurchaseNumber });
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
        /// <param name="PurchaseCompanyNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/PurchaseAttachmentFile")]
        public async Task<IActionResult> GetPurchaseAttachmenFile(string PurchaseCompanyNumber, string AttachmentFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(PurchaseCompanyNumber) && !string.IsNullOrEmpty(AttachmentFile))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{PurchaseCompanyNumber}\\{AttachmentFile}";
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
