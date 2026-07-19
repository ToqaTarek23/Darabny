using DarabnyProject.Models;
using DarabnyProject.Services;
using DarabnyProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DarabnyProject.Controllers.Admin
{
    [Authorize(Roles = "Admin,Teacher")]
    public class AdminChapterController : Controller
    {
        private readonly IChapterService chapterService;
        private readonly ICourseService  courseService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminChapterController(IChapterService chapterService, ICourseService courseService,
            UserManager<ApplicationUser> userManager)
        {
            this.chapterService = chapterService;
            this.courseService = courseService;
            _userManager = userManager;
        }

        
        private List<Course> GetAllowedCourses()
        {
            return User.IsInRole("Admin")
                ? courseService.GetAll()
                : courseService.GetAll()
                    .Where(c => c.InstructorId == _userManager.GetUserId(User))
                    .ToList();
        }

        private bool OwnsChapter(Chapter ch)
        {
            if (User.IsInRole("Admin")) return true;
            var userId = _userManager.GetUserId(User);
            var course = courseService.GetById(ch.CourseId);
            return course?.InstructorId == userId;
        }

        [Route("/admin/chapters")]
        public IActionResult Index()
        {
            var chapters = chapterService.GetAllWithCourse();

            
            if (User.IsInRole("Teacher"))
            {
                var userId = _userManager.GetUserId(User);
                chapters = chapters
                    .Where(ch => ch.Course?.InstructorId == userId)
                    .ToList();
            }

            return View(chapters);
        }

        [Route("/admin/chapters/create")]
        public IActionResult Create()
        {
            var vm = chapterService.BuildFormViewModel();
            vm.Courses = GetAllowedCourses();
            return View(vm);
        }

        [HttpPost]
        [Route("/admin/chapters/create")]
        public IActionResult Create(ChapterFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Courses = GetAllowedCourses();
                return View(chapterService.BuildFormViewModel(vm));
            }

            
            if (User.IsInRole("Teacher"))
            {
                var userId = _userManager.GetUserId(User);
                var course = courseService.GetById(vm.CourseId);
                if (course?.InstructorId != userId)
                    return Forbid();
            }

            chapterService.Add(new Chapter { Name = vm.Name, CourseId = vm.CourseId, LessonCounter = 0, ChapterLength = 0 });
            TempData["msg"] = "Chapter added successfully.";
            return RedirectToAction("create");
        }

        [Route("/admin/chapters/edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var ch = chapterService.GetById(id);
            if (ch == null) return NotFound();
            if (!OwnsChapter(ch)) return Forbid();

            var vm = chapterService.BuildFormViewModel(new ChapterFormViewModel
            {
                Id       = ch.Id,
                Name     = ch.Name,
                CourseId = ch.CourseId
            });
            vm.Courses = GetAllowedCourses();
            return View(vm);
        }

        [HttpPost]
        [Route("/admin/chapters/edit")]
        public IActionResult Edit(ChapterFormViewModel vm)
        {
            var ch = chapterService.GetById(vm.Id);
            if (ch == null) return NotFound();
            if (!OwnsChapter(ch)) return Forbid();

            if (!ModelState.IsValid)
            {
                vm.Courses = GetAllowedCourses();
                return View(chapterService.BuildFormViewModel(vm));
            }

            chapterService.Update(new Chapter { Id = vm.Id, Name = vm.Name, CourseId = vm.CourseId });
            return RedirectToAction("index");
        }

        [Route("/admin/chapters/delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var ch = chapterService.GetWithCourseAndLessons(id);
            if (ch == null) return NotFound();
            if (!OwnsChapter(ch)) return Forbid();
            return View(ch);
        }

        [HttpPost]
        [Route("/admin/chapters/delete/{id:int}")]
        public IActionResult ConfirmDelete(int id)
        {
            var ch = chapterService.GetById(id);
            if (ch == null) return NotFound();
            if (!OwnsChapter(ch)) return Forbid();
            chapterService.Delete(id);
            return RedirectToAction("index");
        }
    }
}
