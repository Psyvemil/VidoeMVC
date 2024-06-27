using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.Models;
using vidoeMVC.ViewModels.Categories;
using vidoeMVC.ViewModels.Users;
using vidoeMVC.ViewModels;
using vidoeMVC.DAL;

namespace vidoeMVC.Controllers
{
    public class VideoPageController(UserManager<AppUser> _userManager,VidoeDBContext _context) : Controller
    {
        public async Task< IActionResult> Index(int? id)
        {

            var video = await _context.Videos
                .Include(c=>c.Author)
                .Include(t=>t.Tags).ThenInclude(t=>t.Tag)
                .Include(c=>c.VCategories).ThenInclude(c=>c.Category)
                .Include(v => v.Casts).ThenInclude(vc => vc.User)
                .Include(a=>a.Author).ThenInclude(a=>a.Followers)
                .FirstOrDefaultAsync(v => v.Id == id);
            var videos = await _context.Videos.ToListAsync();
            if (video == null)
            {
                return NotFound(); 
            }



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
               
                users = appUsers,
                Video = video,
                Videos = videos,
            };

            return View(homeVM);
        }
    }
}
