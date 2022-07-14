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
using System.Threading.Tasks;

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
        public async Task<List<ProductSetting>> GetProduct()
        {
            List<ProductSetting> productSettings = new List<ProductSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {ProductLog} order by ProductNumber ";
                        productSettings = connection.Query<ProductSetting>(sql).ToList();
                        string company = "SELECT * FROM CompanySetting";
                        var Company = connection.Query<CompanySetting>(company).ToList();
                        foreach (var item in productSettings)
                        {
                            var companydata = Company.SingleOrDefault(g => g.CompanyNumber == item.ProductCompanyNumber);
                            if (companydata != null)
                            {
                                item.ProductCompanyNumber = companydata.CompanyName;
                            }
                        }
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
        /// 產品種類代碼查詢
        /// </summary>
        /// <param name="ProductType">產品種類代碼</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/ProductNumber/ProductType/{ProductType}")]
        public async Task<List<ProductSetting>> GetProductType(string ProductType)
        {
            List<ProductSetting> productSettings = new List<ProductSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {ProductLog} WHERE ProductType = @ProductType order by ProductNumber ";
                        productSettings = connection.Query<ProductSetting>(sql, new { ProductType = ProductType }).ToList();
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
        /// 產品代碼查詢
        /// </summary>
        /// <param name="ProductNumber">產品代碼</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/ProductNumber/{ProductNumber}")]
        public async Task<List<ProductSetting>> GetProductNumber(string ProductNumber)
        {
            List<ProductSetting> productSettings = new List<ProductSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {ProductLog} WHERE ProductNumber = @ProductNumber ";
                        productSettings = connection.Query<ProductSetting>(sql, new { ProductNumber = ProductNumber }).ToList();
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
        /// 產品資訊新增
        /// </summary>
        /// <param name="productSetting">產品資訊物件</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ProductSetting> PostProduct(ProductSetting productSetting)
        {
            try
            {
                List<ProductSetting> ProductSettings = new List<ProductSetting>();
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  {ProductLog} WHERE ProductNumber Like CONCAT(@ProductNumber,'%')";
                        ProductSettings = connection.Query<ProductSetting>(sql, new { ProductNumber = productSetting.ProductNumber }).ToList();
                    }
                });
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"INSERT INTO {ProductLog}(ProductNumber , ProductName , ProductModel , ProductType , ProductCategory , FootPrint , Remark , Explanation , ProductCompanyNumber) VALUES " +
                            $"(@ProductNumber,@ProductName,@ProductModel,@ProductType,@ProductCategory,@FootPrint,@Remark,@Explanation" +
                            $",@ProductCompanyNumber)";
                        int ProductIndex = 0;
                        foreach (var item in ProductSettings)
                        {
                            if (productSetting.ProductNumber + (ProductSettings.Count().ToString()).PadLeft(4, '0') == item.ProductNumber)
                            {
                                ProductIndex = ProductSettings.Count() + 1;
                                break;
                            }
                        }
                        DateIndex = connection.Execute(sql, new
                        {
                            ProductNumber = productSetting.ProductNumber + (ProductIndex.ToString()).PadLeft(4, '0'),
                            ProductName = productSetting.ProductName,
                            ProductModel = productSetting.ProductModel,
                            ProductType = productSetting.ProductType,
                            ProductCategory = productSetting.ProductCategory,
                            FootPrint = productSetting.FootPrint,
                            Remark = productSetting.Remark,
                            Explanation = productSetting.Explanation,
                            ProductCompanyNumber = productSetting.ProductCompanyNumber
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    productSetting.ProductNumber = productSetting.ProductNumber + (ProductSettings.Count().ToString()).PadLeft(4, '0');
                    return productSetting;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// 產品資訊更新
        /// </summary>
        /// <param name="productSetting">產品資訊物件</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductSetting productSetting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE {ProductLog} SET ProductName = @ProductName,ProductModel = @ProductModel,ProductType = @ProductType,ProductCategory = @ProductCategory,FootPrint = @FootPrint," +
                            $"Remark = @Remark,Explanation = @Explanation ,ProductCompanyNumber = @ProductCompanyNumber" +
                            $" WHERE ProductNumber = @ProductNumber";
                        DateIndex = connection.Execute(sql, new
                        {
                            ProductNumber = productSetting.ProductNumber,
                            ProductName = productSetting.ProductName,
                            ProductModel = productSetting.ProductModel,
                            ProductType = productSetting.ProductType,
                            ProductCategory = productSetting.ProductCategory,
                            FootPrint = productSetting.FootPrint,
                            Remark = productSetting.Remark,
                            Explanation = productSetting.Explanation,
                            ProductCompanyNumber = productSetting.ProductCompanyNumber
                        });
                    }
                });
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
        /// 產品資訊刪除
        /// </summary>
        /// <param name="productSetting">產品資訊物件</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(ProductSetting productSetting)
        {
            try
            {
                int DateIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"DELETE FROM {ProductLog} WHERE ProductNumber = @ProductNumber";
                        DateIndex = connection.Execute(sql, new
                        {
                            ProductNumber = productSetting.ProductNumber,
                        });
                    }
                });
                if (DateIndex > 0)
                {
                    return Ok($"{productSetting.ProductName}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{productSetting.ProductName}資訊，刪除失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{productSetting.ProductName}資訊，刪除失敗");
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
        [Route("/api/ProductAttachmentFile")]
        public async Task<IActionResult> PostProductAttachmentFile(string ProductNumber, string ProductName, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(ProductNumber) && !string.IsNullOrEmpty(ProductName))
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{ProductNumber}";
                        if (!Directory.Exists(WorkPath))
                        {
                            Directory.CreateDirectory($"{WorkPath}");
                        }
                        else
                        {
                            foreach (string file in Directory.GetFileSystemEntries(WorkPath))
                            {
                                if (System.IO.File.Exists(file))
                                {
                                    System.IO.File.Delete(file);
                                }
                            }
                        }
                        WorkPath += $"\\{AttachmentFile.FileName}";
                        using (var stream = new FileStream(WorkPath, FileMode.Create))
                        {
                            var fs = new BinaryReader(AttachmentFile.OpenReadStream());
                            int filelong = Convert.ToInt32(AttachmentFile.Length);
                            var bytes = new byte[filelong];
                            fs.Read(bytes, 0, filelong);
                            stream.Write(bytes, 0, filelong);
                            fs.Close();
                            stream.Flush();
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string sql = $"UPDATE {ProductLog} SET FileName = @FileName WHERE ProductNumber = @ProductNumber AND ProductName = @ProductName";
                            connection.Execute(sql, new { FileName = AttachmentFile.FileName, ProductNumber = ProductNumber, ProductName = ProductName });
                        }
                    });
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
        /// <summary>
        /// 產品附件檔案下載
        /// </summary>
        /// <param name="ProductNumber">產品編碼</param>
        /// <param name="ProductName">產品名稱</param>
        /// <param name="AttachmentFile">附件檔案</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/ProductAttachmentFile")]
        public async Task<IActionResult> GetProductAttachmentFile(string ProductNumber, string ProductName, string AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(ProductNumber) && !string.IsNullOrEmpty(ProductName))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{ProductNumber}\\{AttachmentFile}";
                    if (System.IO.File.Exists(WorkPath))
                    {
                        var memoryStream = new MemoryStream();
                        await Task.Run(() =>
                        {
                            FileStream stream = new FileStream(WorkPath, FileMode.Open);
                            stream.CopyTo(memoryStream);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            stream.Close();
                        });
                        return new FileStreamResult(memoryStream, $"application/{FileExtension}") { FileDownloadName = AttachmentFile };
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest($"{ProductName}檔案下載失敗");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{ProductName}檔案下載失敗");
            }
        }
    }
}
