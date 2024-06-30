using Microsoft.Identity.Client;

namespace vidoeMVC.ViewModels.Users
{
    public class UserEditVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FBlink { get; set; } = "www.facebook.com";
        public string Xlink { get; set; } = "x.com";
        public string Instlink { get; set; } = "www.instagram.com";
        public string AboutMe { get; set; } 
        public string Password { get; set; }

        public string NewPassword { get; set; }

        public string NewPassConfirm { get; set; }

        public IFormFile ProfPhot { get; set; }
    }
}
