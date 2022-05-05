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
    public class ProductDepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string SqlDB { get; set; }
        public ProductDepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
            SqlDB = _configuration["SqlDB"];
        }
        /// <summary>
        /// 查詢全部部門
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ProductDepartmentSetting>> GetProductDepartment()
        {
            List<ProductDepartmentSetting> productSettings = new List<ProductDepartmentSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductDepartmentSetting";
                        productSettings = connection.Query<ProductDepartmentSetting>(sql).ToList();
                    }
                });
                return productSettings;
            }
            catch (Exception)
            {
                return productSettings;
            }
        }
        /// <summary>
        /// 新增部門
        /// </summary>
        /// <param name="productDepartmentSetting"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostProductDepartment(ProductDepartmentSetting productDepartmentSetting)
        {
            try
            {
                List<ProductDepartmentSetting> productDepartmentSettings = new List<ProductDepartmentSetting>();
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductDepartmentSetting WHERE DepartmentNumber = @DepartmentNumber";
                        productDepartmentSettings = connection.Query<ProductDepartmentSetting>(sql, new { DepartmentNumber = productDepartmentSetting.DepartmentNumber }).ToList();
                    }
                });
                if (productDepartmentSettings.Count == 0)
                {
                    int DateIndex = 0;
                    await Task.Run(() =>
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"INSERT INTO ProductDepartmentSetting (DepartmentNumber , DepartmentName) VALUES " +
                                $"(@DepartmentNumber,@DepartmentName)";
                            DateIndex = connection.Execute(sql, new
                            {
                                DepartmentNumber = productDepartmentSetting.DepartmentNumber,
                                DepartmentName = productDepartmentSetting.DepartmentName
                            });
                        }
                    });
                    if (DateIndex > 0)
                    {
                        return Ok($"{productDepartmentSetting.DepartmentName}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{productDepartmentSetting.DepartmentName}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{productDepartmentSetting.DepartmentName}資訊，編碼已存在");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{productDepartmentSetting.DepartmentName}資訊，上傳失敗");
            }
        }
        /// <summary>
        /// 更新部門
        /// </summary>
        /// <param name="productDepartmentSetting"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutProductDepartment(ProductDepartmentSetting productDepartmentSetting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE ProductDepartmentSetting SET DepartmentName = @DepartmentName" +
                            $" WHERE DepartmentNumber = @DepartmentNumber";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productDepartmentSetting.DepartmentNumber,
                            DepartmentName = productDepartmentSetting.DepartmentName
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productDepartmentSetting.DepartmentName}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{productDepartmentSetting.DepartmentName}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productDepartmentSetting.DepartmentName}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 刪除部門
        /// </summary>
        /// <param name="productDepartmentSetting"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(ProductDepartmentSetting productDepartmentSetting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"DELETE FROM ProductDepartmentSetting WHERE DepartmentNumber = @DepartmentNumber";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productDepartmentSetting.DepartmentNumber
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productDepartmentSetting.DepartmentName}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{productDepartmentSetting.DepartmentName}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productDepartmentSetting.DepartmentName}資訊，刪除失敗");
            }
        }
    }
}
