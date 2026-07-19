using DarabnyProject.Data;
using DarabnyProject.Services;
using Microsoft.AspNetCore.Authorization;
using DarabnyProject.Models;
using DarabnyProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DarabnyProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DarabnyDbContext             _db;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IProgressService progressService;

        public DashboardController(DarabnyDbContext db, UserManager<ApplicationUser> userManager,
            IProgressService progressService)
        {
            _db          = db;
            _userManager = userManager;
            this.progressService = progressService;
        }

        public IActionResult Admin() => Redirect("/admin");

        public IActionResult Instructor()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                ViewData["BodyClass"] = "instructor-theme";
                return View("InstructorPage");
            }

            if (User.IsInRole("Teacher") || User.IsInRole("Admin"))
                return Redirect("/admin");

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Student()
        {
            ViewData["BodyClass"] = "dashboard-theme";

            var user   = await _userManager.GetUserAsync(User);
            var userId = user?.Id ?? "";

            var enrollments  = _db.Enrollments
                .Include(e => e.Course).ThenInclude(c => c.Category)
                .Where(e => e.UserId == userId).ToList();

            var favorites    = _db.Favorites
                .Include(f => f.Course).ThenInclude(c => c.Category)
                .Where(f => f.UserId == userId).ToList();

            var certificates = _db.Certificates
                .Include(c => c.Course)
                .Where(c => c.UserId == userId).ToList();

            var name     = user?.Name ?? "Student";
            var initials = name.Split(' ')
                .Take(2)
                .Select(w => w[0].ToString().ToUpper())
                .Aggregate("", (a, b) => a + b);

            var progressMap = new Dictionary<int, int>();
            foreach (var e in enrollments)
                progressMap[e.CourseId] = progressService.GetCourseProgress(userId, e.CourseId);

            var vm = new StudentDashboardViewModel
            {
                StudentName       = name,
                StudentInitials   = initials,
                StudentEmail      = user?.Email,
                StudentBio        = user?.Bio,
                MemberSince       = user?.SignUpDate ?? DateTime.Now,
                IsActive          = user?.IsActive ?? false,
                Enrollments       = enrollments,
                Favorites         = favorites,
                Certificates      = certificates,
                CourseProgressMap = progressMap
            };

            return View("StudentDashboard", vm);
        }
    }
}
