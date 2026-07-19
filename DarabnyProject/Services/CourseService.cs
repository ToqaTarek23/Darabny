using DarabnyProject.Models;
using DarabnyProject.Repositories;
using DarabnyProject.ViewModels;

namespace DarabnyProject.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository courseRepo;
        private readonly ICategoryRepository categoryRepo;

        public CourseService(ICourseRepository courseRepo, ICategoryRepository categoryRepo)
        {
            this.courseRepo = courseRepo;
            this.categoryRepo = categoryRepo;
        }


        public List<Course> GetAll()               => courseRepo.GetAll();
        public List<Course> GetAllWithCategory()   => courseRepo.GetAllWithCategory();
        public Course? GetById(int id)             => courseRepo.GetById(id);
        public Course? GetWithDetails(int id)      => courseRepo.GetWithDetails(id);
        public Course? GetWithRequirements(int id) => courseRepo.GetWithRequirements(id);

        public List<Course> Search(string? query, int? categoryId, string? sort)
            => courseRepo.Search(query, categoryId, sort);

        
        public CourseFormViewModel BuildFormViewModel(CourseFormViewModel? vm = null)
        {
            vm ??= new CourseFormViewModel();
            vm.Categories = categoryRepo.GetAll();
            
            return vm;
        }

        public void Add(Course course, string? requirementsText, string? learnText)
            => courseRepo.Add(course, requirementsText, learnText);

        public void Update(Course course, string? requirementsText, string? learnText)
            => courseRepo.Update(course, requirementsText, learnText);

        public void Delete(int id) => courseRepo.Delete(id);
    }
}
