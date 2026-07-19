using DarabnyProject.Data;
using DarabnyProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DarabnyProject.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly DarabnyDbContext db;

        public ChapterRepository(DarabnyDbContext db)
        {
            this.db = db;
        }

        public List<Chapter> GetAll() => db.Chapters.ToList();

        public List<Chapter> GetAllWithCourse()
            => db.Chapters.Include(ch => ch.Course).Include(ch => ch.Lessons).ToList();

        public Chapter? GetById(int id) => db.Chapters.Find(id);

        public Chapter? GetWithCourseAndLessons(int id)
            => db.Chapters
                .Include(ch => ch.Course)
                .Include(ch => ch.Lessons)
                .FirstOrDefault(ch => ch.Id == id);

        public void Add(Chapter ch)
        {
            db.Chapters.Add(ch);
            db.SaveChanges();
        }

        public void Update(Chapter ch)
        {
            var existing = db.Chapters.Find(ch.Id);
            if (existing == null) return;
            existing.Name     = ch.Name;
            existing.CourseId = ch.CourseId;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var ch = GetWithCourseAndLessons(id);
            if (ch == null) return;
            db.Chapters.Remove(ch);
            db.SaveChanges();
        }

        public void Save() => db.SaveChanges();
    }
}
