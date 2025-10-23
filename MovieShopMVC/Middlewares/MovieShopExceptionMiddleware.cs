using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MovieShopMVC.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MovieShopExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MovieShopExceptionMiddleware> _logger;

        public MovieShopExceptionMiddleware(RequestDelegate next, ILogger<MovieShopExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try { 
                await _next(httpContext);
            }
            // exception 统一捕获点
            catch (Exception ex)
            {
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

            }
            httpContext.Response.Redirect("/home/error");
            return;
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
