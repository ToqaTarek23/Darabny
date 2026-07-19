using DarabnyProject.Models;

namespace DarabnyProject.Repositories
{
    public interface IReviewRepository
    {
        List<Review> GetByCourse(int courseId);
        bool HasReviewed(string userId, int courseId);
        void Add(Review review);
    }
}
