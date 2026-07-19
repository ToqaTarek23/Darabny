using DarabnyProject.Models;
using DarabnyProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarabnyProject.Controllers
{

    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService favoriteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoriteController(IFavoriteService favoriteService, UserManager<ApplicationUser> userManager)
        {
            this.favoriteService = favoriteService;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Toggle(int courseId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
                favoriteService.Toggle(userId, courseId);

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
                return Redirect(referer);

            return RedirectToAction("CourseDetails", "Home", new { id = courseId });
        }
    }
}
