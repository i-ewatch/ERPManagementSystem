using Dapper;
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
    public class ProductCategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        private string ProductCategoryLog { get; set; }
        public ProductCategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
            SqlDB = _configuration["SqlDB"];
            ProductCategoryLog = _configuration["ProductCategoryLog"];
        }
        /// <summary>
        /// 查詢全部產品類別資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ProductCategorySetting> GetCategoryProduct()
        {
            List<ProductCategorySetting> productCategorySettings = new List<ProductCategorySetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {ProductCategoryLog} order by CategoryNumber ";
                    productCategorySettings = connection.Query<ProductCategorySetting>(sql).ToList();
                }
                return productCategorySettings;
            }
            catch (Exception)
            {
                return productCategorySettings;
            }
        }

        /// <summary>
        /// 產品類別資訊新增
        /// </summary>
        /// <param name="productCategorySetting">產品類別資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostProduct(ProductCategorySetting productCategorySetting)
        {
            try
            {
                List<ProductCategorySetting> productCategorySettings = new List<ProductCategorySetting>();
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {ProductCategoryLog} WHERE CategoryNumber = @CategoryNumber";
                    productCategorySettings = connection.Query<ProductCategorySetting>(sql, new { CategoryNumber = productCategorySetting.CategoryNumber }).ToList();
                }
                if (productCategorySettings.Count == 0)
                {
                    int DateIndex = 0;
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"INSERT INTO {ProductCategoryLog}(CategoryNumber , CategoryName) VALUES " +
                            $"(@CategoryNumber,@CategoryName)";
                        DateIndex = connection.Execute(sql, new { CategoryNumber = productCategorySetting.CategoryNumber, CategoryName = productCategorySetting.CategoryName });
                    }
                    if (DateIndex > 0)
                    {
                        return Ok($"{productCategorySetting.CategoryName}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{productCategorySetting.CategoryName}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{productCategorySetting.CategoryName}資訊，編碼已存在");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{productCategorySetting.CategoryName}資訊，上傳失敗");
            }
        }

        /// <summary>
        /// 產品類別資訊更新
        /// </summary>
        /// <param name="productCategorySetting">產品類別資訊物件</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateProductCategory(ProductCategorySetting productCategorySetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"UPDATE {ProductCategoryLog} SET CategoryName = @CategoryName " +
                        $" WHERE CategoryNumber = @CategoryNumber";
                    DateIndex = connection.Execute(sql, new { CategoryNumber = productCategorySetting.CategoryNumber, CategoryName = productCategorySetting.CategoryName });
                }
                if (DateIndex > 0)
                {
                    return Ok($"{productCategorySetting.CategoryName}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{productCategorySetting.CategoryName}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productCategorySetting.CategoryName}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 產品類別資訊刪除
        /// </summary>
        /// <param name="productCategorySetting">產品類別資訊物件</param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteProductCategory(ProductCategorySetting productCategorySetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"DELETE FROM {ProductCategoryLog} WHERE CategoryNumber = @CategoryNumber";
                    DateIndex = connection.Execute(sql, new { CategoryNumber = productCategorySetting.CategoryNumber });
                }
                if (DateIndex > 0)
                {
                    return Ok($"{productCategorySetting.CategoryName}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{productCategorySetting.CategoryName}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productCategorySetting.CategoryName}資訊，刪除失敗");
            }
        }
    }
}
