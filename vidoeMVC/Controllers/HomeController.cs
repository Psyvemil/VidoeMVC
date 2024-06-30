using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using vidoeMVC.DAL;
using vidoeMVC.Models;
using vidoeMVC.Services;
using vidoeMVC.ViewModels;
using vidoeMVC.ViewModels.Categories;
using vidoeMVC.ViewModels.Users;
using vidoeMVC.ViewModels.Videos;


namespace vidoeMVC.Controllers
{
    public class HomeController(UserManager<AppUser> _userManager,VidoeDBContext _context) : Controller
    {



        public async Task<IActionResult> Index(int? page)
        {
            const int pageSize = 4; // Number of items per page
            var categories = await _context.Categories.ToListAsync();

            // Query videos with pagination
            var videos = await _context.Videos
                .Include(v => v.Author)
                .OrderByDescending(v => v.CreatedTime)
                .Skip((page - 1 ?? 0) * pageSize)
                .Take(pageSize)
                .Select(v => new VideoViewModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    TumbnailUrl = v.TumbnailUrl,
                    VideoUrl = v.VideoUrl,
                    CreatedTime = v.CreatedTime,
                    Author = new UserVM
                    {
                        UserName = v.Author.UserName,
                        Id = v.Author.Id
                    }
                }).ToListAsync();

            // Calculate total pages based on total videos count
            var totalVideos = await _context.Videos.CountAsync();
            var totalPages = (int)Math.Ceiling(totalVideos / (double)pageSize);

            // Pagination logic
            var pageNumber = page ?? 1; // Default to first page
            var pAginationVM = new PaginationVM
            {
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            var appUsers = await _userManager.Users
                .Include(u => u.Followers)
                .ThenInclude(f => f.Follower)
                .Include(u => u.Followees)
                .ThenInclude(f => f.Followee).OrderByDescending(u => u.Followers.Count)
       
                .Select(u => new UserVM
                {
                    UserName = u.UserName,
                    Id = u.Id,
                    ProfPhotURL=u.ProfilPhotoURL,
                    Followers = u.Followers ?? new List<UserFollow>(),
                    Followees = u.Followees ?? new List<UserFollow>()
                }).ToListAsync();

            var homeVM = new HomeVM
            {
                categories = categories.Select(c => new GetCategoryVM
                {
                    Name = c.Name,
                    Id = c.Id
                }).ToList(),
                users = appUsers,
                VideoViewModels = videos,
                paginationVM = pAginationVM // Assign pagination info to HomeVM
            };

            return View(homeVM);
        }


        [HttpGet]
        public async Task< IActionResult> Search(string query)
        {
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
            if (string.IsNullOrWhiteSpace(query))
            {
                return View("Search", new HomeVM());
            }

            var videos = _context.Videos
                .Include(v => v.Author)
                .Where(v => v.Title.Contains(query) ||
                            v.Description.Contains(query) ||
                            v.Tags.Any(t => t.Tag.Name.Contains(query)) ||
                            v.VCategories.Any(c => c.Category.Name.Contains(query)) ||
                            v.Author.UserName.Contains(query))
                .Select(v => new VideoViewModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    TumbnailUrl = v.TumbnailUrl,
                    VideoUrl = v.VideoUrl,
                    Description= v.Description,
                    CreatedTime = v.CreatedTime,
                    Author = new UserVM
                    {
                        UserName = v.Author.UserName,
                        Id = v.Author.Id
                    }
                })
                .ToList();
            HomeVM homeVM = new HomeVM
                {
                VideoViewModels = videos,
                users = appUsers
            };
            return View("Search", homeVM);
        }
    }
}

    



           