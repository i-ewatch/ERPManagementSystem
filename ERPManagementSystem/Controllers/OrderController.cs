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
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "訂購單";
            SqlDB = _configuration["SqlDB"];
        }
        /// <summary>
        /// 查詢單筆訂購單父子資料
        /// </summary>
        /// <param name="OrderNumber">進貨單號</param>
        /// <returns></returns>
        [HttpGet("{OrderNumber}")]
        public async Task<List<OrderSetting>> GetOrder(string OrderNumber)
        {
            List<OrderSetting> result = new List<OrderSetting>();
            List<OrderSubSetting> Sub = new List<OrderSubSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  OrderMainSetting " +
                                        $"where OrderNumber=@OrderNumber " +
                                        $"order by OrderNumber ";
                        result = connection.Query<OrderSetting>(sql, new { OrderNumber = OrderNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  OrderSubSetting " +
                                        $"where  OrderNumber=@OrderNumber " +
                                        $"order by OrderNumber ";
                        Sub = connection.Query<OrderSubSetting>(sql, new { OrderNumber = OrderNumber }).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].OrderSub = (from S in Sub
                                                  where S.OrderNumber == result[i].OrderNumber
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
        /// 查詢全部年份訂購單父資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("Year")]
        public async Task<List<OrderMainSetting>> GetOrder_Year(string Year)
        {
            List<OrderMainSetting> OrderMains = new List<OrderMainSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  OrderMainSetting " +
                                        $"where LEFT(OrderNumber,4) = @Year " +
                                        $"order by OrderNumber ";
                        OrderMains = connection.Query<OrderMainSetting>(sql, new { Year }).ToList();
                    }
                });
                return OrderMains;
            }
            catch (Exception)
            {
                return OrderMains;
            }
        }
        /// <summary>
        /// 訂購單父子新增
        /// </summary>
        /// <param name="value"></param>
        // POST api/<OrderMainController>
        [HttpPost]
        public async Task<List<OrderSetting>> PostOrder([FromBody] OrderSetting orderSetting)
        {
            List<OrderSetting> result = new List<OrderSetting>();
            List<OrderMainSetting> CheckOrderNumber = new List<OrderMainSetting>();
            string ResultOrderNumber;
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                    {
                        string sql = $"SELECT top 1 * FROM  OrderMainSetting " +
                                        $"where OrderNumber like @OrderNumber " +
                                        $"order by OrderNumber desc ";
                        CheckOrderNumber = connection.Query<OrderMainSetting>(sql, new { OrderNumber = orderSetting.OrderDate.ToString("yyyyMMdd") + "%" }).ToList();
                    }
                    if (CheckOrderNumber.Count == 0)
                    {
                        ResultOrderNumber = orderSetting.OrderDate.ToString("yyyyMMdd") + "0001";     // 當天無進貨，由 0001開始
                    }
                    else
                    {
                        ResultOrderNumber = orderSetting.OrderDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckOrderNumber[0].OrderNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆進貨單號+1
                    }
                    orderSetting.OrderNumber = ResultOrderNumber;
                    double mTotal = 0;
                    for (int i = 0; i < orderSetting.OrderSub.Count(); i++)
                    {
                        orderSetting.OrderSub[i].OrderNumber = orderSetting.OrderNumber;
                        mTotal += Convert.ToDouble(orderSetting.OrderSub[i].ProductTotal);
                    }
                    orderSetting.Total = mTotal;

                    if (orderSetting.OrderTax == 0)
                    {
                        orderSetting.Tax = Math.Round(mTotal * 0.05, 0);
                    }
                    orderSetting.TotalTax = mTotal + orderSetting.Tax;
                    //using (var ts = new TransactionScope())
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增父
                        {
                            string sql = $"INSERT INTO OrderMainSetting(OrderNumber , ProjectNumber , OrderDate , OrderCompanyNumber , OrderDirectoryNumber ,Address , OrderEmployeeNumber , Remark , Total , Tax , TotalTax , TotalQty , FileName , OrderNote,InvalidFlag ) " +
                                            $"Values( @OrderNumber, @ProjectNumber , @OrderDate , @OrderCompanyNumber ,@OrderDirectoryNumber ,@Address , @OrderEmployeeNumber , @Remark , @Total , @Tax , @TotalTax , @TotalQty , @FileName , @OrderNote,@InvalidFlag)";
                            connection.Execute(sql, orderSetting);
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                        {
                            string sql = $"INSERT INTO OrderSubSetting( OrderNumber , OrderNo ,  ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal ) " +
                                            $"Values( @OrderNumber , @OrderNo , @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal)";
                            connection.Execute(sql, orderSetting.OrderSub);
                        }
                        //ts.Complete();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢父
                    {
                        string sql = $"SELECT * FROM  OrderMainSetting where OrderNumber=@OrderNumber ";
                        result = connection.Query<OrderSetting>(sql, new { OrderNumber = ResultOrderNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢子
                    {
                        string sql = $"SELECT * FROM  OrderSubSetting " +
                                        $"where OrderNumber=@OrderNumber " +
                                        $"order by  OrderNumber , OrderNo ";
                        result[0].OrderSub = connection.Query<OrderSubSetting>(sql, new { OrderNumber = ResultOrderNumber }).ToList();
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
        /// 訂購單父子更新
        /// </summary>
        // PUT api/<OrderController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateOrder(OrderSetting orderSetting)
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
                            string sql = $"UPDATE OrderMainSetting SET " +
                                        $"OrderDate = @OrderDate , OrderCompanyNumber = @OrderCompanyNumber , ProjectNumber = @ProjectNumber ," +
                                        $"OrderEmployeeNumber = @OrderEmployeeNumber , Remark = @Remark , Total  = @Total , Tax = @Tax , TotalTax = @TotalTax , TotalQty = @TotalQty , FileName = @FileName , OrderNote = @OrderNote ,InvalidFlag = @InvalidFlag " +
                                        "Where  OrderNumber=@OrderNumber ";
                            DateMainIndex = connection.Execute(sql, orderSetting);
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete OrderSubSetting Where OrderNumber=@OrderNumber ";
                                connection.Execute(DeleteSql, new { OrderNumber = orderSetting.OrderNumber });
                            }
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                            {
                                string sql = $"INSERT INTO OrderSubSetting( OrderNumber , OrderNo ,  ProductName , ProductUnit , ProductQty , ProductPrice , ProductTotal ) " +
                                                $"Values( @OrderNumber , @OrderNo ,  @ProductName , @ProductUnit , @ProductQty , @ProductPrice , @ProductTotal)";
                                connection.Execute(sql, orderSetting.OrderSub);
                            }
                        });
                        return Ok($"{orderSetting.OrderNumber}資訊，更新成功!");
                    }
                    else
                    {
                        return BadRequest($"{orderSetting.OrderNumber}資訊，更新父資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{orderSetting.OrderNumber}資訊，更新失敗");
            }
        }
        // DELETE api/<OrderController>/5
        [HttpDelete("{OrderNumber}")]
        public async Task<IActionResult> DeleteOrder(string OrderNumber)
        {
            try
            {
                int DateMainIndex = 0;
                //using (var ts = new TransactionScope())
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{OrderNumber}";
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
                            string DeleteSql = "delete OrderMainSetting Where  OrderNumber=@OrderNumber ";
                            DateMainIndex = connection.Execute(DeleteSql, new { OrderNumber = OrderNumber });
                        }
                    });
                    if (DateMainIndex > 0)
                    {
                        await Task.Run(() =>
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))
                            {
                                string DeleteSql = "delete OrderSubSetting Where  OrderNumber=@OrderNumber ";
                                DateMainIndex = connection.Execute(DeleteSql, new { OrderNumber = OrderNumber });
                            }
                        });
                        return Ok($"{OrderNumber}資訊，刪除成功!");
                    }
                    else
                    {
                        return BadRequest($"{OrderNumber}資訊，刪除資料失敗");
                    }
                    //ts.Complete();
                }
            }
            catch (Exception)
            {
                return BadRequest($"{OrderNumber}資訊，刪除失敗");
            }
        }
        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="OrderDate"></param>
        /// <param name="OrderNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/OrderAttachmentFile")]
        public async Task<IActionResult> PostOrderAttachmenFile(DateTime OrderDate, string OrderNumber, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null)
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{OrderNumber}";
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
                        List<OrderMainSetting> CheckOrderNumber = new List<OrderMainSetting>();
                        string ResultOrderNumber;
                        if (OrderNumber == null)
                        {
                            using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                            {
                                string sql = $"SELECT top 1 * FROM  OrderMainSetting " +
                                                $"where OrderNumber like @OrderNumber " +
                                                $"order by OrderNumber desc ";
                                CheckOrderNumber = connection.Query<OrderMainSetting>(sql, new { OrderNumber = OrderDate.ToString("yyyyMMdd") + "%" }).ToList();
                            }
                            if (CheckOrderNumber.Count == 0)
                            {
                                ResultOrderNumber = OrderDate.ToString("yyyyMMdd") + "0001";     // 當天無進貨，由 0001開始
                            }
                            else
                            {
                                ResultOrderNumber = OrderDate.ToString("yyyyMMdd") + (Convert.ToInt32(CheckOrderNumber[0].OrderNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆進貨單號+1
                            }
                            OrderNumber = ResultOrderNumber;
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"UPDATE OrderMainSetting SET FileName = @FileName  WHERE  and OrderNumber=@OrderNumber";
                            connection.Execute(sql, new { FileName = AttachmentFile.FileName, OrderNumber = OrderNumber });
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
        /// <param name="OrderNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/OrderAttachmentFile")]
        public async Task<IActionResult> GetOrderAttachmenFile(string OrderNumber, string AttachmentFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(OrderNumber) && !string.IsNullOrEmpty(AttachmentFile))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{OrderNumber}\\{AttachmentFile}";
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
