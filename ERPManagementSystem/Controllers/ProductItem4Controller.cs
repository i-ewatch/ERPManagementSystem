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
    public class ProductItem4Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string SqlDB { get; set; }
        public ProductItem4Controller(IConfiguration configuration)
        {
            _configuration = configuration;
            SqlDB = _configuration["SqlDB"];
        }
        /// <summary>
        /// 查詢全部項目4
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ProductItem4Setting>> GetProductItem3()
        {
            List<ProductItem4Setting> productSettings = new List<ProductItem4Setting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductItem4Setting";
                        productSettings = connection.Query<ProductItem4Setting>(sql).ToList();
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
        /// 新增項目4
        /// </summary>
        /// <param name="productItem4Setting"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostProductItem3(ProductItem4Setting productItem4Setting)
        {
            try
            {
                List<ProductItem4Setting> productDepartmentSettings = new List<ProductItem4Setting>();
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductItem4Setting WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number AND Item2Number = @Item2Number AND Item3Number = @Item3Number  AND Item4Number = @Item4Number";
                        productDepartmentSettings = connection.Query<ProductItem4Setting>(sql, new
                        {
                            DepartmentNumber = productItem4Setting.DepartmentNumber,
                            Item1Number = productItem4Setting.Item1Number,
                            Item2Number = productItem4Setting.Item2Number,
                            Item3Number = productItem4Setting.Item3Number,
                            Item4Number = productItem4Setting.Item4Number
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
                            string sql = $"INSERT INTO ProductItem4Setting (DepartmentNumber,Item1Number , Item2Number, Item3Number, Item4Number, Item4Name) VALUES " +
                                $"(@DepartmentNumber,@Item1Number,@Item2Number,@Item3Number,@Item4Number,@Item4Name)";
                            DateIndex = connection.Execute(sql, new
                            {
                                DepartmentNumber = productItem4Setting.DepartmentNumber,
                                Item1Number = productItem4Setting.Item1Number,
                                Item2Number = productItem4Setting.Item2Number,
                                Item3Number = productItem4Setting.Item3Number,
                                Item4Number = productItem4Setting.Item4Number,
                                Item4Name = productItem4Setting.Item4Name
                            });
                        }
                    });
                    if (DateIndex > 0)
                    {
                        return Ok($"{productItem4Setting.Item4Name}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{productItem4Setting.Item4Name}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{productItem4Setting.Item4Name}資訊，編碼已存在");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{productItem4Setting.Item4Name}資訊，上傳失敗");
            }
        }
        /// <summary>
        /// 更新項目4
        /// </summary>
        /// <param name="productItem4Setting"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutProductDepartment(ProductItem4Setting productItem4Setting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE ProductItem4Setting SET Item4Name = @Item4Name" +
                            $" WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number AND Item2Number = @Item2Number AND Item3Number = @Item3Number  AND Item4Number = @Item4Number";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productItem4Setting.DepartmentNumber,
                            Item1Number = productItem4Setting.Item1Number,
                            Item2Number = productItem4Setting.Item2Number,
                            Item3Number = productItem4Setting.Item3Number,
                            Item4Number = productItem4Setting.Item4Number,
                            Item4Name = productItem4Setting.Item4Name
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productItem4Setting.Item4Name}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{productItem4Setting.Item4Name}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productItem4Setting.Item4Name}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 刪除項目4
        /// </summary>
        /// <param name="productItem4Setting"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(ProductItem4Setting productItem4Setting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"DELETE FROM ProductItem4Setting WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number AND Item2Number = @Item2Number AND Item3Number = @Item3Number  AND Item4Number = @Item4Number";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productItem4Setting.DepartmentNumber,
                            Item1Number = productItem4Setting.Item1Number,
                            Item2Number = productItem4Setting.Item2Number,
                            Item3Number = productItem4Setting.Item3Number,
                            Item4Number = productItem4Setting.Item4Number
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productItem4Setting.Item4Name}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{productItem4Setting.Item4Name}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productItem4Setting.Item4Name}資訊，刪除失敗");
            }
        }
    }
}
