using Microsoft.AspNetCore.Identity;

namespace vidoeMVC.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
