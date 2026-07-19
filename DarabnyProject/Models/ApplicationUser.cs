using DarabnyProject.Enums;
using Microsoft.AspNetCore.Identity;

namespace DarabnyProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string? PhotoURL { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime SignUpDate { get; set; } = DateTime.Now;
        public string? Bio { get; set; }
        public UserRole Role { get; set; } = UserRole.Student;  

        public List<Course>?       TeachingCourses { get; set; }
        public List<Enrollment>?   Enrollments     { get; set; }
        public List<Review>?       Reviews         { get; set; }
        public List<Favorites>?    Favorites       { get; set; }
        public List<Certificates>? Certificates    { get; set; }
        public List<Progress>?     Progress        { get; set; }
        public List<ContactUs>?    ContactMessages { get; set; }
    }
}
