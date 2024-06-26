using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using vidoeMVC.DAL;
using vidoeMVC.Enums;
using vidoeMVC.Models;
using vidoeMVC.Services;
using vidoeMVC.ViewModels;
using vidoeMVC.ViewModels.Videos;

namespace vidoeMVC.Controllers
{
    [Authorize]
    public class VideoUploadController : Controller
    {
        private readonly VidoeDBContext _context;
        private readonly CloudinaryService _cloudinaryService;
        private readonly UserManager<AppUser> _userManager;

        public VideoUploadController(VidoeDBContext context, CloudinaryService cloudinaryService, UserManager<AppUser> userManager)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {

            ViewBag.PrivacyOptions = Enum.GetValues(typeof(VideoStatus))
               .Cast<VideoStatus>()
               .Select(v => new SelectListItem
               {
                   Value = ((int)v).ToString(),
                   Text = v.ToString()
               })
               .ToList();


            ViewBag.Languages = Enum.GetValues(typeof(Language))
                 .Cast<Language>()
                 .Select(l => new SelectListItem
                 {
                     Value = ((int)l).ToString(),
                     Text = l.ToString()
                 })
                 .ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Users = _context.Users.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VideoUploadVM model)
        {
                var user = await _userManager.GetUserAsync(User);
                model.AuthorId = user.Id;
            if (ModelState.IsValid)
            {
                string videoUrl;
                string thumbnailUrl;

                using (var videoStream = model.Video.OpenReadStream())
                {
                    var videoUploadResult = await _cloudinaryService.UploadVideoAsync(videoStream, model.Video.FileName);
                     videoUrl = videoUploadResult.Url.ToString();
                }

                using (var thumbnailStream = model.Tumbnail.OpenReadStream())
                {
                    var thumbnailUploadResult = await _cloudinaryService.UploadThumbnailAsync(thumbnailStream, model.Tumbnail.FileName);
                     thumbnailUrl = thumbnailUploadResult.Url.ToString();
                }

                var video = new Video
                {
                    Title = model.Title,
                    Description = model.Description,
                    VideoUrl = videoUrl,
                    TumbnailUrl = thumbnailUrl,
                    AuthorId = user.Id,
                    //Privacy =   model.Privacy ,
                    //Languages =  model.Language ,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                    Tags = model.TagsIDs?.Select(tagId => new VideoTag { TagId = tagId }).ToList(),
                    Cast = model.CastIDs?.Select(castId => new AppUser { Id = castId }).ToList(),
                    VCategories = model.CategoriesIDs?.Select(catId => new VideoCategory { CategoryId = catId }).ToList(),
                    IsDeleted = false,

                };

                // Add categories
                if (model.CategoriesIDs != null && model.CategoriesIDs.Any())
                {
                    video.VCategories = model.CategoriesIDs.Select(id => new VideoCategory
                    {
                        CategoryId = id
                    }).ToList();
                }

                // Add tags
                if (model.TagsIDs != null && model.TagsIDs.Any())
                {
                    video.Tags = model.TagsIDs.Select(id => new VideoTag
                    {
                        TagId = id
                    }).ToList();
                }

                // Add cast members
                if (model.CastIDs != null && model.CastIDs.Any())
                {
                    video.Cast = model.CastIDs.Select(id => _context.Users.Find(id)).ToList();
                }

                _context.Videos.Add(video);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Users = _context.Users.ToList();
            return View(model);


        }
    }
}

//using CloudinaryDotNet.Actions;
//using CloudinaryDotNet;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using vidoeMVC.DAL;
//using vidoeMVC.Enums;
//using vidoeMVC.Models;
//using vidoeMVC.Services;
//using vidoeMVC.ViewModels.Videos;
//using Video = vidoeMVC.Models.Video;

//namespace vidoeMVC.Controllers
//{
//    public class VideoUploadController : Controller
//    {
//        private readonly UserManager<AppUser> _userManager;
//        private readonly CloudinaryService _cloudinary;
//        private readonly VidoeDBContext _context;

//        public VideoUploadController(UserManager<AppUser> userManager, CloudinaryService cloudinaryService, VidoeDBContext context)
//        {
//            _userManager = userManager;
//            _cloudinary = cloudinaryService;
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var videos = await _context.Videos
//                .Include(v => v.Author)
//                .Include(v => v.VCategories).ThenInclude(vc => vc.Category)
//                .Include(v => v.Tags).ThenInclude(vt => vt.Tag)
//                .ToListAsync();
//            return View(videos);
//        }

//        public IActionResult Create()
//        {
//            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
//            ViewBag.Tags = new SelectList(_context.Tags, "Id", "Name");
//            ViewBag.Users = new SelectList(_context.Users, "Id", "UserName");
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(VideoUploadVM model)
//        {
//            if (ModelState.IsValid)
//            {
//                // Получаем текущего пользователя
//                var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == User.Identity.Name);
//                if (currentUser == null)
//                {
//                    return Unauthorized();
//                }

//                // Сохранение видео файла на Cloudinary
//                var uploadResult = await UploadFileToCloudinary(model.Video);
//                var thumbnailUploadResult = await UploadFileToCloudinary(model.Tumbnail);

//                if (uploadResult == null || thumbnailUploadResult == null)
//                {
//                    ModelState.AddModelError("", "Error uploading files.");
//                    return View(model);
//                }

//                var video = new Video
//                {
//                    Title = model.Title,
//                    Description = model.Description,
//                    VideoUrl = uploadResult.SecureUrl.ToString(),
//                    TumbnailUrl = thumbnailUploadResult.SecureUrl.ToString(),
//                    CreatedTime = DateTime.Now,
//                    UpdatedTime = DateTime.Now,
//                    AuthorId = currentUser.Id,
//                    Privacy = new List<VideoStatus> { model.Privacy },
//                    Languages = new List<Language> { model.Language }
//                };

//                _context.Videos.Add(video);
//                await _context.SaveChangesAsync();

//                // Добавление категорий
//                foreach (var categoryId in model.CategoriesIDs)
//                {
//                    var videoCategory = new VideoCategory
//                    {
//                        VideoId = video.Id,
//                        CategoryId = categoryId
//                    };
//                    _context.VideoCategories.Add(videoCategory);
//                }

//                // Добавление тегов
//                foreach (var tagId in model.TagsIDs)
//                {
//                    var videoTag = new VideoTag
//                    {
//                        VideoId = video.Id,
//                        TagId = tagId
//                    };
//                    _context.VideoTags.Add(videoTag);
//                }

//                // Добавление участников
//                foreach (var castId in model.CastIDs)
//                {
//                    var castMember = await _context.Users.FindAsync(castId);
//                    if (castMember != null)
//                    {
//                        video.Cast.Add(castMember);
//                    }
//                }

//                await _context.SaveChangesAsync();

//                return RedirectToAction(nameof(Index));
//            }

//            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
//            ViewBag.Tags = new SelectList(_context.Tags, "Id", "Name");
//            ViewBag.Users = new SelectList(_context.Users, "Id", "UserName");

//            return View(model);
//        }

//        private async Task<UploadResult> UploadFileToCloudinary(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                return null;
//            }

//            var uploadParams = new VideoUploadParams()
//            {
//                File = new FileDescription(file.FileName, file.OpenReadStream())
//            };

//            var uploadResult = await _cloudinary.UploadVideoAsync(uploadParams);
//            return uploadResult;
//        }
//    }
//}












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
