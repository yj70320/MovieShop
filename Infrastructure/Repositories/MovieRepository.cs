using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Movie>> Get30HighestGrossingMovie()
        {
            // select top 30 * from Movie order by Revenue desc
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToListAsync();  // ToList() 同步方法
            return movies;
        }

        public IEnumerable<Movie> Get30HighestRatedMovie()
        {
            //throw new NotImplementedException();
            var movies = _dbContext.Movies.Where(m => m.ReviewsOfMovie.Any())
                .OrderByDescending(m => m.ReviewsOfMovie.Average(r => r.Rating))
                .ThenByDescending(m => m.ReviewsOfMovie.Count()).ThenBy(m => m.Id)
                .Include(m => m.GenresOfMovie).ThenInclude(mg => mg.Genre).Include(m => m.Trailers).Take(30);
            return movies;
        }
        public async override Task<Movie> GetById(int id)
        {
            //_dbContext.Movies.FirstOrDefault(m => m.Id == id);
            // 连接表格加到 movie 中：Trailers, cast
            // Include: 加载直接关联的导航属性
            // ThenInclude: 加载 间接关联（嵌套层级）的导航属性
            var movie = await _dbContext.Movies
                .Include(m => m.GenresOfMovie).ThenInclude(mg => mg.Genre)
                .Include(m => m.CastsOfMovie).ThenInclude(mc => mc.Cast)
                .Include(m => m.Trailers)
                .FirstOrDefaultAsync(m => m.Id == id);    // FirstOrDefault() 是同步方法，加上 Async 变成异步方法，最面前要加 await
            // 加入 average rating
            movie.Rating = await _dbContext.Reviews.Where(r => r.MovieId == id)
                .AverageAsync(r => r.Rating);             // Average() 是同步方法，加上 Async 变成异步方法，最面前要加 await
                                                          //.Select(r => r.Rating).AsEnumerable().DefaultIfEmpty(0).Average();

            return movie;
        }

        public async Task<PagedResultSet<Movie>> GetMoviesByGenres(int genreId, int pageSize = 30, int pageNumber = 1)
        {
            var totalMoviesCountByGenre = await _dbContext.MovieGenres
                .Where(m => m.GenreId == genreId)
                .CountAsync();
            if (totalMoviesCountByGenre == 0) throw new Exception("No Movie found for that Genre.");
            var movies = await _dbContext.MovieGenres
                .Where(m => m.GenreId == genreId)
                .Include(m => m.Movie)
                .OrderBy(m => m.Movie.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(m => m.Movie)  // 转成 Movie 类型
                .ToListAsync();
            PagedResultSet<Movie> pagedMovies = new PagedResultSet<Movie>(movies, pageNumber, pageSize, totalMoviesCountByGenre);
            return pagedMovies;
        }
    }
}
