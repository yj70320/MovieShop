using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        // async: 告诉编译器这个方法用异步方式执行，这个方法很耗时间，先去做别的事情
        // 异步方法，返回值用 Task<> 包围。e.g. 返回 void => Task, 返回 int => Task<int>
        public async Task<IActionResult> Details(int id) // http://movieshop.com/movies/details
        {
            // await 只能等待一个对象，一个任务对象，后面跟的方法必须返回一个 Task
            //var movieDetails = _movieService.GetMovieDetails(id);
            var movieDetails = await _movieService.GetMovieDetails(id);
            return View(movieDetails);
        }
    }
}
