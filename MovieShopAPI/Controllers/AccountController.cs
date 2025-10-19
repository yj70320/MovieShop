using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // 1. 验证用户身份，查询数据库，检查邮箱和密码是否匹配，验证成功后返回一个用户对象（user）
            var user = await _accountService.ValidateUser(model.Email, model.Password);

            // JWT 
            // 2. 生成 Claims (用户信息)
            // Claim（声明） 是一组用户身份信息，嵌入在 JWT 里
            // Token 是最终生成的“整张通行证”，而 Claims 是通行证上记录的“身份信息”
            var claims = new List<Claim>
            {
                // 每个 Claim 是一个键值对
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("language", "English"),
            };
            var claimIdentity = new ClaimsIdentity(claims);

            // 3. 使用配置文件的密钥签名生成 JWT
            // 从配置文件读取密钥与参数
            var privateKey = _configuration["privateKey"];
            var expirationTime = _configuration.GetValue<int>("expirationHours");
            var issuer = _configuration["issuer"];
            var audience = _configuration["audience"];

            // 构建签名和加密密钥
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var jwtExpiration = DateTime.UtcNow.AddHours(expirationTime);

            // 设置 Token 的元数据,类似于一份“证书说明”
            var tokenDescription = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Subject = claimIdentity,
                Issuer = issuer,
                Audience = audience,
                Expires = jwtExpiration,
            };

            // 4. 返回 Token 给客户端
            // 生成 JWT Token
            var handler = new JwtSecurityTokenHandler();           // 创建、序列化 JWT 的类
            var jwtToken = handler.CreateToken(tokenDescription);  // 生成一个 Token 对象
            return Ok(new {token = handler.WriteToken(jwtToken)}); // 把 Token 对象转换成字符串，发给客户端
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = await _accountService.RegisterUser(model);
            return Ok(user);
        }
    }
}
