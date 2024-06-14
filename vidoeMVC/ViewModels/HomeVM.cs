using vidoeMVC.Models;
using vidoeMVC.ViewModels.Categories;
using vidoeMVC.ViewModels.Users;

namespace vidoeMVC.ViewModels
{
    public class HomeVM
    {
        public List<UserVM> users {  get; set; }

        public List<GetCategoryVM> categories { get; set; }

    }
}
