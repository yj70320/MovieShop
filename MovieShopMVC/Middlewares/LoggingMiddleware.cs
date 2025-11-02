using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MovieShopMVC.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MovieShopExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public LoggingMiddleware(RequestDelegate next, ILogger<MovieShopExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // 1. 组装异常信息
                var exceptionDetails = new
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    ExceptionDate = DateTime.UtcNow,
                    ExceptionType = ex.GetType().FullName,
                    Path = httpContext.Request.Path.Value,
                    HttpMethod = httpContext.Request.Method,
                    User = httpContext.User.Identity?.IsAuthenticated == true ? httpContext.User.Identity!.Name : null
                };

                // 2. 写文件到 Logs/（按天滚动）
                var logsDir = Path.Combine(AppContext.BaseDirectory, "Logs");
                Directory.CreateDirectory(logsDir);
                var filePath = Path.Combine(logsDir, $"errors-{DateTime.UtcNow:yyyyMMdd}.log");
                var text =
                    $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {exceptionDetails.HttpMethod} {exceptionDetails.Path}\n" +
                    $"{exceptionDetails.ExceptionType}: {exceptionDetails.Message}\n" +
                    $"{exceptionDetails.StackTrace}\n\n";
                await File.AppendAllTextAsync(filePath, text);

                // 3. 用 ILogger 再记一份（如果接了 Serilog 会落到文件）
                _logger.LogError(ex, "Unhandled exception at {Method} {Path}",
                    exceptionDetails.HttpMethod, exceptionDetails.Path);

                // 4. 开发环境：交给开发者异常页（红页）
                if (_env.IsDevelopment())
                    throw;

                // 5. 生产环境：若响应尚未开始，清空并重定向到自定义错误页
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.Clear();
                    // 先给 500，再做 302 重定向（浏览器最终会去 /Home/Error）
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    httpContext.Response.Redirect("/Home/Error");
                    return; // 终止中间件链
                }

                // 6. 响应已开始，不能再改，只能继续抛出
                throw;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
