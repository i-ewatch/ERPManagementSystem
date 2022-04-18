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
    public class ProjectController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string WorkPath { get; set; }
        private string SqlDB { get; set; }
        public ProjectController(IConfiguration configuration)
        {
            _configuration = configuration;
            WorkPath = "專案";
            SqlDB = _configuration["SqlDB"];
        }
        /// <summary>
        /// 查詢全部【專案】父子資料
        /// </summary>
        /// <returns></returns>
        // GET: api/<ProjectController>
        [HttpGet]
        public async Task<List<ProjectSetting>> GetProject()
        {
            List<ProjectSetting> result = new List<ProjectSetting>();
            List<ProjectEmployeeSetting> Sub = new List<ProjectEmployeeSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProjectSetting order by ProjectNumber ";
                        result = connection.Query<ProjectSetting>(sql).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProjectEmployeeSetting order by ProjectNumber , EmployeeNumber ";
                        Sub = connection.Query<ProjectEmployeeSetting>(sql).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].ProjectEmployee = (from S in Sub
                                                         where S.ProjectNumber == result[i].ProjectNumber
                                                         select S).ToList();
                        }
                    }
                });
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        /// <summary>
        /// 查詢單筆【專案】父子資料
        /// </summary>
        /// <param name="ProjectNumber"></param>
        /// <returns></returns>
        // GET api/<ProjectController>/5
        [HttpGet("{ProjectNumber}")]
        public async Task<List<ProjectSetting>> GetProject(string ProjectNumber)
        {
            List<ProjectSetting> result = new List<ProjectSetting>();
            List<ProjectEmployeeSetting> Sub = new List<ProjectEmployeeSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProjectSetting " +
                                        $"where ProjectNumber=@ProjectNumber " +
                                        $"order by ProjectNumber ";
                        result = connection.Query<ProjectSetting>(sql, new { ProjectNumber = ProjectNumber }).ToList();
                    }
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProjectEmployeeSetting " +
                                        $"where ProjectNumber=@ProjectNumber " +
                                        $"order by ProjectNumber , EmployeeNumber ";
                        Sub = connection.Query<ProjectEmployeeSetting>(sql, new { ProjectNumber = ProjectNumber }).ToList();
                    }

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].ProjectEmployee = (from S in Sub
                                                         where S.ProjectNumber == result[i].ProjectNumber
                                                         select S).ToList();
                        }
                    }
                });
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        /// <summary>
        /// 專案父子新增
        /// </summary>
        /// <param name="projectSetting">專案父子</param>
        // POST api/<ProjectController>
        [HttpPost]
        public async Task<IActionResult> InserterProject(ProjectSetting projectSetting)
        {
            List<ProjectSetting> result = new List<ProjectSetting>();
            List<ProjectEmployeeSetting> Sub = new List<ProjectEmployeeSetting>();
            try
            {
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"SELECT * FROM  ProjectSetting WHERE ProjectNumber = @ProjectNumber";
                        result = connection.Query<ProjectSetting>(sql, new { ProjectNumber = projectSetting.ProjectNumber }).ToList();
                    }
                });
                if (result.Count == 0)
                {
                    int DateIndex = 0;
                    await Task.Run(() =>
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增父
                        {
                            string sql = $"INSERT INTO ProjectSetting( ProjectNumber , ProjectName , Remark , ProjectIncome , ProjectCost , ProjectProfit , ProjectBonusCommission , PostingDate , ProfitSharingDate ) " +
                                            $"Values( @ProjectNumber , @ProjectName , @Remark , @ProjectIncome , @ProjectCost , @ProjectProfit , @ProjectBonusCommission , @PostingDate , @ProfitSharingDate )";
                            connection.Execute(sql, projectSetting);
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                        {
                            for (int i = 0; i < projectSetting.ProjectEmployee.Count(); i++)
                            {
                                projectSetting.ProjectEmployee[i].ProjectNumber = projectSetting.ProjectNumber;
                            }
                            string sql = $"INSERT INTO ProjectEmployeeSetting( ProjectNumber , EmployeeNumber , BonusRatio ) " +
                                            $"Values( @ProjectNumber , @EmployeeNumber , @BonusRatio)";
                            connection.Execute(sql, projectSetting.ProjectEmployee);
                        }
                    });

                    return Ok($"{projectSetting.ProjectName}資訊，上傳成功!");
                }
                else
                {
                    return BadRequest($"{projectSetting.ProjectName}資訊，編碼已存在");
                }
            }
            catch (Exception)
            {

                return BadRequest($"{projectSetting.ProjectName}資訊，上傳失敗");
            }
        }

        // PUT api/<ProjectController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateProject(ProjectSetting projectSetting)
        {
            try
            {
                int DateMainIndex = 0;
                int DateSubIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string sql = $"UPDATE ProjectSetting SET " +
                                    $"ProjectName = @ProjectName , Remark = @Remark , FileName=@FileName , ProjectIncome = @ProjectIncome , ProjectCost= @ProjectCost , " +
                                    $"ProjectProfit = @ProjectProfit , ProjectBonusCommission = @ProjectBonusCommission , PostingDate  = @PostingDate , ProfitSharingDate = @ProfitSharingDate " +
                                    "Where ProjectNumber=@ProjectNumber ";
                        DateMainIndex = connection.Execute(sql, projectSetting);
                    }
                });
                if (DateMainIndex > 0)
                {
                    await Task.Run(() =>
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string DeleteSql = "delete ProjectEmployeeSetting Where ProjectNumber=@ProjectNumber ";
                            connection.Execute(DeleteSql, new { ProjectNumber = projectSetting.ProjectNumber });
                        }
                        using (IDbConnection connection = new SqlConnection(SqlDB))     // 新增子
                        {
                            for (int i = 0; i < projectSetting.ProjectEmployee.Count(); i++)
                            {
                                projectSetting.ProjectEmployee[i].ProjectNumber = projectSetting.ProjectNumber;
                            }
                            string sql = $"INSERT INTO ProjectEmployeeSetting( ProjectNumber , EmployeeNumber , BonusRatio ) " +
                                            $"Values( @ProjectNumber , @EmployeeNumber , @BonusRatio)";
                            connection.Execute(sql, projectSetting.ProjectEmployee);
                        }
                    });
                }
                return Ok($"{projectSetting.ProjectName}資訊，更新成功!");
            }
            catch (Exception)
            {
                return BadRequest($"{projectSetting.ProjectName}資訊，更新失敗");
            }
        }

        //// DELETE api/<ProjectController>/5
        [HttpDelete("{ProjectNumber}")]
        public async Task<IActionResult> DeleteProject(string ProjectNumber)
        {
            try
            {
                int DateMainIndex = 0;
                await Task.Run(() =>
                {
                    using (IDbConnection connection = new SqlConnection(SqlDB))
                    {
                        string DeleteSql = "delete ProjectSetting Where ProjectNumber=@ProjectNumber";
                        DateMainIndex = connection.Execute(DeleteSql, new { ProjectNumber = ProjectNumber });
                    }
                });
                if (DateMainIndex >0)
                {
                    await Task.Run(() =>
                    {
                        using (IDbConnection connection = new SqlConnection(SqlDB))
                        {
                            string DeleteSql = "delete ProjectEmployeeSetting Where ProjectNumber=@ProjectNumber ";
                            DateMainIndex = connection.Execute(DeleteSql, new { ProjectNumber = ProjectNumber});
                        }
                    });
                    return Ok($"{ProjectNumber}資訊，刪除成功!");
                }
                else
                {
                    return BadRequest($"{ProjectNumber}資訊，刪除資料失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{ProjectNumber}資訊，刪除失敗");
            }
        }

        /// <summary>
        /// 專案附件檔案更新
        /// </summary>
        /// <param name="ProjectNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/ProjectAttachmentFile")]
        public async Task<IActionResult> PostProjectAttachmentFile(string ProjectNumber, IFormFile AttachmentFile)
        {
            try
            {
                if (AttachmentFile != null && !string.IsNullOrEmpty(ProjectNumber))
                {
                    await Task.Run(() =>
                    {
                        WorkPath += $"\\{ProjectNumber}";
                        if (!Directory.Exists(WorkPath))
                        {
                            Directory.CreateDirectory($"{WorkPath}");
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
                            string sql = $"UPDATE ProjectSetting SET FileName = @FileName  WHERE ProjectNumber = @ProjectNumber";
                            connection.Execute(sql, new { FileName = AttachmentFile.FileName, ProjectNumber = ProjectNumber });
                        }
                    });
                    return Ok($"{ProjectNumber}檔案上傳成功");
                }
                else
                {
                    return BadRequest($"{ProjectNumber}檔案上傳失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{ProjectNumber}檔案上傳失敗");
            }
        }
        /// <summary>
        /// 專案附件檔案下載
        /// </summary>
        /// <param name="ProjectNumber"></param>
        /// <param name="AttachmentFile"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/ProjectAttachmentFile")]
        public async Task<IActionResult> GetProjectAttachmentFile(string ProjectNumber, string AttachmentFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(AttachmentFile) && !string.IsNullOrEmpty(ProjectNumber))
                {
                    string FileExtension = AttachmentFile.Split('.')[1];
                    WorkPath += $"\\{ProjectNumber}\\{AttachmentFile}";
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
                    return BadRequest($"{ProjectNumber}檔案下載失敗");
                }
            }
            catch (Exception)
            {
                return BadRequest($"{ProjectNumber}檔案下載失敗");
            }
        }
    }
}
