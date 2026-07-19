using DarabnyProject.Enums;
using DarabnyProject.Models;
using DarabnyProject.Services;
using DarabnyProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DarabnyProject.Controllers.Admin
{
    [Authorize(Roles = "Admin,Teacher")]
    public class AdminCourseController : Controller
    {
        private readonly ICourseService courseService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminCourseController(ICourseService courseService, UserManager<ApplicationUser> userManager)
        {
            this.courseService = courseService;
            _userManager = userManager;
        }

        private CourseFormViewModel BuildVM(CourseFormViewModel? vm = null)
        {
            vm = courseService.BuildFormViewModel(vm);
            // Admin sees all instructors; Teacher only sees themselves
            vm.Instructors = User.IsInRole("Admin")
                ? _userManager.Users.Where(u => u.IsActive).ToList()
                : _userManager.Users.Where(u => u.Id == _userManager.GetUserId(User)).ToList();
            return vm;
        }

        [Route("/admin/courses")]
        public IActionResult Index()
        {
           
            var courses = User.IsInRole("Admin")
                ? courseService.GetAllWithCategory()
                : courseService.GetAllWithCategory()
                    .Where(c => c.InstructorId == _userManager.GetUserId(User))
                    .ToList();

            return View(courses);
        }

        [Route("/admin/courses/create")]
        public IActionResult Create()
        {
            var vm = BuildVM();
            // Teacher: auto-set instructor to self
            if (User.IsInRole("Teacher"))
                vm.InstructorId = _userManager.GetUserId(User);
            return View(vm);
        }

        [HttpPost]
        [Route("/admin/courses/create")]
        public IActionResult Create(CourseFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(BuildVM(vm));

            
            if (User.IsInRole("Teacher"))
                vm.InstructorId = _userManager.GetUserId(User);

            var course = new Course
            {
                Name           = vm.Name,
                Description    = vm.Description,
                Price          = vm.Price,
                PriceType      = vm.Price == 0 ? PriceType.Free : PriceType.Paid,
                CategoryId     = vm.CategoryId,
                InstructorId   = string.IsNullOrWhiteSpace(vm.InstructorId) ? null : vm.InstructorId,
                Level          = vm.Level,
                Language       = string.IsNullOrWhiteSpace(vm.Language) ? "Arabic" : vm.Language,
                CourseLength   = vm.CourseLength,
                LastUpdated    = DateTime.Now,
                URL            = "",
                StudentNumbers = 0,
                ReviewsNumbers = 0
            };

            courseService.Add(course, vm.RequirementsText, vm.LearnText);
            TempData["msg"] = $"Course \"{vm.Name}\" added successfully.";
            return RedirectToAction("create");
        }

        [Route("/admin/courses/{id:int}")]
        public IActionResult Details(int id)
        {
            var course = courseService.GetWithDetails(id);
            if (course == null) return NotFound();

            if (User.IsInRole("Teacher") && course.InstructorId != _userManager.GetUserId(User))
                return Forbid();
            return View(course);
        }

        [Route("/admin/courses/edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var course = courseService.GetWithRequirements(id);
            if (course == null) return NotFound();

            if (User.IsInRole("Teacher") && course.InstructorId != _userManager.GetUserId(User))
                return Forbid();

            var vm = new CourseFormViewModel
            {
                Id               = course.Id,
                Name             = course.Name,
                Description      = course.Description,
                Price            = course.Price,
                CategoryId       = course.CategoryId,
                InstructorId     = course.InstructorId,
                Level            = course.Level,
                Language         = course.Language,
                CourseLength     = course.CourseLength,
                RequirementsText = course.Requirements?.FirstOrDefault()?.Requirement ?? "",
                LearnText        = course.WhatWillLearn?.FirstOrDefault()?.Objective  ?? ""
            };
            return View(BuildVM(vm));
        }

        [HttpPost]
        [Route("/admin/courses/edit")]
        public IActionResult Edit(CourseFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(BuildVM(vm));

            if (User.IsInRole("Teacher"))
                vm.InstructorId = _userManager.GetUserId(User);

            var course = new Course
            {
                Id           = vm.Id,
                Name         = vm.Name,
                Description  = vm.Description,
                Price        = vm.Price,
                PriceType    = vm.Price == 0 ? PriceType.Free : PriceType.Paid,
                CategoryId   = vm.CategoryId,
                InstructorId = string.IsNullOrWhiteSpace(vm.InstructorId) ? null : vm.InstructorId,
                Level        = vm.Level,
                Language     = string.IsNullOrWhiteSpace(vm.Language) ? "Arabic" : vm.Language,
                CourseLength = vm.CourseLength
            };

            courseService.Update(course, vm.RequirementsText, vm.LearnText);
            return RedirectToAction("index");
        }

        [Route("/admin/courses/delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var course = courseService.GetWithDetails(id);
            if (course == null) return NotFound();
            if (User.IsInRole("Teacher") && course.InstructorId != _userManager.GetUserId(User))
                return Forbid();
            return View(course);
        }

        [HttpPost]
        [Route("/admin/courses/delete/{id:int}")]
        public IActionResult ConfirmDelete(int id)
        {
            var course = courseService.GetWithDetails(id);
            if (User.IsInRole("Teacher") && course?.InstructorId != _userManager.GetUserId(User))
                return Forbid();
            courseService.Delete(id);
            return RedirectToAction("index");
        }
    }
}
