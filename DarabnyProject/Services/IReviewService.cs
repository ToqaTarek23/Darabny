using DarabnyProject.Models;

namespace DarabnyProject.Services
{
    public interface IReviewService
    {
        List<Review> GetByCourse(int courseId);
        bool HasReviewed(string userId, int courseId);
        void Add(string userId, int courseId, decimal rating, string comment);
    }
}
