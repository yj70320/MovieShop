using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieShopMVC.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MovieShopExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MovieShopExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;


        public MovieShopExceptionMiddleware(RequestDelegate next, 
            ILogger<MovieShopExceptionMiddleware> logger, 
            IWebHostEnvironment env )
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try { 
                await _next(httpContext);
            }
            // exception 统一捕获点
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception at {Path}", httpContext.Request.Path);

                // 开发环境：交给开发红页
                if (_env.IsDevelopment())
                {
                    throw;
                }

                // 已开始响应：不能再写入/重定向
                if (httpContext.Response.HasStarted)
                {
                    _logger.LogWarning("Response already started, skipping custom JSON error response.");
                    return;
                }

                // 定义异常都有什么信息
                var exceptionDetails = new
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    ExceptionDate = DateTime.UtcNow,
                    ExceptionType = ex.GetType(),
                    Path = httpContext.Request.Path,
                    HttpMethod = httpContext.Request.Method,
                    User = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : null
                    // Email, UserId, QueryString, Headers, etc
                };
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json; charset=utf-8";

                await httpContext.Response.WriteAsync(
                    JsonSerializer.Serialize(exceptionDetails, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                );

                return;

            }
            //httpContext.Response.Redirect("/home/error");
            //return;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MovieShopExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseMovieShopExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MovieShopExceptionMiddleware>();
        }
    }
}
