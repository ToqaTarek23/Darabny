using DarabnyProject.Repositories;

namespace DarabnyProject.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository enrollmentRepo;

        public EnrollmentService(IEnrollmentRepository enrollmentRepo)
        {
            this.enrollmentRepo = enrollmentRepo;
        }

        public bool IsEnrolled(string userId, int courseId)
            => enrollmentRepo.IsEnrolled(userId, courseId);

        public void Toggle(string userId, int courseId)
        {
            if (enrollmentRepo.IsEnrolled(userId, courseId))
                enrollmentRepo.Remove(userId, courseId);
            else
                enrollmentRepo.Add(userId, courseId);
        }
    }
}
