using DarabnyProject.Models;
using DarabnyProject.Services;
using DarabnyProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarabnyProject.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public AdminCategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [Route("/admin/categories")]
        public IActionResult Index() => View(categoryService.GetAll());

        [Route("/admin/categories/create")]
        public IActionResult Create() => View(categoryService.BuildFormViewModel());

        [HttpPost]
        [Route("/admin/categories/create")]
        public IActionResult Create(CategoryFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(categoryService.BuildFormViewModel(vm));

            categoryService.Add(new Category { Name = vm.Name, Description = vm.Description });
            TempData["successMsg"] = $"Category \"{vm.Name}\" created successfully.";
            return RedirectToAction("create");
        }

        [Route("/admin/categories/edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var cat = categoryService.GetById(id);
            if (cat == null) return NotFound();

            return View(categoryService.BuildFormViewModel(new CategoryFormViewModel
            {
                Id          = cat.Id,
                Name        = cat.Name,
                Description = cat.Description
            }));
        }

        [HttpPost]
        [Route("/admin/categories/edit")]
        public IActionResult Edit(CategoryFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(categoryService.BuildFormViewModel(vm));

            categoryService.Update(new Category { Id = vm.Id, Name = vm.Name, Description = vm.Description });
            return RedirectToAction("index");
        }

        [Route("/admin/categories/delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var cat = categoryService.GetById(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost]
        [Route("/admin/categories/delete/{id:int}")]
        public IActionResult ConfirmDelete(int id)
        {
            categoryService.Delete(id);
            return RedirectToAction("index");
        }
    }
}
