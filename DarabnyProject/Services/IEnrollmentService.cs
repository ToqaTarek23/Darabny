namespace DarabnyProject.Services
{
    public interface IEnrollmentService
    {
        bool IsEnrolled(string userId, int courseId);
        void Toggle(string userId, int courseId);
    }
}
