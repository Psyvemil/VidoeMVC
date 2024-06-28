using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.Models;
using vidoeMVC.Services;
using vidoeMVC.ViewModels;
using vidoeMVC.ViewModels.Users;

namespace vidoeMVC.Controllers
{
    public class SettingsController(UserManager<AppUser> _userManager, CloudinaryService _cloudinaryService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> UserSetting()
        {
            var user = await _userManager.GetUserAsync(User);
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
                  PhoneNumber = u.PhoneNumber,


                  Followers = u.Followers ?? new List<UserFollow>(),
                  Followees = u.Followees ?? new List<UserFollow>()
              }).ToListAsync();

            var usert = await _userManager.Users.Select(u => new UserVM
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
             ).FirstOrDefaultAsync(id => id.Id == user.Id);
            UserEditVM userEditVM = new UserEditVM();
            HomeVM homeVM = new HomeVM
            {
                users = appUsers,
                UserL = usert,
                UserEditVM = userEditVM,

            };
            return View(homeVM);
        }
        [HttpPost]
        public async Task<IActionResult> UserSetting(UserVM userVM ,UserEditVM Phorto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (Phorto?.ProfPhot != null)
                {
                    string photoURL;
                    using (var photoStream = Phorto.ProfPhot.OpenReadStream())
                    {
                        var photoUploadResult = await _cloudinaryService.UploadPhotoAsync(photoStream, Phorto.ProfPhot.FileName);
                        photoURL = photoUploadResult.Url.ToString();
                    }

                    user.ProfilPhotoURL = photoURL;
                }

                user.Name = userVM.Name;
                user.Surname = userVM.Surname;
                user.PhoneNumber = userVM.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index"); // или другой метод, который вы хотите вызвать после обновления
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(userVM);
        }
    }
}