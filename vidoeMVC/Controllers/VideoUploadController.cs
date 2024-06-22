using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.DAL;
using vidoeMVC.Models;
using vidoeMVC.Services;
using vidoeMVC.ViewModels;
using vidoeMVC.ViewModels.Users;
using vidoeMVC.ViewModels.Videos;

namespace vidoeMVC.Controllers
{
    public class VideoUploadController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly CloudinaryService _cloudinaryService;
        private readonly VidoeDBContext _context;

        public VideoUploadController(UserManager<AppUser> userManager, CloudinaryService cloudinaryService, VidoeDBContext context)
        {
            _userManager = userManager;
            _cloudinaryService = cloudinaryService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new VideoUploadVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(VideoUploadVM video, IFormFile videoFile, IFormFile thumbnailFile)
        {
            if (videoFile != null && thumbnailFile != null)
            {
                using var videoStream = videoFile.OpenReadStream();
                using var thumbnailStream = thumbnailFile.OpenReadStream();

                var videoUploadResult = await _cloudinaryService.UploadVideoAsync(videoStream, videoFile.FileName);
                var thumbnailUploadResult = await _cloudinaryService.UploadThumbnailAsync(thumbnailStream, thumbnailFile.FileName);

                ViewBag.VideoUrl = videoUploadResult.SecureUrl.ToString();
                ViewBag.TumbnailUrl = thumbnailUploadResult.SecureUrl.ToString();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var newVideo = new Video
            {
                Title = video.Title,
                Description = video.Description,
                VideoUrl = ViewBag.VideoUrl,
                TumbnailUrl = ViewBag.ThumbnailUrl,
                AuthorId = user.Id, 
                Privacy = video.Privacy,
                Languages = video.Languages,
                VCategories = video.VCategories,
                Cast = video.Cast,
                Tags = video.Tags
            };

            _context.Videos.Add(newVideo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
