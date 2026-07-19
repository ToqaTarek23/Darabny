using DarabnyProject.Models;

namespace DarabnyProject.ViewModels
{
    public class CoursesPageViewModel
    {
        public List<CourseCardViewModel> Courses    { get; set; } = new();
        public List<Category>            Categories { get; set; } = new();

        
        public string? Query      { get; set; }
        public int?    CategoryId { get; set; }
        public string? Sort       { get; set; }

        public int TotalCount => Courses.Count;
    }
}
