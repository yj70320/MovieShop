using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        // 保存注入的仓储对象
        private readonly IMovieRepository _movieRepository;
        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public List<MovieCard> Get30HighestGrossingMovies()
        {
            //var movies = new List<MovieCard>
            //{
            //    new MovieCard { Title = "Inception", Id = 1, PosterUrl = "https://image.tmdb.org/t/p/w342/9gk7adHYeDvHkCSEqAvQlN1V9Su.jpg" },
            //    new MovieCard { Title = "Interstellar", Id = 2, PosterUrl = "https://image.tmdb.org/t/p/w342/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg" },
            //    new MovieCard { Title = "The Dark Knight", Id = 3, PosterUrl = "https://image.tmdb.org/t/p/w342/qJ2tW6WMUDux911r6m7haRef0WH.jpg" },
            //    new MovieCard { Title = "Deadpool", Id = 4, PosterUrl = "https://image.tmdb.org/t/p/w342/yGSxMiF0cYuAiyuve5DA6bnWbkm.jpg" },
            //    new MovieCard { Title = "The Avengers", Id = 5, PosterUrl = "https://image.tmdb.org/t/p/w342/RvYMy2wcKCBAz24UyPD7xwmjaTn.jpg" }
            //};
            var movies = _movieRepository.Get30HighestGrossingMovie();
            var movieCards = new List<MovieCard>();
            foreach (var movie in movies) { 
                movieCards.Add(new MovieCard { Id = movie.Id , Title = movie.Title, PosterUrl = movie.PosterUrl});
            };
            return movieCards;
        }

        public MovieDetailModel GetMovieDetails(int id)
        {
            var movie = _movieRepository.GetById(id);
            var movieDetails = new MovieDetailModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Overview = movie.Overview,
                Tagline = movie.Tagline,
                Budget = movie.Budget,
                Revenue = movie.Revenue,
                ImdbUrl = movie.ImdbUrl,
                TmdbUrl = movie.TmdbUrl,
                PosterUrl = movie.PosterUrl,
                BackdropUrl = movie.BackdropUrl,
                OriginalLanguage = movie.OriginalLanguage,
                ReleaseDate = movie.ReleaseDate,
                Runtime = movie.Runtime,
                Price = movie.Price,
                Rating = movie.Rating,
            };
            movieDetails.Trailers = new List<TrailerModel>();
            foreach (var trailer in movie.Trailers)
            {
                movieDetails.Trailers.Add(new TrailerModel { Id = trailer.Id, Name = trailer.Name, TrailerUrl = trailer.TrailerUrl });
            }
            movieDetails.Genres = new List<GenreModel>();
            foreach (var genre in movie.GenresOfMovie)
            {
                movieDetails.Genres.Add(new GenreModel { Id = genre.GenreId, Name = genre.Genre.Name });
            }
            return movieDetails;
        }
    }
}
