using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }


        [HttpGet]
        [Route("top-grossing")] // 定义路由规则，url最后的那部分
        public async Task<IActionResult> Get30HighestGrossingMovies()
        {
            var movies = await _movieService.Get30HighestGrossingMovies();
            if (!movies.Any())
            {
                return NotFound(new { errorMessage = "Mo Movies Found" });
            }
            else
            {
                return Ok(movies);
            }
        }
        [HttpGet]
        [Route("{id:int}")] // url: api/movies/1
        public async Task<IActionResult> GetMovieDetails(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);
            if (movie == null)
            {
                return NotFound(new { errorMessage = "Mo Movies Found" });
            }
            else
            {
                return Ok(movie);
            }
        }
    }
}
