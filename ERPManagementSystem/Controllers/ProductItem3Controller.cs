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
    public class ProductItem3Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string SqlDB { get; set; }
        public ProductItem3Controller(IConfiguration configuration)
        {
            _configuration = configuration;
            SqlDB = _configuration["SqlDB"];
        }
        /// <summary>
        /// 查詢全部項目3
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ProductItem3Setting>> GetProductItem3()
        {
            List<ProductItem3Setting> productSettings = new List<ProductItem3Setting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductItem3Setting";
                        productSettings = connection.Query<ProductItem3Setting>(sql).ToList();
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
        /// 新增項目3
        /// </summary>
        /// <param name="productItem3Setting"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostProductItem3(ProductItem3Setting productItem3Setting)
        {
            try
            {
                List<ProductItem3Setting> productDepartmentSettings = new List<ProductItem3Setting>();
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductItem3Setting WHERE DepartmentNumber = @DepartmentNumber  AND Item1Number = @Item1Number AND Item2Number = @Item2Number AND Item3Number = @Item3Number";
                        productDepartmentSettings = connection.Query<ProductItem3Setting>(sql, new
                        {
                            DepartmentNumber = productItem3Setting.DepartmentNumber,
                            Item1Number = productItem3Setting.Item1Number,
                            Item2Number = productItem3Setting.Item2Number,
                            Item3Number = productItem3Setting.Item3Number
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
                            string sql = $"INSERT INTO ProductItem3Setting (DepartmentNumber,Item1Number , Item2Number, Item3Number, Item3Name) VALUES " +
                                $"(@DepartmentNumber,@Item1Number,@Item2Number,@Item3Number,@Item3Name)";
                            DateIndex = connection.Execute(sql, new
                            {
                                DepartmentNumber = productItem3Setting.DepartmentNumber,
                                Item1Number = productItem3Setting.Item1Number,
                                Item2Number = productItem3Setting.Item2Number,
                                Item3Number = productItem3Setting.Item3Number,
                                Item3Name = productItem3Setting.Item3Name
                            });
                        }
                    });
                    if (DateIndex > 0)
                    {
                        return Ok($"{productItem3Setting.Item3Name}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{productItem3Setting.Item3Name}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{productItem3Setting.Item3Name}資訊，編碼已存在");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{productItem3Setting.Item3Name}資訊，上傳失敗");
            }
        }
        /// <summary>
        /// 更新項目3
        /// </summary>
        /// <param name="productItem3Setting"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutProductDepartment(ProductItem3Setting productItem3Setting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE ProductItem3Setting SET Item3Name = @Item3Name" +
                            $" WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number AND Item2Number = @Item2Number AND Item3Number = @Item3Number";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productItem3Setting.DepartmentNumber,
                            Item1Number = productItem3Setting.Item1Number,
                            Item2Number = productItem3Setting.Item2Number,
                            Item3Number = productItem3Setting.Item3Number,
                            Item3Name = productItem3Setting.Item3Name
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productItem3Setting.Item3Name}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{productItem3Setting.Item3Name}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productItem3Setting.Item3Name}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 刪除項目3
        /// </summary>
        /// <param name="productItem3Setting"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(ProductItem3Setting productItem3Setting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"DELETE FROM ProductItem3Setting WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number AND Item2Number = @Item2Number AND Item3Number = @Item3Number";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productItem3Setting.DepartmentNumber,
                            Item1Number = productItem3Setting.Item1Number,
                            Item2Number = productItem3Setting.Item2Number,
                            Item3Number = productItem3Setting.Item3Number
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productItem3Setting.Item3Name}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{productItem3Setting.Item3Name}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productItem3Setting.Item3Name}資訊，刪除失敗");
            }
        }
    }
}
