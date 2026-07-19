using DarabnyProject.Models;
using DarabnyProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarabnyProject.Controllers
{

    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService reviewService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(IReviewService reviewService, UserManager<ApplicationUser> userManager)
        {
            this.reviewService = reviewService;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Add(int courseId, decimal rating, string comment)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null && !string.IsNullOrWhiteSpace(comment))
                reviewService.Add(userId, courseId, rating, comment);

            return RedirectToAction("CourseDetails", "Home", new { id = courseId });
        }
    }
}
