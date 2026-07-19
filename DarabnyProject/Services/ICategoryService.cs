using DarabnyProject.Models;
using DarabnyProject.ViewModels;

namespace DarabnyProject.Services
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category? GetById(int id);
        CategoryFormViewModel BuildFormViewModel(CategoryFormViewModel? vm = null);
        void Add(Category cat);
        void Update(Category cat);
        void Delete(int id);
    }
}
