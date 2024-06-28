using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using vidoeMVC.DAL;
using vidoeMVC.Models;
using vidoeMVC.ViewModels;
using vidoeMVC.ViewModels.Categories;
using vidoeMVC.ViewModels.Users;


namespace vidoeMVC.Controllers
{
    public class HomeController(UserManager<AppUser> _userManager,VidoeDBContext _context) : Controller
    {
        

      
        public async Task<IActionResult> Index()
        {
           

            var categories = await _context.Categories.ToListAsync();
            var videos = await _context.Videos.Include(v=>v.Author).ToListAsync();
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

            var sa= await _context.Videos.Where(s=>s.Author.UserName==User.Identity.Name).ToListAsync();
            var homeVM = new HomeVM
            {
                categories = categories.Select(c => new GetCategoryVM
                {
                    Name = c.Name,
                    Id = c.Id
                }).ToList(),
                users = appUsers,
                Videos = videos,
               
            };

            return View(homeVM);
        }

    }

}

           