using DarabnyProject.Data;
using DarabnyProject.Enums;
using DarabnyProject.Models;

namespace DarabnyProject.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly DarabnyDbContext db;

        public EnrollmentRepository(DarabnyDbContext db)
        {
            this.db = db;
        }

        public bool IsEnrolled(string userId, int courseId)
            => db.Enrollments.Any(e => e.UserId == userId && e.CourseId == courseId);

        public void Add(string userId, int courseId)
        {
            if (IsEnrolled(userId, courseId)) return;
            db.Enrollments.Add(new Enrollment
            {
                UserId   = userId,
                CourseId = courseId,
                Status   = EnrollmentStatus.Active
            });
            db.SaveChanges();
        }

        public void Remove(string userId, int courseId)
        {
            var e = db.Enrollments.FirstOrDefault(x => x.UserId == userId && x.CourseId == courseId);
            if (e == null) return;
            db.Enrollments.Remove(e);
            db.SaveChanges();
        }
    }
}
