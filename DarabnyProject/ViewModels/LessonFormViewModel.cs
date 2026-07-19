using DarabnyProject.Models;
using System.ComponentModel.DataAnnotations;

namespace DarabnyProject.ViewModels
{
    public class LessonFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a chapter")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a chapter")]
        public int ChapterId { get; set; }

        [Required(ErrorMessage = "Lesson title is required")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
        [MaxLength(200)]
        public string Name { get; set; } = "";

        
        public List<Chapter> Chapters { get; set; } = new();
    }
}
