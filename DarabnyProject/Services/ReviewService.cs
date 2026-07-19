using DarabnyProject.Models;
using DarabnyProject.Repositories;

namespace DarabnyProject.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository reviewRepo;

        public ReviewService(IReviewRepository reviewRepo)
        {
            this.reviewRepo = reviewRepo;
        }

        public List<Review> GetByCourse(int courseId)
            => reviewRepo.GetByCourse(courseId);

        public bool HasReviewed(string userId, int courseId)
            => reviewRepo.HasReviewed(userId, courseId);

        public void Add(string userId, int courseId, decimal rating, string comment)
        {
            if (HasReviewed(userId, courseId)) return;
            reviewRepo.Add(new Review
            {
                UserId   = userId,
                CourseId = courseId,
                Rating   = rating,
                Comment  = comment
            });
        }
    }
}
