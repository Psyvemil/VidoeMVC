using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using vidoeMVC.DAL;
using vidoeMVC.Interfaces;
using vidoeMVC.Models;

namespace vidoeMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    
    public class AdminVideosController(VidoeDBContext _context,EmailService _emailService) : Controller
    {

        public async Task<IActionResult> Index(string searchString)
        {
            IQueryable<Video> videos = _context.Videos.Include(v => v.Author);

            if (!String.IsNullOrEmpty(searchString))
            {
                videos = videos.Where(v =>
                    v.Title.Contains(searchString) ||
                    v.Description.Contains(searchString) ||
                    v.Author.UserName.Contains(searchString) ||
                    v.Id.ToString().Contains(searchString) ||
                     v.VideoUrl.Contains(searchString)
                );
            }

            List<Video> videoList = await videos.ToListAsync();
            return View(videoList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var video = await _context.Videos.Include(v=>v.Author)
                .Include(v => v.VCategories)
                .Include(v => v.Tags)
                .Include(v => v.Casts)
                .Include(v => v.Comments)
                .Include(v => v.Like)
                .Include(v=>v.VideoReports)// Включаем связанные комментарии
                .FirstOrDefaultAsync(v => v.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            if (video.VideoReports != null && video.VideoReports.Any())
            {
                _context.VideoReports.RemoveRange(video.VideoReports);
            }
            if (video.Like != null && video.Like.Any())
            {
                _context.VideoLikes.RemoveRange(video.Like);
            }
            // Удаляем связанные комментарии
            if (video.Comments != null && video.Comments.Any())
            {
                _context.VideoComments.RemoveRange(video.Comments);
            }

            // Удаляем связанные категории
            if (video.VCategories != null && video.VCategories.Any())
            {
                _context.VideoCategories.RemoveRange(video.VCategories);
            }

            // Удаляем связанные теги
            if (video.Tags != null && video.Tags.Any())
            {
                _context.VideoTags.RemoveRange(video.Tags);
            }

            // Удаляем связанные актеров
            if (video.Casts != null && video.Casts.Any())
            {
                _context.VideoCasts.RemoveRange(video.Casts);
            }

            // Удаляем само видео
            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            // Отправка уведомления на почту автору видео
            var emailMessage = $"Your video '{video.Title}' has been deleted.";
            await _emailService.SendEmailAsync(video.Author.Email, "Video Deleted", emailMessage);

            return RedirectToAction("Index");
        }

    }

}
