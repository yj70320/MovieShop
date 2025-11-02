using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Models;
using System.Diagnostics;

namespace MovieShopMVC.Controllers
{
    public class HomeController : Controller
    {
        // private readonly：只能在 声明变量 和 构造函数 中赋值，之后不能被修改（保证依赖注入后不可变）
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieService _movieService;
        private readonly int x = 1;

        public HomeController(ILogger<HomeController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
            x = 2;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // development / production 环境错误测试
            //int x = 1;
            //int y = 0;
            //int z = x / y;
            //throw new Exception("Test error for logging middleware");


            //ViewBag.Title = "MovieShop Home Page Title";
            //ViewBag.description = new List<string>() {"abc", "def"};
            //ViewData["Title"] = "MovieShop Home Page Title";

            //var movies = new List<MovieCard>
            //    {
            //        new MovieCard { Title = "Inception", Id = 1, PosterUrl = "https://image.tmdb.org/t/p/w342/9gk7adHYeDvHkCSEqAvQlN1V9Su.jpg" },
            //        new MovieCard { Title = "Interstellar", Id = 2, PosterUrl = "https://image.tmdb.org/t/p/w342/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg" },
            //        new MovieCard { Title = "The Dark Knight", Id = 3, PosterUrl = "https://image.tmdb.org/t/p/w342/qJ2tW6WMUDux911r6m7haRef0WH.jpg" },
            //        new MovieCard { Title = "Deadpool", Id = 4, PosterUrl = "https://image.tmdb.org/t/p/w342/yGSxMiF0cYuAiyuve5DA6bnWbkm.jpg" },
            //        new MovieCard { Title = "The Avengers", Id = 5, PosterUrl = "https://image.tmdb.org/t/p/w342/RvYMy2wcKCBAz24UyPD7xwmjaTn.jpg" }
            //    };
            //var movieService = new MovieService();
            var movies = await _movieService.Get30HighestGrossingMovies();
            return View(movies);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // ?. 表示“如果 Activity.Current 不为 null，就取它的 Id”，否则返回 null
            // ?? HttpContext.TraceIdentifier：如果上面的值是 null，就用当前 HTTP 请求的唯一标识符（TraceIdentifier）代替。
            //                                保证了无论是否启用了诊断系统，都能有一个唯一的请求编号。
            // Activity.Current?.Id ?? HttpContext.TraceIdentifier：有 tracing 就用 tracing ID，没 tracing 就用请求 ID。
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
