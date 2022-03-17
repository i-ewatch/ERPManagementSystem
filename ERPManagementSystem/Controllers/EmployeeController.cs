using Dapper;
using ERPManagementSystem.Modules;
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
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string SqlDB { get; set; }
        private string EmployeeLog { get; set; }
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            SqlDB = _configuration["SqlDB"];
            EmployeeLog = _configuration["EmployeeLog"];
        }
        /// <summary>
        /// 查詢全部員工資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeSetting>> GetEmployee()
        {
            List<EmployeeSetting> employeeSettings = new List<EmployeeSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {EmployeeLog}";
                        employeeSettings = connection.Query<EmployeeSetting>(sql).ToList();
                    }
                });
                return employeeSettings;
            }
            catch (Exception)
            {
                return employeeSettings;
            }
        }
        /// <summary>
        /// 員工編碼查詢
        /// </summary>
        /// <param name="EmployeeNumber">員工編碼</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/EmployeeNumber/{EmployeeNumber}")]
        public async Task<List<EmployeeSetting>> GetEmployeeNumber(string EmployeeNumber)
        {
            List<EmployeeSetting> employeeSettings = new List<EmployeeSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {EmployeeLog} WHERE EmployeeNumber = @EmployeeNumber";
                        employeeSettings = connection.Query<EmployeeSetting>(sql, new { EmployeeNumber = EmployeeNumber }).ToList();
                    }
                });
                return employeeSettings;
            }
            catch (Exception)
            {
                return employeeSettings;
            }
        }
        /// <summary>
        /// 員工資訊新增
        /// </summary>
        /// <param name="employeeSetting">員工資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InserterEmployee(EmployeeSetting employeeSetting)
        {
            try
            {
                List<EmployeeSetting> employeeSettings = new List<EmployeeSetting>();
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {EmployeeLog} WHERE EmployeeNumber = @EmployeeNumber";
                        employeeSettings = connection.Query<EmployeeSetting>(sql, new { EmployeeNumber = employeeSetting.EmployeeNumber }).ToList();
                    }
                });
                if (employeeSettings.Count == 0)
                {
                    int DateIndex = 0;
                    await Task.Run(() =>
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"INSERT INTO {EmployeeLog}(EmployeeNumber,EmployeeName,Phone,Address,Token,AccountNo,PassWord) VALUES " +
                                $"(@EmployeeNumber,@EmployeeName,@Phone,@Address,@Token,@AccountNo,@PassWord)";
                            DateIndex = connection.Execute(sql, new
                            {
                                EmployeeNumber = employeeSetting.EmployeeNumber,
                                EmployeeName = employeeSetting.EmployeeName,
                                Phone = employeeSetting.Phone,
                                Address = employeeSetting.Address,
                                Token = employeeSetting.Token,
                                AccountNo = employeeSetting.AccountNo,
                                PassWord = employeeSetting.PassWord
                            });
                        }
                    });
                    if (DateIndex > 0)
                    {
                        return Ok($"{employeeSetting.EmployeeName}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{employeeSetting.EmployeeName}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{employeeSetting.EmployeeName}資訊，編碼已存在");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{employeeSetting.EmployeeName}資訊，上傳失敗");
            }
        }
        /// <summary>
        /// 員工資訊更新
        /// </summary>
        /// <param name="employeeSetting">員工資訊物件</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(EmployeeSetting employeeSetting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE {EmployeeLog} SET EmployeeName = @EmployeeName,Phone = @Phone,Address = @Address,Token = @Token,AccountNo = @AccountNo,PassWord = @PassWord" +
                            $" WHERE EmployeeNumber = @EmployeeNumber";
                        DateIndex = connection.Execute(sql, new
                        {
                            EmployeeNumber = employeeSetting.EmployeeNumber,
                            EmployeeName = employeeSetting.EmployeeName,
                            Phone = employeeSetting.Phone,
                            Address = employeeSetting.Address,
                            Token = employeeSetting.Token,
                            AccountNo = employeeSetting.AccountNo,
                            PassWord = employeeSetting.PassWord
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{employeeSetting.EmployeeName}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{employeeSetting.EmployeeName}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{employeeSetting.EmployeeName}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 員工登入
        /// </summary>
        /// <param name="employeeSetting"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/EmployeeLogin")]
        public async Task<List<EmployeeSetting>> LoginEmployee(string Account, string PassWord)
        {
            List<EmployeeSetting> EmployeeSetting = new List<EmployeeSetting>();
            try
            {
                await Task.Run(() =>
                {
                    if (!string.IsNullOrEmpty(Account) && !string.IsNullOrEmpty(PassWord))
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"SELECT * FROM  {EmployeeLog} WHERE AccountNo = @AccountNo AND PassWord = @PassWord";
                            EmployeeSetting = connection.Query<EmployeeSetting>(sql, new { AccountNo = Account, PassWord = PassWord }).ToList();
                        }
                    }
                });
                return EmployeeSetting;
            }
            catch (Exception)
            {
                return EmployeeSetting;
            }
        }
        /// <summary>
        /// 員工資訊刪除
        /// </summary>
        /// <param name="employeeSetting">員工資訊物件</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(EmployeeSetting employeeSetting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"DELETE FROM {EmployeeLog} WHERE EmployeeNumber = @EmployeeNumber";
                        DateIndex = connection.Execute(sql, new
                        {
                            EmployeeNumber = employeeSetting.EmployeeNumber
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{employeeSetting.EmployeeName}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{employeeSetting.EmployeeName}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{employeeSetting.EmployeeName}資訊，刪除失敗");
            }
        }
    }
}
