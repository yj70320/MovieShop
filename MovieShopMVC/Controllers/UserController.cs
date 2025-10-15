using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MovieShopMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> Purchase()
        {
            // 读取和使用 Cookie
            // 最差的方法：直接读取 Cookie，读取的是加密信息，还需要解密再进行对比认证
            //var user = this.HttpContext.Request.Cookies["MovieShopAuthCookie"];

            // 次优的方法：自动确认是否认证成功
            //var isLoggedIn = this.HttpContext.User.Identity.IsAuthenticated;
            //if (!isLoggedIn)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            //else
            //{
            //    var userId = Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //}
            // Get purchased movies by userId and pass to view

            // 最好的方法：方法前面加上 [Authorize]，向框架声明这里需要额外的规则，即登录、授权
            // 加在类的前面，则是表示类中所有的方法都需要 用户登录认证
            var userId = Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return View();
        }
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> Favorites()
        {
            var userId = Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            var userId = Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View();
        }
    }
}
