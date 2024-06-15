using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.Models;
using vidoeMVC.ViewModels.Users;

namespace vidoeMVC.Controllers
{
    public class MySubscriptionController(UserManager<AppUser> _userManager) : Controller
    {
        public async Task< IActionResult> Index()
        {
            var appUsers = await _userManager.Users
                .Include(u => u.Followers)
                .ThenInclude(f => f.Follower)               
                .Select(u => new UserVM
                {
                    UserName = u.UserName,
                    Id = u.Id,
                    Followers = u.Followers ?? new List<UserFollow>()                   
                }).ToListAsync();
            return View(appUsers);
        }
    }
}
