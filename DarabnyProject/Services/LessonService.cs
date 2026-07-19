using DarabnyProject.Models;
using DarabnyProject.Repositories;
using DarabnyProject.ViewModels;

namespace DarabnyProject.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository lessonRepo;
        private readonly IChapterRepository chapterRepo;

        public LessonService(ILessonRepository lessonRepo, IChapterRepository chapterRepo)
        {
            this.lessonRepo = lessonRepo;
            this.chapterRepo = chapterRepo;
        }


        public List<Lesson> GetAll()                        => lessonRepo.GetAll();
        public List<Lesson> GetAllWithChapterAndCourse()    => lessonRepo.GetAllWithChapterAndCourse();
        public Lesson? GetById(int id)                      => lessonRepo.GetById(id);
        public Lesson? GetWithChapterAndCourse(int id)      => lessonRepo.GetWithChapterAndCourse(id);
        public void Add(Lesson l)                           => lessonRepo.Add(l);
        public void Update(Lesson l)                        => lessonRepo.Update(l);
        public void Delete(int id)                          => lessonRepo.Delete(id);

        public LessonFormViewModel BuildFormViewModel(LessonFormViewModel? vm = null)
        {
            vm ??= new LessonFormViewModel();
            vm.Chapters = chapterRepo.GetAllWithCourse();
            return vm;
        }
    }
}
