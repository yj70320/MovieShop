using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Contracts.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDbContext<MovieShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieShopDbConnection"));
    //b => b.MigrationsAssembly("Infrastructure")); // 迁移生成到 Infrastructure（DbContext 所在项目）
});

// 告诉框架：所有带有 Bearer <token> 的请求该如何被验证、签名怎么验证、是否检查发行方和受众
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,        // 要验证签名密钥是否合法。否则任何 Token 都会被接受
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["privateKey"])),  // 设置验证签名用的密钥,必须与生成 Token 时的密钥一致
            ValidateIssuer = false,                 // 不验证 Token 的发行方（Issuer）
            ValidateAudience = false                // 不验证 Token 的接收方（Audience）
        };
    }
);

var app = builder.Build();
Console.WriteLine($"ENV={app.Environment.EnvironmentName}");

//// 探针，确认跑的就是 MVC 的 Program.cs
//app.MapGet("/env", () => Results.Text($"ENV={app.Environment.EnvironmentName}"));
//app.MapGet("/ping", () => Results.Text("MovieShopMVC Program.cs"));

//app.Use(async (ctx, next) =>
//{
//    try { await next(); }
//    catch
//    {
//        ctx.Response.Clear();
//        ctx.Response.Redirect("/Home/Error");
//    }
//});
// Configure the HTTP request pipeline.F
if (app.Environment.IsDevelopment())
{
    // 在 生产环境（Production） 或 测试环境（Staging） 才会启用 Swagger
    // 在 开发环境（Development） 不启用
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
