using Dapper;
using ERPManagementSystem.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentItemController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string SqlDB { get; set; }
        public PaymentItemController(IConfiguration configuration)
        {
            _configuration = configuration;
            SqlDB = _configuration["SqlDB"];
        }
        /// <summary>
        /// 取得全部代墊代付品項
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PaymentItemSetting>> GetPaymentItem()
        {
            List<PaymentItemSetting> paymentItemSettings = new List<PaymentItemSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  PaymentItemSetting";
                        paymentItemSettings = connection.Query<PaymentItemSetting>(sql).ToList();
                    }
                });
                return paymentItemSettings;
            }
            catch (Exception)
            {
                return paymentItemSettings;
            }
        }
        /// <summary>
        /// 新增代墊代付品項
        /// </summary>
        /// <param name="paymentItemSetting"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostPaymentItem(PaymentItemSetting paymentItemSetting)
        {
            try
            {
                int Index = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"INSERT INTO PaymentItemSetting (PaymentItemNo,PaymentItemName) VALUES (@PaymentItemNo,@PaymentItemName)";
                        Index = connection.Execute(sql, paymentItemSetting);
                    }
                });
                if (Index > 0)
                {
                    return Ok($"{paymentItemSetting.PaymentItemNo}資訊，新增成功");
                }
                else
                {
                    return BadRequest($"{paymentItemSetting.PaymentItemNo}資訊，新增失敗");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{paymentItemSetting.PaymentItemNo}資訊，新增失敗");
            }
        }
        /// <summary>
        /// 更新代墊代付品項
        /// </summary>
        /// <param name="paymentItemSetting"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutPaymentItem(PaymentItemSetting paymentItemSetting)
        {
            try
            {
                int Index = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE PaymentItemSetting SET PaymentItemName = @PaymentItemName  WHERE PaymentItemNo=@PaymentItemNo";
                        Index = connection.Execute(sql, paymentItemSetting);
                    }
                });
                if (Index > 0)
                {
                    return Ok($"{paymentItemSetting.PaymentItemNo}資訊，更新成功");
                }
                else
                {
                    return BadRequest($"{paymentItemSetting.PaymentItemNo}資訊，更新失敗");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{paymentItemSetting.PaymentItemNo}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 刪除代墊代付品項
        /// </summary>
        /// <param name="paymentItemSetting"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePaymentItem(PaymentItemSetting paymentItemSetting)
        {
            try
            {
                int Index = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"DELETE FROM PaymentItemSetting WHERE PaymentItemNo=@PaymentItemNo";
                        Index = connection.Execute(sql, new { PaymentItemNo = paymentItemSetting.PaymentItemNo });
                    }
                });
                if (Index > 0)
                {
                    return Ok($"{paymentItemSetting.PaymentItemNo}資訊，刪除成功");
                }
                else
                {
                    return BadRequest($"{paymentItemSetting.PaymentItemNo}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{paymentItemSetting.PaymentItemNo}資訊，刪除失敗");
            }
        }
    }
}
