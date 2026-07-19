using DarabnyProject.Data;
using DarabnyProject.Models;
using DarabnyProject.Services;
using DarabnyProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DarabnyProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly DarabnyDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ICourseService     courseService;
        private readonly ICategoryService   categoryService;
        private readonly IEnrollmentService enrollmentService;
        private readonly IFavoriteService   favoriteService;
        private readonly IReviewService     reviewService;
        private readonly IProgressService   progressService;

        public HomeController(DarabnyDbContext db, UserManager<ApplicationUser> userManager,
            ICourseService courseService, ICategoryService categoryService,
            IEnrollmentService enrollmentService, IFavoriteService favoriteService,
            IReviewService reviewService, IProgressService progressService)
        {
            _db          = db;
            _userManager = userManager;
            this.courseService = courseService;
            this.categoryService = categoryService;
            this.enrollmentService = enrollmentService;
            this.favoriteService = favoriteService;
            this.reviewService = reviewService;
            this.progressService = progressService;
        }

        public IActionResult Index()   => View("index");
        public IActionResult About()   => View("about");

        public IActionResult ContactUs()
            => View(new ContactUsViewModel());

        [HttpPost]
        public IActionResult ContactUs(ContactUsViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            _db.ContactUs.Add(new ContactUs
            {
                Name    = vm.Name,
                Email   = vm.Email,
                Subject = vm.Subject ?? "",
                Message = vm.Message
            });
            _db.SaveChanges();

            TempData["success"] = "Message sent successfully!";
            return RedirectToAction("ContactUs");
        }

        public IActionResult Courses(string? query, int? categoryId, string? sort)
        {
            var courses    = courseService.Search(query, categoryId, sort);
            var categories = categoryService.GetAll();

            var vm = new CoursesPageViewModel
            {
                Courses = courses.Select(c => new CourseCardViewModel
                {
                    Id           = c.Id,
                    Name         = c.Name,
                    Description  = c.Description,
                    CategoryName = c.Category?.Name ?? "",
                    CategoryId   = c.CategoryId,
                    Level        = c.Level.ToString(),
                    Language     = c.Language,
                    Price        = c.Price,
                    PriceType    = c.PriceType
                }).ToList(),
                Categories = categories,
                Query      = query,
                CategoryId = categoryId,
                Sort       = sort ?? "default"
            };

            return View(vm);
        }

        public IActionResult CourseDetails(int id)
        {
            var course = courseService.GetWithDetails(id);
            if (course == null) return NotFound();

            var userId = _userManager.GetUserId(User); 

            var vm = new CourseDetailsViewModel
            {
                Id              = course.Id,
                Name            = course.Name,
                Description     = course.Description,
                Price           = course.Price,
                DiscountedPrice = course.DiscountedPrice,
                PriceType       = course.PriceType,
                Level           = course.Level,
                Language        = course.Language,
                CourseLength    = course.CourseLength,
                StudentNumbers  = course.StudentNumbers,
                LastUpdated     = course.LastUpdated,
                CategoryName    = course.Category?.Name    ?? "",
                InstructorName  = course.Instructor?.Name  ?? "",
                InstructorBio   = course.Instructor?.Bio,
                Requirements    = course.Requirements?.Select(r => r.Requirement).ToList() ?? new(),
                WhatWillLearn   = course.WhatWillLearn?.Select(w => w.Objective).ToList()  ?? new(),
                Chapters        = course.Chapters?.ToList() ?? new(),
                Reviews         = course.Reviews?.Where(r => r.IsApproved).ToList()        ?? new(),
                IsEnrolled      = userId != null && enrollmentService.IsEnrolled(userId, id),
                IsFavorited     = userId != null && favoriteService.IsFavorited(userId, id),
                HasReviewed          = userId != null && reviewService.HasReviewed(userId, id),
                CourseProgressPercent = userId != null ? progressService.GetCourseProgress(userId, id) : 0,
                CompletedLessonIds   = userId != null
                    ? course.Chapters?
                        .SelectMany(ch => ch.Lessons ?? new())
                        .Where(l => progressService.IsCompleted(userId, l.Id))
                        .Select(l => l.Id)
                        .ToHashSet() ?? new()
                    : new()
            };

            return View(vm);
        }
    }
}
