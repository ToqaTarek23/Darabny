using DarabnyProject.Models;
using DarabnyProject.ViewModels;

namespace DarabnyProject.Services
{
    public interface ICourseService
    {
        List<Course> GetAll();
        List<Course> GetAllWithCategory();
        Course? GetById(int id);
        Course? GetWithDetails(int id);
        Course? GetWithRequirements(int id);
        List<Course> Search(string? query, int? categoryId, string? sort);
        CourseFormViewModel BuildFormViewModel(CourseFormViewModel? vm = null);
        void Add(Course course, string? requirementsText, string? learnText);
        void Update(Course course, string? requirementsText, string? learnText);
        void Delete(int id);
    }
}
