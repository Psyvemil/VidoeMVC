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
    public class LikedVidController(UserManager<AppUser> _userManager, VidoeDBContext _context) : Controller
    {
        public async Task<IActionResult> Index(int? page)
        {
            const int pageSize = 8; // Number of items per page

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

            var likedVideosQuery = _context.Videos
                .Include(v => v.Author)
                .ThenInclude(a => a.Followers)
                .Include(v => v.Like)
                .Where(v => v.Like.Any(l => l.UserId == finduser.Id && l.LikeDislike.IsLike));

            // Calculate total count of liked videos
            var totalLikedVideos = await likedVideosQuery.CountAsync();

            // Pagination logic
            var pageNumber = page ?? 1; // Default to first page
            var totalPages = (int)Math.Ceiling(totalLikedVideos / (double)pageSize);

            var videos = await likedVideosQuery
                .OrderByDescending(v => v.CreatedTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginationVM = new PaginationVM
            {
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            var homeVM = new HomeVM
            {
                users = appUsers,
                UserL = userL,
                Videos = videos,
                paginationVM = paginationVM
            };

            return View(homeVM);
        }
    }
}
