using Dapper;
using ERPManagementSystem.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ERPManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        private string ProductLog { get; set; }
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "產品";
            SqlDB = _configuration["SqlDB"];
            ProductLog = _configuration["ProductLog"];
        }
        /// <summary>
        /// 查詢全部產品資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ProductSetting> GetProduct()
        {
            List<ProductSetting> productSettings = new List<ProductSetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {ProductLog} order by ProductNumber ";
                    productSettings = connection.Query<ProductSetting>(sql).ToList();
                }
                return productSettings;
            }
            catch (Exception)
            {
                return productSettings;
            }
        }

        /// <summary>
        /// 產品種類代碼查詢
        /// </summary>
        /// <param name="ProductType">產品種類代碼</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/ProductNumber/ProductType/{ProductType}")]
        public List<ProductSetting> GetProductType(string ProductType)
        {
            List<ProductSetting> productSettings = new List<ProductSetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {ProductLog} WHERE ProductType = N'{ProductType}' order by ProductNumber ";
                    productSettings = connection.Query<ProductSetting>(sql).ToList();
                }
                return productSettings;
            }
            catch (Exception)
            {
                return productSettings;
            }
        }

        /// <summary>
        /// 產品代碼查詢
        /// </summary>
        /// <param name="ProductNumber">產品代碼</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/ProductNumber/{ProductNumber}")]
        public List<ProductSetting> GetProductNumber(string ProductNumber)
        {
            List<ProductSetting> productSettings = new List<ProductSetting>();
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {ProductLog} WHERE ProductNumber = N'{ProductNumber}' ";
                    productSettings = connection.Query<ProductSetting>(sql).ToList();
                }
                return productSettings;
            }
            catch (Exception)
            {
                return productSettings;
            }
        }

        /// <summary>
        /// 產品資訊新增
        /// </summary>
        /// <param name="productSetting">產品資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/InserterProduct")]
        public IActionResult PostProduct(ProductSetting productSetting)
        {
            try
            {
                List<CompanySetting> companySettings = new List<CompanySetting>();
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"SELECT * FROM  {ProductLog} WHERE ProductNumber = N'{productSetting.ProductNumber}'";
                    companySettings = connection.Query<CompanySetting>(sql).ToList();
                }
                if (companySettings.Count == 0)
                {
                    int DateIndex = 0;
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"INSERT INTO {ProductLog}(ProductNumber , ProductName , ProductModel , ProductType , ProductCategory , FootPrint , Remark , Explanation , ProductCompanyNumber) VALUES " +
                            $"(N'{productSetting.ProductNumber}',N'{productSetting.ProductName}',N'{productSetting.ProductModel}',{productSetting.ProductType},N'{productSetting.ProductCategory}',N'{productSetting.FootPrint}',N'{productSetting.Remark}',N'{productSetting.Explanation}'" +
                            $",N'{productSetting.Explanation}')";
                        DateIndex = connection.Execute(sql);
                    }
                    if (DateIndex > 0)
                    {
                        return Ok($"{productSetting.ProductName}資訊，上傳成功!");
                    }
                    else
                    {
                        return BadRequest($"{productSetting.ProductName}資訊，上傳失敗");
                    }
                }
                else
                {
                    return BadRequest($"{productSetting.ProductName}資訊，編碼已存在");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{productSetting.ProductName}資訊，上傳失敗");
            }
        }
        /// <summary>
        /// 產品資訊更新
        /// </summary>
        /// <param name="productSetting">產品資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/UpdateProduct")]
        public IActionResult UpdateProduct(ProductSetting productSetting)
        {
            try
            {
                int DateIndex = 0;
                using (IDbConnection connection = new SqlConnection(SqlDB))
                {
                    string sql = $"UPDATE {ProductLog} SET ProductName = N'{productSetting.ProductName}',ProductModel = N'{productSetting.ProductModel}',ProductType = {productSetting.ProductType},ProductCategory = N'{productSetting.ProductCategory}',FootPrint = N'{productSetting.FootPrint}'," +
                        $"Remark = N'{productSetting.Remark}',Explanation = N'{productSetting.Explanation}' ,ProductCompanyNumber = N'{productSetting.ProductCompanyNumber}'" +
                        $" WHERE ProductNumber = N'{productSetting.ProductNumber}'";
                    DateIndex = connection.Execute(sql);
                }
                if (DateIndex > 0)
                {
                    return Ok($"{productSetting.ProductName}資訊，更新成功!");
                }
                else
                {
                    return BadRequest($"{productSetting.ProductName}資訊，更新失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productSetting.ProductName}資訊，更新失敗");
            }
        }
        /// <summary>
        /// 產品附件檔案更新
        /// </summary>
        /// <param name="ProductNumber">產品編碼</param>
        /// <param name="ProductName">產品名稱</param>
        /// <param name="AttachmentFile">附件檔案</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/UpdateProductAttachmentFile")]
        public IActionResult PostProductAttachmentFile(string ProductNumber, string ProductName, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(ProductNumber) && !string.IsNullOrEmpty(ProductName))
                {
                    WorkPath += $"\\{ProductNumber}";
                    if (!Directory.Exists(WorkPath))
                    {
                        Directory.CreateDirectory($"{WorkPath}");
                    }
                    WorkPath += $"\\{AttachmentFile.FileName}";
                    using (var stream = new FileStream(WorkPath, FileMode.Create))
                    {
                        AttachmentFile.CopyToAsync(stream);
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE {ProductLog} SET FileName = N'{AttachmentFile.FileName}' WHERE ProductNumber = N'{ProductNumber}' AND ProductName = N'{ProductName}'";
                        connection.Execute(sql);
                    }
                    return Ok($"{ProductName}檔案上傳成功");
                }
                else
                {
                    return BadRequest($"{ProductName}檔案上傳失敗");
                }
                #region 資料庫存檔
                //byte[] attachmentFile = new byte[AttachmentFile.Length];
                //AttachmentFile.OpenReadStream().Read(attachmentFile, 0, Convert.ToInt32(AttachmentFile.Length));
                //using (IDbConnection connection = new SqlConnection(SqlDB))
                //{
                //    string sql = $"UPDATE {ProductLog} SET AttachmentFile = @file,FileExtension = N'{FileExtension}' WHERE ProductNumber = N'{ProductNumber}' AND ProductName = N'{ProductName}'";
                //    connection.Execute(sql, new { file = attachmentFile });
                //}
                //return Ok($"{ProductName}檔案上傳成功");
                #endregion
            }
            catch (Exception)
            {

                return BadRequest($"{ProductName}檔案上傳失敗");
            }
        }
    }
}
