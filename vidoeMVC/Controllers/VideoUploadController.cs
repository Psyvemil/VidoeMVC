using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using vidoeMVC.DAL;
using vidoeMVC.Enums;
using vidoeMVC.Models;
using vidoeMVC.Services;
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
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(VideoUploadVM video, IFormFile videoFile, IFormFile thumbnailFile)
        {
            if (videoFile == null)
            {
                return Content("video null");
            }
            if (thumbnailFile == null)
            {
                return Content("tumb null");
            }
            using var videoStream = videoFile.OpenReadStream();
                using var thumbnailStream = thumbnailFile.OpenReadStream();

                var videoUploadResult = await _cloudinaryService.UploadVideoAsync(videoStream, videoFile.FileName);
                var thumbnailUploadResult = await _cloudinaryService.UploadThumbnailAsync(thumbnailStream, thumbnailFile.FileName);

               


                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

            var newVideo = new Video
            {
                Title = video.Title,
                Description = video.Description,
                VideoUrl = videoUploadResult.SecureUrl.ToString(),
                TumbnailUrl = thumbnailUploadResult.SecureUrl.ToString(),
                AuthorId = user.Id,
                Privacy = video.videoStat,
                Languages = video.LanguageIDs.Select(s=>new Language {}).ToList(),
                DateCreated = video.DateTime,
                VCategories=video.CategoriesIDs.Select(c=>new VideoCategory { CategoryId=c}).ToList(),
                Tags=video.TagsIDs.Select(t=>new VideoTag {  TagId=t}).ToList(),
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                Author=user,
                Cast=video.CastIDs.Select(c=>new AppUser { Id=c}).ToList(),
                IsDeleted=false,
                };
            // Adding related entities
            

            _context.Videos.Add(newVideo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}












//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using NuGet.Protocol;
//using NuGet.Versioning;
//using vidoeMVC.DAL;
//using vidoeMVC.Models;
//using vidoeMVC.Services;
//using vidoeMVC.ViewModels;
//using vidoeMVC.ViewModels.Users;
//using vidoeMVC.ViewModels.Videos;

//namespace vidoeMVC.Controllers
//{
//    public class VideoUploadController : Controller
//    {
//        private readonly UserManager<AppUser> _userManager;
//        private readonly CloudinaryService _cloudinaryService;
//        private readonly VidoeDBContext _context;

//        public VideoUploadController(UserManager<AppUser> userManager, CloudinaryService cloudinaryService, VidoeDBContext context)
//        {
//            _userManager = userManager;
//            _cloudinaryService = cloudinaryService;
//            _context = context;
//        }

//        [HttpGet]
//        public async Task <IActionResult> Index(UserVM userid)
//        {




//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Index(UserVM userid,VideoUploadVM video, IFormFile videoFile, IFormFile thumbnailFile)
//        {

//            if (videoFile == null && thumbnailFile == null)
//            {
//                return Content("bosh qaldii");
//            }
//                using var videoStream = videoFile.OpenReadStream();
//                using var thumbnailStream = thumbnailFile.OpenReadStream();

//                var videoUploadResult = await _cloudinaryService.UploadVideoAsync(videoStream, videoFile.FileName);
//                var thumbnailUploadResult = await _cloudinaryService.UploadThumbnailAsync(thumbnailStream, thumbnailFile.FileName);

//                ViewBag.VideoUrl = videoUploadResult.SecureUrl.ToString();
//                ViewBag.TumbnailUrl = thumbnailUploadResult.SecureUrl.ToString();

//                var user = await _userManager.GetUserAsync(User);
//                if (user == null)
//                {
//                    return Unauthorized();
//                }

//                var newVideo = new Video
//                {
//                    Title = video.Title,
//                    Description = video.Description,
//                    VideoUrl = videoUploadResult.SecureUrl.ToString(),
//                    TumbnailUrl = thumbnailUploadResult.SecureUrl.ToString(),
//                    AuthorId = userid.Id,
//                    Privacy = video.Privacy,
//                    Languages = video.Languages,
//                    VCategories = video.VCategories,
//                    Cast = video.Cast,
//                    Tags = video.Tags
//                };

//            _context.Videos.Add(newVideo);
//            await _context.SaveChangesAsync();

//            return RedirectToAction(nameof(Index), "Home");
//        }
//    }
//}
