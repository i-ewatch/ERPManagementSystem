using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Filters
{
    public class TokenFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            bool TokenFlag = context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues Token);
            var Ignore = (from a in context.ActionDescriptor.EndpointMetadata where a.GetType() == typeof(AllowAnonymousAttribute) select a).FirstOrDefault();
            if (Ignore == null)
            {
                if (TokenFlag)
                {
                    var tDll = this.GetType().Assembly.GetName();
                    if (Token.ToString() != tDll.Version.ToString())
                    {
                        context.Result = new JsonResult(new ResultJson()
                        {
                            Data = "Error",
                            HttpCode = 401,
                            Error = "版本錯誤!"
                        });
                    }
                }
                else
                {
                    context.Result = new JsonResult(new ResultJson()
                    {
                        Data = "Error",
                        HttpCode = 401,
                        Error = "請登入帳號!"
                    });
                }
            }
        }
    }
}
