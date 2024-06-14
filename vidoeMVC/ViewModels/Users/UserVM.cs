using vidoeMVC.Models;

namespace vidoeMVC.ViewModels.Users
{
    public class UserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
        public ICollection<UserFollow> Followees { get; set; } = new List<UserFollow>();


    }
}
