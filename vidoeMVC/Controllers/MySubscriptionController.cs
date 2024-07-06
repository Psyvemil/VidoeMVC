using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.DAL;
using vidoeMVC.Models;
using vidoeMVC.ViewModels;
using vidoeMVC.ViewModels.Users;
using vidoeMVC.ViewModels.Videos;

namespace vidoeMVC.Controllers
{
    public class MySubscriptionController(UserManager<AppUser> _userManager, VidoeDBContext _context) : Controller
    {
        private const int PageSize = 8; // Number of items per page

        public async Task<IActionResult> Index(int? page)
        {
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

            var finduser = await _userManager.GetUserAsync(User);

            if (finduser == null)
            {
                // Handle the case where the user is not found
                return NotFound();
            }

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
            }).FirstOrDefaultAsync(u => u.Id == finduser.Id);

            var subscriptionsQuery = _context.Users
                .Include(u => u.Videos)
                .ThenInclude(v => v.Author)
                .Where(u => u.Followers.Any(f => f.FollowerId == finduser.Id));

            // Calculate total count of subscriptions
            var totalSubscriptions = await subscriptionsQuery.CountAsync();

            // Pagination logic
            var pageNumber = page ?? 1; // Default to first page
            var totalPages = (int)Math.Ceiling(totalSubscriptions / (double)PageSize);

            var subscriptions = await subscriptionsQuery
                .OrderByDescending(u => u.UserName)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(u => new UserVM
                {
                    UserName = u.UserName,
                    Id = u.Id,
                    ProfPhotURL = u.ProfilPhotoURL,
                    Followers = u.Followers ?? new List<UserFollow>(),
                    Followees = u.Followees ?? new List<UserFollow>(),
                    Videos = u.Videos.Select(v => new VideoViewModel
                    {
                        Id = v.Id,
                        Title = v.Title,
                        TumbnailUrl = v.TumbnailUrl,
                        VideoUrl = v.VideoUrl,
                        CreatedTime = v.CreatedTime,
                        ViewCount = v.ViewCount,
                        Author = new UserVM
                        {
                            UserName = v.Author.UserName,
                            Id = v.Author.Id
                        }
                    }).ToList()
                }).ToListAsync();

            var paginationVM = new PaginationVM
            {
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = PageSize
            };

            var homeVM = new HomeVM
            {
                users = subscriptions,
                UserL = userL,
                paginationVM = paginationVM
            };

            return View(homeVM);
        }
    }
}
