using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.Models;
using vidoeMVC.ViewModels.Categories;
using vidoeMVC.ViewModels.Users;
using vidoeMVC.ViewModels;
using vidoeMVC.DAL;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace vidoeMVC.Controllers
{
    public class VideoPageController(UserManager<AppUser> _userManager, VidoeDBContext _context) : Controller
    {
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var finduser = await _userManager.GetUserAsync(User);
            UserVM userL = null;
            if (finduser != null)
            {
                userL = await _userManager.Users.Select(u => new UserVM
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
            }

            var video = await _context.Videos
                .Include(c => c.Author)
                .Include(t => t.Tags).ThenInclude(t => t.Tag)
                .Include(c => c.VCategories).ThenInclude(c => c.Category)
                .Include(v => v.Casts).ThenInclude(vc => vc.User)
                .Include(a => a.Author).ThenInclude(a => a.Followers)
                .Include(v => v.Like).ThenInclude(vl => vl.LikeDislike)
                .Include(v => v.Comments).ThenInclude(vc => vc.Comment)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            video.ViewCount++;
            _context.Update(video);
            await _context.SaveChangesAsync();

            var categoryIds = video.VCategories.Select(vc => vc.CategoryId).ToList();

            var relatedVideos = await _context.Videos
                .Include(v => v.Author)
                .Include(v => v.VCategories).ThenInclude(vc => vc.Category)
                .Where(v => v.VCategories.Any(vc => categoryIds.Contains(vc.CategoryId)))
                .ToListAsync();

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

            video.Comments = video.Comments.OrderByDescending(c => c.Comment.CreatedTime).ToList();

            var homeVM = new HomeVM
            {
                users = appUsers,
                Video = video,
                Videos = relatedVideos,
                UserL=userL
            };

            return View(homeVM);
        }

        [HttpPost]
public async Task<IActionResult> Like(int videoId, bool isLike)
{
    var userId = _userManager.GetUserId(User);
    if (userId == null)
    {
        return Unauthorized();
    }

    var videoLike = await _context.VideoLikes
        .Include(vl => vl.LikeDislike)
        .FirstOrDefaultAsync(vl => vl.VideoId == videoId && vl.UserId == userId);

    if (videoLike != null)
    {
        videoLike.LikeDislike.IsLike = isLike;
    }
    else
    {
        var likeDislike = new LikeDislike { IsLike = isLike };
        _context.LikeDislikes.Add(likeDislike);
        await _context.SaveChangesAsync();

        videoLike = new VideoLike
        {
            VideoId = videoId,
            UserId = userId,
            LikeDislikeId = likeDislike.Id,
            LikeDislike = likeDislike
        };
        _context.VideoLikes.Add(videoLike);
    }

    await _context.SaveChangesAsync();

    return Ok();
}

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(int videoId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(User);
            var comment = new Comment
            {
                Text = text,
                CreatedTime = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var videoComment = new VideoComment
            {
                VideoId = videoId,
                CommentId = comment.Id,
                UserId = userId
            };

            _context.VideoComments.Add(videoComment);
            await _context.SaveChangesAsync();

            var video = await _context.Videos
                .Include(v => v.Comments)
                    .ThenInclude(vc => vc.Comment)
                .Include(v => v.Comments)
                    .ThenInclude(vc => vc.User)
                .FirstOrDefaultAsync(v => v.Id == videoId);

            // Order comments by CreatedTime descending
            video.Comments = video.Comments.OrderByDescending(c => c.Comment.CreatedTime).ToList();

            return PartialView("_CommentsPartial", video.Comments);
        }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null)
        {
            return NotFound();
        }
            var userId = _userManager.GetUserId(User);
            var videoComment = await _context.VideoComments
                .Include(vc => vc.Comment)
                .FirstOrDefaultAsync(vc => vc.CommentId == commentId && vc.UserId == userId);

            if (videoComment == null)
            {
                return NotFound();
            }

        

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        // Optionally, remove VideoComment entry as well if necessary
        _context.VideoComments.Remove(videoComment);
        await _context.SaveChangesAsync();

        return Ok();
    }


}
}
