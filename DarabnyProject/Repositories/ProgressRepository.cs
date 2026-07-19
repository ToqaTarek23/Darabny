using DarabnyProject.Data;
using DarabnyProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DarabnyProject.Repositories
{
    public class ProgressRepository : IProgressRepository
    {
        private readonly DarabnyDbContext db;

        public ProgressRepository(DarabnyDbContext db)
        {
            this.db = db;
        }

        public bool IsCompleted(string userId, int lessonId)
            => db.Progress.Any(p => p.UserId == userId && p.LessonId == lessonId && p.IsCompleted);

        public void MarkComplete(string userId, int lessonId)
        {
            var existing = db.Progress
                .FirstOrDefault(p => p.UserId == userId && p.LessonId == lessonId);

            if (existing != null)
            {
                existing.IsCompleted     = true;
                existing.ProgressPercent = 100;
                existing.LastAccessedAt  = DateTime.Now;
            }
            else
            {
                db.Progress.Add(new Progress
                {
                    UserId          = userId,
                    LessonId        = lessonId,
                    IsCompleted     = true,
                    ProgressPercent = 100,
                    WatchedSeconds  = 0,
                    LastAccessedAt  = DateTime.Now
                });
            }
            db.SaveChanges();
        }

        public int GetCourseProgress(string userId, int courseId)
        {
            
            var totalLessons = db.Lessons
                .Include(l => l.Chapter)
                .Count(l => l.Chapter != null && l.Chapter.CourseId == courseId);

            if (totalLessons == 0) return 0;

            
            var completedLessons = db.Progress
                .Include(p => p.Lesson).ThenInclude(l => l!.Chapter)
                .Count(p => p.UserId == userId
                         && p.IsCompleted
                         && p.Lesson != null
                         && p.Lesson.Chapter != null
                         && p.Lesson.Chapter.CourseId == courseId);

            return (int)Math.Round((double)completedLessons / totalLessons * 100);
        }
    }
}
