using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? HashedPassword { get; set; }
        public string? Salt { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDate { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
        public bool? IsLocked { get; set; }
        public int? AccessFailedCount { get; set; }

        // 导航属性 navigation property
        public ICollection<Review> ReviewsOfUser { get; set; }
        public ICollection<Favorites> FavoritesOfUser { get; set; }
        public ICollection<Purchase> PurchasesOfUser { get; set;}
        public ICollection<Role> RolesOfUser { get; set; }
    }
}
