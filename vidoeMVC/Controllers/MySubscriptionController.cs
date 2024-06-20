using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.Models;
using vidoeMVC.ViewModels.Categories;
using vidoeMVC.ViewModels;
using vidoeMVC.ViewModels.Users;

namespace vidoeMVC.Controllers
{
    public class MySubscriptionController(UserManager<AppUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var appUsers = await _userManager.Users
               .Include(u => u.Followers)
               .ThenInclude(f => f.Follower)
               .Include(u => u.Followees)
               .ThenInclude(f => f.Followee)
               .Select(u => new UserVM
               {
                   UserName = u.UserName,
                   Id = u.Id,
                   Followers = u.Followers ?? new List<UserFollow>(),
                   Followees = u.Followees ?? new List<UserFollow>()
               }).ToListAsync();

            var homeVM = new HomeVM
            {
                
                users = appUsers
            };

            return View(homeVM);
        }
    }
}