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
        

        public async Task< IActionResult> Index()
        {
            List<UserVM> AppUsers = await _userManager.Users.Select(u => new UserVM
            {
                UserName = u.UserName,
                Id = u.Id,
                Followers = u.Followers,
                Followees = u.Followees

            }).ToListAsync();
            List<GetCategoryVM> Categories =await _context.Categories.Select(c=> new GetCategoryVM
            {
                Name = c.Name,
                Id = c.Id,
            }).ToListAsync();
            HomeVM homeVM = new HomeVM
            {
                categories = Categories,
                users = AppUsers
            };

            return View(homeVM);
        }    
        
    }

}

           