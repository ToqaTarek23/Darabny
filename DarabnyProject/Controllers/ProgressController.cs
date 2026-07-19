using DarabnyProject.Models;
using DarabnyProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DarabnyProject.Controllers
{
    [Authorize]
    public class ProgressController : Controller
    {
        private readonly IProgressService progressService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProgressController(IProgressService progressService, UserManager<ApplicationUser> userManager)
        {
            this.progressService = progressService;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult MarkComplete(int lessonId, int courseId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
                progressService.MarkComplete(userId, lessonId);

            return RedirectToAction("CourseDetails", "Home", new { id = courseId });
        }
    }
}
