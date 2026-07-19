using DarabnyProject.Models;

namespace DarabnyProject.Repositories
{
    public interface IEnrollmentRepository
    {
        bool IsEnrolled(string userId, int courseId);
        void Add(string userId, int courseId);
        void Remove(string userId, int courseId);
    }
}
