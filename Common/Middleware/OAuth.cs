using Common.Abstract.Configurations;
using Common.Abstract.Entities;
using Common.Abstract.Extension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Common.Abstract.Middleware
{
    public class OAuth
    {
        private readonly RequestDelegate _next;
        public AuthCredential _authCredential { get; }
        private IWebHostEnvironment _hostingEnvironment { get; }
        private readonly ILogger<OAuth> _logger;
        public OAuth(RequestDelegate next, IOptions<AuthCredential> authCredential, IWebHostEnvironment hostingEnvironment, ILogger<OAuth> logger)
        {
            _next = next;
            _authCredential = authCredential.Value;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["auth"];
            try
            {
                if (!string.IsNullOrEmpty(authHeader))
                {
                    context.Response.ContentType = "application/json";
                    authHeader = authHeader.Decode();
                    if (!string.IsNullOrEmpty(authHeader))
                    {
                        //if ((authHeader.Substring(0, authHeader.IndexOf(':'))) == _authCredential.ClientId &&
                           // (authHeader.Substring(authHeader.IndexOf(':') + 1)) == _authCredential.ClientSecret)
                            await _next.Invoke(context);
                        //else
                           // await context.Response.WriteAsync(Methods.InvalidRequest());
                    }
                    else
                        await context.Response.WriteAsync(Methods.InvalidRequest());
                }
                else if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("/swagger"))
                    await _next.Invoke(context);
                else
                    await context.Response.Body.WriteAsync(File.ReadAllBytes(_hostingEnvironment.WebRootPath + context.Request.Path.Value));
                //await context.Response.WriteAsync(Static.InvalidRequest());

            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
                //_logger.LogError(exception, exception.Message);
                //if (_hostingEnvironment.EnvironmentName == EnvironmentSetting.Development.ToString())
                //{

                //    while (exception.InnerException != null) exception = exception.InnerException;
                //   // await context.Response.WriteAsync("auth1:" + authHeader + ":" + "from-oath-catch:" + exception.Message + " on line# " + exception.ToString());
                //}
                //else
                //{
                //    //_logger.LogError(exception, exception.Message);
                //    await context.Response.WriteAsync(Methods.InvalidRequest());
                //}

            }
            return;
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new Response();            
            switch (exception)
            {
                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid token"))
                    {
                        errorResponse.StatusCode = response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.Message = ex.Message;
                        break;
                    }
                    errorResponse.StatusCode = response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case KeyNotFoundException ex:
                    errorResponse.StatusCode =  response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = ex.Message;
                    break;
                default:
                    errorResponse.StatusCode = response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Internal Server errors. Check Logs!";
                    break;
            }
            _logger.LogError(exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
