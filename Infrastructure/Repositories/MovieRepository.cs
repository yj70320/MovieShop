using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
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

        public IEnumerable<Movie> Get30HighestGrossingMovie()
        {
            // select top 30 * from Movie order by Revenue desc
            var movies = _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToList();
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
        public override Movie GetById(int id)
        {
            //_dbContext.Movies.FirstOrDefault(m => m.Id == id);
            // 连接表格加到 movie 中：Trailers, cast
            // Include: 加载直接关联的导航属性
            // ThenInclude: 加载 间接关联（嵌套层级）的导航属性
            var movie = _dbContext.Movies
                .Include(m => m.GenresOfMovie).ThenInclude(mg => mg.Genre)
                .Include(m => m.Trailers)
                .Include(m => m.CastsOfMovie).ThenInclude(mc => mc.Cast)
                .FirstOrDefault(m => m.Id == id);
            // 加入 average rating
            var avgRating = _dbContext.Reviews.Where(r => r.MovieId == id)
                .Select(r => r.Rating).AsEnumerable().DefaultIfEmpty(0).Average();
            movie.Rating = avgRating;
            return movie;
        }
    }
}
