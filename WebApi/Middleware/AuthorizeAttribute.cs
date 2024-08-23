using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Models;

namespace WebApi.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext Context)
        {
            var account = Context.HttpContext.Items["User"];
            if (account == null)
            {
                // not logged in
                Context.Result = new JsonResult(new { message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            else
            {
                var LoginAcc = (LoginModel)account;
                LogUser(LoginAcc);
            }
        }

        protected virtual void LogUser(LoginModel Model)
        {            
        }
    }
}
