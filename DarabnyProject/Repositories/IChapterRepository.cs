using DarabnyProject.Models;

namespace DarabnyProject.Repositories
{
    public interface IChapterRepository : IRepository<Chapter>
    {
        List<Chapter> GetAllWithCourse();
        Chapter? GetWithCourseAndLessons(int id);
    }
}
