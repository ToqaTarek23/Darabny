using DarabnyProject.Models;
using DarabnyProject.Repositories;
using DarabnyProject.ViewModels;

namespace DarabnyProject.Services
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository chapterRepo;
        private readonly ICourseRepository courseRepo;

        public ChapterService(IChapterRepository chapterRepo, ICourseRepository courseRepo)
        {
            this.chapterRepo = chapterRepo;
            this.courseRepo = courseRepo;
        }


        public List<Chapter> GetAll()                   => chapterRepo.GetAll();
        public List<Chapter> GetAllWithCourse()         => chapterRepo.GetAllWithCourse();
        public Chapter? GetById(int id)                 => chapterRepo.GetById(id);
        public Chapter? GetWithCourseAndLessons(int id) => chapterRepo.GetWithCourseAndLessons(id);
        public void Add(Chapter ch)                     => chapterRepo.Add(ch);
        public void Update(Chapter ch)                  => chapterRepo.Update(ch);
        public void Delete(int id)                      => chapterRepo.Delete(id);

        public ChapterFormViewModel BuildFormViewModel(ChapterFormViewModel? vm = null)
        {
            vm ??= new ChapterFormViewModel();
            vm.Courses = courseRepo.GetAll();
            return vm;
        }
    }
}
