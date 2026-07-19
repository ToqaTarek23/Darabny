using DarabnyProject.Models;
using DarabnyProject.ViewModels;

namespace DarabnyProject.Services
{
    public interface ILessonService
    {
        List<Lesson> GetAll();
        List<Lesson> GetAllWithChapterAndCourse();
        Lesson? GetById(int id);
        Lesson? GetWithChapterAndCourse(int id);
        LessonFormViewModel BuildFormViewModel(LessonFormViewModel? vm = null);
        void Add(Lesson l);
        void Update(Lesson l);
        void Delete(int id);
    }
}
