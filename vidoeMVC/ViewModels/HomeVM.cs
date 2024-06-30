using vidoeMVC.Models;
using vidoeMVC.ViewModels.Categories;
using vidoeMVC.ViewModels.Users;
using vidoeMVC.ViewModels.Videos;

namespace vidoeMVC.ViewModels
{
    public class HomeVM
    {
        public List<UserVM> users {  get; set; }
        public UserVM UserL { get; set; }
        public List<GetCategoryVM> categories { get; set; }

        public  Video Video { get; set; } = new Video();
        public List<Video> Videos { get; set; } = new List<Video>();

        public List<VideoViewModel> VideoViewModels { get; set; }
        public UserEditVM UserEditVM { get; set; }
        public PaginationVM paginationVM { get; internal set; }
    }
}
