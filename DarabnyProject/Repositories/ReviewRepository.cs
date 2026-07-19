using DarabnyProject.Data;
using DarabnyProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DarabnyProject.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DarabnyDbContext db;

        public ReviewRepository(DarabnyDbContext db)
        {
            this.db = db;
        }

        public List<Review> GetByCourse(int courseId)
            => db.Reviews
                .Include(r => r.User)
                .Where(r => r.CourseId == courseId && r.IsApproved)
                .OrderByDescending(r => r.AddedTime)
                .ToList();

        public bool HasReviewed(string userId, int courseId)
            => db.Reviews.Any(r => r.UserId == userId && r.CourseId == courseId);

        public void Add(Review review)
        {
            review.AddedTime = DateTime.Now;
            review.IsApproved = true;
            db.Reviews.Add(review);
            db.SaveChanges();
        }
    }
}
