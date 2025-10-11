using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class MovieDetailModel
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
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        //public string? UpdatedBy { get; set; }
        //public string? CreatedBy { get; set; }
        public decimal? Rating { get; set; }

        // 一个电影对应多个演员
        public List<CastModel> Casts { get; set; }
        public List<TrailerModel> Trailers { get; set; }
        public List<GenreModel> Genres { get; set; }

    }
}
