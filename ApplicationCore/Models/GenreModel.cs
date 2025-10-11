using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class GenreModel
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
