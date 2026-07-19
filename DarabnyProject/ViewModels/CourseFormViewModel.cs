using DarabnyProject.Enums;
using DarabnyProject.Models;
using System.ComponentModel.DataAnnotations;

namespace DarabnyProject.ViewModels
{
    public class CourseFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
        [MaxLength(150, ErrorMessage = "Title must not exceed 150 characters")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Description is required")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
        [MaxLength(1000)]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Category is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        public string? InstructorId { get; set; }

        public CourseLevel Level { get; set; } = CourseLevel.Beginner;

        [MaxLength(50)]
        public string Language { get; set; } = "Arabic";

        [Range(0, 99999, ErrorMessage = "Invalid price")]
        public decimal Price { get; set; } = 0;

        [Range(0, 9999)]
        public int CourseLength { get; set; } = 0;

        public string? RequirementsText { get; set; }
        public string? LearnText { get; set; }

        public List<Category>  Categories  { get; set; } = new();
        public List<ApplicationUser> Instructors { get; set; } = new();
    }
}
