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
    public class ProductItem1Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string SqlDB { get; set; }
        public ProductItem1Controller(IConfiguration configuration)
        {
            _configuration = configuration;
            SqlDB = _configuration["SqlDB"];
        }
        /// <summary>
        /// 查詢全部項目1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ProductItem1Setting>> GetProductItem1()
        {
            List<ProductItem1Setting> productSettings = new List<ProductItem1Setting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductItem1Setting";
                        productSettings = connection.Query<ProductItem1Setting>(sql).ToList();
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
        /// 新增項目1
        /// </summary>
        /// <param name="productItem1Setting"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostProductItem1(ProductItem1Setting productItem1Setting)
        {
            try
            {
                List<ProductItem1Setting> productDepartmentSettings = new List<ProductItem1Setting>();
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductItem1Setting WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number";
                        productDepartmentSettings = connection.Query<ProductItem1Setting>(sql, new
                        {
                            DepartmentNumber = productItem1Setting.DepartmentNumber,
                            Item1Number = productItem1Setting.Item1Number
                        }).ToList();
                    }
                });
                if (productDepartmentSettings.Count == 0)
                {
                    int DateIndex = 0;
                    await Task.Run(() =>
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"INSERT INTO ProductItem1Setting (DepartmentNumber , Item1Number, Item1Name) VALUES " +
                                $"(@DepartmentNumber,@Item1Number,@Item1Name)";
                            DateIndex = connection.Execute(sql, new
                            {
                                DepartmentNumber = productItem1Setting.DepartmentNumber,
                                Item1Number = productItem1Setting.Item1Number,
                                Item1Name = productItem1Setting.Item1Name
                            });
                        }
                    });
                    if (DateIndex > 0)
                    {
                        return Ok($"{productItem1Setting.Item1Name}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{productItem1Setting.Item1Name}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{productItem1Setting.Item1Name}資訊，編碼已存在");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{productItem1Setting.Item1Name}資訊，上傳失敗");
            }
        }
        /// <summary>
        /// 更新項目1
        /// </summary>
        /// <param name="productItem1Setting"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutProductDepartment(ProductItem1Setting productItem1Setting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE ProductItem1Setting SET Item1Name = @Item1Name" +
                            $" WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productItem1Setting.DepartmentNumber,
                            Item1Number = productItem1Setting.Item1Number,
                            Item1Name = productItem1Setting.Item1Name
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productItem1Setting.Item1Name}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{productItem1Setting.Item1Name}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productItem1Setting.Item1Name}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 刪除項目1
        /// </summary>
        /// <param name="productItem1Setting"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(ProductItem1Setting productItem1Setting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"DELETE FROM ProductItem1Setting WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productItem1Setting.DepartmentNumber,
                            Item1Number = productItem1Setting.Item1Number
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productItem1Setting.Item1Name}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{productItem1Setting.Item1Name}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productItem1Setting.Item1Name}資訊，刪除失敗");
            }
        }
    }
}
