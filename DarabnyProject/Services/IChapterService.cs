using DarabnyProject.Models;
using DarabnyProject.ViewModels;

namespace DarabnyProject.Services
{
    public interface IChapterService
    {
        List<Chapter> GetAll();
        List<Chapter> GetAllWithCourse();
        Chapter? GetById(int id);
        Chapter? GetWithCourseAndLessons(int id);
        ChapterFormViewModel BuildFormViewModel(ChapterFormViewModel? vm = null);
        void Add(Chapter ch);
        void Update(Chapter ch);
        void Delete(int id);
    }
}
