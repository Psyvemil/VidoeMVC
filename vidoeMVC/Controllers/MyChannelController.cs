    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
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
                var finduser = await _userManager.GetUserAsync(User);
                var videos = await _context.Videos.Where(u=>u.AuthorId==finduser.Id)
                    .Include(v => v.Author).ThenInclude(v=>v.Followers)
                
                .ToListAsync();
            var us = await _context.Users.FirstOrDefaultAsync(id => id.Id == finduser.Id);
            if (us.Followers?.Count() != null)
            ViewBag.userCount=us.Followers?.Count();
            else ViewBag.userCount=0;

            //var luser = await _userManager.Users.Where(i => i.Id == finduser.Id).Include(u => u.Followers).ThenInclude(f => f.Follower);
            
                
            

                var appUsers = await _userManager.Users
                  .Include(u => u.Followers)
                  .ThenInclude(f => f.Follower)
                  .Include(u => u.Followees)
                  .ThenInclude(f => f.Followee)
                  .Select(u => new UserVM
                  {
                      ProfPhotURL = u.ProfilPhotoURL,
                      UserName = u.UserName,
                      Id = u.Id,
                      Followers = u.Followers ?? new List<UserFollow>(),
                      Followees = u.Followees ?? new List<UserFollow>()
                  }).ToListAsync();
            var userL = await _userManager.Users.Select(u => new UserVM
            {
                UserName = u.UserName,
                Id = u.Id,
                BirthDate = u.BirthDate,
                Surname = u.Surname,
                Name = u.Name,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                ProfPhotURL = u.ProfilPhotoURL
            }
             ).FirstOrDefaultAsync(id => id.Id == finduser.Id);
            var homeVM = new HomeVM
            {
                UserL=userL,
                users = appUsers,
                Videos = videos,
                //user = luser,
                };
                return View(homeVM);
            }
        }
    }
