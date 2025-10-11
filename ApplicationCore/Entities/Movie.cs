using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    [Table("Movie")]
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Overview { get; set; }
        public string? Tagline { get; set; }
        public decimal? Budget { get; set; }
        public decimal? Revenue { get; set; }
        public string? ImdbUrl { get; set; }
        public string? TmdbUrl { get; set; }
        public string? PosterUrl { get; set; }
        public string? BackdropUrl { get; set; }
        public string? OriginalLanguage { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? Runtime { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public decimal? Rating { get; set; }

        // 导航属性 navigation property
        public ICollection<Trailer> Trailers { get; set; }
        //public ICollection<Genre> Genres { get; set; }  // 导航属性，表示一个 Movie 可以对应多个 Genre
        public ICollection<MovieGenre> GenresOfMovie { get; set; }
        public ICollection<MovieCast> CastsOfMovie { get; set;}
        public ICollection<MovieCrew> CrewersOfMovie { get; set; }
        public ICollection<Review> ReviewsOfMovie { get; set; }
        public ICollection<Favorites> FavoritesOfMovie { get; set; }
        public ICollection<Purchase> PurchasesOfMovie { get; set; }
    }
}
