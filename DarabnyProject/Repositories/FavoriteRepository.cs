using DarabnyProject.Data;
using DarabnyProject.Models;

namespace DarabnyProject.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly DarabnyDbContext db;

        public FavoriteRepository(DarabnyDbContext db)
        {
            this.db = db;
        }

        public bool IsFavorited(string userId, int courseId)
            => db.Favorites.Any(f => f.UserId == userId && f.CourseId == courseId);

        public void Add(string userId, int courseId)
        {
            if (IsFavorited(userId, courseId)) return;
            db.Favorites.Add(new Favorites { UserId = userId, CourseId = courseId });
            db.SaveChanges();
        }

        public void Remove(string userId, int courseId)
        {
            var f = db.Favorites.FirstOrDefault(x => x.UserId == userId && x.CourseId == courseId);
            if (f == null) return;
            db.Favorites.Remove(f);
            db.SaveChanges();
        }
    }
}
