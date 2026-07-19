using DarabnyProject.Models;
using System.ComponentModel.DataAnnotations;

namespace DarabnyProject.ViewModels
{
    public class ChapterFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a course")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Chapter title is required")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
        [MaxLength(150)]
        public string Name { get; set; } = "";

        
        public List<Course> Courses { get; set; } = new();
    }
}
