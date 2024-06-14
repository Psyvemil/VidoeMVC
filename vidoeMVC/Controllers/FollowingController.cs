using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.DAL;
using vidoeMVC.Models;

namespace vidoeMVC.Controllers
{
    public class FollowingController(UserManager<AppUser> _userManager,VidoeDBContext _context) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Follow(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userToFollow = await _context.Users.FindAsync(id);

            if (userToFollow == null || currentUser == null)
            {
                return NotFound();
            }

            var follow = new UserFollow
            {
                FollowerId = currentUser.Id,
                FolloweeId = userToFollow.Id
            };

            _context.UserFollows.Add(follow);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userToUnfollow = await _context.Users.FindAsync(id);

            if (userToUnfollow == null || currentUser == null)
            {
                return NotFound();
            }

            var follow = await _context.UserFollows
                .FirstOrDefaultAsync(uf => uf.FollowerId == currentUser.Id && uf.FolloweeId == userToUnfollow.Id);

            if (follow != null)
            {
                _context.UserFollows.Remove(follow);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
