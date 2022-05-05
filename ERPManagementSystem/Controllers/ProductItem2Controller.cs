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
    public class ProductItem2Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string SqlDB { get; set; }
        public ProductItem2Controller(IConfiguration configuration)
        {
            _configuration = configuration;
            SqlDB = _configuration["SqlDB"];
        }
        /// <summary>
        /// 查詢全部項目2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ProductItem2Setting>> GetProductItem2()
        {
            List<ProductItem2Setting> productSettings = new List<ProductItem2Setting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductItem2Setting";
                        productSettings = connection.Query<ProductItem2Setting>(sql).ToList();
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
        /// 新增項目2
        /// </summary>
        /// <param name="productItem2Setting"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostProductItem2(ProductItem2Setting productItem2Setting)
        {
            try
            {
                List<ProductItem2Setting> productDepartmentSettings = new List<ProductItem2Setting>();
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductItem2Setting WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number AND Item2Number = @Item2Number";
                        productDepartmentSettings = connection.Query<ProductItem2Setting>(sql, new
                        {
                            DepartmentNumber = productItem2Setting.DepartmentNumber,
                            Item1Number = productItem2Setting.Item1Number,
                            Item2Number = productItem2Setting.Item2Number
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
                            string sql = $"INSERT INTO ProductItem2Setting (DepartmentNumber,Item1Number , Item2Number, Item2Name) VALUES " +
                                $"(@DepartmentNumber,@Item1Number,@Item2Number,@Item2Name)";
                            DateIndex = connection.Execute(sql, new
                            {
                                DepartmentNumber = productItem2Setting.DepartmentNumber,
                                Item1Number = productItem2Setting.Item1Number,
                                Item2Number = productItem2Setting.Item2Number,
                                Item2Name = productItem2Setting.Item2Name
                            });
                        }
                    });
                    if (DateIndex > 0)
                    {
                        return Ok($"{productItem2Setting.Item2Name}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{productItem2Setting.Item2Name}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{productItem2Setting.Item2Name}資訊，編碼已存在");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{productItem2Setting.Item2Name}資訊，上傳失敗");
            }
        }
        /// <summary>
        /// 更新項目2
        /// </summary>
        /// <param name="productItem2Setting"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutProductDepartment(ProductItem2Setting productItem2Setting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE ProductItem2Setting SET Item2Name = @Item2Name" +
                            $" WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number AND Item2Number = @Item2Number";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productItem2Setting.DepartmentNumber,
                            Item1Number = productItem2Setting.Item1Number,
                            Item2Number = productItem2Setting.Item2Number,
                            Item2Name = productItem2Setting.Item2Name
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productItem2Setting.Item2Name}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{productItem2Setting.Item2Name}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productItem2Setting.Item2Name}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 刪除項目2
        /// </summary>
        /// <param name="productItem2Setting"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(ProductItem2Setting productItem2Setting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"DELETE FROM ProductItem2Setting WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number AND Item2Number = @Item2Number";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productItem2Setting.DepartmentNumber,
                            Item1Number = productItem2Setting.Item1Number,
                            Item2Number = productItem2Setting.Item2Number
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productItem2Setting.Item2Name}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{productItem2Setting.Item2Name}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productItem2Setting.Item2Name}資訊，刪除失敗");
            }
        }
    }
}
