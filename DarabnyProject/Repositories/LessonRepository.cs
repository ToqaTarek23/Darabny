using DarabnyProject.Data;
using DarabnyProject.Enums;
using DarabnyProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DarabnyProject.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly DarabnyDbContext db;

        public LessonRepository(DarabnyDbContext db)
        {
            this.db = db;
        }

        public List<Lesson> GetAll() => db.Lessons.ToList();

        public List<Lesson> GetAllWithChapterAndCourse()
            => db.Lessons
                .Include(l => l.Chapter).ThenInclude(ch => ch.Course)
                .ToList();

        public Lesson? GetById(int id) => db.Lessons.Find(id);

        public Lesson? GetWithChapterAndCourse(int id)
            => db.Lessons
                .Include(l => l.Chapter).ThenInclude(ch => ch.Course)
                .FirstOrDefault(l => l.Id == id);

        public void Add(Lesson l)
        {
            l.Type          = LessonType.Video;
            l.LessonURL     = "";
            l.IsFreePreview = false;
            l.LessonLength  = 0;
            db.Lessons.Add(l);
            db.SaveChanges();
        }

        public void Update(Lesson l)
        {
            var existing = db.Lessons.Find(l.Id);
            if (existing == null) return;
            existing.Name      = l.Name;
            existing.ChapterId = l.ChapterId;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var l = db.Lessons.Find(id);
            if (l == null) return;
            db.Lessons.Remove(l);
            db.SaveChanges();
        }

        public void Save() => db.SaveChanges();
    }
}
