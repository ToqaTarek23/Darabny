using DarabnyProject.Models;
using DarabnyProject.Repositories;
using DarabnyProject.ViewModels;

namespace DarabnyProject.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepo;

        public CategoryService(ICategoryRepository categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }

        public List<Category> GetAll()     => categoryRepo.GetAll();
        public Category? GetById(int id)   => categoryRepo.GetById(id);
        public void Add(Category cat)      => categoryRepo.Add(cat);
        public void Update(Category cat)   => categoryRepo.Update(cat);
        public void Delete(int id)         => categoryRepo.Delete(id);

        public CategoryFormViewModel BuildFormViewModel(CategoryFormViewModel? vm = null)
            => vm ?? new CategoryFormViewModel();
    }
}
