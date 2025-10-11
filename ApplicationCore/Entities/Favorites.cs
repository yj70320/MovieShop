using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    [Table("Favorites")]
    public class Favorites
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
    }
}
