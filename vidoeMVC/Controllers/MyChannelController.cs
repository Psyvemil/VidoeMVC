using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.Models;
using vidoeMVC.ViewModels.Users;

namespace vidoeMVC.Controllers
{
    public class MyChannelController(UserManager<AppUser> _userManager) : Controller
    {
        public async Task<ActionResult> Index() 
        {
            var users = await _userManager.Users.Select(u => new UserVM
            {
                UserName = u.UserName,
                Id = u.Id,
                Followers = u.Followers,
                Followees = u.Followees

            }).ToListAsync();
            return View(users);
        }
    }
}
