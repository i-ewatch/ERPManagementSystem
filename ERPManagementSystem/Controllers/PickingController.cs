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
    public class PickingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        public PickingController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "領料";
            SqlDB = _configuration["SqlDB"];
        }

        /// <summary>
        /// 查詢全部【領料】和【領料退回】父子資料
        /// </summary>
        /// <returns></returns>
        // GET: api/<PickingController>
        [HttpGet]
        public async Task<List<PickingSetting>> GetPicking()
        {
            List<PickingSetting> result = new List<PickingSetting>();
            List<PickingSubSetting> Sub = new List<PickingSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PickingMainSetting order by PickingFlag,PickingNumber ";
                        result = connection.Query<PickingSetting>(sql).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PickingSubSetting order by PickingFlag,PickingNumber,PickingNo ";
                        Sub = connection.Query<PickingSubSetting>(sql).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].PickingSub = (from S in Sub
                                                    where S.PickingFlag == result[i].PickingFlag && S.PickingNumber == result[i].PickingNumber
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
        /// 查詢全部【單】【領料】或【領料退回】領料父子資料
        /// <param name="PickingFlag">銷貨旗標 </param>
        /// </summary>
        /// <returns></returns>
        [HttpGet("{PickingFlag}")]
        public async Task<List<PickingSetting>> GetPicking(int PickingFlag)
        {
            List<PickingSetting> result = new List<PickingSetting>();
            List<PickingSubSetting> Sub = new List<PickingSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PickingMainSetting " +
                                        $"where PickingFlag=@PickingFlag " +
                                        $"order by PickingFlag,PickingNumber ";
                        result = connection.Query<PickingSetting>(sql, new { PickingFlag = PickingFlag }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PickingSubSetting " +
                                        $"where PickingFlag=@PickingFlag " +
                                        $"order by PickingFlag,PickingNumber,PickingNo ";
                        Sub = connection.Query<PickingSubSetting>(sql, new { PickingFlag = PickingFlag }).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].PickingSub = (from S in Sub
                                                    where S.PickingFlag == result[i].PickingFlag && S.PickingNumber == result[i].PickingNumber
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

        /// 查詢單筆【領料】或【領料退回】父子資料
        /// </summary>
        /// <param name="PickingFlag">銷貨旗標 </param>
        /// <param name="PickingNumber">銷貨單號</param>
        /// <returns></returns>
        [HttpGet("{PickingFlag}/{PickingNumber}")]
        public async Task<List<PickingSetting>> GetPicking(int PickingFlag, string PickingNumber)
        {
            List<PickingSetting> result = new List<PickingSetting>();
            List<PickingSubSetting> Sub = new List<PickingSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PickingMainSetting " +
                                        $"where PickingFlag=@PickingFlag and PickingNumber=@PickingNumber " +
                                        $"order by PickingFlag,PickingNumber ";
                        result = connection.Query<PickingSetting>(sql, new { PickingFlag = PickingFlag, PickingNumber = PickingNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PickingSubSetting " +
                                        $"where PickingFlag=@PickingFlag and PickingNumber=@PickingNumber " +
                                        $"order by PickingFlag,PickingNumber,PickingNo ";
                        Sub = connection.Query<PickingSubSetting>(sql, new { PickingFlag = PickingFlag, PickingNumber = PickingNumber }).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].PickingSub = (from S in Sub
                                                    where S.PickingFlag == result[i].PickingFlag && S.PickingNumber == result[i].PickingNumber
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
        /// 查詢全部【單】【領料】或【領料退回】領料父資料
        /// </summary>
        /// <param name="PickingNumber">銷貨單號(YYYYmm)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Picking/PickingNumber")]
        public async Task<List<PickingMainSetting>> GetPicking(string PickingNumber)
        {
            List<PickingMainSetting> salesMains = new List<PickingMainSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PickingMainSetting " +
                                        $"where PickingNumber >= @PickingNumberStart AND  PickingNumber <= @PickingNumberEnd " +
                                        $"order by PickingFlag,PickingNumber ";
                        salesMains = connection.Query<PickingMainSetting>(sql, new { PickingNumberStart = PickingNumber + "010000", PickingNumberEnd = PickingNumber + "319999" }).ToList();
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
        /// 領料父子新增
        /// </summary>
        /// <param name="pickingSetting"></param>
        // POST api/<PickingMainController>
        [HttpPost]
        public async Task<List<PickingSetting>> PostPicking([FromBody] PickingSetting pickingSetting)
        {
            List<PickingSetting> result = new List<PickingSetting>();
            List<PickingMainSetting> CheckPickingNumber = new List<PickingMainSetting>();
            string ResultPickingNumber;
            try
            {
                await Task.Run(() =>
                {
                    //pickingMainSetting.PickingFlag = 5;       // 5.領料，6.領料退回
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                    {
                        string sql = $"SELECT top 1 * FROM  PickingMainSetting " +
                                        $"where PickingFlag=@PickingFlag and PickingNumber like @PickingNumber " +
                                        $"order by PickingNumber desc ";
                        CheckPickingNumber = connection.Query<PickingMainSetting>(sql, new { PickingFlag = pickingSetting.PickingFlag, PickingNumber = pickingSetting.PickingDate.ToString("yyyyMMdd") + "%" }).ToList();
                    }
                    if (CheckPickingNumber.Count == 0)
                    {
                        ResultPickingNumber = pickingSetting.PickingDate.ToString("yyyyMMdd") + "0001";     // 當天無領料，由 0001開始
                    }
                    else
                    {
                        ResultPickingNumber = pickingSetting.PickingDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckPickingNumber[0].PickingNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆領料單號+1
                    }
                    pickingSetting.PickingNumber = ResultPickingNumber;
                    double mTotal = 0;
                    for (int i = 0; i < pickingSetting.PickingSub.Count(); i++)
                    {
                        pickingSetting.PickingSub[i].PickingFlag = pickingSetting.PickingFlag;
                        pickingSetting.PickingSub[i].PickingNumber = pickingSetting.PickingNumber;
                        mTotal += Convert.ToDouble(pickingSetting.PickingSub[i].ProductTotal);
                    }
                    pickingSetting.Total = mTotal;

                    if (pickingSetting.PickingTax == 0)
                    {
                        pickingSetting.Tax = Math.Round(mTotal * 0.05, 0);
                    }
                    pickingSetting.TotalTax = mTotal + pickingSetting.Tax;
                    //using (var ts = new TransactionScope())
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增父
                        {
                            string sql = $"INSERT INTO PickingMainSetting(PickingFlag , PickingNumber , PickingDate , PickingCustomerNumber , ProjectNumber , PickingTax , PickingInvoiceNo , PickingEmployeeNumber , Remark , Total , Tax , TotalTax , Posting , FileName , TakeACut , Cost , ProfitSharing , PostingDate) " +
                                            $"Values(@PickingFlag , @PickingNumber , @PickingDate , @PickingCustomerNumber , @ProjectNumber , @PickingTax , @PickingInvoiceNo , @PickingEmployeeNumber , @Remark , @Total , @Tax , @TotalTax , @Posting , @FileName, @TakeACut , @Cost , @ProfitSharing , @PostingDate )";
                            connection.Execute(sql, pickingSetting);
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                        {
                            string sql = $"INSERT INTO PickingSubSetting(PickingFlag , PickingNumber , PickingNo , ProductNumber , ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal , Cost , CostTotal ) " +
                                            $"Values(@PickingFlag , @PickingNumber , @PickingNo , @ProductNumber , @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal , @Cost , @CostTotal )";
                            connection.Execute(sql, pickingSetting.PickingSub);
                        }
                        //ts.Complete();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢父
                    {
                        string sql = $"SELECT * FROM  PickingMainSetting " +
                                        $"where PickingFlag=@PickingFlag and PickingNumber=@PickingNumber ";
                        result = connection.Query<PickingSetting>(sql, new { PickingFlag = pickingSetting.PickingFlag, PickingNumber = ResultPickingNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢子
                    {
                        string sql = $"SELECT * FROM  PickingSubSetting " +
                                        $"where PickingFlag=@PickingFlag and PickingNumber=@PickingNumber " +
                                        $"order by PickingFlag , PickingNumber , PickingNo ";
                        result[0].PickingSub = connection.Query<PickingSubSetting>(sql, new { PickingFlag = pickingSetting.PickingFlag, PickingNumber = ResultPickingNumber }).ToList();
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
        // PUT api/<PickingController>/5
        [HttpPut]
        public async Task<IActionResult> UpdatePicking(PickingSetting pickingSetting)
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
                            string sql = $"UPDATE PickingMainSetting SET " +
                                        $"PickingDate = @PickingDate , PickingCustomerNumber = @PickingCustomerNumber , PickingTax = @PickingTax , PickingInvoiceNo= @PickingInvoiceNo , " +
                                        $"PickingEmployeeNumber = @PickingEmployeeNumber , Remark = @Remark , Total  = @Total , Tax = @Tax , TotalTax = @TotalTax , Posting = @Posting , FileName = @FileName, TakeACut = @TakeACut , Cost = @Cost , ProfitSharing = @ProfitSharing , PostingDate = @PostingDate, ProfitSharingDate = @ProfitSharingDate  " +
                                        "Where PickingFlag=@PickingFlag and PickingNumber=@PickingNumber ";
                            DateMainIndex = connection.Execute(sql, pickingSetting);
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete PickingSubSetting Where PickingFlag=@PickingFlag and PickingNumber=@PickingNumber ";
                                connection.Execute(DeleteSql, new { PickingFlag = pickingSetting.PickingFlag, PickingNumber = pickingSetting.PickingNumber });
                            }
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                            {
                                string sql = $"INSERT INTO PickingSubSetting(PickingFlag , PickingNumber , PickingNo , ProductNumber , ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal , Cost , CostTotal ) " +
                                            $"Values(@PickingFlag , @PickingNumber , @PickingNo , @ProductNumber , @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal , @Cost , @CostTotal )";
                                connection.Execute(sql, pickingSetting.PickingSub);
                            }
                        });
                        return Ok($"{pickingSetting.PickingNumber}資訊，更新成功!");
                    }
                    else
                    {
                        return BadRequest($"{pickingSetting.PickingNumber}資訊，更新父資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{pickingSetting.PickingNumber}資訊，更新失敗");
            }
        }

        /// <summary>
        /// 更新父資料
        /// </summary>
        /// <param name="pickingMainSetting"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/Picking/UpdatePickingMain")]
        public async Task<IActionResult> UpdatePickingMain(PickingMainSetting pickingMainSetting)
        {
            try
            {
                int DateMainIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE PickingMainSetting SET " +
                                    $" Posting = @Posting , TakeACut = @TakeACut , Cost = @Cost , ProfitSharing = @ProfitSharing , PostingDate = @PostingDate, ProfitSharingDate = @ProfitSharingDate " +
                                    "Where PickingFlag=@PickingFlag and PickingNumber=@PickingNumber ";
                        DateMainIndex = connection.Execute(sql, pickingMainSetting);
                    }
                });
                if (DateMainIndex > 0)
                {
                    return Ok($"{pickingMainSetting.PickingNumber}資訊，更新父資料成功!");
                }
                else
                {
                    return BadRequest($"{pickingMainSetting.PickingNumber}資訊，更新父資料失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{pickingMainSetting.PickingNumber}資訊，更新父資料失敗");
            }
        }

        // DELETE api/<PickingController>/5
        [HttpDelete("{PickingFlag}/{PickingNumber}")]
        public async Task<IActionResult> DeletePicking(int PickingFlag, string PickingNumber)
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
                            string DeleteSql = "delete PickingMainSetting " +
                                                "Where PickingFlag=@PickingFlag and PickingNumber=@PickingNumber ";
                            DateMainIndex = connection.Execute(DeleteSql, new { PickingFlag = PickingFlag, PickingNumber = PickingNumber });
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete PickingSubSetting " +
                                                    "Where PickingFlag=@PickingFlag and PickingNumber=@PickingNumber ";
                                DateMainIndex = connection.Execute(DeleteSql, new { PickingFlag = PickingFlag, PickingNumber = PickingNumber });
                            }
                        });
                        return Ok($"{PickingNumber}資訊，刪除成功!");
                    }
                    else
                    {
                        return BadRequest($"{PickingNumber}資訊，刪除資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{PickingNumber}資訊，更新失敗");
            }
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="PickingFlag"></param>
        /// <param name="PickingCustomerNumber"></param>
        /// <param name="PickingDate"></param>
        /// <param name="PickingNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/PickingAttachmentFile")]
        public async Task<IActionResult> PostPickingAttachmenFile(int PickingFlag, string PickingCustomerNumber, DateTime PickingDate, string PickingNumber, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null)
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{PickingCustomerNumber}";
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
                        List<PickingMainSetting> CheckPickingNumber = new List<PickingMainSetting>();
                        string ResultPickingNumber;
                        if (PickingNumber == null)
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                            {
                                string sql = $"SELECT top 1 * FROM  PickingMainSetting " +
                                                $"where PickingFlag=@PickingFlag and PickingNumber like @PickingNumber " +
                                                $"order by PickingNumber desc ";
                                CheckPickingNumber = connection.Query<PickingMainSetting>(sql, new { PickingFlag = PickingFlag, PickingNumber = PickingDate.ToString("yyyyMMdd") + "%" }).ToList();
                            }
                            if (CheckPickingNumber.Count == 0)
                            {
                                ResultPickingNumber = PickingDate.ToString("yyyyMMdd") + "0001";     // 當天無進貨，由 0001開始
                            }
                            else
                            {
                                ResultPickingNumber = PickingDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckPickingNumber[0].PickingNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆進貨單號+1
                            }
                            PickingNumber = ResultPickingNumber;
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"UPDATE PickingMainSetting SET FileName = @FileName  WHERE PickingFlag=@PickingFlag and PickingNumber=@PickingNumber";
                            connection.Execute(sql, new { FileName = AttachmentFile.FileName, PickingFlag = PickingFlag, PickingNumber = PickingNumber });
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
        /// <param name="PickingCustomerNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/PickingAttachmentFile")]
        public async Task<IActionResult> GetPickingAttachmenFile(string PickingCustomerNumber, string AttachmentFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(PickingCustomerNumber) && !string.IsNullOrEmpty(AttachmentFile))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{PickingCustomerNumber}\\{AttachmentFile}";
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
