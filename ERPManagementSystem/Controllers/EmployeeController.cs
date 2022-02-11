﻿using Dapper;
using ERPManagementSystem.Modules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
        public List<EmployeeSetting> GetEmployee()
        {
            List<EmployeeSetting> employeeSettings = new List<EmployeeSetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {EmployeeLog}";
                    employeeSettings = connection.Query<EmployeeSetting>(sql).ToList();
                }
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
        public List<EmployeeSetting> GetEmployeeNumber(string EmployeeNumber)
        {
            List<EmployeeSetting> employeeSettings = new List<EmployeeSetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {EmployeeLog} WHERE EmployeeNumber = N'{EmployeeNumber}'";
                    employeeSettings = connection.Query<EmployeeSetting>(sql).ToList();
                }
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
        [Route("/api/InserterEmployee")]
        public IActionResult InserterEmployee(EmployeeSetting employeeSetting)
        {
            try
            {
                List<EmployeeSetting> employeeSettings = new List<EmployeeSetting>();
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {EmployeeLog} WHERE EmployeeNumber = N'{employeeSetting.EmployeeNumber}'";
                    employeeSettings = connection.Query<EmployeeSetting>(sql).ToList();
                }
                if (employeeSettings.Count == 0)
                {
                    int DateIndex = 0;
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"INSERT INTO {EmployeeLog}(EmployeeNumber,EmployeeName,Phone,Address,Token) VALUES " +
                            $"(N'{employeeSetting.EmployeeNumber}',N'{employeeSetting.EmployeeName}',N'{employeeSetting.Phone}',N'{employeeSetting.Address}',{employeeSetting.Token})";
                        DateIndex = connection.Execute(sql);
                    }
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
        [HttpPost]
        [Route("/api/UpdateEmployee")]
        public IActionResult UpdateEmployee(EmployeeSetting employeeSetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"UPDATE {EmployeeLog} SET EmployeeName = N'{employeeSetting.EmployeeName}',Phone = N'{employeeSetting.Phone}',Address = N'{employeeSetting.Address}',Token = {employeeSetting.Token}" +
                        $" WHERE EmployeeNumber = N'{employeeSetting.EmployeeNumber}'";
                    DateIndex = connection.Execute(sql);
                }
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
    }
}
