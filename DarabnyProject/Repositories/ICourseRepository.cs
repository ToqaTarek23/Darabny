using DarabnyProject.Models;

namespace DarabnyProject.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        List<Course> GetAllWithCategory();
        Course? GetWithDetails(int id);
        Course? GetWithRequirements(int id);
        List<Course> Search(string? query, int? categoryId, string? sort);
        void Add(Course course, string? requirementsText, string? learnText);
        void Update(Course course, string? requirementsText, string? learnText);
    }
}
