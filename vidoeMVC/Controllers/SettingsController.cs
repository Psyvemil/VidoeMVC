using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.DAL;
using vidoeMVC.Models;
using vidoeMVC.Services;
using vidoeMVC.ViewModels;
using vidoeMVC.ViewModels.Users;

namespace vidoeMVC.Controllers
{
    public class SettingsController(UserManager<AppUser> _userManager, CloudinaryService _cloudinaryService, VidoeDBContext _context) : Controller
    {


        [HttpGet]
        public async Task<IActionResult> UserSetting()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userVM = await _context.Users
                .Where(u => u.Id == user.Id)
                .Select(u => new UserVM
                {
                    UserName = u.UserName,
                    Id = u.Id,
                    BirthDate = u.BirthDate,
                    Surname = u.Surname,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    ProfPhotURL = u.ProfilPhotoURL ?? string.Empty,
                    Instlink = u.Instlink,
                    Xlink = u.Xlink,
                    FBlink = u.FBlink,
                    AboutMe = u.AboutMe,
                    Followers = u.Followers ?? new List<UserFollow>(),
                    Followees = u.Followees ?? new List<UserFollow>()
                })
                .FirstOrDefaultAsync();

            if (userVM == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var appUsers = await _userManager.Users.Where(id => id.Id == user.Id)
              .Include(u => u.Followers)
              .ThenInclude(f => f.Follower)
              .Include(u => u.Followees)
              .ThenInclude(f => f.Followee)
              .Select(u => new UserVM
              {
                  UserName = u.UserName,
                  Id = u.Id,
                  BirthDate = u.BirthDate,
                  Surname = u.Surname,
                  Name = u.Name,
                  Email = u.Email,



                  Followers = u.Followers ?? new List<UserFollow>(),
                  Followees = u.Followees ?? new List<UserFollow>()
              }).ToListAsync();

            var userEditVM = new UserEditVM
            {
                Name = userVM.Name,
                Surname = userVM.Surname,
                PhoneNumber = userVM.PhoneNumber,
             
                Instlink = userVM.Instlink,
                Xlink = userVM.Xlink,
                FBlink = userVM.FBlink,
                AboutMe = userVM.AboutMe
            };

            var homeVM = new HomeVM
            {
                users = appUsers,
                UserL = userVM,
                UserEditVM = userEditVM,
            };

            return View(homeVM);
        }

        [HttpPost]
        public async Task<IActionResult> UserSetting(HomeVM uData)
        {
            if (uData.UserEditVM!=null)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (uData?.UserEditVM.ProfPhot != null)
                {
                    string photoURL;
                    using (var photoStream = uData.UserEditVM.ProfPhot.OpenReadStream())
                    {
                        var photoUploadResult = await _cloudinaryService.UploadPhotoAsync(photoStream, uData.UserEditVM.ProfPhot.FileName);
                        photoURL = photoUploadResult.Url.ToString();
                    }

                    user.ProfilPhotoURL = photoURL;
                }

                user.Name = uData.UserEditVM.Name;
                user.Surname = uData.UserEditVM.Surname;
                user.PhoneNumber = uData.UserEditVM.PhoneNumber;
                
                user.Instlink = uData.UserEditVM.Instlink;
                user.FBlink = uData.UserEditVM.FBlink;
                user.Xlink = uData.UserEditVM.Xlink;
                user.AboutMe = uData.UserEditVM.AboutMe;

                var result = await _userManager.UpdateAsync(user);
                
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index),"Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            

            

            return BadRequest();
        }
    }
}
