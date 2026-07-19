using DarabnyProject.Models;
using DarabnyProject.Services;
using DarabnyProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DarabnyProject.Controllers.Admin
{
    [Authorize(Roles = "Admin,Teacher")]
    public class AdminLessonController : Controller
    {
        private readonly ILessonService  lessonService;
        private readonly IChapterService chapterService;
        private readonly ICourseService  courseService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminLessonController(ILessonService lessonService, IChapterService chapterService,
            ICourseService courseService, UserManager<ApplicationUser> userManager)
        {
            this.lessonService = lessonService;
            this.chapterService = chapterService;
            this.courseService = courseService;
            _userManager = userManager;
        }

        private List<Chapter> GetAllowedChapters()
        {
            var chapters = chapterService.GetAllWithCourse();
            if (User.IsInRole("Admin")) return chapters;

            var userId = _userManager.GetUserId(User);
            return chapters
                .Where(ch => ch.Course?.InstructorId == userId)
                .ToList();
        }

        private bool OwnsLesson(Lesson lesson)
        {
            if (User.IsInRole("Admin")) return true;
            var userId  = _userManager.GetUserId(User);
            var chapter = chapterService.GetById(lesson.ChapterId);
            var course  = chapter != null ? courseService.GetById(chapter.CourseId) : null;
            return course?.InstructorId == userId;
        }

        [Route("/admin/lessons")]
        public IActionResult Index()
        {
            var lessons = lessonService.GetAllWithChapterAndCourse();

            if (User.IsInRole("Teacher"))
            {
                var userId = _userManager.GetUserId(User);
                lessons = lessons
                    .Where(l => l.Chapter?.Course?.InstructorId == userId)
                    .ToList();
            }

            return View(lessons);
        }

        [Route("/admin/lessons/create")]
        public IActionResult Create()
        {
            var vm = lessonService.BuildFormViewModel();
            vm.Chapters = GetAllowedChapters();
            return View(vm);
        }

        [HttpPost]
        [Route("/admin/lessons/create")]
        public IActionResult Create(LessonFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Chapters = GetAllowedChapters();
                return View(lessonService.BuildFormViewModel(vm));
            }

            if (User.IsInRole("Teacher"))
            {
                var userId  = _userManager.GetUserId(User);
                var chapter = chapterService.GetById(vm.ChapterId);
                var course  = chapter != null ? courseService.GetById(chapter.CourseId) : null;
                if (course?.InstructorId != userId) return Forbid();
            }

            lessonService.Add(new Lesson { Name = vm.Name, ChapterId = vm.ChapterId });
            TempData["msg"] = "Lesson added successfully.";
            return RedirectToAction("create");
        }

        [Route("/admin/lessons/edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var lesson = lessonService.GetById(id);
            if (lesson == null) return NotFound();
            if (!OwnsLesson(lesson)) return Forbid();

            var vm = lessonService.BuildFormViewModel(new LessonFormViewModel
            {
                Id        = lesson.Id,
                Name      = lesson.Name,
                ChapterId = lesson.ChapterId
            });
            vm.Chapters = GetAllowedChapters();
            return View(vm);
        }

        [HttpPost]
        [Route("/admin/lessons/edit")]
        public IActionResult Edit(LessonFormViewModel vm)
        {
            var lesson = lessonService.GetById(vm.Id);
            if (lesson == null) return NotFound();
            if (!OwnsLesson(lesson)) return Forbid();

            if (!ModelState.IsValid)
            {
                vm.Chapters = GetAllowedChapters();
                return View(lessonService.BuildFormViewModel(vm));
            }

            lessonService.Update(new Lesson { Id = vm.Id, Name = vm.Name, ChapterId = vm.ChapterId });
            return RedirectToAction("index");
        }

        [Route("/admin/lessons/delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var lesson = lessonService.GetWithChapterAndCourse(id);
            if (lesson == null) return NotFound();
            if (!OwnsLesson(lesson)) return Forbid();
            return View(lesson);
        }

        [HttpPost]
        [Route("/admin/lessons/delete/{id:int}")]
        public IActionResult ConfirmDelete(int id)
        {
            var lesson = lessonService.GetById(id);
            if (lesson == null) return NotFound();
            if (!OwnsLesson(lesson)) return Forbid();
            lessonService.Delete(id);
            return RedirectToAction("index");
        }
    }
}
