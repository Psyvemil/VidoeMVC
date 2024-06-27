using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using vidoeMVC.DAL;
using vidoeMVC.Models;
using vidoeMVC.ViewModels;
using vidoeMVC.ViewModels.Users;

namespace vidoeMVC.Controllers
{
    public class MyChannelController(UserManager<AppUser> _userManager,VidoeDBContext _context) : Controller
    {
        public async Task<ActionResult> Index() 
        {
            var user = await _userManager.GetUserAsync(User);
            var videos = await _context.Videos.Where(u=>u.AuthorId==user.Id)
                .Include(v => v.Author).ThenInclude(a=>a.Followers)
                
            .ToListAsync();
           

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
                Videos = videos
            };
            return View(homeVM);
        }
    }
}
