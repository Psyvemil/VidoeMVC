using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.Models;
using vidoeMVC.ViewModels.Users;
using vidoeMVC.ViewModels;
using vidoeMVC.DAL;
using vidoeMVC.Enums;

namespace vidoeMVC.Controllers
{
    public class ChannelController(UserManager<AppUser> _userManager, VidoeDBContext _context) : Controller
    {
        public async Task<ActionResult> Index(string uid)
        {
            var videos = await _context.Videos.Where(u => u.AuthorId == uid && u.Privacy.Contains(VideoStatus.Public))
                .Include(v => v.Author).ThenInclude(v => v.Followers)
                .ToListAsync();

            var appUsers = await _userManager.Users
              .Include(u => u.Followers).ThenInclude(f => f.Follower)
              .Include(u => u.Followees).ThenInclude(f => f.Followee)
              .Select(u => new UserVM
              {
                  ProfPhotURL = u.ProfilPhotoURL,
                  UserName = u.UserName,
                  Id = u.Id,
                  Followers = u.Followers ?? new List<UserFollow>(),
                  Followees = u.Followees ?? new List<UserFollow>()
              }).ToListAsync();

            var userL = await _userManager.Users
                .Where(u => u.Id == uid)
                .Select(u => new UserVM
                {
                    UserName = u.UserName,
                    Id = u.Id,
                    BirthDate = u.BirthDate,
                    Surname = u.Surname,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    ProfPhotURL = u.ProfilPhotoURL,
                    FBlink = u.FBlink,
                    Xlink = u.Xlink,
                    Instlink = u.Instlink,
                    Followers=u.Followers 
                    
                })
                .FirstOrDefaultAsync();

            var homeVM = new HomeVM
            {
                UserL = userL,
                users = appUsers,
                Videos = videos
            };

            return View(homeVM);
        }
    }
}
