using DarabnyProject.Enums;

namespace DarabnyProject.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public EnrollmentStatus Status { get; set; }

        public string UserId { get; set; } = "";
        public ApplicationUser? User { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
