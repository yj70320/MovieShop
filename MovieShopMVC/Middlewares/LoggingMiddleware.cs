using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieShopMVC.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, IWebHostEnvironment env)
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
                // 异常信息
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
                    // UtcNow：格林尼治时间，本地时间可以用 Now
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {exceptionDetails.HttpMethod} {exceptionDetails.Path}\n" +
                    $"{exceptionDetails.ExceptionType}: {exceptionDetails.Message}\n" +
                    $"{exceptionDetails.StackTrace}\n\n";
                await File.AppendAllTextAsync(filePath, text);

                // 3. 用 ILogger 和 Serilog 记录错误
                _logger.LogError(ex, "Unhandled exception at {Method} {Path}",
                    exceptionDetails.HttpMethod, exceptionDetails.Path);

                //// 4. 开发环境：不要动响应，直接把异常抛回去，让开发红页接管
                //if (_env.IsDevelopment()) throw;

                //// 5. 生产环境：只有在响应还没开始时，才清空并重定向到自定义错误页
                //if (!httpContext.Response.HasStarted)
                //{
                //    httpContext.Response.Clear();
                //    // 先给 500，再做 302 重定向（浏览器最终会去 /Home/Error）
                //    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                //    httpContext.Response.Redirect("/Home/Error");
                //    return; // 终止中间件链
                //}
                //else
                //{
                //    // 如果响应已经开始，只能写日志，不能再重定向
                //    _logger.LogWarning("Response already started, skipping redirect.");
                //    return; // 只返回，不再 throw/写任何东西
                //}

                //// 6. 响应已开始，不能再改，只能继续抛出
                //throw;
                // 4. 开发环境：直接抛给开发红页
                if (_env.IsDevelopment())
                {
                    throw;
                }

                // 5. 生产环境：只在响应未开始时写入 JSON；已开始就只记日志返回
                if (httpContext.Response.HasStarted)
                {
                    _logger.LogWarning("Response already started, skipping custom JSON error response.");
                    return;
                }
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json; charset=utf-8";
                
                await httpContext.Response.WriteAsync(
                    JsonSerializer.Serialize(exceptionDetails, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                );
                return;
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
