using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace MovieShopMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;   
        }
        [HttpGet]
        public async Task<IActionResult> Login() { 
            // return any login page
            return View(); 
        }

        [HttpPost] // 处理表单数据，验证用户名和密码
        public async Task<IActionResult> Login(LoginModel model) {

            var user = await _accountService.ValidateUser(model.Email, model.Password);

            // Claim 是一条关于用户的“声明信息”，例如用户名、邮箱、角色、权限等。
            // 它被用在 认证 Authentication 和 授权 Authorization 里，用来描述用户是谁、能做什么。
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                //new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToShortDateString())
                // new Claim("Language", "English")  // DIY Claim
            };

            // 在用户登录后，把用户信息保存到服务器的 Cookie 中，从而实现“登录状态”的保持
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity));

            // 登录后跳转回主页
            return LocalRedirect("~/"); 
        }

        [HttpGet]
        public async Task<IActionResult> Register() 
        { 
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model) 
        {
            // 前端验证可以被绕过，需要在后端 (RegisterModel.cs, YearValidationAttrbutes.cs) 同样进行验证
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await _accountService.RegisterUser(model);
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            // 登出后跳转回主页
            return LocalRedirect("~/");
        }
    }
}
