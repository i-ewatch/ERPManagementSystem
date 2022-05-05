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
    public class ProductItem5Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string SqlDB { get; set; }
        public ProductItem5Controller(IConfiguration configuration)
        {
            _configuration = configuration;
            SqlDB = _configuration["SqlDB"];
        }
        /// <summary>
        /// 查詢全部項目5
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ProductItem5Setting>> GetProductItem3()
        {
            List<ProductItem5Setting> productSettings = new List<ProductItem5Setting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductItem5Setting";
                        productSettings = connection.Query<ProductItem5Setting>(sql).ToList();
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
        /// 新增項目5
        /// </summary>
        /// <param name="productItem5Setting"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostProductItem3(ProductItem5Setting productItem5Setting)
        {
            try
            {
                List<ProductItem5Setting> productDepartmentSettings = new List<ProductItem5Setting>();
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProductItem5Setting WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number AND Item2Number = @Item2Number AND Item3Number = @Item3Number  AND Item4Number = @Item4Number AND Item5Number = @Item5Number";
                        productDepartmentSettings = connection.Query<ProductItem5Setting>(sql, new
                        {
                            DepartmentNumber = productItem5Setting.DepartmentNumber,
                            Item1Number = productItem5Setting.Item1Number,
                            Item2Number = productItem5Setting.Item2Number,
                            Item3Number = productItem5Setting.Item3Number,
                            Item4Number = productItem5Setting.Item4Number,
                            Item5Number = productItem5Setting.Item5Number
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
                            string sql = $"INSERT INTO ProductItem5Setting (DepartmentNumber,Item1Number , Item2Number, Item3Number, Item4Number, Item5Number, Item5Name) VALUES " +
                                $"(@DepartmentNumber,@Item1Number,@Item2Number,@Item3Number,@Item4Number,@Item5Number,@Item5Name)";
                            DateIndex = connection.Execute(sql, new
                            {
                                DepartmentNumber = productItem5Setting.DepartmentNumber,
                                Item1Number = productItem5Setting.Item1Number,
                                Item2Number = productItem5Setting.Item2Number,
                                Item3Number = productItem5Setting.Item3Number,
                                Item4Number = productItem5Setting.Item4Number,
                                Item5Number = productItem5Setting.Item5Number,
                                Item5Name = productItem5Setting.Item5Name
                            });
                        }
                    });
                    if (DateIndex > 0)
                    {
                        return Ok($"{productItem5Setting.Item5Name}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{productItem5Setting.Item5Name}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{productItem5Setting.Item5Name}資訊，編碼已存在");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{productItem5Setting.Item5Name}資訊，上傳失敗");
            }
        }
        /// <summary>
        /// 更新項目5
        /// </summary>
        /// <param name="productItem5Setting"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutProductDepartment(ProductItem5Setting productItem5Setting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE ProductItem5Setting SET Item5Name = @Item5Name" +
                            $" WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number AND Item2Number = @Item2Number AND Item3Number = @Item3Number  AND Item4Number = @Item4Number  AND Item5Number = @Item5Number";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productItem5Setting.DepartmentNumber,
                            Item1Number = productItem5Setting.Item1Number,
                            Item2Number = productItem5Setting.Item2Number,
                            Item3Number = productItem5Setting.Item3Number,
                            Item4Number = productItem5Setting.Item4Number,
                            Item5Number = productItem5Setting.Item5Number,
                            Item5Name = productItem5Setting.Item5Name
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productItem5Setting.Item5Name}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{productItem5Setting.Item5Name}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productItem5Setting.Item5Name}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 刪除項目5
        /// </summary>
        /// <param name="productItem5Setting"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(ProductItem5Setting productItem5Setting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"DELETE FROM ProductItem5Setting WHERE DepartmentNumber = @DepartmentNumber AND Item1Number = @Item1Number AND Item2Number = @Item2Number AND Item3Number = @Item3Number  AND Item4Number = @Item4Number AND Item5Number = @Item5Number";
                        DateIndex = connection.Execute(sql, new
                        {
                            DepartmentNumber = productItem5Setting.DepartmentNumber,
                            Item1Number = productItem5Setting.Item1Number,
                            Item2Number = productItem5Setting.Item2Number,
                            Item3Number = productItem5Setting.Item3Number,
                            Item4Number = productItem5Setting.Item4Number,
                            Item5Number = productItem5Setting.Item5Number
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productItem5Setting.Item5Name}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{productItem5Setting.Item5Name}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productItem5Setting.Item5Name}資訊，刪除失敗");
            }
        }
    }
}
