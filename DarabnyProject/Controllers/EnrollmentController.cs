using DarabnyProject.Models;
using DarabnyProject.Data;
using DarabnyProject.Services;
using DarabnyProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DarabnyProject.Controllers
{

    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService enrollmentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DarabnyDbContext _db;

        public EnrollmentController(
            IEnrollmentService enrollmentService,
            UserManager<ApplicationUser> userManager,
            DarabnyDbContext db)
        {
            this.enrollmentService = enrollmentService;
            _userManager = userManager;
            _db = db;
        }

        [HttpPost]
        public IActionResult Toggle(int courseId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var wasEnrolled = enrollmentService.IsEnrolled(userId, courseId);
                enrollmentService.Toggle(userId, courseId);

                if (!wasEnrolled)
                    return Json(new { success = true, action = "showConfirmation", courseId });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public IActionResult Confirmation(int courseId)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Json(new { error = "Unauthorized" });

            if (!enrollmentService.IsEnrolled(userId, courseId))
                return Json(new { error = "Not enrolled" });

            var course = _db.Courses
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
                return Json(new { error = "Course not found" });

            var amount = course.PriceType == Enums.PriceType.Free
                ? 0m
                : course.DiscountedPrice ?? course.Price;

            var vm = new EnrollmentConfirmationViewModel
            {
                CourseId = course.Id,
                CourseName = course.Name,
                Amount = amount,
                Currency = "EGP",
                PaymentStatus = amount == 0 ? "Free enrollment confirmed" : "Payment approved",
                TransactionId = $"DRB-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}",
                ConfirmedAt = DateTime.Now,
                IsFreeCourse = amount == 0
            };

            return Json(vm);
        }
    }
}
