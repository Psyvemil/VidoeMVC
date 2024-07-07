using Microsoft.AspNetCore.Mvc.Rendering;

namespace vidoeMVC.ViewModels.Users
{
    public class UserRoleVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();

        public IList<SelectListItem> AvailableRoles { get; set; } = new List<SelectListItem>();
        public string SelectedRole { get; set; }
        public string SelectedRoleId { get; set; }
    }
}
