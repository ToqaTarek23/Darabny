using DarabnyProject.Models;

namespace DarabnyProject.Repositories
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        List<Lesson> GetAllWithChapterAndCourse();
        Lesson? GetWithChapterAndCourse(int id);
    }
}
