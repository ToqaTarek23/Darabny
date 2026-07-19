using DarabnyProject.Enums;
using DarabnyProject.Models;

namespace DarabnyProject.ViewModels
{
    public class StudentDashboardViewModel
    {
        public string StudentName    { get; set; } = "Student";
        public string StudentInitials { get; set; } = "ST";
        public string? StudentEmail  { get; set; }
        public string? StudentBio    { get; set; }
        public DateTime MemberSince  { get; set; }
        public bool IsActive         { get; set; }

        public List<Enrollment>   Enrollments  { get; set; } = new();
        public List<Favorites>    Favorites    { get; set; } = new();
        public List<Certificates> Certificates { get; set; } = new();

        public int TotalEnrolled  => Enrollments.Count;
        public int InProgress     => Enrollments.Count(e => e.Status == EnrollmentStatus.Active);
        public int Completed      => Enrollments.Count(e => e.Status == EnrollmentStatus.Completed);
        public int TotalFavorites => Favorites.Count;
       
        public Dictionary<int, int> CourseProgressMap { get; set; } = new();
    }
}
