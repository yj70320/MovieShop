using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    [Table("Genre")]
    public class Genre
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        //public ICollection<Movie> Movies { get; set; }  // 导航属性，，表示一个 Genre 可以对应多个 Movie
        public ICollection<MovieGenre> MoviesOfGenre { get; set; }
    }
}
