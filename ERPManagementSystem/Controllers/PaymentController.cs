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
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "代墊代付";
            SqlDB = _configuration["SqlDB"];
        }
        /// <summary>
        /// 取得單一代墊代付
        /// </summary>
        /// <returns></returns>
        [HttpGet("{PaymentNumber}")]
        public async Task<List<PaymentSetting>> GetPayment(string PaymentNumber)
        {
            List<PaymentSetting> paymentSettings = new List<PaymentSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PaymentSetting WHERE PaymentNumber = @PaymentNumber";
                        paymentSettings = connection.Query<PaymentSetting>(sql, new { PaymentNumber }).ToList();
                    }
                });
                return paymentSettings;
            }
            catch (Exception)
            {
                return paymentSettings;
            }
        }
        /// <summary>
        /// 取得全部未付款代墊代付
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Payment/TransferDate")]
        public async Task<List<PaymentSetting>> GetPaymentTransferDate()
        {
            List<PaymentSetting> paymentSettings = new List<PaymentSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PaymentSetting WHERE TransferDate IS NULL";
                        paymentSettings = connection.Query<PaymentSetting>(sql).ToList();
                        string paymentitem = $"SELECT * FROM  PaymentItemSetting";
                        var PaymentItem = connection.Query<PaymentItemSetting>(paymentitem).ToList();
                        string project = "SELECT * FROM ProjectSetting";
                        var Project = connection.Query<ProjectSetting>(project).ToList();
                        string employee = "SELECT * FROM EmployeeSetting";
                        var Employee = connection.Query<EmployeeSetting>(employee).ToList();
                        foreach (var item in paymentSettings)
                        {
                            var paymentitemdata = PaymentItem.SingleOrDefault(g => g.PaymentItemNo == item.PaymentItemNo);
                            var projectdata = Project.SingleOrDefault(g => g.ProjectNumber == item.ProjectNumber);
                            var employeedata = Employee.SingleOrDefault(g => g.EmployeeNumber == item.EmployeeNumber);
                            if (paymentitemdata != null)
                            {
                                item.PaymentItemNo = paymentitemdata.PaymentItemName;
                            }
                            if (projectdata != null)
                            {
                                item.ProjectNumber = projectdata.ProjectName;
                            }
                            if (employeedata != null)
                            {
                                item.EmployeeNumber = employeedata.EmployeeName;
                            }
                        }
                    }
                });
                return paymentSettings;
            }
            catch (Exception)
            {
                return paymentSettings;
            }
        }
        /// <summary>
        /// 取得年分代墊代付
        /// </summary>
        /// <param name="YearDate">yyyy</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Payment/YearDate")]
        public async Task<List<PaymentSetting>> GetYearPayment(string YearDate)
        {
            List<PaymentSetting> paymentSettings = new List<PaymentSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PaymentSetting WHERE PaymentNumber >= @PaymentNumberStart AND PaymentNumber <= @PaymentNumberEnd";
                        paymentSettings = connection.Query<PaymentSetting>(sql, new { PaymentNumberStart = YearDate + "01010000", PaymentNumberEnd = YearDate + "12319999" }).ToList();
                    }
                });
                return paymentSettings;
            }
            catch (Exception)
            {
                return paymentSettings;
            }
        }
        /// <summary>
        /// 取得月分代墊代付
        /// </summary>
        /// <param name="MonthDate">yyyyMM</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Payment/MonthDate")]
        public async Task<List<PaymentSetting>> GetMonthPayment(string MonthDate)
        {
            List<PaymentSetting> paymentSettings = new List<PaymentSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PaymentSetting WHERE PaymentNumber >= @PaymentNumberStart AND PaymentNumber <= @PaymentNumberEnd";
                        paymentSettings = connection.Query<PaymentSetting>(sql, new { PaymentNumberStart = MonthDate + "010000", PaymentNumberEnd = MonthDate + "319999" }).ToList();
                        string paymentitem = $"SELECT * FROM  PaymentItemSetting";
                        var PaymentItem = connection.Query<PaymentItemSetting>(paymentitem).ToList();
                        string project = "SELECT * FROM ProjectSetting";
                        var Project = connection.Query<ProjectSetting>(project).ToList();
                        string employee = "SELECT * FROM EmployeeSetting";
                        var Employee = connection.Query<EmployeeSetting>(employee).ToList();
                        foreach (var item in paymentSettings)
                        {
                            var paymentitemdata = PaymentItem.SingleOrDefault(g => g.PaymentItemNo == item.PaymentItemNo);
                            var projectdata = Project.SingleOrDefault(g => g.ProjectNumber == item.ProjectNumber);
                            var employeedata = Employee.SingleOrDefault(g => g.EmployeeNumber == item.EmployeeNumber);
                            if (paymentitemdata != null)
                            {
                                item.PaymentItemNo = paymentitemdata.PaymentItemName;
                            }
                            if (projectdata != null)
                            {
                                item.ProjectNumber = projectdata.ProjectName;
                            }
                            if (employeedata != null)
                            {
                                item.EmployeeNumber = employeedata.EmployeeName;
                            }
                        }
                    }
                });
                return paymentSettings;
            }
            catch (Exception)
            {
                return paymentSettings;
            }
        }
        /// <summary>
        /// 取得員工月份代墊代付
        /// </summary>
        /// <param name="EmployeeNumber">員工代碼</param>
        /// <param name="MonthDate">yyyyMM</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Payment/EmployeeMonthDate")]
        public async Task<List<PaymentSetting>> GetEmployeeMonthPayment(string EmployeeNumber,string MonthDate)
        {
            List<PaymentSetting> paymentSettings = new List<PaymentSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PaymentSetting WHERE PaymentNumber >= @PaymentNumberStart AND PaymentNumber <= @PaymentNumberEnd AND EmployeeNumber = @EmployeeNumber";
                        paymentSettings = connection.Query<PaymentSetting>(sql, new { PaymentNumberStart = MonthDate + "010000", PaymentNumberEnd = MonthDate + "319999" , EmployeeNumber}).ToList();
                    }
                });
                return paymentSettings;
            }
            catch (Exception)
            {
                return paymentSettings;
            }
        }
        /// <summary>
        /// 新增代墊代付
        /// </summary>
        /// <param name="PaymentSetting"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<PaymentSetting>> PostPayment(PaymentSetting PaymentSetting)
        {
            List<PaymentSetting> paymentSettings = new List<PaymentSetting>();
            List<PaymentSetting> result = new List<PaymentSetting>();
            string ResultPurchaseNumber;
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))     // 查詢當天最後一筆單號
                    {
                        string sql = $"SELECT top 1 * FROM  PaymentSetting " +
                                        $"WHERE PaymentNumber like @PaymentNumber " +
                                        $"order by PaymentNumber desc ";
                        paymentSettings = connection.Query<PaymentSetting>(sql, new { PaymentNumber = PaymentSetting.PaymentDate.ToString("yyyyMMdd") + "%" }).ToList();
                    }
                    if (paymentSettings.Count == 0)
                    {
                        ResultPurchaseNumber = PaymentSetting.PaymentDate.ToString("yyyyMMdd") + "0001";     // 當天無進貨，由 0001開始
                    }
                    else
                    {
                        ResultPurchaseNumber = PaymentSetting.PaymentDate.ToString("yyyyMMdd") + (Convert.ToInt32(paymentSettings[0].PaymentNumber.Substring(8, 4)) + 1).ToString().PadLeft(4, '0');     // 當天最後一筆進貨單號+1
                    }
                    PaymentSetting.PaymentNumber = ResultPurchaseNumber;
                    using (IDbConnection connection = new SqlConnection(SqlDB))//新增
                    {
                        string sql = "";
                        //if (PaymentSetting.TransferDate != null)
                        //{
                            sql = $"INSERT INTO PaymentSetting (PaymentNumber,ProjectNumber,PaymentDate,PaymentInvoiceNo,PaymentItemNo,PaymentUse,EmployeeNumber,PaymentAmount,PaymentMethod,Remark,TransferDate )" +
                           $" VALUES (@PaymentNumber,@ProjectNumber,@PaymentDate,@PaymentInvoiceNo,@PaymentItemNo,@PaymentUse,@EmployeeNumber,@PaymentAmount,@PaymentMethod,@Remark,@TransferDate)";
                        //}
                        //else
                        //{
                        //    sql = $"INSERT INTO PaymentSetting (PaymentNumber,PaymentDate,PaymentInvoiceNo,PaymentItemNo,PaymentUse,EmployeeNumber,PaymentAmount,PaymentMethod,Remark )" +
                        //  $" VALUES (@PaymentNumber,@PaymentDate,@PaymentInvoiceNo,@PaymentItemNo,@PaymentUse,@EmployeeNumber,@PaymentAmount,@PaymentMethod,@Remark)";
                        //}
                        connection.Execute(sql, PaymentSetting);
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))//查詢
                    {
                        string sql = $"SELECT top 1 * FROM  PaymentSetting " +
                                      $"WHERE PaymentNumber = @PaymentNumber ";
                        result = connection.Query<PaymentSetting>(sql, new { PaymentNumber = PaymentSetting.PaymentNumber }).ToList();
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
        /// 修改代墊代付
        /// </summary>
        /// <param name="PaymentSetting"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutPayment(PaymentSetting PaymentSetting)
        {
            try
            {
                int Index = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))//修改
                    {
                        string sql = $"UPDATE PaymentSetting SET " +
                        $"ProjectNumber=@ProjectNumber,PaymentDate=@PaymentDate,PaymentInvoiceNo=@PaymentInvoiceNo,PaymentItemNo=@PaymentItemNo,PaymentUse=@PaymentUse,EmployeeNumber=@EmployeeNumber,PaymentAmount=@PaymentAmount,PaymentMethod=@PaymentMethod,Remark=@Remark,TransferDate=@TransferDate" +
                        $" WHERE PaymentNumber = @PaymentNumber";
                        Index = connection.Execute(sql, PaymentSetting);
                    }
                });
                if (Index > 0)
                {
                    return Ok($"{PaymentSetting.PaymentNumber}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{PaymentSetting.PaymentNumber}資訊，更新失敗!");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{PaymentSetting.PaymentNumber}資訊，更新失敗!");
            }
        }
        /// <summary>
        /// 刪除代墊代付
        /// </summary>
        /// <param name="PaymentNumber"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePayment(PaymentSetting PaymentSetting)
        {
            try
            {
                int Index = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))//刪除
                    {
                        string sql = $"DELETE FROM PaymentSetting WHERE PaymentNumber = @PaymentNumber";
                        Index = connection.Execute(sql, new { PaymentNumber = PaymentSetting.PaymentNumber });
                    }
                });
                if (Index > 0)
                {
                    return Ok($"{PaymentSetting.PaymentNumber}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{PaymentSetting.PaymentNumber}資訊，刪除失敗!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 代墊代付附件檔案更新
        /// </summary>
        /// <param name="PaymentNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/PaymentAttachmentFile")]
        public async Task<IActionResult> PostPaymentAttachmentFile(string PaymentNumber, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(PaymentNumber))
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{PaymentNumber}";
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
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"UPDATE PaymentSetting SET FileName = @FileName  WHERE PaymentNumber = @PaymentNumber";
                            connection.Execute(sql, new { FileName = AttachmentFile.FileName, PaymentNumber = PaymentNumber });
                        }
                    });
                    return Ok($"{PaymentNumber}檔案上傳成功");
                }
                else
                {
                    return BadRequest($"{PaymentNumber}檔案上傳失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{PaymentNumber}檔案上傳失敗");
            }
        }
        /// <summary>
        /// 代墊代付附件檔案下載
        /// </summary>
        /// <param name="PaymentNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/PaymentAttachmentFile")]
        public async Task<IActionResult> GetPaymentAttachmentFile(string PaymentNumber, string AttachmentFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(AttachmentFile) && !string.IsNullOrEmpty(PaymentNumber))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{PaymentNumber}\\{AttachmentFile}";
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
                    return BadRequest($"{PaymentNumber}檔案下載失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{PaymentNumber}檔案下載失敗");
            }
        }
    }
}
