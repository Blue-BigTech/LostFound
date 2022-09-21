using Common.Abstract.Configurations;
using Common.Abstract.Entities;
using Common.Abstract.Extension;
using Common.Abstract.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Net;

namespace Common.Abstract.Middleware
{
    public class AuthorizationActionFilter : Attribute, IAsyncActionFilter
    {
        private Jwt _jwt { get; }
        public AuthorizationActionFilter(IOptions<Jwt> jwt)
        {
            _jwt = jwt.Value;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                context.HttpContext.Response.ContentType = "application/json";
                Response response = context.HttpContext.TokenFetch().ValidateToken(_jwt);
                if (response.IsValidated)
                {
                    if (context.HttpContext.ValidatePermission())
                        await next();
                    else
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return;
                    }
                }
                else
                {
                    //context.HttpContext.Response.StatusCode = (int)response.HttpCode;
                }
            }
            catch (Exception)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Redirect; //Unauthorized 
                return;
            }

            return;
        }
    }
}
