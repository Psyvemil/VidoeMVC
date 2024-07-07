using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

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
                    Privacy = new List<VideoStatus> { model.Privacy },
                    Languages = model.Language,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                    IsDeleted = false,
                    Author = user
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
                if (!string.IsNullOrEmpty(model.Tags))
                {
                    var tagNames = model.Tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();
                    video.Tags = new List<VideoTag>();

                    foreach (var tagName in tagNames)
                    {
                        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                        if (tag == null)
                        {
                            tag = new Tag { Name = tagName };
                            _context.Tags.Add(tag);
                            await _context.SaveChangesAsync();
                        }
                        video.Tags.Add(new VideoTag { TagId = tag.Id });
                    }
                }

                // Add cast members
                if (model.CastIDs != null && model.CastIDs.Any())
                {
                    video.Casts = model.CastIDs.Select(id => new VideoCast
                    {
                        UserId = id
                    }).ToList();
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var video = await _context.Videos
                .Include(v => v.VCategories).ThenInclude(vc => vc.Category)
                .Include(v => v.Tags).ThenInclude(vt => vt.Tag)
                .Include(v => v.Casts).ThenInclude(vc => vc.User)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            var model = new VideoEditVM
            {
                id = video.Id, // Ensure your VideoEditVM has an Id property
                Title = video.Title,
                TumbnailUrl = video.TumbnailUrl,
                Description = video.Description,
                Privacy = video.Privacy.FirstOrDefault(), // Assuming Privacy is a single enum value
                Language = video.Languages,
                CategoriesIDs = video.VCategories.Select(vc => vc.CategoryId).ToArray(),
                Tags = string.Join(",", video.Tags.Select(vt => vt.Tag.Name)),
                CastIDs = video.Casts.Select(vc => vc.UserId).ToArray()
            };

            ViewBag.PrivacyOptions = Enum.GetValues(typeof(VideoStatus))
               .Cast<VideoStatus>()
               .Select(v => new SelectListItem
               {
                   Value = ((int)v).ToString(),
                   Text = v.ToString(),
                   Selected = v == model.Privacy
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

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VideoEditVM model, IFormFile? Thumbnail)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var video = await _context.Videos
                    .Include(v => v.VCategories)
                    .Include(v => v.Tags)
                    .Include(v => v.Casts)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (video == null)
                {
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);

                // Update video properties
                video.Title = model.Title;
                video.Description = model.Description;
                video.Privacy = new List<VideoStatus> { model.Privacy };
                video.Languages = model.Language;
                video.UpdatedTime = DateTime.Now;

                // Update Thumbnail if provided
                if (Thumbnail != null)
                {
                    using (var thumbnailStream = Thumbnail.OpenReadStream())
                    {
                        var thumbnailUploadResult = await _cloudinaryService.UploadThumbnailAsync(thumbnailStream, Thumbnail.FileName);
                        video.TumbnailUrl = thumbnailUploadResult.Url.ToString();
                    }
                }

                // Update categories
                if (model.CategoriesIDs != null)
                {
                    // Remove old categories not in model.CategoriesIDs
                    var existingCategoryIds = video.VCategories?.Select(vc => vc.CategoryId).ToList() ?? new List<int>();
                    var categoriesToRemove = video.VCategories?.Where(vc => !model.CategoriesIDs.Contains(vc.CategoryId)).ToList();
                    _context.VideoCategories.RemoveRange(categoriesToRemove);

                    // Add new categories
                    var categoriesToAdd = model.CategoriesIDs.Where(cid => !existingCategoryIds.Contains(cid)).Select(cid => new VideoCategory
                    {
                        VideoId = id,
                        CategoryId = cid
                    }).ToList();
                    _context.VideoCategories.AddRange(categoriesToAdd);
                }

                // Update tags
                if (!string.IsNullOrEmpty(model.Tags))
                {
                    var tagNames = model.Tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();

                    // Remove tags not in model.Tags
                    if (video.Tags != null)
                    {
                        var tagsToRemove = video.Tags.Where(vt => !tagNames.Contains(vt.Tag?.Name)).ToList();
                        _context.VideoTags.RemoveRange(tagsToRemove);
                    }

                    // Add new tags
                    foreach (var tagName in tagNames)
                    {
                        var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                        if (existingTag == null)
                        {
                            existingTag = new Tag { Name = tagName };
                            _context.Tags.Add(existingTag);
                            await _context.SaveChangesAsync(); // Save the new tag
                        }

                        var existingVideoTag = await _context.VideoTags.FirstOrDefaultAsync(vt => vt.VideoId == id && vt.TagId == existingTag.Id);
                        if (existingVideoTag == null)
                        {
                            _context.VideoTags.Add(new VideoTag { VideoId = id, TagId = existingTag.Id });
                        }
                    }
                }
                else
                {
                    // If model.Tags is empty, remove all existing tags
                    _context.VideoTags.RemoveRange(video.Tags);
                }

                // Update cast members
                if (model.CastIDs != null)
                {
                    // Remove old casts not in model.CastIDs
                    var existingCastIds = video.Casts.Select(vc => vc.UserId).ToList();
                    var castsToRemove = video.Casts.Where(vc => !model.CastIDs.Contains(vc.UserId)).ToList();
                    _context.VideoCasts.RemoveRange(castsToRemove);

                    // Add new casts
                    var castsToAdd = model.CastIDs.Where(cid => !existingCastIds.Contains(cid)).Select(cid => new VideoCast
                    {
                        VideoId = id,
                        UserId = cid
                    }).ToList();
                    _context.VideoCasts.AddRange(castsToAdd);
                }
                else
                {
                    // If model.CastIDs is null, remove all existing casts
                    _context.VideoCasts.RemoveRange(video.Casts);
                }

                _context.Update(video);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            // If ModelState is not valid, reload necessary data and return to the edit view
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

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var video = await _context.Videos
                .Include(v => v.VCategories)
                .Include(v => v.Tags)
                .Include(v => v.Casts)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            // Remove associated categories
            if (video.VCategories != null && video.VCategories.Any())
            {
                _context.VideoCategories.RemoveRange(video.VCategories);
            }

            // Remove associated tags
            if (video.Tags != null && video.Tags.Any())
            {
                _context.VideoTags.RemoveRange(video.Tags);
            }

            // Remove associated cast members
            if (video.Casts != null && video.Casts.Any())
            {
                _context.VideoCasts.RemoveRange(video.Casts);
            }

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
