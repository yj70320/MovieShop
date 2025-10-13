using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Login() { return View(); }

        [HttpPost] // 处理表单数据，验证用户名和密码
        public async Task<IActionResult> Login(LoginModel model) { return View(); }

        [HttpGet]
        public async Task<IActionResult> Register() { return View(); }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model) {
            var user = await _accountService.RegisterUser(model);
            return RedirectToAction("Login");
        }
    }
}
